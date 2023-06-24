// Namespace holding all the domain interfaces
namespace AuthFlow.Domain.Interfaces
{
    /// <summary>
    /// IEntity is an interface that should be implemented by all domain entities.
    /// It provides a standard structure for identifying and tracking the state of entities.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of an entity. 
        /// This property should be used to uniquely identify each instance of an implementing class.
        /// This is typically used as a primary key in the context of a database.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether the entity is currently active or not.
        /// Implementing classes should use this property to track the active status of an entity.
        /// This can be used to soft delete entities or toggle their active status without deleting records.
        /// </summary>
        bool Active { get; set; }
    }
}
