using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DataBaseTree.Model.Loaders;
using DataBaseTree.Model.Tree;
using DataBaseTree.Model.Tree.DbEntities;

namespace DataBaseTree.Model
{
	[DataContract(Name = "SaveData")]
	[KnownType("KnownType")]
	public class SaveData
	{
		[DataMember(Name ="Loader")]
		public Loader Loader { get; private set; }

		[DataMember(Name = "Root")]
		public DbObject Root { get; private set; }

		public SaveData(Loader loader,DbObject root)
		{
			Loader = loader;
			Root = root;
		}

		private static IEnumerable<Type> KnownType()
		{

			foreach (var type in typeof(Loader).Assembly.GetTypes())
			{
				if (type.IsSubclassOf(typeof(Loader)))
					yield return type;
			}

			yield return typeof(Server);
		}
	}
}
