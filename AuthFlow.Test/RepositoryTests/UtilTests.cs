using AuthFlow.Domain.Entities;

namespace AuthFlow.Test.RepositoryTests
{
    [TestFixture]
    public class UtilTests
    {

        public static string GetMaximumLength(int maximumLength, string value)
        {
            if (maximumLength > 0)
            {
                if (value.Length < maximumLength)
                {
                    value = value.PadRight(maximumLength +1, '_');
                }
            }

            return value;
        }

        public static string GetMinimumLength(int minimumLength, string value)
        {
            if (minimumLength > 0)
            {
                if (value.Length > minimumLength)
                {
                    value = value.Substring(0, minimumLength - 1);
                }
            }

            return value;
        }


        public static User GetUser(string name = "doe", int minimumLength = 0, int maximumLength = 0)
        {

            var userName = $"john.{name}";
            userName = GetValueModified(minimumLength, maximumLength, userName);

            var email = $"john.{name}@example.com";
            email = GetValueModified(minimumLength, maximumLength, email);

            var password = "password";
            password = GetValueModified(minimumLength, maximumLength, password);

            return new User
            {
                Username = userName,
                Email = email,
                Password = password
            };
        }

        public static string GetValueModified(int minimumLength, int maximumLength, string userName)
        {
            userName = GetMaximumLength(maximumLength, userName);
            userName = GetMinimumLength(minimumLength, userName);
            return userName;
        }
    }
}