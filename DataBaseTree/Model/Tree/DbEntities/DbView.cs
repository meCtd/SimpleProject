using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "view")]
	public class DbView : TableData
	{
		public override DbEntityEnum Type => DbEntityEnum.View;

		public override bool CanHaveDefinition => true;

		public DbView(string name) : base(name)
		{
		}

		
	}
}
