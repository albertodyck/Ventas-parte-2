﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class SQL : IBaseDeDatos
    {
        private readonly string connectionString;

        public string ConnectionString => connectionString;

        public SQL(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Debe especificar la conexion");
            }

            this.connectionString = connectionString;

        }

        public bool ProbarConexion()
        {

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public object Scalar(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    return sqlCommand.ExecuteScalar();
                }
            }
        }

        public object Scalar(SqlCommand sqlCommand, SqlTransaction sqlTransaction)
        {
            using (sqlCommand)
            {
                try
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Transaction = sqlTransaction;
                    return sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public int NonQuery(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public int NonQuery(SqlCommand sqlCommand, SqlTransaction sqlTransaction)
        {
            using (sqlCommand)
            {
                try
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Transaction = sqlTransaction;
                    return sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public Dictionary<string, object> Reader(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    int cuentaReader = sqlDataReader.FieldCount - 1;

                    Dictionary<string, object> dicRespuesta = new Dictionary<string, object>();

                    while (sqlDataReader.Read())
                    {
                        if (!sqlDataReader.IsDBNull(0))
                        {
                            for (int i = 0; i <= cuentaReader; i++)
                            {
                                dicRespuesta.Add(
                                    sqlDataReader.GetName(i).ToString(),
                                    sqlDataReader.GetValue(i));
                            }
                        }
                    }

                    return dicRespuesta;
                }
            }
        }

        public DataTable ObtenerDataTable(string query)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();

                    DataTable dtRespuesta = new DataTable();

                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dtRespuesta);
                    }

                    return dtRespuesta;
                }
            }
        }


    }
}
