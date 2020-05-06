using System;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DataBaseTree.Model.DataBaseConnection
{
	[DataContract(Name = "ConnectionData")]
	public abstract class ConnectionData
	{
		public abstract DatabaseTypeEnum Type { get; }

		[DataMember(Name = "Server")]
		public string Server { get; set; }

		[DataMember(Name = "Port")]
		public uint Port { get; set; }

		[DataMember(Name = "InitialCatalog")]
		public string InitialCatalog { get; set; }

		public abstract string DefaultDatabase { get; }

		[DataMember(Name = "UserId")]
		public string UserId { get; set; }

		public string Password { get; set; }

		public abstract string ConnectionString { get; }

		public abstract DbConnection GetConnection();

		public abstract bool TestConnection();

		public virtual Task<bool> TestConnectionAsync()
		{
			return Task.Run<bool>(new Func<bool>(TestConnection));
		}


	}
}
