using Microsoft.VisualStudio.TestTools.UnitTesting;
using JHashimoto.Repositories.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace JHashimoto.Repositories.Database.Tests {
    [TestClass()]
    public class DbProviderFactoryRegistrarTests {
        [TestMethod()]
        public void RegisterFactoryTest() {
            new DbProviderFactoryRegistrar().RegisterFactory(DbProviderTypes.SqlServer);
            DbProviderFactory factory = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");
            Assert.IsNotNull(factory);
            Assert.AreEqual(factory.GetType(), typeof(SqlClientFactory));
        }
    }
}