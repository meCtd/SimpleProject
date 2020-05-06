using System.Windows.Input;
using DataBaseTree.Model.DataBaseConnection;
using Prism.Mvvm;

namespace DataBaseTree.ViewModel.ConnectionViewModel
{
	public abstract class BaseConnectionViewModel : BindableBase
	{
		#region Fields

		protected string _server;

		protected uint _port;

		protected string _initialCatalog;

		protected string _userId;

		protected string _password;

		protected bool _isBusy;

		private bool _canChange;

		#endregion

		#region Properties

		public bool CanChange
		{
			get { return _canChange; }
			set { SetProperty(ref _canChange, value); }
		}

		public string Server
		{
			get { return _server; }
			set { SetProperty(ref _server, value); }
		}

		public uint Port
		{
			get { return _port; }
			set
			{
				SetProperty(ref _port, value);
			}
		}

		public string InitialCatalog
		{
			get { return _initialCatalog; }
			set
			{
				SetProperty(ref _initialCatalog, value);
			}
		}

		public string UserId
		{
			get { return _userId; }
			set
			{
				SetProperty(ref _userId, value);
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				SetProperty(ref _password, value);
			}
		}

		public bool IsBusy
		{
			get { return _isBusy; }
			set { SetProperty(ref _isBusy, value); }
		}

		public ConnectionData Connection { get; protected set; }

		public abstract ICommand CreateConnectionCommand { get; }

		public abstract ICommand TestConnectionCommand { get; }

		#endregion

	}
}

