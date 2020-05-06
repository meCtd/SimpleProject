using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using DataBaseTree.Model.Tree.DbEntities;

namespace DataBaseTree.Model.Tree
{
	public class DbEntityFactory
	{
		#region Fields

		private static DbEntityFactory _instance;

		private Dictionary<DbEntityEnum, Func<DbDataReader, DbObject>> _creator;

		protected IReadOnlyDictionary<DbEntityEnum, Func<DbDataReader, DbObject>> _objectCreator =>
			_creator ?? (_creator = SetCreator());

		#endregion

		public static DbEntityFactory ObjectCreator => _instance ?? (_instance = new DbEntityFactory());

		private DbEntityFactory()
		{

		}

		protected virtual Dictionary<DbEntityEnum, Func<DbDataReader, DbObject>> SetCreator()
		{
			Dictionary<DbEntityEnum, Func<DbDataReader, DbObject>> dictionary = new Dictionary<DbEntityEnum, Func<DbDataReader, DbObject>>
			{
				[DbEntityEnum.Database] = (s) => new Database(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Schema] = (s) => new Schema(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Table] = (s) => new Table(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.View] = (s) => new DbView(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Key] = (s) => new Key(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Index] = (s) => new Index(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Trigger] = (s) => new Trigger(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Constraint] = (s) => new Constraint(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Procedure] = (s) => new Procedure(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Function] = (s) => new Function(s.GetString(s.GetOrdinal(Constants.NameProperty))),
				[DbEntityEnum.Column] = (s) =>
				{
					int? length = null;
					int? precision = null;
					int? scale = null;
					if (!s.IsDBNull(s.GetOrdinal(Constants.PrecisionProperty)))
						precision = s.GetByte(s.GetOrdinal(Constants.PrecisionProperty));

					if (!s.IsDBNull(s.GetOrdinal(Constants.ScaleProperty)))
						scale = s.GetByte(s.GetOrdinal(Constants.ScaleProperty));

					if (!s.IsDBNull(s.GetOrdinal(Constants.MaxLengthProperty)))
						length = s.GetInt16(s.GetOrdinal(Constants.MaxLengthProperty));

					return new Column((s.GetString(s.GetOrdinal(Constants.NameProperty))), new DbType(s.GetString(s.GetOrdinal(Constants.TypeNameProperty)), length, precision, scale));
				},
				[DbEntityEnum.Parameter] = (s) =>
				{
					int? length = null;
					int? precision = null;
					int? scale = null;
					if (!s.IsDBNull(s.GetOrdinal(Constants.PrecisionProperty)))
						precision = s.GetByte(s.GetOrdinal(Constants.PrecisionProperty));

					if (!s.IsDBNull(s.GetOrdinal(Constants.ScaleProperty)))
						scale = s.GetByte(s.GetOrdinal(Constants.ScaleProperty));

					if (!s.IsDBNull(s.GetOrdinal(Constants.MaxLengthProperty)))
						length = s.GetInt16(s.GetOrdinal(Constants.MaxLengthProperty));

					return new Parameter((s.GetString(s.GetOrdinal(Constants.NameProperty))), new DbType(s.GetString(s.GetOrdinal(Constants.TypeNameProperty)), length, precision, scale));
				},

			};
			return dictionary;
		}

		public DbObject Create(DbDataReader reader, DbEntityEnum type)
		{
			return _objectCreator[type](reader);
		}
	}
}
