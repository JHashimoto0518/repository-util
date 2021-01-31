using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHashimoto.Repositories.Database {
    public sealed class DbProviderTypes {
        public string InvariantName { get; private init; }

        public DbProviderFactory Factory { get; private init; }

        private DbProviderTypes(string invariantName, DbProviderFactory factory) {
            this.InvariantName = invariantName;
            this.Factory = factory;
        }

        public override bool Equals(object obj) {
            var target = obj as DbProviderTypes;
            if (target is null) return false;
            return this.InvariantName == target.InvariantName;
        }

        public override int GetHashCode() {
            return this.InvariantName.GetHashCode();
        }

        public static DbProviderTypes SqlServer = new DbProviderTypes("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
    }
}
