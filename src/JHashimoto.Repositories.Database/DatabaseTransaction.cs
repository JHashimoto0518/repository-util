using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using JHashimoto.Infrastructure.Diagnostics;
using System.Diagnostics;

namespace JHashimoto.Repositories.Database {
    internal class DatabaseTransaction {

        public DbConnection connection { get; }

        public DbTransaction Instance { get; private set; }

        public bool HasBeenStarted { get; private set; }

        public DatabaseTransaction(DbConnection connection) {
            Guard.ArgumentNotNull<DbConnection>(connection, "connection");
            Guard.Assert(connection.State == ConnectionState.Open, "データベースに接続されていません。");

            this.connection = connection;
            this.HasBeenStarted = false;
        }

        public void Begin() {
            Guard.Assert(this.HasBeenStarted == false, "トランザクションを開始できません。トランザクションは既に開始されています。");

            this.Instance = this.connection.BeginTransaction();
            this.HasBeenStarted = true;
        }

        public void Commit() {
            this.Instance?.Commit();
            this.HasBeenStarted = false;
        }

        public void Rollback() {
            this.Instance?.Rollback();
            this.HasBeenStarted = false;
        }
    }
}
