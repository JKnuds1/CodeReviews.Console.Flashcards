using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Spectre.Console;

namespace Flashcards
{
    static class FlashCards
    {
        internal static void AddFlashcard()
        {

            string front = UserInput.GetStringInput("Write the front of the flashcard:\n");
            string back = UserInput.GetStringInput("Write the back of the flashcard:\n");
            var stack = Stacks.ChooseStack();
            if (stack != Stacks.GoBackMessage)
            {
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
                {
                    connection.Open();

                    connection.Execute("INSERT INTO Flashcards (Front, Back, Stack) VALUES (@Front, @Back, @Stack)", new { Front = front, Back = back, Stack = stack });

                    connection.Close();
                }
            }
            Console.Clear();

        }
        internal static void UpdateFlashcard()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();
                string UpdateIndex = ChooseFlashCard("Select the [green] Card ID [/] you want to update:");
                if (UpdateIndex != Stacks.GoBackMessage)
                {
                    Console.WriteLine("Write new front for the flashcard.\n");
                    string front = Console.ReadLine();
                    Console.WriteLine("\nWrite new back for the flashcard.\n");
                    string back = Console.ReadLine();
                    connection.Execute("UPDATE Flashcards SET Front = @Front, Back = @Back WHERE Id=@Id", new { Front = front, Back = back, Id = UpdateIndex });
                }
                connection.Close();
            }
            Console.Clear();

        }
        internal static void ViewFlashcard()
        {
            int shownId = 1;
            List<Card> cards = GetFlashCards();
            if (cards.Any())
            {
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
                {
                    connection.Open();

                    var table = new Table();
                    table.AddColumn("Card ID");
                    table.AddColumn("Front");
                    table.AddColumn("Back");
                    foreach (Card card in cards)
                    {

                        table.AddRow(shownId.ToString(), card.Front, card.Back);
                        shownId++;
                    }

                    AnsiConsole.Write(new Panel(table).Header("Card List", Justify.Center));
                    Console.ReadLine();
                    connection.Close();
                }
            }
            Console.Clear();

        }

        internal static void DeleteFlashCard()
        {

            string deleteIndex = ChooseFlashCard("Select the [green]Card ID[/] you want to delete:");
            if (deleteIndex != Stacks.GoBackMessage)
            {
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
                {
                    connection.Open();
                    connection.Execute("DELETE FROM Flashcards WHERE Id=@Id", new { Id = deleteIndex });

                    connection.Close();
                }

            }
            Console.Clear();
        }

        internal static string ChooseFlashCard(string title)
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();
                List<Card> cards = GetFlashCards();
                if (!cards.Any())
                {
                    return Stacks.GoBackMessage;
                }
                else
                {
                    var table = new Table();
                    table.AddColumn("ID");
                    table.AddColumn("Front");
                    table.AddColumn("Back");
                    int shownId = 1;
                    foreach (Card card in cards)
                    {
                        table.AddRow(shownId.ToString(), card.Front, card.Back);
                        shownId++;
                    }
                    var cardId = cards.ConvertAll(c => c.Id);
                    List<string> shownIds = Enumerable.Range(1, cardId.Count).Select(i => i.ToString()).ToList();
                    shownIds.Add(Stacks.GoBackMessage);

                    AnsiConsole.Write(new Panel(table).Header("Card List", Justify.Center));

                    var selectId = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title(title)
                        .PageSize(3)
                        .AddChoices(shownIds));

                    if (selectId == Stacks.GoBackMessage)
                    {
                        return selectId;
                    }

                    int selectedId = Int32.Parse(selectId);
                    return cardId[selectedId - 1];
                }
            }
        }
        internal static List<Card> GetFlashCards()
        {
            string stack = Stacks.ChooseStack();
            if (stack == Stacks.GoBackMessage)
            {
                return new List<Card> { };
            }

            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();

                List<Card> cards = connection.Query<Card>("SELECT * FROM Flashcards WHERE Stack = @Stack", new { Stack = stack }).ToList();
                return cards;
            }
        }
    }

}
