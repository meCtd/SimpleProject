using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "trigger")]
	public class Trigger : DbObject
	{
		public override bool CanHaveDefinition => true;

		public override DbEntityEnum Type => DbEntityEnum.Trigger;

		public Trigger(string name) : base(name)
		{
		}
	}
}
