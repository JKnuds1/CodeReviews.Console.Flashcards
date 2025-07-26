using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    static class UserInput
    {

        internal static string GetStringInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}
