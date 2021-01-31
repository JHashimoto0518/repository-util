using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Reflection;
using JHashimoto.Infrastructure.Diagnostics;

namespace JHashimoto.Repositories.Database {

    /// <summary>
    /// 永続化の手段としてデータベースを使用するRepositoryクラスのコンテキストを表します。
    /// </summary>
    public class DatabaseRepositoryContext : IDisposable {

        private readonly DbConnection connection;

        private readonly DatabaseTransaction transaction;

        public bool IsOpened {
            get { return this.connection?.State == ConnectionState.Open; }
        }

        public DatabaseRepositoryContext(DbProviderTypes dbProviderTypes, string connectionString) {
            Guard.ArgumentNotNullOrWhiteSpace(connectionString, "connectionString");
            
            // [How to use Microsoft.Data.SqlClient with DbProviderFactories? · Issue #239 · dotnet/SqlClient](https://github.com/dotnet/SqlClient/issues/239)
            new DbProviderFactoryRegistrar().RegisterFactory(dbProviderTypes);
            
            this.connection = DbProviderFactories.GetFactory(dbProviderTypes.InvariantName).CreateConnection();
            this.connection.ConnectionString = connectionString;
            this.connection.Open();
            this.transaction = new DatabaseTransaction(this.connection);
        }

        #region 終了処理

        /// <summary>
        /// <see cref="DatabaseRepositoryContext"/>オブジェクトがガベージコレクションにより収集される前に、<see cref="DatabaseRepositoryContext"/>がリソースを解放し、
        /// その他のクリーンアップ操作を実行できるようにします。
        /// </summary>
        ~DatabaseRepositoryContext() {
            Dispose();
        }

        /// <summary>
        /// 既にDisoseが呼ばれた場合は<c>true</c>。まだ呼ばれていない場合は<c>false</c>。
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースを解放します。
        /// </summary>
        /// <param name="disposing">
        /// マネージ リソースとアンマネージ リソースの両方を解放する場合は<c>true</c>。アンマネージ リソースだけを解放する場合は<c>false</c>。
        /// </param>
        private void Dispose(bool disposing) {
            if (disposed) {
                return;
            }

            // アンマネージリソースを解放する。
            this.connection?.Dispose();

            disposed = true;
        }

        #endregion 終了処理

        /// <summary>
        /// データベースへの変更を開始する前に呼び出します。
        /// </summary>
        public void BeginChanges() {
            this.transaction.Begin();
        }

        /// <summary>
        /// データベースへの変更を保存します。
        /// </summary>
        /// <remarks>
        /// <para>トランザクションが開始されている場合、トランザクションをコミットします。トランザクションが開始されていない場合は、何もしません。</para>
        /// </remarks> 
        public void SaveChanges() {
            this.transaction.Commit();
        }

        /// <summary>
        /// データベースへの変更をキャンセルします。
        /// </summary>
        /// <remarks>
        /// <para>トランザクションが開始されている場合、トランザクションがロールバックされます。トランザクションが開始されていない場合は、何もしません。</para>
        /// </remarks> 
        public void CancelChanges() {
            this.transaction.Rollback();
        }

        public IEnumerable<T> QuerySql<T>(string sql, object param = null) {
            Guard.ArgumentNotNullOrWhiteSpace(sql, "sql");

            return this.connection.Query<T>(sql, param, this.transaction.Instance);
        }

        public IEnumerable<T> QueryStoredProcedure<T>(string storedProcedureName, object param = null) {
            Guard.ArgumentNotNullOrWhiteSpace(storedProcedureName, "storedProcedureName");

            return this.connection.Query<T>(storedProcedureName, param, this.transaction.Instance, commandType: CommandType.StoredProcedure);
        }

        public int ExecuteSql(string sql, object param = null) {
            Guard.ArgumentNotNullOrWhiteSpace(sql, "sql");

            return this.connection.Execute(sql, param, this.transaction.Instance);
        }

        public int ExecuteStoredProcedure(string storedProcedureName, object param = null) {
            Guard.ArgumentNotNullOrWhiteSpace(storedProcedureName, "storedProcedureName");

            return this.connection.Execute(storedProcedureName, param, this.transaction.Instance, commandType: CommandType.StoredProcedure);
        }
    }
}
