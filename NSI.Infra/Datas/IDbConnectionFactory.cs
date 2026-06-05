using System.Data;

namespace NSI.Infra.Datas
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        IDbConnection GetDbLogConnection();
    }


}
