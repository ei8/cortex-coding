using System.Collections.Generic;

namespace ei8.Cortex.Coding
{
    /// <summary>
    /// Represents a dictionary of Networks.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NetworkDictionary<T> : INetworkDictionary<T>
    {
        private readonly IDictionary<T, Network> internalDictionary;

        /// <summary>
        /// Constructs a NetworkDictionary.
        /// </summary>
        public NetworkDictionary() : this(new Dictionary<T, Network>()) { }

        /// <summary>
        /// Constructs a NetworkDictionary using 
        /// the specified Dictionary.
        /// </summary>
        /// <param name="internalDictionary"></param>
        public NetworkDictionary(IDictionary<T, Network> internalDictionary)
        {
            this.internalDictionary = internalDictionary;
        }

        /// <summary>
        /// Retrieves the Network that corresponds to the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Network this[T id] => this.GetById(id);

        /// <summary>
        /// Adds a network using the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void Add(T id, Network value) => this.internalDictionary.Add(id, value);

        /// <summary>
        /// Gets a Network using the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Network GetById(T id) => this.internalDictionary[id];

        /// <summary>
        /// Tries to get a Network using the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns>True if successful, false if otherwise.</returns>
        public bool TryGetById(T id, out Network value) => this.internalDictionary.TryGetValue(id, out value);
    }
}
