// ***********************************************************************
// Assembly         : VisioForge.Core
// Author           : roman
// Created          : 07-14-2020
//
// Last Modified By : roman
// Last Modified On : 03-04-2021
// ***********************************************************************
// <copyright file="BDADB.cs" company="VisioForge">
//     VisioForge (c) 2006 - 2021
// </copyright>
// <summary>Defines the DB type.</summary>
// ***********************************************************************

namespace VisioForge.DirectShowLib.BDA.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// Class BDADB.
    /// </summary>
    internal class BDADB
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private static string connectionString;

        /// <summary>
        /// The database mutex name.
        /// </summary>
        // private const string DbMutexName = @"Global\RikPVRDB";

        // private static Mutex mutex;

        /// <summary>
        /// The transacted connection.
        /// </summary>
        private LockedDbConnection transactedConnection;

        // private DbTransaction transaction;

        //// Methods
        //static DB()
        //{
        //    bool flag;
        //    connectionString = GetDefaultConnectionString();
        //    mutex = new Mutex(false, @"Global\RikPVRDB", out flag);
        //    if (flag)
        //    {
        //        MutexSecurity accessControl = mutex.GetAccessControl();
        //        MutexAccessRule rule = new MutexAccessRule(AppMutex.GetEveryoneSecurityIdentifier().Translate(typeof(NTAccount)), MutexRights.FullControl, AccessControlType.Allow);
        //        accessControl.AddAccessRule(rule);
        //        mutex.SetAccessControl(accessControl);
        //    }
        //}

        //public void Cleanup()
        //{
        //    this.ExecuteNonQuery("VACUUM", new object[0]);
        //}

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            // this.transaction.Commit();
            this.transactedConnection.Dispose();
            this.transactedConnection = null;
        }

        //public LockedDbConnection CreateConnection()
        //{
        //    if (this.transactedConnection != null)
        //    {
        //        return this.transactedConnection;
        //    }
        //    DbConnection conn = new SQLiteConnection(ConnectionString);
        //    conn.Open();
        //    return new LockedDbConnection(conn, mutex);
        //}

        //public object CreateParameter(int i, object value)
        //{
        //    return new SQLiteParameter("@" + i, value);
        //}

        //public void CreateTransaction()
        //{
        //    this.transactedConnection = this.CreateConnection();
        //    this.transaction = this.transactedConnection.Conn.BeginTransaction();
        //}

        //public DataSet ExecuteDataSet(LockedDbConnection conn, string sql, params object[] parameters)
        //{
        //    using (DbCommand command = conn.Conn.CreateCommand())
        //    {
        //        command.CommandText = sql;
        //        command.Connection = conn.Conn;
        //        if (this.transactedConnection != null)
        //        {
        //            command.Transaction = this.transaction;
        //        }
        //        for (int i = 0; i < parameters.Length; i++)
        //        {
        //            if (parameters[i] == null)
        //            {
        //                parameters[i] = DBNull.Value;
        //            }
        //            command.Parameters.Add(this.CreateParameter(i, parameters[i]));
        //        }
        //        DataSet dataSet = new DataSet();
        //        new SQLiteDataAdapter((SQLiteCommand)command).Fill(dataSet);
        //        return dataSet;
        //    }
        //}

        //public void ExecuteNonQuery(string sql, params object[] parameters)
        //{
        //    LockedDbConnection conn = this.CreateConnection();
        //    try
        //    {
        //        this.ExecuteNonQuery(conn, sql, parameters);
        //    }
        //    finally
        //    {
        //        if (this.transactedConnection == null)
        //        {
        //            conn.Dispose();
        //        }
        //    }
        //}

        //public void ExecuteNonQuery(LockedDbConnection conn, string sql, params object[] parameters)
        //{
        //    using (DbCommand command = conn.Conn.CreateCommand())
        //    {
        //        command.CommandText = sql;
        //        command.Connection = conn.Conn;
        //        if (this.transactedConnection != null)
        //        {
        //            command.Transaction = this.transaction;
        //        }
        //        for (int i = 0; i < parameters.Length; i++)
        //        {
        //            if (parameters[i] == null)
        //            {
        //                parameters[i] = DBNull.Value;
        //            }
        //            command.Parameters.Add(this.CreateParameter(i, parameters[i]));
        //        }
        //        command.ExecuteNonQuery();
        //    }
        //}

        //public DbDataReader ExecuteReader(string sql, params object[] parameters)
        //{
        //    DbDataReader reader;
        //    LockedDbConnection conn = this.CreateConnection();
        //    try
        //    {
        //        reader = this.ExecuteReader(conn, sql, parameters);
        //    }
        //    finally
        //    {
        //        if (this.transactedConnection == null)
        //        {
        //            conn.Dispose();
        //        }
        //    }
        //    return reader;
        //}

        //public DbDataReader ExecuteReader(LockedDbConnection conn, string sql, params object[] parameters)
        //{
        //    using (DbCommand command = conn.Conn.CreateCommand())
        //    {
        //        command.CommandText = sql;
        //        command.Connection = conn.Conn;
        //        if (this.transactedConnection != null)
        //        {
        //            command.Transaction = this.transaction;
        //        }
        //        for (int i = 0; i < parameters.Length; i++)
        //        {
        //            if (parameters[i] == null)
        //            {
        //                parameters[i] = DBNull.Value;
        //            }
        //            command.Parameters.Add(this.CreateParameter(i, parameters[i]));
        //        }
        //        return command.ExecuteReader();
        //    }
        //}

        //public object ExecuteScalar(string sql, params object[] parameters)
        //{
        //    object obj2;
        //    LockedDbConnection conn = this.CreateConnection();
        //    try
        //    {
        //        obj2 = this.ExecuteScalar(conn, sql, parameters);
        //    }
        //    finally
        //    {
        //        if (this.transactedConnection == null)
        //        {
        //            conn.Dispose();
        //        }
        //    }
        //    return obj2;
        //}

        //public object ExecuteScalar(LockedDbConnection conn, string sql, params object[] parameters)
        //{
        //    using (DbCommand command = conn.Conn.CreateCommand())
        //    {
        //        command.CommandText = sql;
        //        command.Connection = conn.Conn;
        //        if (this.transactedConnection != null)
        //        {
        //            command.Transaction = this.transaction;
        //        }
        //        for (int i = 0; i < parameters.Length; i++)
        //        {
        //            if (parameters[i] == null)
        //            {
        //                parameters[i] = DBNull.Value;
        //            }
        //            command.Parameters.Add(this.CreateParameter(i, parameters[i]));
        //        }
        //        return command.ExecuteScalar();
        //    }
        //}

        /// <summary>
        /// Gets the comma separated list.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>System.String.</returns>
        public static string GetCommaSeparatedList(IList<string> items)
        {
            if ((items == null) || (items.Count == 0))
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (items.Count - 1); i++)
            {
                builder.AppendFormat("{0}, ", items[i]);
            }

            builder.AppendFormat("{0}", items[items.Count - 1]);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the comma separated list with quotes.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>System.String.</returns>
        public static string GetCommaSeparatedListWithQuotes(IList<string> items)
        {
            if ((items == null) || (items.Count == 0))
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < (items.Count - 1); i++)
            {
                builder.AppendFormat("'{0}',", items[i].Replace("'", "''"));
            }

            builder.AppendFormat("'{0}'", items[items.Count - 1].Replace("'", "''"));
            return builder.ToString();
        }

        //public static string GetDefaultConnectionString()
        //{
        //    return string.Format("Data Source={0}", GetPath());
        //}

        //public int? GetModuleVersion(string module)
        //{
        //    return (int?)this.ExecuteScalar("SELECT Version FROM Versions WHERE ModuleName = ?", new object[] { module });
        //}

        //private static string GetPath()
        //{
        //    return Utility.GetPath(Environment.SpecialFolder.CommonApplicationData, "RikPVR.db");
        //}

        //public string GetSetting(string key)
        //{
        //    object obj2 = this.ExecuteScalar("SELECT Value FROM Settings WHERE Key = ?", new object[] { key });
        //    if ((obj2 != null) && !(obj2 is DBNull))
        //    {
        //        return Convert.ToString(obj2);
        //    }
        //    return null;
        //}

        //public T GetSetting<T>(string key)
        //{
        //    object obj2 = this.ExecuteScalar("SELECT Value FROM Settings WHERE Key = ?", new object[] { key });
        //    if ((obj2 == null) || (obj2 is DBNull))
        //    {
        //        return default(T);
        //    }
        //    if ((typeof(T) == typeof(bool)) && (obj2 is string))
        //    {
        //        bool flag;
        //        int num;
        //        if (bool.TryParse((string)obj2, out flag))
        //        {
        //            return (T)flag;
        //        }
        //        if (int.TryParse((string)obj2, out num))
        //        {
        //            return (T)(num == 1);
        //        }
        //    }
        //    else if ((typeof(T) == typeof(TimeSpan)) && (obj2 is string))
        //    {
        //        return (T)TimeSpan.Parse((string)obj2);
        //    }
        //    return (T)Convert.ChangeType(obj2, typeof(T));
        //}

        //public T GetSetting<T>(string key, T defaultValue)
        //{
        //    object obj2 = this.ExecuteScalar("SELECT Value FROM Settings WHERE Key = ?", new object[] { key });
        //    if ((obj2 == null) || (obj2 is DBNull))
        //    {
        //        return defaultValue;
        //    }
        //    try
        //    {
        //        if ((typeof(T) == typeof(bool)) && (obj2 is string))
        //        {
        //            bool flag;
        //            int num;
        //            if (bool.TryParse((string)obj2, out flag))
        //            {
        //                return (T)flag;
        //            }
        //            if (int.TryParse((string)obj2, out num))
        //            {
        //                return (T)(num == 1);
        //            }
        //        }
        //        else if ((typeof(T) == typeof(TimeSpan)) && (obj2 is string))
        //        {
        //            return (T)TimeSpan.Parse((string)obj2);
        //        }
        //        return (T)Convert.ChangeType(obj2, typeof(T));
        //    }
        //    catch (FormatException)
        //    {
        //        return defaultValue;
        //    }
        //}

        //public static string RenameParametersForSqlServer(string sql)
        //{
        //    SqlServerParameterReplacer replacer = new SqlServerParameterReplacer();
        //    return Regex.Replace(sql, @"\?", new MatchEvaluator(replacer.Evaluator));
        //}

        //public void RollbackTransaction()
        //{
        //    this.transaction.Rollback();
        //    this.transactedConnection.Dispose();
        //    this.transactedConnection = null;
        //}

        //public void SetSetting<T>(string key, T value)
        //{
        //    if (this.GetSetting(key) == null)
        //    {
        //        this.ExecuteNonQuery("INSERT INTO Settings (Key, Value) SELECT ?, ?", new object[] { key, value });
        //    }
        //    else
        //    {
        //        this.ExecuteNonQuery("UPDATE Settings SET Value = ? WHERE Key = ?", new object[] { value, key });
        //    }
        //}

        //public bool TableExists(string tableName)
        //{
        //    return (this.ExecuteScalar("SELECT * FROM sqlite_master WHERE\ttype = 'table' AND name = ?", new object[] { tableName }) != null);
        //}

        //public static void UpdateDatabase()
        //{
        //    try
        //    {
        //        string path = GetPath();
        //        if (!File.Exists(path))
        //        {
        //            SQLiteConnection.CreateFile(path);
        //            Utility.CheckFileSecurity(path);
        //        }
        //        DB db = new DB();
        //        if (!db.TableExists("Versions"))
        //        {
        //            SqlUtilities.RunScript(Resources.DBBasics);
        //        }
        //        if (db.GetModuleVersion("RikPVR").Value < 2)
        //        {
        //            SqlUtilities.RunScript(Resources.Settings);
        //        }
        //        DbProvider.DefaultConnectionString = ConnectionString;
        //        MigrationManager.UpgradeMax(Assembly.GetExecutingAssemb(), new SqliteProvider(), new string[0]);
        //        db.Cleanup();
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.WriteError(exception);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                connectionString = value;
            }
        }

        /// <summary>
        /// Class DBLock.
        /// Implements the <see cref="System.IDisposable" />.
        /// </summary>
        /// <seealso cref="System.IDisposable" />
        internal class DBLock : IDisposable
        {
            /// <summary>
            /// The has lock.
            /// </summary>
            private bool hasLock;

            /// <summary>
            /// The mutex.
            /// </summary>
            private Mutex mutex;

            /// <summary>
            /// Initializes a new instance of the <see cref="DBLock"/> class.
            /// </summary>
            /// <param name="mutex">The mutex.</param>
            public DBLock(Mutex mutex)
            {
                this.mutex = mutex;
                try
                {
                    mutex.WaitOne();
                }
                catch (AbandonedMutexException)
#pragma warning disable S108 // Nested blocks of code should not be left empty
                {
                }
#pragma warning restore S108 // Nested blocks of code should not be left empty

                this.hasLock = true;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DBLock"/> class.
            /// </summary>
            /// <param name="mutex">The mutex.</param>
            /// <param name="timeout">The timeout.</param>
            public DBLock(Mutex mutex, int timeout)
            {
                this.mutex = mutex;
                if (mutex.WaitOne(timeout, false))
                {
                    this.hasLock = true;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance has lock.
            /// </summary>
            /// <value><c>true</c> if this instance has lock; otherwise, <c>false</c>.</value>
            public bool HasLock
            {
                get
                {
                    return this.hasLock;
                }
            }

            #region IDisposable Support

            /// <summary>
            /// The disposed value.
            /// </summary>
            private bool disposedValue = false; // To detect redundant calls

            /// <summary>
            /// Releases unmanaged and - optionally - managed resources.
            /// </summary>
            /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // dispose managed state (managed objects).
                    }

                    if (hasLock)
                    {
                        mutex.ReleaseMutex();
                    }

                    // free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // set large fields to null.

                    disposedValue = true;
                }
            }

            // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.

            /// <summary>
            /// Finalizes an instance of the <see cref="DBLock"/> class.
            /// </summary>
            ~DBLock()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }

            // This code added to correctly implement the disposable pattern.

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);

                // uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            #endregion
        }

        /// <summary>
        /// Class SqlServerParameterReplacer.
        /// </summary>
