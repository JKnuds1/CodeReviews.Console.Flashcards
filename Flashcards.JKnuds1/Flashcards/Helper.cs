using System;
using System.Collections.Generic;
using System.Configuration;

namespace Flashcards
{
    public static class Helper
    {
        private static Random rng = new Random();

        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                (list[n], list[k]) = (list[k], list[n]);
            }
            return list;
        }
    }
}
