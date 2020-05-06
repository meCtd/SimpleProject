using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract]
	[KnownType(typeof(Column))]
	[KnownType(typeof(Trigger))]
	[KnownType(typeof(Index))]
	public abstract class TableData : DbObject
	{
		public override bool CanHaveDefinition => false;

		protected TableData(string name) : base(name)
		{
		}

		protected override bool CanBeChild(DbObject obj)
		{
			DbEntityEnum type = obj.Type;
			return type == DbEntityEnum.Column || type == DbEntityEnum.Trigger || type == DbEntityEnum.Index;
		}

	}
}