#pragma warning disable S1144 // Unused private types or members should be removed
        private class SqlServerParameterReplacer
        {
            /// <summary>
            /// The index.
            /// </summary>
            private int index;

            /// <summary>
            /// Evaluators the specified input.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns>System.String.</returns>
#pragma warning disable S1172 // Unused method parameters should be removed
            public string Evaluator(Match input)
#pragma warning restore S1172 // Unused method parameters should be removed
            {
                return ("@" + this.index++);
            }
        }
#pragma warning restore S1144 // Unused private types or members should be removed
    }

    /// <summary>
    /// Class LockedDbConnection.
    /// Implements the <see cref="System.IDisposable" />.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class LockedDbConnection : IDisposable
    {
#pragma warning disable S1104 // Fields should not have public accessibility

        /// <summary>
        /// The connection.
        /// </summary>
        public DbConnection Conn;

        /// <summary>
        /// The l.
        /// </summary>
        private BDADB.DBLock l;
        private bool disposedValue;

#pragma warning restore S1104 // Fields should not have public accessibility

        /// <summary>
        /// Initializes a new instance of the <see cref="LockedDbConnection"/> class.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="mutex">The mutex.</param>
        public LockedDbConnection(DbConnection conn, Mutex mutex)
        {
            this.Conn = conn;
            this.l = new BDADB.DBLock(mutex);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                Conn.Dispose();
                l.Dispose();

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LockedDbConnection()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}