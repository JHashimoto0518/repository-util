using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHashimoto.Repositories.Database {

    [AttributeUsage(AttributeTargets.Field)]
    public class DbProviderInvariantNameAttribute : Attribute {
        public string InvariantName { get; }

        public DbProviderInvariantNameAttribute(string invariantName) {
            this.InvariantName = invariantName;
        }
    }
}
