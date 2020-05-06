using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "database")]
	[KnownType(typeof(Schema))]
	public class Database : DbObject
	{
		public override DbEntityEnum Type => DbEntityEnum.Database;

		public override bool CanHaveDefinition => false;

		public Database(string name) : base(name)
		{
		}

		protected override bool CanBeChild(DbObject obj)
		{
			return obj.Type == DbEntityEnum.Schema;
		}


	}
}
