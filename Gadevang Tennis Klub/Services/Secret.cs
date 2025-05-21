namespace Gadevang_Tennis_Klub.Services
{
    public abstract class Secret
    {
        protected static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GadevangTennis;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
