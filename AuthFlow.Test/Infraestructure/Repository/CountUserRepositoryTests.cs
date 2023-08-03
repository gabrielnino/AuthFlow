namespace AuthFlow.Test.Infraestructure.Repository
{
    using AuthFlow.Domain.Entities;
    using AuthFlow.Test.Infraestructure.Repository.BaseTest;
    using FluentAssertions;

    [TestFixture]
    public class CountUserRepositoryTests : BaseTests
    {
        private const string success = "The search in the User entity completed successfully.";

      
        [Test]
        public Task When_GetCountFilter_FilterAndrea_Then_Success()
        {
            // Given
             GetAllPeople();

            // When
            var result =  _userRepository.GetCountFilter("Andrea");

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeGreaterThan(0);
            return Task.CompletedTask;
        }
        
        [Test]
        public Task When_GetCountFilter_FilterStringEmpty_Then_Success()
        {
            // Given
            GetAllPeople();

            // When
            var result =  _userRepository.GetCountFilter(string.Empty);

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeGreaterThan(0);
            return Task.CompletedTask;
        }


        [Test]
        public Task When_GetCountFilter_FilterEmailAddress_Then_Success()
        {
            // Given
             GetAllPeople();

            // When
            var result = _userRepository.GetCountFilter( "ricardo.morales@email.com");

            // Then
            UtilTest<int>.Assert(result);
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeGreaterThan(0);
            return Task.CompletedTask;
        }


        [Test]
        public Task When_GetCountFilter_FilterDaniela_Then_Success()
        {
            // Given
             GetAllPeople();

            // When
            var result =  _userRepository.GetCountFilter("Daniela");

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeGreaterThan(0);
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test]
        public Task When_GetCountFilter_Filter_ruiz_Then_Success()
        {
            // Given
             GetAllPeople();

            // When
            var result = _userRepository.GetCountFilter("ruiz");

            // Then
            result.Should().NotBeNull();
            result.Result.Message.Should().Be(success);
            result.Result.IsSuccessful.Should().BeTrue();
            result.Result.Data.Should().BeGreaterThan(0);
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        private async Task GetAllPeople()
        {
            var user1 = await addNewUserByName("Andrea", "andrea.gomez@email.com");
            var user2 = await addNewUserByName("Javier", "javier.rodriguez@email.com");
            var user3 = await addNewUserByName("Daniela", "daniela.martinez@email.com");
            var user4 = await addNewUserByName("Miguel", "miguel.gutierrez@email.com");
            var user5 = await addNewUserByName("Alejandra", "alejandra.lopez@email.com");
            var user6 = await addNewUserByName("Adriana", "adriana.sanchez@email.com");
            var user7 = await addNewUserByName("Roberto", "roberto.gonzalez@email.com");
            var user8 = await addNewUserByName("Camila", "camila.ramirez@email.com");
            var user9 = await addNewUserByName("Santiago", "santiago.torres@email.com");
            var user10 = await addNewUserByName("Marcela", "marcela.perez@email.com");
            var user11 = await addNewUserByName("Ricardo", "ricardo.morales@email.com");
            var user12 = await addNewUserByName("Fernanda", "fernanda.ruiz@email.com");
            var user13 = await addNewUserByName("Leonardo", "leonardo.diaz@email.com");
            var user14 = await addNewUserByName("Cristina", "cristina.ruiz@email.com");
            var user15 = await addNewUserByName("Federico", "federico.vargas@email.com");
            var user16 = await addNewUserByName("Isabella", "isabella.ruiz@email.com");
            var user17 = await addNewUserByName("Antonio", "antonio.herrera@email.com");
        }

        private async Task<User> addNewUserByName(string userName, string email)
        {
            User user = GetUser(userName, email);
            var repo = await _userRepository.Add(user);
            user.Id = repo.Data;
            return user;
        }
    }
}