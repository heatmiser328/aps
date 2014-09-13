using System;
using System.Data;
using System.IO;

using ica.aps.data.interfaces;
using ica.aps.data.factory;

namespace TestHelpers
{
	internal static class TestPaths
	{
        internal static string TestFolderPath
		{
			get
			{
				if (_testfolderpath == null)
				{
                    string dir = AppDomain.CurrentDomain.BaseDirectory;
                    if (dir.Contains("bin\\Debug"))
                        _testfolderpath = dir.Substring(0, dir.IndexOf("bin\\Debug"));
                    else
                        _testfolderpath = dir.Substring(0, dir.IndexOf("bin\\Release"));
				}
				return _testfolderpath;
			}
		}

        internal static string TestDataFolderPath
        {
            get
            {
                if (_testdatafolderpath == null)
                {
                    _testdatafolderpath = Path.Combine(TestFolderPath, "data\\");
                }
                return _testdatafolderpath;
            }
        }

        private static string _testfolderpath = null;
        private static string _testdatafolderpath = null;
	}

    internal static class TestData
    {
        internal static IDBFactory Database
        {
            get
            {
                /*
                System.Configuration.ExeConfigurationFileMap map = new System.Configuration.ExeConfigurationFileMap();
                map.ExeConfigFilename = Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "tests.dll.config");
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(map, System.Configuration.ConfigurationUserLevel.None);
                System.Configuration.ConnectionStringSettings cs = config.ConnectionStrings.ConnectionStrings["aps"];
                */
                System.Configuration.ConnectionStringSettings cs = System.Configuration.ConfigurationManager.ConnectionStrings["aps"];

                return new DBFactory(cs.ProviderName, string.Format(cs.ConnectionString, TestPaths.TestDataFolderPath));
            }
        }

        internal static void ResetBlank(IDBFactory factory)
        {
			/*
            File.Copy(Path.Combine(TestHelpers.TestPaths.TestFolderPath, "src\\App.Config"),
                                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ica.aps.services.dll.config"), true);
			*/
            if (factory.IsSqlServerCeProvider)
            {
                File.Copy(Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "aps_backup.sdf"),
                                    Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "aps.sdf"), true);
            }
            else if (factory.IsSqlServerProvider)
            {
                using (IDbConnection conn = factory.Create())
                {
                    conn.Open();

                    string sql = string.Format(@"RESTORE DATABASE {0} FROM DISK = '{1}' WITH REPLACE",
                                                conn.Database,
                                                Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, "aps.bak"));
                    // change to master
                    conn.ChangeDatabase("master");
                    // perform restore
		            using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static void Reset(IDBFactory factory)
        {
            Reset(factory, ".sql");
        }

        internal static void Reset(IDBFactory factory, string sqlscript)
        {
            ResetBlank(factory);

            if (!string.IsNullOrEmpty(sqlscript))
                ExecuteScript(factory, Path.Combine(TestHelpers.TestPaths.TestDataFolderPath, sqlscript));
        }

        internal static void ExecuteScript(IDBFactory factory, string scriptfile)
        {
            using (StreamReader reader = new StreamReader(scriptfile))
            {
                string sqlscript = reader.ReadToEnd();
                string[] commands = sqlscript.Split(new string[] { "GO", "Go", "gO", "go", ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (commands != null && commands.Length > 0)
                {
                    using (IDbConnection conn = factory.Create())
                    {
                        conn.Open();
                        foreach (string command in commands)
                        {
                            if (!string.IsNullOrEmpty(command.Replace(System.Environment.NewLine, "")) && 
                                 command != ";" && 
                                 string.Compare(command, "go", true) != 0)
                            {
                                using (IDbCommand cmd = conn.CreateCommand())
                                {
                                    try
                                    {
                                        //System.Diagnostics.Trace.Write(command);
                                        cmd.CommandText = command;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        while (ex != null)
                                        {
                                            System.Diagnostics.Trace.Write(ex.Message);
                                            ex = ex.InnerException;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }            
        }

        internal static void ExecuteSQL(IDBFactory factory, string sql)
        {
            using (IDbConnection conn = factory.Create())
            {
                conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    //System.Diagnostics.Trace.Write(sql);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal static void ExecuteSQL(IDBFactory factory, string[] sqlcmds)
        {
            using (IDbConnection conn = factory.Create())
            {
                conn.Open();
                IDbTransaction trans = conn.BeginTransaction();
                foreach (string sql in sqlcmds)
                {
                    //System.Diagnostics.Trace.Write(sql);
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = trans;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
                trans.Commit();
            }
        }

        internal static object ExecuteSQLScalar(IDBFactory factory, string sql)
        {
            using (IDbConnection conn = factory.Create())
            {
                conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    //System.Diagnostics.Trace.Write(sql);
                    cmd.CommandText = sql;
                    return cmd.ExecuteScalar();                    
                }
            }
        }
    }
}
