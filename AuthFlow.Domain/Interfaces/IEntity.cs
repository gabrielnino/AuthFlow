namespace AuthFlow.Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        public bool Active { get; set; }
    }
}
