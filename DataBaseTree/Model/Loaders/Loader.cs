using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DataBaseTree.Model.DataBaseConnection;
using DataBaseTree.Model.Providers;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.Model.Loaders
{
	[DataContract(Name = "Loader", IsReference = true)]
	[KnownType("KnownType")]
	public abstract class Loader
	{
		protected ScriptProvider _provider;
		
		[DataMember(Name = "ConnectionData")]
		public ConnectionData Connection { get;  set; }

		public Hierarchy Hierarchy => Hierarchy.HierarchyObject;

		public abstract Task LoadChildren(DbObject obj);

		public abstract Task LoadChildren(DbObject obj, DbEntityEnum childType);

		public abstract Task LoadProperties(DbObject obj);

		protected Loader(ConnectionData connection, ScriptProvider provider)
		{
			Connection = connection;
			_provider = provider;
		}

		private static IEnumerable<Type> KnownType()
		{
			return typeof(ConnectionData).Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(ConnectionData)));
		}
	}
}
