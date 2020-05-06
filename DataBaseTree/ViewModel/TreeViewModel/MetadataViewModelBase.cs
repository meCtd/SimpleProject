using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.ViewModel.TreeViewModel
{
	public abstract class MetadataViewModelBase : TreeViewItemViewModelBase
	{
		#region Fields

		private bool _isBusy;

		#endregion

		#region Properties

		public abstract DbEntityEnum Type { get; }

		public abstract string Icon { get; }

		public abstract DbObject Model { get; }

		public virtual TreeRootViewModel Root { get; }

		public bool IsBusy
		{
			get { return _isBusy; }
			set { SetProperty(ref _isBusy, value); }
		}

		#endregion

		protected MetadataViewModelBase(MetadataViewModelBase parent, bool canBeChild) : base(parent, canBeChild)
		{
			Root = parent?.Root;
		}
		
		public virtual async void RefreshTreeItem()
		{
			
			if (Model.IsPropertyLoaded)
			{
				Model.DeleteProperties();
				await Root.LoadProperties(this);
			}
			if (HasFakeChild)
				return;
			Model.DeleteChildrens();
			Children.Clear();
			LoadChildren();
			OnTreeChanged(this, EventArgs.Empty);
		}
	}
}
