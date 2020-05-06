using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "server")]
	[KnownType(typeof(Database))]
	public class Server : DbObject
	{
		public override DbEntityEnum Type => DbEntityEnum.Server;
		public override bool CanHaveDefinition => false;

		public Server(string name) : base(name)
		{
		}

		protected override bool CanBeChild(DbObject obj)
		{
			return obj.Type == DbEntityEnum.Database;
		}
	}
}
