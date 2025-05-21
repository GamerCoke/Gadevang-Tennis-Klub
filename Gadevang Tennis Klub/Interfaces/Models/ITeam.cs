using Gadevang_Tennis_Klub.Models;

namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface ITeam
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Trainer Trainer { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public string ActiveDay { get; set; }

    }
}
