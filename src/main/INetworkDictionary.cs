namespace ei8.Cortex.Coding
{
    /// <summary>
    /// Represents a dictionary of Networks.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INetworkDictionary<T>
    {
        /// <summary>
        /// Tries to get a Network using the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns>True if successful, false if otherwise.</returns>
        bool TryGetById(T id, out Network value);

        /// <summary>
        /// Gets a Network using the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Network GetById(T id);

        /// <summary>
        /// Retrieves the Network that corresponds 
        /// to the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Network this[T id] { get; }

        /// <summary>
        /// Adds a network using the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        void Add(T id, Network value);
    }
}
