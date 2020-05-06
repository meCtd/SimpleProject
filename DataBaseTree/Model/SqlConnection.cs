using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTree.Model
{
	class SqlConnectionData
	{
		public SqlConnectionStringBuilder ConndectionData { get; }

		public SqlConnection CreateConnection => new SqlConnection(ConndectionData.ConnectionString);


	}
}
