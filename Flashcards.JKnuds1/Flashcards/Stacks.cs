using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Spectre.Console;

namespace Flashcards
{
    static class Stacks
    {
        internal static string GoBackMessage = "Go back...";
        internal static bool CheckNameInStack(string nameInput)
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();

                var duplicateList = connection.Query("SELECT * FROM Stack WHERE Name=@Name", new { Name = nameInput }).ToList();
                bool notDuplicate = !duplicateList.Any();
                return notDuplicate;
            }
        }
        internal static void ViewStack()
        {
            List<string> stacks = GetStack();
            var table = new Table();

            table.AddColumn(" ");
            table.AddColumn(new TableColumn("[bold]Stack Names[/]").Centered());
            table.Border = TableBorder.SimpleHeavy;
            table.AddRow(" ");
            foreach (string stack in stacks)
            {
                table.AddRow("-", stack);
            }
            AnsiConsole.Write(table);
            Console.WriteLine("Press Enter to go back to the menu...");
            Console.ReadLine();
            Console.Clear();
        }


        internal static void AddStack()
        {
            Console.WriteLine("Insert the name of the stack.\n");
            string name = Console.ReadLine();

            while (!CheckNameInStack(name) || name.Trim() == "")
            {
                Console.WriteLine("Input is either a duplicate or empty.\nPlease try again...\n");
                name = Console.ReadLine();
            }

            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();
                connection.Execute("INSERT INTO Stack (Name) VALUES (@Name)", new { Name = name });


                connection.Close();
            }
            Console.Clear();

        }

        internal static void DeleteStack()
        {

            string deleteName = ChooseStack();
            if (deleteName != GoBackMessage)
            {
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
                {
                    connection.Open();
                    connection.Execute("DELETE FROM Stack WHERE Name = @Name", new { Name = deleteName });

                    connection.Close();
                }
            }


        }
        internal static List<string> GetStack()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();

                List<string> ListofStacks = connection.Query<string>("SELECT * FROM Stack").ToList();
                return ListofStacks;
            }

        }
        internal static string ChooseStack()
        {
            var StackChoices = GetStack();
            StackChoices.Add(GoBackMessage);
            string stack = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose One Stack or Go back to menu.")
            .AddChoices(StackChoices));
            return stack;

        }

    }
}
