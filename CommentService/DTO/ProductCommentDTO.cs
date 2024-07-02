namespace CommentService.DTO
{
    public class ProductCommentDTO
    {
        public Guid UUID { get; set; }

        public Guid CustomerUUID { get; set; }

        public DateTime DateTime { get; set; }

        public int Points { get; set; }

        public string Description { get; set; }
    }
}
