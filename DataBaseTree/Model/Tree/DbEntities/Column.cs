using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "column")]
	
	public class Column : TypeObject
	{
		public override DbEntityEnum Type => DbEntityEnum.Column;

		public override bool CanHaveDefinition => false;

		public Column(string name, DbType columnType) : base(name, columnType)
		{
		}
	}
}
