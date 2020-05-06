using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using DataBaseTree.Framework;
using DataBaseTree.Model;
using DataBaseTree.Model.Loaders;
using DataBaseTree.Model.Printers;
using DataBaseTree.Model.Tree;
using DataBaseTree.Model.Tree.DbEntities;
using DataBaseTree.View;
using DataBaseTree.ViewModel.TreeViewModel;
using Microsoft.Win32;
using Prism.Mvvm;

namespace DataBaseTree.ViewModel
{
	public class TreeWindowViewModel : BindableBase
	{

		#region Fields

		private IEnumerable<TreeRootViewModel> _root;

		private string _searchText;

		private IPrinterFactory _printerFactory;

		private string _definitionText;

		private DbEntityEnum _searchMask;

		private bool _isFilterEnabled;

		private bool _isSaveinInProcess;

		private IEnumerable<MetadataViewModelBase> _searchMatches;

		private IEnumerator<MetadataViewModelBase> _searchEnumerator;

		#endregion

		#region Properties

		public IEnumerable<TreeRootViewModel> Root
		{
			get { return _root; }
			set { SetProperty(ref _root, value); }
		}

		public ObservableCollection<KeyValuePair<string, object>> ItemProperties { get; }

		public string SearchText
		{
			get { return _searchText; }
			set
			{
				SetProperty(ref _searchText, value);
				_searchMatches = null;
			}
		}

		public string DefinitionText
		{
			get { return _definitionText; }
			set
			{
				SetProperty(ref _definitionText, value);

			}
		}

		public bool IsFilterEnabled
		{
			get { return _isFilterEnabled; }
			set
			{
				SetProperty(ref _isFilterEnabled, value);
				_searchMatches = null;
			}
		}

		public DbEntityEnum SearchMask
		{
			get { return _searchMask; }
			set
			{
				SetProperty(ref _searchMask, value);
				_searchMatches = null;
			}
		}

		#endregion

		public TreeWindowViewModel()
		{
			ItemProperties = new ObservableCollection<KeyValuePair<string, object>>();
			_searchMask = DbEntityEnum.All;
		}

		#region Commands

		#region ConnectCommand

		private RelayCommand _connectCommand;

		public RelayCommand ConnectCommand
		{
			get
			{
				return _connectCommand ?? (_connectCommand = new RelayCommand(
						   (o) => Connect()));
			}
		}

		private void Connect()
		{
			ConnectionWindow window = new ConnectionWindow();
			ConnectionWindowViewModel data = (ConnectionWindowViewModel)window.DataContext;
			if (window.ShowDialog() == true)
			{
				switch (data.SelectedBaseType)
				{
					case DatabaseTypeEnum.MsSql:
						TreeRootViewModel root =
							new TreeRootViewModel(new MsSqlLoader(data.SelectedViewModel.Connection));
						root.TreeChanged += (sender, e) => _searchMatches = null;

						Root = new TreeRootViewModel[] { root };
						_printerFactory = new MsSqlPrinterFactory();
						break;
				}
			}
		}

		#endregion

		#region RemoveConnectionCommand

		private RelayCommand _removeConnectionCommand;

		public RelayCommand RemoveConnectionCommand
		{
			get
			{
				return _removeConnectionCommand ?? (_removeConnectionCommand = new RelayCommand(
						   (o) => Root = null, CanRemove));
			}
		}

		private bool CanRemove(object o)
		{
			return Root != null && !Root.First().IsLoadingInProcess;
		}
		#endregion

		#region RefreshCommand

		private RelayCommand<MetadataViewModelBase> _refreshCommand;

		public RelayCommand<MetadataViewModelBase> RefreshCommand
		{
			get { return _refreshCommand ?? (_refreshCommand = new RelayCommand<MetadataViewModelBase>(Refresh, CanRefresh)); }
		}

		private void Refresh(MetadataViewModelBase o)
		{
			o.RefreshTreeItem();
			if (o.Model.IsPropertyLoaded)
				ShowProperties(o);
			o.Model.Definition = string.Empty;
		}

		private bool CanRefresh(MetadataViewModelBase o)
		{
			if (o == null)
				return false;

			return !o.Root.IsLoadingInProcess && !o.IsBusy && o.Root.IsConnected;
		}

