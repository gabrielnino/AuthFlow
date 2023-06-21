// Namespace holding all the domain interfaces
namespace AuthFlow.Domain.Interfaces
{
    // IEntity is an interface that should be implemented by all domain entities.
    // It provides a standard structure for identifying and tracking the state of entities.
    public interface IEntity
    {
        // The unique identifier of an entity. 
        // This property should be used to uniquely identify each instance of an implementing class.
        int Id { get; set; }

        // A boolean value indicating whether the entity is currently active or not.
        // Implementing classes should use this property to track the active status of an entity.
        public bool Active { get; set; }
    }
}
