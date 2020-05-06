using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "type-object")]
	public abstract class TypeObject : DbObject
	{
		[DataMember(Name = "object-member-type")]
		public DbType ObjectMemberType { get; private set; }

		protected TypeObject(string name, DbType type) : base(name)
		{
			ObjectMemberType = type;
		}
		public override string ToString()
		{
			StringBuilder name = new StringBuilder();
			name.Append($"{Name} ({ObjectMemberType.Name}");
			switch (ObjectMemberType.Name)
			{
				case "nvarchar":
				case "varchar":
				case "nchar":
				case "сhar":
				case "varbinary":
				case "binary":
					name.Append(ObjectMemberType.Length == -1 ? $"(max))" : $"({ObjectMemberType.Length}))");
					break;

				case "time":
				case "datetime2":
					name.Append($"({ObjectMemberType.Scale}))");
					break;

				case "decimal":
					name.Append($"({ObjectMemberType.Precision},{ObjectMemberType.Scale})");
					break;
				default:
					name.Append(")");
					break;
			}
			return name.ToString();
		}
	}
}