		#endregion

		#region RestoreConnection

		private RelayCommand _resotreCommand;

		public RelayCommand RestoreCommand
		{
			get
			{
				return _resotreCommand ??
					   (_resotreCommand = new RelayCommand(Restore, CanRestore));
			}
		}

		private void Restore(object o)
		{
			Root.First().RestoreConnection(true);
		}

		private bool CanRestore(object o)
		{
			return Root != null && !Root.First().IsConnected;
		}

		#endregion

		#region LoadPropertiesCommand

		private RelayCommand<MetadataViewModelBase> _loadPropertiesCommand;

		public RelayCommand<MetadataViewModelBase> LoadPropertiesCommand
		{
			get { return _loadPropertiesCommand ?? (_loadPropertiesCommand = new RelayCommand<MetadataViewModelBase>(LoadProperties, CanLoadProperties)); }
		}

		private async void LoadProperties(MetadataViewModelBase obj)
		{
			if (obj.Model.IsPropertyLoaded)
				obj.Root.RefreshProperties(obj);
			await obj.Root.LoadProperties(obj);
			ItemProperties.Clear();
			ItemProperties.AddRange(obj.Model.Properties);
		}

		private bool CanLoadProperties(MetadataViewModelBase obj)
		{

			if (obj is CategoryViewModel || obj == null)
				return false;

			return !obj.Root.IsLoadingInProcess && !obj.IsBusy && obj.Root.IsConnected;
		}

		#endregion

		#region ShowDefinitionCommand

		private RelayCommand<MetadataViewModelBase> _showDefinitionCommandCommand;

		public RelayCommand<MetadataViewModelBase> ShowDefinitionCommand
		{
			get { return _showDefinitionCommandCommand ?? (_showDefinitionCommandCommand = new RelayCommand<MetadataViewModelBase>(ShowDefinition, CanShowDefinition)); }
		}

		private async void ShowDefinition(MetadataViewModelBase obj)
		{
			obj.IsBusy = true;
			obj.Root.IsLoadingInProcess = true;
			try
			{
				if (!string.IsNullOrWhiteSpace(obj.Model.Definition))
				{
					DefinitionText = obj.Model.Definition;
					return;
				}
				if (obj.Type == DbEntityEnum.Table)
				{
					if (!obj.Model.IsChildrenLoaded(null).GetValueOrDefault(false))
						await obj.Root.LoadModel(obj, DbEntityEnum.All);
					foreach (var child in obj.Model.Children)
					{
						if (!child.IsPropertyLoaded)
						{
							obj.Root.DbLoader.Connection.InitialCatalog = obj.Model.DataBaseName;
							await obj.Root.DbLoader.LoadProperties(child);
							if (obj.Root.IsDefaultDatabase)
								obj.Root.DbLoader.Connection.InitialCatalog = string.Empty;
						}
					}
				}

				//get printer

				DefinitionText = obj.Model.Definition = _printerFactory.GetPrinter(obj.Model).GetDefintition(obj.Model);

			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				obj.IsBusy = false;
				obj.Root.IsLoadingInProcess = false;
			}

		}

		private bool CanShowDefinition(MetadataViewModelBase obj)
		{
			if (obj == null )
				return false;
			if (obj.Type == DbEntityEnum.Table &&
			    (!obj.Model.IsChildrenLoaded(DbEntityEnum.Column).GetValueOrDefault(false) && !obj.Root.IsConnected))
				return false;
			return !(obj is CategoryViewModel) && _printerFactory.IsSupported(obj.Model) && !obj.IsBusy && !obj.Root.IsLoadingInProcess;
		}

		#endregion

		#region ShowPropertiesCommand

		private RelayCommand<MetadataViewModelBase> _showPropertiesCommand;

		public RelayCommand<MetadataViewModelBase> ShowPropertiesCommand
		{
			get { return _showPropertiesCommand ?? (_showPropertiesCommand = new RelayCommand<MetadataViewModelBase>(ShowProperties, CanShowProperties)); }
		}

		private void ShowProperties(MetadataViewModelBase obj)
		{
			ItemProperties.Clear();
			if (obj.Model.IsPropertyLoaded)
				ItemProperties.AddRange(obj.Model.Properties);
		}

