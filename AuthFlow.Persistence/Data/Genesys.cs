using AuthFlow.Domain.Entities;

namespace AuthFlow.Persistence.Data
{
    // The Genesys class is an internal helper class used for generating initial user data.
    internal class Genesys
    {
        const string commonPassword = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
        // GetUsers method generates a list of User entities to be initially populated in the database.
        public static List<User> GetUsers()
        {
            // Common password for all initial users.
           

            // Define initial users
            var users = new List<User>
            {
                // The properties of each user are set here including the Username, Password, Email, CreatedAt, UpdatedAt, and Active status.
                new User { Username = "usernameanonymous", Password = "72b28030ce99fa4d0ab678f1d4a4374cc0d7bb676eb4307b0fa105f4c66b644e", Email = "user.anonymous@withoutemail.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = true },
                new User { Username = "luis.nino", Password = commonPassword, Email = "luis.nino@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "maria.perez", Password = commonPassword, Email = "maria.perez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "juan.gomez", Password = commonPassword, Email = "juan.gomez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "ana.sanchez", Password = commonPassword, Email = "ana.sanchez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "pedro.rodriguez", Password = commonPassword, Email = "pedro.rodriguez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "carla.mendez", Password = commonPassword, Email = "carla.mendez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "pablo.martinez", Password = commonPassword, Email = "pablo.martinez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "sara.torres", Password = commonPassword, Email = "sara.torres@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "lucas.fernandez", Password = commonPassword, Email = "lucas.fernandez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "marina.castro", Password = commonPassword, Email = "marina.castro@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "diego.cortez", Password = commonPassword, Email = "diego.cortez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "eva.gonzalez", Password = commonPassword, Email = "eva.gonzalez@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "andres.morales", Password = commonPassword, Email = "andres.morales@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                new User { Username = "irene.gil", Password = commonPassword, Email = "irene.gil@email.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Active = false },
                };
            // Return the list of initial users
            return users;
        }
        public static List<User> GetMasiveUsers()
        {
           var firstNames = new List<string>
            {
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
            };

            var middleNames = new List<string>
            {
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
            };

            var lastNames = new List<string>
            {
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
            };

            var secondLastNames = new List<string>
            {
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
            }.Take(10);

            var users = new List<User>();

            foreach (var first in firstNames)
            {
                foreach(var middleName in middleNames)
                {
                    foreach(var lastName in lastNames)
                    {
                        foreach (var secondLastname in secondLastNames)
                        {
                            var name = $"{first}.{middleName}.{lastName}.{secondLastname}";
                            name = name.Length > 50 ? name.Substring(0, 48) : name;
                            var user = new User
                            {
                                Username = name,
                                Password = commonPassword,
                                Email = $"{name}@withoutemail.com",
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                Active = true
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
    }


    }
