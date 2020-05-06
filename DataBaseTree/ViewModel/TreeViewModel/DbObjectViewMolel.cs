using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.ViewModel.TreeViewModel
{
	public class DbObjectViewMolel : MetadataViewModelBase
	{
		#region Properties

		public override DbObject Model { get; }

		public override string Name => Model.ToString();

		public override DbEntityEnum Type => Model.Type;

		public override string Icon
		{
			get
			{
				switch (Type)
				{
					case DbEntityEnum.Server:
						return "/Resources/Icons/Server.png";
					case DbEntityEnum.Database:
						return "/Resources/Icons/Database.png";
					case DbEntityEnum.Schema:
						return "/Resources/Icons/Schema.png";
					case DbEntityEnum.Table:
						return "/Resources/Icons/Table.png";
					case DbEntityEnum.View:
						return "/Resources/Icons/View.png";
					case DbEntityEnum.Function:
						return "/Resources/Icons/Function.png";
					case DbEntityEnum.Procedure:
						return "/Resources/Icons/Procedure.png";
					case DbEntityEnum.Constraint:
						return "/Resources/Icons/Constraint.png";
					case DbEntityEnum.Column:
						return "/Resources/Icons/Column.png";
					case DbEntityEnum.Trigger:
						return "/Resources/Icons/Trigger.png";
					case DbEntityEnum.Parameter:
						return "/Resources/Icons/Parameter.png";
					case DbEntityEnum.Key:
						return "/Resources/Icons/Key.png";
					case DbEntityEnum.Index:
						return "/Resources/Icons/Index.png";
					default:
						throw new ArgumentException();
				}
			}
		}

		#endregion

		public DbObjectViewMolel(MetadataViewModelBase parent, DbObject model) : base(parent, parent.Root.DbLoader.Hierarchy.IsPossibleChilds(model.Type))
		{
			Model = model;
		}

		protected override async void LoadChildren()
		{
			IsBusy = false;
			Root.IsLoadingInProcess = false;
			try
			{
				
				IEnumerable<DbEntityEnum> childs = Root.DbLoader.Hierarchy.GetChildTypes(Type).ToArray();
				if (childs.Count() > 1)
				{
					foreach (var type in childs)
					{
						Children.Add(new CategoryViewModel(this, type));
					}
				}

				if (childs.Count() == 1)
				{
					if (Model.IsChildrenLoaded(childs.First()) == false)
					{
						if (!Root.IsConnected)
						{
							Root.RestoreConnection(false);
						}

						if (Root.IsConnected)
						{
							await Root.LoadModel(this,DbEntityEnum.All);
						}
					}

					foreach (var child in Model.Children)
					{
						Children.Add(new DbObjectViewMolel(this, child));
					}
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
	}
}
