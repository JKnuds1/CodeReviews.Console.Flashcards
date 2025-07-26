using System;

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
