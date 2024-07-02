using Core.Interfaces;

namespace UserService.Entities
{
    public class Customer : IEntity
    {
        public Guid UUID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
