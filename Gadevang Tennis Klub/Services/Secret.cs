namespace Gadevang_Tennis_Klub.Services
{
    public abstract class Secret
    {
        protected static string _connectionString = @"Data Source=mssql17.unoeuro.com;Initial Catalog = gremlin_dk_db_zealand; User ID = gremlin_dk; Password=y4RekHnEdc6DtAh2barz;Connect Timeout = 30; Encrypt=True;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
