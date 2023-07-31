using AuthFlow.Domain.Entities;
using FluentAssertions;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class SearchUserRepositoryTests : BaseTests
    {
        private const string success = "The search in the User entity completed successfully.";
        private const string userTryingActiveDoesNotExist = "The User you are trying to active does not exist.";


        [Test]
        public async Task Given__SeventeenUsers_When_GetPageByUser_Then_SuccessResulTrueAndrea()
        {
            // Given
            await GetAllPeople();

            // When
            var result = await _userRepository.GetPageByFilter(0, 3, "Andrea");

            // Then

            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count().Should().Be(1);
            result.Data.FirstOrDefault().Username.Should().Be("Andrea");
        }

        [Test]
        public async Task Given__SeventeenUsers_When_GetPageByUser_Then_SuccessResulTrueThreeUsers()
        {
            // Given
            await GetAllPeople();

            // When
            var result = await _userRepository.GetPageByFilter(0, 3, string.Empty);

            // Then

            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count().Should().Be(3);
        }



        [Test]
        public async Task Given__SeventeenUsers_When_GetPageByUser_Then_SuccessResulTrueByEmail()
        {
            // Given
            await GetAllPeople();

            // When
            var result = await _userRepository.GetPageByFilter(0, 3, "ricardo.morales@email.com");

            // Then

            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count().Should().Be(1);
            result.Data.FirstOrDefault().Email.Should().Be("ricardo.morales@email.com");
        }


        [Test]
        public async Task Given__SeventeenUsers_When_GetPageByUser_Then_SuccessResulTrueByDaniela()
        {
            // Given
            await GetAllPeople();

            // When
            var result = await _userRepository.GetPageByFilter(0, 13, "Daniela");

            // Then

            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count().Should().Be(1);
            result.Data.FirstOrDefault().Email.Should().Be("daniela.martinez@email.com");
        }

        [Test]
        public async Task Given__SeventeenUsers_When_GetPageByUser_Then_SuccessResulTrueByRuiz()
        {
            // Given
            await GetAllPeople();

            // When
            var result = await _userRepository.GetPageByFilter(0, 13, "ruiz");

            // Then

            result?.Message.Should().Be(success);
            result.IsSuccessful.Should().BeTrue();
            result.Data.Count().Should().Be(3);
            result.Data.FirstOrDefault().Email.Should().Be("fernanda.ruiz@email.com");
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