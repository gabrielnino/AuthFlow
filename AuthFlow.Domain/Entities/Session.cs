using AuthFlow.Domain.Interfaces;

namespace AuthFlow.Domain.Entities
{
    public class Session : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
