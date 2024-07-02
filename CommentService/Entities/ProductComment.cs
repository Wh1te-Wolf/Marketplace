using Core.Interfaces;

namespace CommentService.Entities
{
    public class ProductComment : IEntity
    {
        public Guid UUID { get; set; }

        public Guid CustomerUUID { get; set; }

        public DateTime DateTime { get; set; }

        public int Points { get; set; }

        public string Description { get; set; }
    }
}
