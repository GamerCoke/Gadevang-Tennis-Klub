using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class Team : ITeam
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ITrainer Trainer { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public string ActiveDay { get; set; }

        public Team(int iD, string name, string description, ITrainer trainer, int capacity, int price, string activeDay)
        {
            ID = iD;
            Name = name;
            Description = description;
            Trainer = trainer;
            Capacity = capacity;
            Price = price;
            ActiveDay = activeDay;
        }
    }
}
