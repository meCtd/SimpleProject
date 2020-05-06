using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DataBaseTree.Model.Tree;
using DataBaseTree.Model.Tree.DbEntities;

namespace DataBaseTree.Model.Printers
{
	public class MsSqlTablePrinter : IPrinter
	{

		private StringBuilder _defintion;

		private readonly Regex _bracketRegex = new Regex(@"(\,)[\n|\t]+\)", RegexOptions.Compiled);

		public MsSqlTablePrinter()
		{
			_defintion = new StringBuilder();
		}

		public string GetDefintition(DbObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException();
			_defintion.Append($"CREATE TABLE [{obj.SchemaName}].[{obj.Name}](\n");

			SetColumns(obj);
			SetPrimaryUniqueKeys(obj, true);
			SetPrimaryUniqueKeys(obj, false);

			_defintion.Append(")\n\n");

			SetCheckConstraints(obj);
			_defintion.Append("\n\n");

			SetForeingKeyConstraint(obj);

			string definition = _defintion.ToString();
			Match match;
			while ((match = _bracketRegex.Match(definition)).Success)
			{
				definition = definition.Remove(match.Groups[1].Index, 1);
			}
			return definition;

		}


		private void SetColumns(DbObject obj)
		{
			if (!obj.IsChildrenLoaded(DbEntityEnum.Column).GetValueOrDefault(false))
				throw new NotSupportedException();

			foreach (var child in obj.Children.Where(child => child.Type == DbEntityEnum.Column))
			{
				var col = (Column)child;
				_defintion.Append($"\t[{col.Name}] {col.ObjectMemberType} ");
				if ((bool)child.Properties[Constants.IsIdentityProperty])
				{
					_defintion.Append(
						$"IDENTITY ({child.Properties[Constants.SeedValueProperty]},{child.Properties[Constants.SeedIncrementProperty]}) ");
				}
				;
				if (!string.IsNullOrWhiteSpace(child.Properties[Constants.DefaultValueProperty].ToString()))
				{
					_defintion.Append($"CONSTRAINT [DF_{child.SchemaName}_{child.Name}] DEFAULT {child.Properties[Constants.DefaultValueProperty]} ,\n");
				}
				else
				{
					if ((bool)child.Properties[Constants.IsNullableProperty])
					{
						_defintion.Append("NULL ,\n");
					}
					else
						_defintion.Append("NOT NULL ,\n");
				}
			}
		}

		private void SetPrimaryUniqueKeys(DbObject obj, bool keyType)
		{
			string search = keyType ? "PRIMARY" : "UNIQUE";
			string name = keyType ? "PRIMARY KEY" : "UNIQUE";


			IEnumerable<DbObject> matches = obj.Children.Where(key => key.Type == DbEntityEnum.Key && key.Properties[Constants.TypeProperty].ToString().Contains(search));
			foreach (var match in matches)
			{
				_defintion.Append($"CONSTRAINT [{match.Name}] {name} \n(\n");
				IEnumerable<string> columns = match.Properties[Constants.ColumnsProperty].ToString().Split(' ')
					.Where(s => !string.IsNullOrWhiteSpace(s));
				foreach (var column in columns)
				{
					_defintion.Append($"\t[{column}] ,\n");
				}

				_defintion.Append(") ,\n");
			}

		}

		private void SetForeingKeyConstraint(DbObject obj)
		{
			IEnumerable<DbObject> foreingnKeys = obj.Children.Where(child => child.Type == DbEntityEnum.Key&&child.Properties[Constants.TypeProperty].ToString().Contains("FOREIGN"));
			foreach (var key in foreingnKeys)
			{
				_defintion.Append(
					$"ALTER TABLE [{obj.SchemaName}].[{obj.Name}]  WITH CHECK ADD CONSTRAINT [{key.Name}] FOREIGN KEY([{key.Properties[Constants.ColumnsProperty]}]) REFERENCES [{key.Properties[Constants.ReferenceSchemaNameProperty]}].[{key.Properties[Constants.ReferenceTableNameProperty]}] ([{key.Properties[Constants.ReferenceColumnProperty]}])\n");
			}
		}

		private void SetCheckConstraints(DbObject obj)
		{
			IEnumerable<DbObject> constraints = obj.Children.Where(child => child.Type == DbEntityEnum.Constraint);
			foreach (var constraint in constraints)
			{
				_defintion.Append(
					$"ALTER TABLE [{obj.SchemaName}].[{obj.Name}]  WITH CHECK ADD CONSTRAINT [{constraint.Name}] CHECK {constraint.Properties[Constants.DefinitionProperty]} \n");
			}
		}


	}
}