		private bool CanShowProperties(MetadataViewModelBase obj)
		{
			if (obj == null)
				ItemProperties.Clear();
			return obj != null;
		}

		#endregion

		#region SearchCommand

		private RelayCommand _searchCommand;

		public RelayCommand SearchCommand
		{
			get { return _searchCommand ?? (_searchCommand = new RelayCommand(Search, (o) => (Root != null)&&!string.IsNullOrWhiteSpace(_searchText))); }
		}

		private void Search(object o)
		{
			if (_searchMatches == null)
			{
				List<MetadataViewModelBase> matches = new List<MetadataViewModelBase>();

				matches.AddRange(IsFilterEnabled
					? FindMatchesNode(Root.First(), SearchText, SearchMask)
					: FindMatchesNode(Root.First(), SearchText, DbEntityEnum.All));

				_searchMatches = matches;
				_searchEnumerator = _searchMatches.GetEnumerator();

			}

			if (!_searchMatches.Any())
				MessageBox.Show("No matches were found!", "Search", MessageBoxButton.OK, MessageBoxImage.Warning);
			else
			{
				if (!_searchEnumerator.MoveNext())
				{
					_searchEnumerator.Reset();
					_searchEnumerator.MoveNext();
				}

				if (_searchEnumerator.Current != null)
					_searchEnumerator.Current.IsSelected = true;
			}
		}

		private bool Filter(object rootViewItem)
		{
			if (_searchMatches == null)
				return true;
			return _searchMatches.Any(s => s.Root == rootViewItem);
		}

		private IEnumerable<MetadataViewModelBase> FindMatchesNode(MetadataViewModelBase node, string text, DbEntityEnum mask)
		{
			if (node.Name.Contains(text) && !(node is CategoryViewModel) && mask.HasFlag(node.Type))
				yield return node;

			if (node.Children.Count != 0 && !node.HasFakeChild)
			{
				foreach (var treeViewItemViewModelBase in node.Children)
				{
					var treeViewItem = (MetadataViewModelBase)treeViewItemViewModelBase;

					foreach (MetadataViewModelBase res in FindMatchesNode(treeViewItem, text, mask))
					{
						yield return res;
					}
				}
			}
		}

		#endregion

		#region SaveCommand

		private RelayCommand _saveCommand;

		public RelayCommand SaveCommand
		{
			get { return _saveCommand ?? (_saveCommand = new RelayCommand(Save, (o) => Root != null && !_isSaveinInProcess)); }

		}

		private void Save(object o)
		{
			_isSaveinInProcess = true;
			try
			{
				SaveFileDialog save = new SaveFileDialog()
				{
					Filter = "Tree Files (*.tree)|*.tree"
				};
				if (save.ShowDialog() == true)
				{
					using (FileStream fs = new FileStream(save.FileName, FileMode.OpenOrCreate))
					{
						fs.SetLength(0);
						DataContractSerializer saver = new DataContractSerializer(typeof(SaveData));
						saver.WriteObject(fs, new SaveData(Root.First().DbLoader, Root.First().Model));
					}
					MessageBox.Show("Tree was saved!", "Saving", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				_isSaveinInProcess = false;
			}
		}

		#endregion

		#region OpenCommand

		private RelayCommand _openCommand;

		public RelayCommand OpenCommand
		{
			get { return _openCommand ?? (_openCommand = new RelayCommand(Open)); }
		}

		private void Open(object o)
		{
			OpenFileDialog open = new OpenFileDialog()
			{
				Filter = "Tree Files (*.tree)|*.tree"
			};

			if (open.ShowDialog() == true)
			{
				using (FileStream fs = new FileStream(open.FileName, FileMode.Open))
				{

					try
					{
						DataContractSerializer ser = new DataContractSerializer(typeof(SaveData));
						SaveData save = (SaveData)ser.ReadObject(fs);
						Root = new TreeRootViewModel[] { new TreeRootViewModel(save.Root, save.Loader) };
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}

				}

			}
			switch (Root.First().DbLoader.Connection.Type)
			{
				case DatabaseTypeEnum.MsSql:
					_printerFactory = new MsSqlPrinterFactory();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

		}

		#endregion

		#endregion
	}
}