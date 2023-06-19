using AuthFlow.Domain.Interfaces;

namespace AuthFlow.Domain.Entities
{
    public class AccessToken : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
