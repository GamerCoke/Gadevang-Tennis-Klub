﻿using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Activity : IActivity
    {
        public int ID { get; set; }
        public int EventID { get; set; }

        [Required(ErrorMessage = "Titel er påkrævet")]
        [StringLength(32, ErrorMessage = "Titel må ikke være mere end 32 karakterer lang")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Beskrivelse er påkrævet")]
        [StringLength(512, ErrorMessage = "Beskrivelse må ikke være mere end 512 karakterer lang")]
        public string Description { get; set; }

        public DateTime Start { get; set; }
        public TimeOnly End { get; set; }


        public Activity()
        {
            
        }
        public Activity(int eventId, string title, string description, DateTime start, TimeOnly end)
        {
            EventID = eventId;
            Title = title;
            Description = description;
            Start = start;
            End = end;
        }

        public Activity(int id, int eventId, string title, string description, DateTime start, TimeOnly end)
        {
            ID = id;
            EventID = eventId;
            Title = title;
            Description = description;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"ID: {ID}, EventID: {EventID}, Description: {Description}, Start: {Start}, End: {End}";
        }
    }
}
