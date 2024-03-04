using Othello.Models.Interfaces;

namespace Othello.Views;

public class ConsoleView : IGameViewUpdater
{
    public void Update(string message)
    {
        Console.WriteLine(message);
    }

    // Additional methods to interact with the user can be added here
    // For example, displaying the board, showing error messages, etc.
}