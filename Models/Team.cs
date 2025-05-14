using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gadevang_Tennis_Klub.Models
{
    public class Team : ITeam
    {
        public int? ID { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public Trainer Trainer { get; set; }
        [BindProperty]
        public int Capacity { get; set; }
        [BindProperty]
        public int Price { get; set; }
        [BindProperty]
        public string ActiveDay { get; set; }


        public Team()
        {
            ID = 0;
            Name = string.Empty;
            Description = string.Empty;
            Trainer = new Trainer();
            Capacity = 0;
            Price = 0;
            ActiveDay = string.Empty;
        }
        public Team(int iD, string name, string description, Trainer trainer, int capacity, int price, string activeDay)
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
