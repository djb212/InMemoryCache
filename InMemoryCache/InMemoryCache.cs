namespace InMemoryCache
{
    /// <summary>
    /// An in-memory cache with a specified capacity. The least recently used item is removed when the capacity is exceeded.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <remarks>
    /// Initialises a new instance of the InMemoryCache class with a specified capacity.
    /// </remarks>
    /// <param name="capacity">Maximum number of items to be stored in cache</param>
    public class InMemoryCache<TKey, TValue>(int capacity) where TKey : notnull
    {
        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
        private LinkedList<TKey> _lruList = new LinkedList<TKey>();
        private int _capacity = capacity;

        /// <summary>
        /// Adds key-value pair to the cache. If key already exists, throws ArgumentException.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">Thrown when key already exists in cache</exception>
        public void Add(TKey key, TValue value)
        {
            lock (_lruList)
            {
                if (_dictionary.ContainsKey(key))
                {
                    throw new ArgumentException("Key already exists");
                }
                else
                {
                    _lruList.AddLast(key);
                    _dictionary[key] = value;
                    if (_dictionary.Count > _capacity)
                    {
                        if (_lruList.First != null)
                        {
                            var lruKey = _lruList.First.Value;
                            Console.WriteLine($"Cache capacity exceeded. Removing least recently used item with key: {lruKey}");
                            _lruList.RemoveFirst();
                            _dictionary.Remove(lruKey);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the value of a specified key and marks it as recently used.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Value of key</returns>
        /// <exception cref="KeyNotFoundException">Thrown when key is not present in cache</exception>
        public TValue Retrieve(TKey key)
        {
            lock (_lruList)
            {
                if (_dictionary.TryGetValue(key, out var value))
                {
                    _lruList.Remove(key);
                    _lruList.AddLast(key);
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException("Key not found");
                }
            }
        }
    }
}
