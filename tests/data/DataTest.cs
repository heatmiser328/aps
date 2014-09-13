using System;
using System.Text;
using System.Threading;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;

using NUnit.Framework;

using ica.aps.data.factory;
using ica.aps.data.interfaces;

namespace Data
{
    [TestFixture]
    public class DataTest
    {
        private const string cDatabaseName = "aps.sdf";
        private string _connstring;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _connstring = string.Format("Data Source={0}\\{1};password=ica;", TestHelpers.TestPaths.TestDataFolderPath, cDatabaseName);
        }

        [SetUp]
        public void Setup()
        {
            TestHelpers.TestData.ResetBlank(new DBFactory("System.Data.SqlServerCe", _connstring));
        }

        [Test]
        public void CreateConnection()
        {
            IDbConnection conn;
            IDBFactory dbf = new DBFactory("System.Data.SqlServerCe", _connstring);

            conn = dbf.Create();

            Assert.IsNotNull(conn, "failed to create connection");
        }

		/*
        [Test]
        public void TestConnection()
        {
            DbConnection conn;
            DbConnection conn1;

            conn = new SqlCeConnection(_connstring);
            conn1 = new SqlCeConnection(_connstring);

            string cmdText = "SELECT COUNT(*) FROM ";

            DbCommand command = conn.CreateCommand();
            command.CommandText = cmdText;
            string temp = "";

            string cmdText1 = "insert LOCATION (LocationID, Description, ModifiedTDS) values(NEWID(), 'desc', getdate())";

            DbCommand command1 = conn1.CreateCommand();
            command1.CommandText = cmdText1;


            conn.Open();
            conn1.Open();
            temp = command.ExecuteScalar().ToString();
            command1.ExecuteNonQuery();

            conn.Close();
            conn1.Close();

            //Assert.AreEqual("0", );
            //Assert.AreEqual("0", );
        }
		*/
    }

}
