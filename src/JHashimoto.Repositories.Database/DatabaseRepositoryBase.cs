using System;
using System.Diagnostics.Contracts;
using JHashimoto.Infrastructure.Diagnostics;

namespace JHashimoto.Repositories.Database {
    public abstract class DatabaseRepositoryBase {
        protected DatabaseRepositoryContext Context { get; }

        public DatabaseRepositoryBase(DatabaseRepositoryContext context) {
            Guard.ArgumentNotNull<DatabaseRepositoryContext>(context, "context");
            Guard.Assert(context.IsOpened, "データベースに接続されていません。");

            this.Context = context;
        }
    }
}
