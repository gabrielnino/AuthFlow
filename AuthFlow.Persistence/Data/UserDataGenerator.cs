using System.Linq;

namespace AuthFlow.Persistence.Data
{
    using System.Collections.Immutable;
    using User = Domain.Entities.User;

    internal static class UserDataGenerator
    {
        private const string defaultPasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

        private static User CreateUser(string username, string passwordHash, string email, DateTime creationDateTime)
        {
            return new User
            {
                Username = username,
                Password = passwordHash,
                Email = email,
                CreatedAt = creationDateTime,
                UpdatedAt = creationDateTime,
                Active = true
            };
        }

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

        private static string Truncate(this string value, int maxLength) =>
           string.IsNullOrEmpty(value) ? value : value.Length <= maxLength ? value : value[..maxLength];

        public static ImmutableList<User> GetUsers()
        {
            var utcNow = DateTime.UtcNow;
            var usernameAnonymous = "usernameanonymous";
            var anonymousPassword = "72b28030ce99fa4d0ab678f1d4a4374cc0d7bb676eb4307b0fa105f4c66b644e";
            var users = new (string Username, string Email)[]
            {
                (usernameAnonymous, "user.anonymous@withoutemail.com"),
                ("luis.nino", "luis.nino@email.com" ),
                ("maria.perez", "maria.perez@email.com" ),
                ("juan.gomez", "juan.gomez@email.com" ),
                ( "ana.sanchez", "ana.sanchez@email.com" ),
                ("pedro.rodriguez", "pedro.rodriguez@email.com" ),
                ("pablo.martinez", "pablo.martinez@email.com" ),
                ("sara.torres", "sara.torres@email.com" ),
                ("lucas.fernandez", "lucas.fernandez@email.com" ),
                ("marina.castro",  "marina.castro@email.com" ),
                ("diego.cortez", "diego.cortez@email.com" ),
                ("eva.gonzalez", "eva.gonzalez@email.com" ),
                ("andres.morales", "andres.morales@email.com" ),
                ("irene.gil", "irene.gil@email.com" ),
            };
            return (from user in users
                       let password = user.Equals(usernameAnonymous) ? anonymousPassword : defaultPasswordHash
                       select CreateUser(user.Username, password, user.Email, utcNow)).ToImmutableList();

        }

        public static ImmutableList<User> GenerateMassiveUserList(
            int sizeFirstNames = 300,
            int sizeMiddleName = 10,
            int sizeLastname = 10,
            int sizeSecondlastName = 10)
        {
            var utcNow = DateTime.UtcNow;
            var users = new List<User>();
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
                   select CreateUser(name, defaultPasswordHash, email, utcNow)).ToImmutableList();
        }
    }
}
