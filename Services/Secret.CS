using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System;
namespace Gadevang_Tennis_Klub.Services
{
    public class Secret
    {

        private static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TennisTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
