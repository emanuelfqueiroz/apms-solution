﻿

using AffiliatePMS.Application.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AffiliatePMS.Infra.Persistence.Common;

public class APMSDbManager(IConfiguration configuration) : ISqlConnectionFactory, IUnitOfWorkADO, IDisposable
{
    public SqlConnection GetConnection()
    {
        if (_transaction != null)
        {
            return _transaction.Connection;
        }
        return Connection;
    }

    public SqlCommand CreateCommand(string query)
    {
        if (_transaction != null)
        {
            return new SqlCommand(query, _transaction.Connection, _transaction);
        }
        return new SqlCommand(query, Connection);
    }


    private SqlConnection? _connection;
    private SqlTransaction? _transaction;
    public SqlConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(configuration.GetConnectionString("APMSConnection"));
                _connection.Open();
            }

            return _connection;
        }
    }


    public void Commit()
    {
        try
        {
            _transaction!.Commit();
        }
        catch
        {
            _transaction!.Rollback();
            throw;
        }
    }

    public void Rollback()
    {
        _transaction!.Rollback();
    }

    public void BeginTransaction()
    {
        _transaction = Connection.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
        Connection.Close();
        Connection.Dispose();
        _connection = null;
    }


}
