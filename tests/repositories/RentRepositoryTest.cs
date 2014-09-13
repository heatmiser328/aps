using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using NUnit.Framework;

using ica.aps.container;
using ica.aps.data.interfaces;
using ica.aps.data.models;
using ica.aps.data.repositories;
using ica.aps.tests.mocks;

namespace Repositories
{
    [TestFixture]
    public class RentRepositoryTest
    {
        private IIocContainer _container;
        private Guid _employeeID;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _container = MockIocContainer.Create();
            IDBFactory dbf = _container.Create<IDBFactory>();
            using (IDbConnection conn = dbf.Create())
            {
                conn.Open();
                IDbCommand cmd = conn.CreateCommand();                
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT EmployeeID FROM Employee WHERE FirstName = 'Tom'";
                _employeeID = (Guid)cmd.ExecuteScalar();
            }
        }

        [SetUp]
        public void Setup()
        {
            IDBFactory dbf = _container.Create<IDBFactory>();
            TestHelpers.TestData.ResetBlank(dbf);            
        }

        [Test]
        public void GetRents_Default()
        {            
        	IRentRepository rr = new RentRepository(_container);
			IEmployee employee = new Employee {
                ID = _employeeID
			};
            IList<IRent> rents = rr.GetRents(employee);
			Assert.IsNotNull(rents);
			Assert.AreEqual(1, rents.Count);
			
			IRent rent = rents.First();
			Assert.AreEqual(0.83M, rent.RentPct);
			Assert.AreEqual(DateTime.Parse("2000-01-01 00:00:00.000"), rent.EffectiveDate);
        }
		
        [Test]
        public void GetRents_Connection()
        {
            IDBFactory dbf = _container.Create<IDBFactory>();
			using (IDbConnection conn = dbf.Create())
			{
                conn.Open();
				IRentRepository rr = new RentRepository(_container, conn);
				
				IEmployee employee = new Employee {
                    ID = _employeeID
				};
                IList<IRent> rents = rr.GetRents(employee);
				Assert.IsNotNull(rents);
				Assert.AreEqual(1, rents.Count);
				
				IRent rent = rents.First();
				Assert.AreEqual(0.83M, rent.RentPct);
				Assert.AreEqual(DateTime.Parse("2000-01-01 00:00:00.000"), rent.EffectiveDate);
			}
        }
		
    }
}