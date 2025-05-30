﻿namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface IMember : IBasePerson
    {
        public string Sex { get; set; }
        public bool IsAdmin { get; set; }
        public string Bio { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public DateOnly Dob { get; set; }
    }
}
