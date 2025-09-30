using InMemoryCache;

namespace InMemoryCacheTests
{
    public class Tests
    {
        InMemoryCache<int, string> inMemoryCache;

        [SetUp]
        public void Setup()
        {
            /// Arrange
            inMemoryCache = new InMemoryCache<int, string>(3);
            inMemoryCache.Add(1, "one");
            inMemoryCache.Add(2, "two");
            inMemoryCache.Add(3, "three");
        }

        [Test]
        public void Deletes_Least_Recently_Used_When_Over_Capacity()
        {
            // Act
            inMemoryCache.Add(4, "four");

            // Assert
            Assert.Throws<KeyNotFoundException>(() => inMemoryCache.Retrieve(1));
        }

        [Test]
        public void Does_Not_Delete_Recently_Retrieved_Item()
        {
            // Act
            inMemoryCache.Retrieve(1);
            inMemoryCache.Add(4, "four");

            // Assert
            Assert.That(inMemoryCache.Retrieve(1), Is.EqualTo("one"));
            Assert.Throws<KeyNotFoundException>(() => inMemoryCache.Retrieve(2));
        }

        [Test]
        public void Throws_When_Duplicate_Key()
        {
            // Act + Assert
            Assert.Throws<ArgumentException>(() => inMemoryCache.Add(1,"foo"));
        }
    }
}