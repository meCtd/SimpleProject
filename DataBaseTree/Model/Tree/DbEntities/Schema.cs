
using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "schema")]
	[KnownType(typeof(Procedure))]
	[KnownType(typeof(Function))]
	[KnownType(typeof(Table))]
	[KnownType(typeof(DbView))]
	public class Schema : DbObject
	{
		public override DbEntityEnum Type => DbEntityEnum.Schema;

		public override bool CanHaveDefinition => false;

		public Schema(string name) : base(name)
		{
		}

		protected override bool CanBeChild(DbObject obj)
		{
			var type = obj.Type;
			return type == DbEntityEnum.Procedure || type == DbEntityEnum.Function ||
			       type == DbEntityEnum.Table || type == DbEntityEnum.View;

		}
	}
}
