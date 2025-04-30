using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class Team : ITeam
    {
        public int ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ITrainer Trainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Capacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Price { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ActiveDay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
