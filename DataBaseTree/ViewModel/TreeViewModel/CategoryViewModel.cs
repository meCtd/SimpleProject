using System;
using System.Data.Common;
using System.Linq;
using System.Windows;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.ViewModel.TreeViewModel
{
	public class CategoryViewModel : MetadataViewModelBase
	{
		public override DbObject Model { get; }

		public override string Name
		{
			get
			{
				switch (Type)
				{
					case DbEntityEnum.Table:
						return "Tables";
					case DbEntityEnum.View:
						return "Views";
					case DbEntityEnum.Function:
						return "Functions";
					case DbEntityEnum.Procedure:
						return "Procedures";
					case DbEntityEnum.Constraint:
						return "Constraints";
					case DbEntityEnum.Column:
						return "Columns";
					case DbEntityEnum.Trigger:
						return "Triggers";
					case DbEntityEnum.Parameter:
						return "Parameters";
					case DbEntityEnum.Key:
						return "Keys";
					case DbEntityEnum.Index:
						return "Indexes";
					case DbEntityEnum.Schema:
						return "Schemas";

					default:
						throw new ArgumentException();

				}
			}
		}

		public override DbEntityEnum Type { get; }

		public override string Icon => @"/Resources/Icons/Category.png";

		public CategoryViewModel(MetadataViewModelBase parent, DbEntityEnum type) : base(parent, true)
		{
			Type = type;
			Model = parent.Model;
		}

		protected  override async void LoadChildren()
		{
			IsBusy = true;
			Root.IsLoadingInProcess = true;
			try
			{
				if (Model.IsChildrenLoaded(Type) == false)
				{
					if (!Root.IsConnected)
					{
						Root.RestoreConnection(false);
					}

					if (Root.IsConnected)
					{
						await Root.LoadModel(this,Type);
					}
				}

				foreach (var child in Model.Children.Where(s => s.Type == Type))
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
				Root.IsLoadingInProcess = false;
				OnTreeChanged(this, EventArgs.Empty);
			}
		}

		public override void RefreshTreeItem()
		{
			if (HasFakeChild)
				return;
			Model.DeleteChildrens(Type);
			Children.Clear();
			LoadChildren();
			OnTreeChanged(this, EventArgs.Empty);
		}
	}
}
