namespace MonolithApi.Interfaces
{
    public interface IGeneric<T>
    {
        /// <summary>
        /// Add a new object
        /// </summary>
        /// <param name="t">The object we want to add</param>
        /// <param name="UserId">The Id of the user who can do this action</param>
        /// <returns>The object</returns>
        Task<T> Post(string UserId, T t);

        /// <summary>
        /// Update an object
        /// </summary>
        /// <param name="id">Id of the object we want to update</param>
        /// <param name="UserId">The Id of the user who can do this action</param>
        /// <returns>A single address</returns>
        Task<T> Put(int id, string UserId, T t);

        /// <summary>
        /// Delete an object
        /// </summary>
        /// <param name="id">Id of the object we want to delete</param>
        /// <param name="UserId">The Id of the user who can do this action</param>
        Task Delete(int id, string UserId);

        /// <summary>
        /// Get an object
        /// </summary>
        /// <param name="id">Id of the object we want to retrieve</param>
        /// <returns>A single object</returns>
        Task<T> Get(int id);

        /// <summary>
        /// Get All objects
        /// </summary>
        /// <returns>A list of objects</returns>
        Task<IEnumerable<T>> GetAll();
    }
}
