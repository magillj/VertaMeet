using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VertaMeet.Models;

namespace VertaMeet.Domain.Tier
{
    public class UserService
    {
        /* Old Dummy Data 
        public static readonly int NUM_USERS = 100; // Dummy

        private static List<UserModel> Users; // Dummy

        private static Random random = new Random(); // For dummy functionality

        #region Create Dummy Data

        static UserService()
        {
            Users = new List<UserModel>();
            string[] names = GetRandomNames(NUM_USERS);

            for (int i = 0; i < names.Length; i++)
            {
                Users.Add(new UserModel() { Name = names[i], ID = i + 1, Email = "Dummy" + i + "@vertafore.com", Location = UserModel.LOCATION.Bothell });
            }
        }

        private static string[] GetRandomNames(int amount)
        {
            String[] firstNames = { "Adam", "Desmond", "Donald", "Evan", "Frank", "Greg", "Hal", "James", "Richard", "Kenneth", "Juan", "Tom",
                                    "Mary", "Linda", "Sharon", "Jennifer", "Amy", "Rebecca", "Dorothy", "Judy", "Ann", "Heather", "Megan", "Bella",
                                    "Samuel", "Fred", "Geralt", "Ronald", "Nathan", "Jim", "Bob", "Erik", "Derek", "Jeremy", "Zachary", "Sean",
                                    "Tiffany", "Lauren", "Victoria", "Danielle", "Alex", "Sophie", "Ashley", "Isabel", "Olivia", "Susan", "Nicole"};
            String[] lastNames = { "Smith", "Garcia", "Williams", "Clark", "Brown", "Martinez", "Wilson", "Adams", "Parker", "Hill", "Campbell",
                                   "Perez", "Turner", "Cox", "Ward", "Cooper", "Gray", "Chapman", "Wheeler", "Fox", "Lawrence", "Donaldson", "Young",
                                   "Baker", "Collins", "Nguyen", "Perez", "Morgan", "Bell", "Morris", "Rivera", "Reed", "King", "Moore", "Sims", 
                                   "Soto", "Salazar", "Lynch", "Watkins", "Watson", "Vargas", "McCoy", "Wolfe", "Bush", "Vaughn", "Moss", "Cross",
                                   "Blair", "Moran", "Loveless", "Doyle", "Fitzgerald", "Lee", "Li", "Page", "Muromoto", "Underwood", "Coley",
                                   "Hall", "Dizon", "Canady", "Foster", "Kauf", "LaRoche", "Weiss", "O'Connor", "Watson", "Albee", "McGee", "Dover"};

            string[] output = new string[amount];

            for (int i = 0; i < amount; i++)
            {
                output[i] = firstNames[random.Next(firstNames.Length)] + " " + lastNames[random.Next(lastNames.Length)];
            }

            return output;
        }

        #endregion

        #region Service Calls

        public static UserModel GetRandomUser()
        {
            return Users[random.Next(Users.Count)];
        }

        /// <summary>
        /// Returns num random, unique users. Throws ArgumentException if num is greater than the amount of users
        /// </summary>
        public static List<UserModel> GetRandomUsers(int num)
        {
            if (num > Users.Count)
            {
                throw new ArgumentException("num is greater than the total amount of users");
            }

            List<UserModel> output = new List<UserModel>();
            List<int> possibleIndexes = new List<int>(Users.Count);

            for (int i = 0; i < Users.Count; i++)
            {
                possibleIndexes.Add(i);
            }

            for (int j = 0; j < num; j++)
            {
                int randIndex = random.Next(possibleIndexes.Count);
                output.Add(Users[possibleIndexes[randIndex]]);
                possibleIndexes.RemoveAt(randIndex);
            }

            return output;
        }

        #endregion
         * */


    }
}