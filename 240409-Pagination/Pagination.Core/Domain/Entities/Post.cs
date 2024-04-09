namespace FilteringPagination.Domain.Entities
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
