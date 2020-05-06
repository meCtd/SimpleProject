using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows;
using DataBaseTree.Model.DataBaseConnection;
using DataBaseTree.Model.Loaders;
using DataBaseTree.Model.Printers;
using DataBaseTree.Model.Tree;
using DataBaseTree.Model.Tree.DbEntities;
using DataBaseTree.View;
using DataBaseTree.ViewModel.ConnectionViewModel;

namespace DataBaseTree.ViewModel.TreeViewModel
{
	public class TreeRootViewModel : MetadataViewModelBase
	{
		#region Fields

		private bool _isLoadingInProcess;

		private bool _isConnected;

		#endregion

		public bool IsConnected
		{
			get { return _isConnected; }
			set { SetProperty(ref _isConnected, value); }
		}

		public override DbObject Model { get; }

		public override DbEntityEnum Type => DbEntityEnum.Server;

		public override string Icon => "/Resources/Icons/Server.png";

		public Loader DbLoader { get; }

		public override TreeRootViewModel Root => this;

		public bool IsLoadingInProcess
		{
			get { return _isLoadingInProcess; }
			set { SetProperty(ref _isLoadingInProcess, value); }
		}

		public bool IsDefaultDatabase { get; }

		public override string Name => Model.Name;

		public TreeRootViewModel(Loader loader) : base(null, true)
		{
			DbLoader = loader;
			Model = new Server(DbLoader.Connection.Server);
			IsDefaultDatabase = string.IsNullOrWhiteSpace(DbLoader.Connection.InitialCatalog);
			IsConnected = true;
		}

		public TreeRootViewModel(DbObject model, Loader loader) : base(null, true)
		{
			DbLoader = loader;
			Model = model;
			IsDefaultDatabase = string.IsNullOrWhiteSpace(DbLoader.Connection.InitialCatalog);
			IsConnected = false;

		}

		protected override async void LoadChildren()
		{
			IsBusy = true;
			IsLoadingInProcess = true;
			try
			{
				if (!IsDefaultDatabase)
				{
					Database db = new Database(DbLoader.Connection.InitialCatalog);
					Model.AddChild(db);
					Children.Add(new DbObjectViewMolel(this, db));

					return;
				}

				if (Model.IsChildrenLoaded(DbEntityEnum.Database) == false)
				{
					if (!IsConnected)
						RestoreConnection(false);
					if (IsConnected)
						await LoadModel(this, DbEntityEnum.Database);
				}

				foreach (var child in Model.Children)
				{
					Children.Add(new DbObjectViewMolel(this, child));
				}

			}
			catch (DbException e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				IsBusy = false;
				IsLoadingInProcess = false;
			}
		}

		public void RestoreConnection(bool check)
		{
			if (!check)
				if (MessageBox.Show("You're not connected. Are you want to restore connection?", "Connection",
						MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
				{
					return;
				}

			ConnectionWindow window = new ConnectionWindow();
			ConnectionWindowViewModel data = (ConnectionWindowViewModel)window.DataContext;
			data.SelectedBaseType = DbLoader.Connection.Type;

			BaseConnectionViewModel conenctData = data.SelectedViewModel;
			conenctData.CanChange = false;
			conenctData.Server = DbLoader.Connection.Server;
			conenctData.Port = DbLoader.Connection.Port;
			conenctData.UserId = DbLoader.Connection.UserId;

			switch (conenctData)
			{
				case MsSqlConnectionViewModel msSql:
					msSql.IsTcp = ((MsSqlServer)DbLoader.Connection).IsTcp;
					msSql.IntegratedSecurity = ((MsSqlServer)DbLoader.Connection).IntegratedSecurity;
					msSql.Pooling = ((MsSqlServer)DbLoader.Connection).Pooling;
					msSql.ConnectionTimeout = ((MsSqlServer)DbLoader.Connection).ConnectionTimeout;
					break;
			}

			if (window.ShowDialog() == true)
			{
				IsConnected = true;
				Root.DbLoader.Connection = data.SelectedViewModel.Connection;
			}
		}

		public async Task LoadModel(MetadataViewModelBase obj, DbEntityEnum type)
		{
			Root.DbLoader.Connection.InitialCatalog = obj.Model.DataBaseName;
			try
			{
				if (Root.IsConnected)
				{
					if (type == DbEntityEnum.All)
						await DbLoader.LoadChildren(obj.Model);
					else
						await DbLoader.LoadChildren(obj.Model, type);
				}
			}
			catch (DbException d)
			{
				MessageBox.Show(d.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				if (Root.IsDefaultDatabase)
					Root.DbLoader.Connection.InitialCatalog = string.Empty;
			}

		}

		public async Task LoadProperties(MetadataViewModelBase obj)
		{
			obj.IsBusy = true;
			Root.IsLoadingInProcess = true;
			try
			{
				Root.DbLoader.Connection.InitialCatalog = obj.Model.DataBaseName;
				await Root.DbLoader.LoadProperties(obj.Model);

			}
			catch (DbException e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			finally
			{
				obj.IsBusy = false;
				Root.IsLoadingInProcess = false;
				if (Root.IsDefaultDatabase)
					Root.DbLoader.Connection.InitialCatalog = string.Empty;
			}
		}

		public void RefreshProperties(MetadataViewModelBase obj)
		{
			obj.Model.DeleteProperties();
			obj.Model.IsPropertyLoaded = false;
		}

	}

}
