using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Flashcards
{
    class StudySession
    {
        public static StudyScore StudyStack()
        {
            List<Card> cards = FlashCards.GetFlashCards();
            if (cards.Count > 0)
            {
                List<DTO> cardsDTO = ToDTO(cards);
                List<DTO> shuffledCards = Helper.Shuffle(cardsDTO);

                var evaluationChoices = new string[] { "Yes", "No" };
                int score = 0;
                int totalScore = shuffledCards.Count;

                foreach (DTO card in shuffledCards)
                {
                    Console.WriteLine($"{card.Front}");
                    Console.ReadLine();
                    Console.WriteLine($"{card.Back}");

                    var correct = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Where you correct?")
                        .AddChoices(evaluationChoices));
                    if (correct == "Yes")
                    {
                        score++;
                    }
                    Console.Clear();
                }

                Console.WriteLine($"Your score was {score} out of {totalScore}");
                var stack = cards[0].Stack;
                return new StudyScore { Date = DateTime.Now, Score = score, Stack = stack };
            }
            else
            {
                Console.Clear();
                return new StudyScore { Date = DateTime.Now, Score = 0, Stack = " " };
            }

        }

        public static void Study()
        {
            StudyScore result = StudyStack();
            if (result.Stack != " ")
            {
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
                {
                    connection.Open();

                    connection.Execute("INSERT INTO StudySession (Date, Score, Stack) VALUES (@Date, @Score, @Stack)", new { Date = result.Date, Score = result.Score, Stack = result.Stack });

                    connection.Close();
                }
            }


        }
        internal static List<StudyScore> GetStudySessions()
        {
            string stack = Stacks.ChooseStack();
            if (stack == Stacks.GoBackMessage)
            {
                return new List<StudyScore> { };
            }

            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
            {
                connection.Open();

                List<StudyScore> sessions = connection.Query<StudyScore>("SELECT * FROM StudySession WHERE Stack = @Stack", new { Stack = stack }).ToList();
                return sessions;
            }
        }
        internal static void ViewStudySessions()
        {
            List<StudyScore> studySessions = GetStudySessions();

            if (studySessions.Any())
            {
                using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString("FlashCardDB")))
                {
                    connection.Open();
                    var table = new Table();
                    table.AddColumn("Date");
                    table.AddColumn("Score");

                    foreach (StudyScore session in studySessions)
                    {
                        table.AddRow(session.Date.ToString(), session.Score.ToString());
                    }

                    AnsiConsole.Write(new Panel(table).Header("Study Score", Justify.Center));
                    Console.ReadLine();
                    connection.Close();
                }
            }
            Console.Clear();
        }
        public static List<DTO> ToDTO(List<Card> cards)
        {
            return cards.Select(c => new DTO
            {
                Front = c.Front,
                Back = c.Back
            }).ToList();
        }


    }

}

