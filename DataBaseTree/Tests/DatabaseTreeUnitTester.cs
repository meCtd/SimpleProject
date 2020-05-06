using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using DataBaseTree.Model;
using DataBaseTree.Model.DataBaseConnection;
using DataBaseTree.Model.Loaders;
using DataBaseTree.Model.Printers;
using DataBaseTree.Model.Tree;
using DataBaseTree.Model.Tree.DbEntities;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DataBaseTree.Tests
{
	[TestFixture]
	public class DatabaseTreeUnitTester
	{
		public static string Path = (AppDomain.CurrentDomain.BaseDirectory + "test.tree");

		[Test]
		public async Task ConnectionTest()
		{
			MsSqlServer server = new MsSqlServer()
			{
				Server = @"CTDLAPTOP\SQLEXPRESS",
				IntegratedSecurity = true,
				InitialCatalog = "master"
			};

			bool connectionResul = await server.TestConnectionAsync();
			Assert.IsTrue(connectionResul);
		}

		[Test]
		public async Task LoadChildrenTest()
		{
			MsSqlServer server = new MsSqlServer()
			{
				Server = @"CTDLAPTOP\SQLEXPRESS",
				IntegratedSecurity = true
			};
			if (!await server.TestConnectionAsync())
				throw new Exception();

			Database database = new Database("Northwnd");

			MsSqlLoader loader = new MsSqlLoader(server);

			await loader.LoadChildren(database);

			Assert.IsTrue(database.Children.Count != 0);

		}


		[Test]
		public async Task LoadProperties()
		{
			MsSqlServer server = new MsSqlServer()
			{
				Server = @"CTDLAPTOP\SQLEXPRESS",
				IntegratedSecurity = true
			};
			if (!await server.TestConnectionAsync())
				throw new Exception();

			Database database = new Database("Northwnd");

			MsSqlLoader loader = new MsSqlLoader(server);

			await loader.LoadProperties(database);

			Assert.IsTrue(database.Properties.Count != 0);

		}

		[Test]
		public async Task LoadDescription()
		{
			MsSqlServer server = new MsSqlServer()
			{
				Server = @"CTDLAPTOP\SQLEXPRESS",
				IntegratedSecurity = true
			};
			if (!await server.TestConnectionAsync())
				throw new Exception();

			Database database = new Database("Northwnd");
			MsSqlLoader loader = new MsSqlLoader(server);
			await loader.LoadChildren(database);

			DbObject child = database.Children.First();
			new MsSqlPrinterFactory().GetPrinter(child).GetDefintition(child);


			Assert.IsTrue(!string.IsNullOrWhiteSpace(child.Definition));
		}
		
		[Test]
		public async Task SaveTreeTest()
		{
			MsSqlServer server = new MsSqlServer()
			{
				Server = @"CTDLAPTOP\SQLEXPRESS",
				IntegratedSecurity = true
			};
			if (!await server.TestConnectionAsync())
				throw new Exception();

			Database database = new Database("Northwnd");
			MsSqlLoader loader = new MsSqlLoader(server);
			await loader.LoadChildren(database);

			using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
			{
				fs.SetLength(0);
				DataContractSerializer saver = new DataContractSerializer(typeof(SaveData));
				saver.WriteObject(fs, new SaveData(loader,database));
			}

			Assert.IsTrue(File.Exists(Path));
		}

	}

	[SetUpFixture]
	class DeleteTestSavedfile
	{

		[OneTimeTearDown]
		public void DeleteTestFolder()
		{
			if (File.Exists(DatabaseTreeUnitTester.Path))
			{
				File.Delete(DatabaseTreeUnitTester.Path);
			}
		}
	}

}
