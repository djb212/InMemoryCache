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
        public void Add_Deletes_Least_Recently_Used_When_Over_Capacity()
        {
            // Act
            inMemoryCache.Add(4, "four");

            // Assert
            Assert.Throws<KeyNotFoundException>(() => inMemoryCache.Retrieve(1));
        }

        [Test]
        public void Add_Does_Not_Delete_Recently_Retrieved_Item()
        {
            // Act
            inMemoryCache.Retrieve(1);
            inMemoryCache.Add(4, "four");
            var result = inMemoryCache.Retrieve(1);

            // Assert
            Assert.That(result, Is.EqualTo("one"));
            Assert.Throws<KeyNotFoundException>(() => inMemoryCache.Retrieve(2));
        }

        [Test]
        public void Add_Throws_When_Duplicate_Key()
        {
            // Act + Assert
            Assert.Throws<ArgumentException>(() => inMemoryCache.Add(1,"foo"));
        }

        [Test]
        public void TryAdd_Returns_False_When_Duplicate_Key()
        {
            // Act 
            var result = inMemoryCache.TryAdd(1, "foo");
            
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryAdd_Returns_True_When_Successful()
        {
            // Act 
            var result = inMemoryCache.TryAdd(4, "four");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TryRetrieve_Returns_True_When_Successful()
        {
            // Act 
            var result = inMemoryCache.TryRetrieve(1, out var value);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void TryRetrieve_Returns_False_When_Key_Not_Found()
        {
            // Act 
            var result = inMemoryCache.TryRetrieve(4, out var value);
            
            // Assert
            Assert.That(result, Is.False);
            Assert.That(value, Is.Null);
        }

        [Test]
        public void ContainsKey_Returns_True_When_Key_Exists()
        {
            // Act 
            var result = inMemoryCache.ContainsKey(1);
            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ContainsKey_Returns_False_When_Key_Does_Not_Exist()
        {
            // Act 
            var result = inMemoryCache.ContainsKey(4);
            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Count_Returns_Number_Of_Items_In_Cache()
        {
            // Act 
            var result = inMemoryCache.Count;

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }
    }
}