using Spectre.Console;

namespace Flashcards
{
    class Menu
    {
        internal static void menu()
        {
            bool run = true;

            while (run)
            {

                var menuChoices = new string[] { "Stacks", "Flashcards", "Study Session", "Quit" };
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .AddChoices(menuChoices));

                switch (choice)
                {
                    case "Stacks":
                        stacksMenu();
                        break;
                    case "Flashcards":
                        flashcardMenu();
                        break;
                    case "Study Session":
                        studyMenu();
                        break;
                    case "Quit":
                        run = false;
                        break;

                }
            }

        }

        internal static void flashcardMenu()
        {
            bool flashcardRun = true;

            while (flashcardRun)
            {
                var menuChoices = new string[] { "View Flashcards", "Add Flashcards", "Delete Flashcards", "Update Flashcards", "Go back to Main menu" };
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Flashcard Menu")
                    .AddChoices(menuChoices));

                switch (choice)
                {
                    case "View Flashcards":
                        FlashCards.ViewFlashcard();
                        break;
                    case "Add Flashcards":
                        FlashCards.AddFlashcard();
                        break;
                    case "Delete Flashcards":
                        FlashCards.DeleteFlashCard();
                        break;
                    case "Update Flashcards":
                        FlashCards.UpdateFlashcard();
                        break;
                    case "Go back to Main menu":
                        flashcardRun = false;
                        break;


                }
            }
        }
        internal static void stacksMenu()
        {
            bool stackRun = true;
            while (stackRun)
            {
                var menuChoices = new string[] { "View Stacks", "Add Stack", "Delete Stack", "Go back to Main menu" };
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Stacks Menu")
                    .AddChoices(menuChoices));

                switch (choice)
                {
                    case "View Stacks":
                        Stacks.ViewStack();
                        break;
                    case "Add Stack":
                        Stacks.AddStack();
                        break;
                    case "Delete Stack":
                        Stacks.DeleteStack();
                        break;
                    case "Go back to Main menu":
                        stackRun = false;
                        break;

                }
            }
        }
        internal static void studyMenu()
        {
            bool studyRun = true;

            while (studyRun)
            {

                var menuChoices = new string[] { "Study a Stack", "View Previous studies", "Go back to Main Menu" };
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Study Menu")
                    .AddChoices(menuChoices));

                switch (choice)
                {
                    case "Study a Stack":
                        StudySession.Study();
                        break;
                    case "View Previous studies":
                        StudySession.ViewStudySessions();
                        break;
                    case "Go back to Main Menu":
                        studyRun = false;
                        break;

                }
            }

        }
    }
}
