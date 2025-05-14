using System.Runtime.CompilerServices;

namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface ICourt
    {
        public string Type { get; set; }
        public int ID { get; set; }
        public string? Name { get; set; }

    }
}
