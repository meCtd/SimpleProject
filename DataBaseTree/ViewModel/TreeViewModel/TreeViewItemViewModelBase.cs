using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace DataBaseTree.ViewModel.TreeViewModel
{
	public abstract class TreeViewItemViewModelBase : BindableBase
	{
		#region Fields

		private static readonly TreeViewItemViewModelBase _fakeChild = new FakeChild();

		private bool _isExpanded;

		private bool _isSelected;

		#endregion

		#region Properties

		public event EventHandler TreeChanged;
		
		public ObservableCollection<TreeViewItemViewModelBase> Children { get; }

		public virtual bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{

				if (_isExpanded && Parent != null)
					Parent.IsExpanded = true;
				if (HasFakeChild)
				{
					Children.Remove(_fakeChild);
					LoadChildren();
				}

				SetProperty(ref _isExpanded, value);
			}
		}

		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (Parent != null)
					Parent.IsExpanded = true;
				SetProperty(ref _isSelected, value);
			}
		}

		public abstract string Name { get; }

		public bool HasFakeChild
		{
			get { return Children.Count == 1 && Children[0] == _fakeChild; }
		}

		public TreeViewItemViewModelBase Parent { get; }

		protected void OnTreeChanged(object sender, EventArgs e)
		{
			if(Parent!=null)
				Parent.OnTreeChanged(sender,e);
			else
			{
				TreeChanged?.Invoke(sender,e);
			}
		}

		#endregion

		private TreeViewItemViewModelBase()
		{
		}

		protected TreeViewItemViewModelBase(TreeViewItemViewModelBase parent, bool canBeChild)
		{
			Parent = parent;
			Children = new ObservableCollection<TreeViewItemViewModelBase>();
			if (canBeChild)
				Children.Add(_fakeChild);
		}

		protected abstract void LoadChildren();

		private sealed class FakeChild : TreeViewItemViewModelBase
		{
			public override string Name { get; }

			protected override void LoadChildren()
			{
			}

			public void RefreshTreeItem()
			{
			}
		}
	}
}










