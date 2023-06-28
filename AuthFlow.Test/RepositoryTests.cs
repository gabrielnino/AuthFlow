using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthFlow.Domain.Entities;
using AuthFlow.Domain.Interfaces;
using AuthFlow.Infraestructure.Repositories;
using AuthFlow.Persistence.Data;
using AuthFlow.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AuthFlow.Persistence.Tests.Repositories
{
    [TestFixture]
    public class RepositoryTests
    {
        private DbContextOptions<AuthFlowDbContext> _options;

        [SetUp]
        public void Setup() => _options = new DbContextOptionsBuilder<AuthFlowDbContext>()
                .UseInMemoryDatabase(databaseName: "testdb")
                .Options;

        [Test]
        public async Task GivenExistingEntities_WhenGetAllCalled_ThenReturnsAllEntities()
        {
            // Given
            var entities = new List<User>
            {
                new User { Id = 1 },
                new User { Id = 2 },
                new User { Id = 3 }
            };

            using (var context = new AuthFlowDbContext(_options))
            {
                context.Users.AddRange(entities);
                context.SaveChanges();
            }

            using (var context = new AuthFlowDbContext(_options))
            {
                var repository = new UsersRepository(context); // Use concrete UserRepository implementation

                // When
                var result = await repository.GetAll();

                // Then
                Assert.AreEqual(entities.Count, result?.Data.Count());
            }
        }

        [Test]
        public async Task GivenPredicate_WhenGetAllByFilterCalled_ThenReturnsMatchingEntities()
        {
            // Given
            var entities = new List<User>
            {
                new User
                {
                    Id = 1, Username = "username1",
                    Password = "password1",
                    CreatedAt = DateTime.Now,
                    UpdatedAt =  DateTime.Now,
                    Email = "email1@withoutemail.com",
                    Active = true,
                },
                new User
                {
                    Id = 2, Username = "username2",
                    Password = "password2",
                    CreatedAt = DateTime.Now,
                    UpdatedAt =  DateTime.Now,
                    Email = "email2@withoutemail.com",
                    Active = true,
                },
                new User
                {
                    Id = 3, Username = "username3",
                    Password = "password3",
                    CreatedAt = DateTime.Now,
                    UpdatedAt =  DateTime.Now,
                    Email = "email3@withoutemail.com",
                    Active = false,
                }
            };

            using (var context = new AuthFlowDbContext(_options))
            {
                context.Users.AddRange(entities);
                context.SaveChanges();
            }

            using (var context = new AuthFlowDbContext(_options))
            {
                var repository = new UsersRepository(context); // Use concrete UserRepository implementation

                // When
                var result = await repository.GetAllByFilter(u => u.Active.Equals(true));

                // Then
                Assert.AreEqual(2, result?.Data.Count());
            }
        }

        [Test]
        public async Task GivenNewEntity_WhenAddCalled_ThenEntityIsAddedToDatabase()
        {
            // Given
            var entity = new User 
            { 
                Id = 1, Username = "username", 
                Password = "password", 
                CreatedAt = DateTime.Now, 
                UpdatedAt =  DateTime.Now, 
                Email = "email@withoutemail.com" 
            };

            using (var context = new AuthFlowDbContext(_options))
            {
                var repository = new UsersRepository(context); // Use concrete UserRepository implementation

                // When
                var result = await repository.Add(entity);

                // Then
                Assert.Greater(result?.Data, 0);
                Assert.IsNotNull(context.Users.Find(result?.Data));
            }
        }

        [Test]
        public async Task GivenExistingEntity_WhenModifiedCalled_ThenEntityIsModifiedInDatabase()
        {
            // Given
            var entity = new User { Id = 1, Username = "user1" };

            using (var context = new AuthFlowDbContext(_options))
            {
                context.Users.Add(entity);
                context.SaveChanges();
            }

            using (var context = new AuthFlowDbContext(_options))
            {
                var repository = new UsersRepository(context); // Use concrete UserRepository implementation

                // When
                entity.Username = "modified";
                var result = await repository.Modified(entity);

                // Then
                //Assert.IsTrue(result);
                Assert.AreEqual("modified", context.Users.Find(entity.Id).Username);
            }
        }

        [Test]
        public async Task GivenExistingEntity_WhenRemoveCalled_ThenEntityIsRemovedFromDatabase()
        {
            // Given
            var entity = new User { Id = 1 };

            using (var context = new AuthFlowDbContext(_options))
            {
                context.Users.Add(entity);
                context.SaveChanges();
            }

            using (var context = new AuthFlowDbContext(_options))
            {
                var repository = new UsersRepository(context); // Use concrete UserRepository implementation

                // When
                var result = await repository.Remove(entity);

                // Then
                Assert.IsTrue(result);
                Assert.IsNull(context.Users.Find(entity.Id));
            }
        }
    }
}
