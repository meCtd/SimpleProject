using System.Windows;
using System.Windows.Input;
using DataBaseTree.Framework;
using DataBaseTree.Model.DataBaseConnection;
using DataBaseTree.View;

namespace DataBaseTree.ViewModel.ConnectionViewModel
{
	public sealed class MsSqlConnectionViewModel : BaseConnectionViewModel
	{
		#region Fields

		private bool _isTCP = false;

		private uint _connectionTimeout = 5;

		private bool _integratedSecurity;

		private bool _pooling = true;

		#endregion

		#region Properties

		public bool IsTcp
		{
			get { return _isTCP; }
			set
			{
				SetProperty(ref _isTCP, value);

			}
		}

		public uint ConnectionTimeout
		{
			get { return _connectionTimeout; }
			set { SetProperty(ref _connectionTimeout, value); }
		}

		public bool IntegratedSecurity
		{
			get { return _integratedSecurity; }
			set { SetProperty(ref _integratedSecurity, value); }
		}

		public bool Pooling
		{
			get { return _pooling; }
			set { SetProperty(ref _pooling, value); }
		}

		#endregion

		private async void CreateConection(ConnectionWindow window)
		{
			IsBusy = true;
			MsSqlServer server = new MsSqlServer()
			{
				Server = _server,
				Port = _port,
				IsTcp = _isTCP,
				InitialCatalog = _initialCatalog,
				UserId = _userId,
				Password = _password,
				IntegratedSecurity = _integratedSecurity,
				ConnectionTimeout = _connectionTimeout,
				Pooling = _pooling
			};
			if (await server.TestConnectionAsync())
			{
				IsBusy = false;
				Connection = server;
				window.DialogResult = true;
				window.Close();
			}
			else
			{
				IsBusy = false;
				MessageBox.Show("Connection failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			
		}

		private async void TestConnection()
		{
			IsBusy = true;
			MsSqlServer test = new MsSqlServer()
			{
				Server = _server,
				Port = _port,
				IsTcp = _isTCP,
				InitialCatalog = _initialCatalog,
				UserId = _userId,
				Password = _password,
				IntegratedSecurity = _integratedSecurity,
				ConnectionTimeout = _connectionTimeout,
				Pooling = _pooling
			};

			if (await test.TestConnectionAsync())
			{
				IsBusy = false;

				MessageBox.Show("Connection succsessful!", "Test Connection", MessageBoxButton.OK,
					MessageBoxImage.Information);
			}
			else
			{
				IsBusy = false;

				MessageBox.Show("Connection failed!", "Test Connection", MessageBoxButton.OK,
					MessageBoxImage.Warning);
			}
		}

		private bool InputSuccsess()
		{
			bool inputSuccsess = true;
			inputSuccsess = !string.IsNullOrWhiteSpace(Server);

			if (!IntegratedSecurity)
			{
				inputSuccsess = inputSuccsess && !(string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Password));
			}

			return inputSuccsess && !IsBusy;
		}

		private bool InputSuccsess(ConnectionWindow window)
		{
			return InputSuccsess();
		}

		#region Commands

		#region CreateConnectionCommand

		private RelayCommand<ConnectionWindow> _createConnectionCommand;

		public override ICommand CreateConnectionCommand
		{
			get
			{
				return _createConnectionCommand ?? (_createConnectionCommand = new RelayCommand<ConnectionWindow>(
						   CreateConection,
						   InputSuccsess));
			}
		}

		#endregion

		#region TestConnectionCommand

		private RelayCommand _testConnectionCommand;

		public override ICommand TestConnectionCommand
		{
			get
			{
				return _testConnectionCommand ?? (_testConnectionCommand = new RelayCommand(
						   (o) => TestConnection(),
							(o) => InputSuccsess()));
			}
		}

		#endregion

		#endregion

	}
}
