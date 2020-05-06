using System;

namespace DataBaseTree.Model.Tree
{
	[Flags]
	public enum DbEntityEnum
	{
		None = 0,
		Server = 1,
		Database = 2,
		Schema = 4,
		Table = 8,
		View = 16,
		Function = 32,
		Procedure = 64,
		Constraint = 128,
		Column =256,
		Trigger = 512,
		Parameter = 1024,
		Key = 2048,
		Index = 4096,
		Type = 8192,
		All =16383
	}
}
