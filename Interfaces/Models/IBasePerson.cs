namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface IBasePerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Image { get; set; }
    }
}
