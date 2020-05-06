using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;

namespace DataBaseTree.Model.DataBaseConnection
{
	[DataContract(Name = "MsSqlServer")]
	public class MsSqlServer : ConnectionData
	{
		[DataMember(Name = "IntegratedSecurity")]
		public bool IntegratedSecurity { get; set; }

		[DataMember(Name = "Pooling")]
		public bool Pooling { get; set; }

		[DataMember(Name = "IsTcp")]
		public bool IsTcp { get; set; }

		[DataMember(Name = "ConnectionTimeout")]
		public uint ConnectionTimeout { get; set; }

		[DataMember(Name = "Type")]
		public override DatabaseTypeEnum Type => DatabaseTypeEnum.MsSql;

		public override string DefaultDatabase => "master";

		public override string ConnectionString => GetConenctionString();

		private string GetConenctionString()
		{
			StringBuilder connectionString = new StringBuilder();
			connectionString.Append("Data Source = ");
			if (IsTcp)
				connectionString.Append("tcp:");
			connectionString.Append(Server);
			connectionString.Append(IsTcp ? $",{Port};" : ";");
			connectionString.Append("Initial Catalog = ");
			connectionString.Append(string.IsNullOrWhiteSpace(InitialCatalog) ? DefaultDatabase : InitialCatalog);
			connectionString.Append(";");
			connectionString.Append(IntegratedSecurity ? "Integrated Security = true;" : $" User ID = {UserId}; Password = {Password};Pooling = {Pooling};Connection Timeout = {ConnectionTimeout};");
			return connectionString.ToString();
		}

		public override DbConnection GetConnection()
		{
			return new SqlConnection(GetConenctionString());
		}
		
		public override bool TestConnection()
		{
			bool conenctionSuccess = false;
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				try
				{
					connection.Open();
					conenctionSuccess = true;
				}
				catch (Exception)
				{
					conenctionSuccess = false;
				}
			}
			return conenctionSuccess;
		}

	}
}
