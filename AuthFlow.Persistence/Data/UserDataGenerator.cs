namespace AuthFlow.Persistence.Data
{
    using AuthFlow.Persistence.Data.Interface;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Immutable;
    using User = Domain.Entities.User;

    public class UserDataGenerator : IUserDataGenerator
    {
        private const string defaultPasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
        private static readonly ImmutableList<string> FirstNames = ImmutableList.Create
        (
            "Michael", "Sarah", "Jessica", "Jacob", "Mohamed", "Emily", "Joshua", "Amanda", "Andrew", "David",
            "Ashley", "Brian", "Jennifer", "Daniel", "Nicole", "Matthew", "Samantha", "Christopher", "Emma", "Anthony",
            "Michelle", "Justin", "Megan", "Ryan", "Hannah", "William", "Stephanie", "Robert", "Elizabeth", "Joseph",
            "Lauren", "James", "Alicia", "John", "Katherine", "Nicholas", "Rachel", "Kyle", "Rebecca", "Brandon",
            "Anna", "Jonathan", "Danielle", "Tyler", "Grace", "Kevin", "Natalie", "Eric", "Sara", "Alex",
            "Victoria", "Adam", "Madison", "Dylan", "Sophie", "Ben", "Zoe", "Jake", "Julia", "Lucas",
            "Amy", "Sam", "Jasmine", "Max", "Hailey", "Noah", "Eleanor", "Logan", "Melissa", "Jack",
            "Emily", "Isaac", "Lily", "Aidan", "Chloe", "Luke", "Olivia", "Evan", "Charlotte", "Caleb",
            "Abigail", "Connor", "Scarlett", "Mason", "Ella", "Owen", "Amelia", "Liam", "Ava", "Ethan",
            "Harper", "Oliver", "Aria", "Henry", "Riley", "Sebastian", "Addison", "Gabriel", "Aubrey", "Levi"
        );

        private static readonly ImmutableList<string> Middlenames = ImmutableList.Create
        (
            "Aaron", "Patricia", "Jordan", "Brittany", "Peter", "Megan", "Timothy", "Danielle", "Gregory", "Chelsea",
            "Jared", "Amber", "Austin", "Christine", "Cody", "Michelle", "Bryan", "Alicia", "Lucas", "Samantha",
            "Alex", "Allison", "Adrian", "Lori", "Julian", "Erin", "Alan", "Alexis", "Victor", "Madison",
            "Brent", "Crystal", "Dennis", "Heather", "Philip", "Melanie", "Travis", "Courtney", "Spencer", "Alyssa",
            "Brad", "Britney", "Mitchell", "Katie", "Vincent", "Kelly", "Oscar", "Kimberly", "Darryl", "Rebecca",
            "Wayne", "Vanessa", "Corey", "Adriana", "Keith", "Veronica", "Derek", "Savannah", "Johnny", "Jillian",
            "Angel", "Brenda", "Ethan", "Dana", "Shane", "Kathryn", "Brett", "Rachael", "Glenn", "Denise",
            "Trevor", "Tara", "Seth", "Yvonne", "Ray", "Theresa", "Elijah", "Caroline", "Terrence", "Pamela",
            "Leroy", "April", "Wesley", "Rose", "Jimmy", "Joanna", "Lonnie", "Sheila", "Geoffrey", "Kendra",
            "Andre", "Whitney", "Luis", "Sophia", "Gene", "Evelyn", "Zachary", "Molly", "Duane", "Cynthia",
            "Damon", "Stacey", "Darren", "Jamie", "Dean", "Linda", "Dustin", "Olga", "Fernando", "Leslie"
        );

        private static readonly ImmutableList<string> Lastnames = ImmutableList.Create
        (
            "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
            "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson",
            "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King",
            "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter",
            "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins",
            "Stewart", "Sanchez", "Morris", "Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey",
            "Rivera", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez",
            "James", "Watson", "Brooks", "Kelly", "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross",
            "Henderson", "Coleman", "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington",
            "Butler", "Simmons", "Foster", "Gonzales", "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes"
        );

        private static readonly ImmutableList<string> SecondLastnames = ImmutableList.Create
        (
            "Myers", "Ford", "Hamilton", "Graham", "Sullivan", "Wallace", "Woods", "Cole", "West", "Jordan",
            "Owens", "Reynolds", "Fisher", "Ellis", "Harrison", "Gibson", "McDonald", "Cruz", "Marshall", "Ortiz",
            "Gomez", "Murray", "Freeman", "Wells", "Webb", "Simpson", "Stevens", "Tucker", "Porter", "Hunter",
            "Hicks", "Crawford", "Henry", "Boyd", "Mason", "Morales", "Kennedy", "Warren", "Dixon", "Ramos",
            "Reyes", "Burns", "Gordon", "Shaw", "Holmes", "Rice", "Robertson", "Hunt", "Black", "Daniels",
            "Palmer", "Mills", "Nichols", "Grant", "Knight", "Ferguson", "Rose", "Stone", "Hawkins", "Dunn",
            "Perkins", "Hudson", "Spencer", "Gardner", "Stephens", "Payne", "Pierce", "Berry", "Matthews", "Arnold",
            "Wagner", "Willis", "Ray", "Watkins", "Olson", "Carroll", "Duncan", "Snyder", "Hart", "Cunningham",
            "Bradley", "Lane", "Andrews", "Ruiz", "Harper", "Fox", "Riley", "Armstrong", "Carpenter", "Weaver",
            "Greene", "Lawrence", "Elliott", "Chavez", "Sims", "Austin", "Peters", "Kelley", "Franklin", "Lawson"
        );

        private IUserFactory _userFactory;
        private readonly IConfiguration _configuration;
        private readonly string _username;
        private readonly string _password;
        private readonly string _email;
        
        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="userFactory">The user factory.</param>
        public UserDataGenerator(IUserFactory userFactory, IConfiguration configuration)
        {
            _userFactory = userFactory;
            _configuration = configuration;
            _username = _configuration.GetSection("anonymous:username").Value ?? string.Empty;
            _password = _configuration.GetSection("anonymous:password").Value ?? string.Empty;
            _email = _configuration.GetSection("anonymous:email").Value ?? string.Empty;
        }

        /// <summary>
        /// Returns a list of predefined users.
        /// </summary>
        /// <returns>List of predefined users.</returns>
        public User GetUserAnonymous()
        {
            var utcNow = DateTime.UtcNow;
            return _userFactory.CreateUser(_username, _password, _email, utcNow);
        }

        /// <summary>
        /// Returns a list of predefined users.
        /// </summary>
        /// <returns>List of predefined users.</returns>
        public ImmutableList<User> GetUsers()
        {
            var utcNow = DateTime.UtcNow;
            var users = new User[]
            {
                this.GetUserAnonymous(),
                _userFactory.CreateUser("luis.nino", "luis.nino@email.com",defaultPasswordHash, utcNow),
                _userFactory.CreateUser("maria.perez", "maria.perez@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser("juan.gomez", "juan.gomez@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser( "ana.sanchez", "ana.sanchez@email.com",defaultPasswordHash, utcNow),
                _userFactory.CreateUser("pedro.rodriguez", "pedro.rodriguez@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser("pablo.martinez", "pablo.martinez@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser("sara.torres", "sara.torres@email.com",defaultPasswordHash, utcNow),
                _userFactory.CreateUser("lucas.fernandez", "lucas.fernandez@email.com",defaultPasswordHash, utcNow),
                _userFactory.CreateUser("marina.castro",  "marina.castro@email.com",defaultPasswordHash, utcNow),
                _userFactory.CreateUser("diego.cortez", "diego.cortez@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser("eva.gonzalez", "eva.gonzalez@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser("andres.morales", "andres.morales@email.com" ,defaultPasswordHash, utcNow),
                _userFactory.CreateUser("irene.gil", "irene.gil@email.com" ,defaultPasswordHash, utcNow)
            };

            return users.ToImmutableList();
        }

        /// <summary>
        /// Generates a massive list of users.
        /// </summary>
        /// <returns>List of predefined users.</returns>
        public ImmutableList<User> GenerateMassiveUserList(
            int sizeFirstNames = 300,
            int sizeMiddleName = 10,
            int sizeLastname = 10,
            int sizeSecondlastName = 10)
        {
            var utcNow = DateTime.UtcNow;
            var firstsNames = FirstNames.Take(sizeFirstNames);
            var middleNames = Middlenames.Take(sizeMiddleName);
            var lastNames = Lastnames.Take(sizeLastname);
            var secondLastnames = SecondLastnames.Take(sizeSecondlastName);
            return (from firstName in firstsNames
                    from middleName in middleNames
                    from lastName in lastNames
                    from secondLastname in secondLastnames
                    let name = $"{firstName}.{middleName}.{lastName}.{secondLastname}".Truncate(48)
                    let email = $"{name}@email.com"
                    select _userFactory.CreateUser(name, defaultPasswordHash, email, utcNow)).ToImmutableList();
        }
    }

    internal static class StringExtention
    {
        /// <summary>
        /// Truncates a string to a specified length.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="maxLength">The max length.</param>
        /// <returns></returns>
        internal static string Truncate(this string value, int maxLength) =>
           string.IsNullOrEmpty(value) ? value : value.Length <= maxLength ? value : value[..maxLength];
    }
}
