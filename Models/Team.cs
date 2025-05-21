using Gadevang_Tennis_Klub.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Team : ITeam
    {
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        public string ActiveDay { get; set; } = string.Empty;

        [Required]
        public int TrainerId { get; set; }

        // Parameterless constructor
        public Team()
        {
            ID = 0;
            Name = string.Empty;
            Description = string.Empty;
            Capacity = 0;
            Price = 0;
            ActiveDay = string.Empty;
            TrainerId = 0;
        }

        // Full constructor without Trainer object
        public Team(int? id, string name, string description, int trainerId, int capacity, int price, string activeDay)
        {
            ID = id;
            Name = name;
            Description = description;
            TrainerId = trainerId;
            Capacity = capacity;
            Price = price;
            ActiveDay = activeDay;
        }
    }
}
