using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database();
            Database.CreateStackTable();
            Database.CreateFlashCardTable();
            Database.CreateStudySessionTable();

            Menu.menu();
            
        }
    }
}
