using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class Trainer : BasePerson, ITrainer
    {
        public Trainer() : base() { }
        public Trainer(int id, string name, string phone, string email, string? image)
            : base(id, name, phone, email, image) { }
    }
}
