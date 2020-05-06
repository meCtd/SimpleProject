using System;
using DataBaseTree.Model;
using DataBaseTree.ViewModel.ConnectionViewModel;
using Prism.Mvvm;

namespace DataBaseTree.ViewModel
{
	public class ConnectionWindowViewModel : BindableBase

	{
		#region Fields

		private BaseConnectionViewModel _selectedViewModel;

		private DatabaseTypeEnum _selectedBaseType;

		#endregion

		#region Properties

		public DatabaseTypeEnum SelectedBaseType
		{
			get { return _selectedBaseType; }
			set
			{
				SetProperty(ref _selectedBaseType, value);
				OnDataBaseTypeEnumChanged();
			}
		}

		public BaseConnectionViewModel SelectedViewModel
		{
			get { return _selectedViewModel; }
			set
			{
				SetProperty(ref _selectedViewModel, value);
			}
		}

		#endregion

		public ConnectionWindowViewModel()
		{
			SelectedViewModel = new MsSqlConnectionViewModel();
			SelectedViewModel.CanChange = true;
			DataBaseTypeChanged += ChangeDataContext;
		}

		#region Events

		public event EventHandler DataBaseTypeChanged;

		private void OnDataBaseTypeEnumChanged()
		{
			DataBaseTypeChanged?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		private void ChangeDataContext(object sender, EventArgs e)
		{
			switch (_selectedBaseType)
			{

				case DatabaseTypeEnum.MsSql:
					SelectedViewModel = new MsSqlConnectionViewModel();
					break;
				default:
					SelectedViewModel = null;
					break;
			}
		}
	}
}

