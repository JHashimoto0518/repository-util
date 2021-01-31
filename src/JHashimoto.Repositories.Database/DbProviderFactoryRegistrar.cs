using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace JHashimoto.Repositories.Database {
    public class DbProviderFactoryRegistrar {

        public void RegisterFactory(DbProviderTypes dbProviderTypes) {
            if (dbProviderTypes == DbProviderTypes.SqlServer) {
                DbProviderFactories.RegisterFactory(DbProviderTypes.SqlServer.InvariantName, DbProviderTypes.SqlServer.Factory);
            }
        }
    }
}
