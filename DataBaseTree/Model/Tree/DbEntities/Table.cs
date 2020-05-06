using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "table")]
	[KnownType(typeof(Constraint))]
	[KnownType(typeof(Key))]
	public class Table : TableData
	{
		public override DbEntityEnum Type => DbEntityEnum.Table;

		public Table(string name) : base(name)
		{
		}

		protected override bool CanBeChild(DbObject obj)
		{
			return base.CanBeChild(obj) || obj.Type == DbEntityEnum.Key ||
				   obj.Type == DbEntityEnum.Constraint;
		}

	}
}
