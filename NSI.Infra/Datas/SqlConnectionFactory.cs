using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace NSI.Infra.Datas
{
    public class SqlConnectionFactory: IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        //public SqlConnectionFactory(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DBCS")
            );
        }

        public IDbConnection GetDbLogConnection()
        {
            var connStr = _configuration.GetConnectionString("DBLog");
            return new SqlConnection(connStr);
        }
    }
}
