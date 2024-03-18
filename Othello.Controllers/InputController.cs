using Othello.Controllers.Interfaces;
using Othello.Models;

namespace Othello.Controllers
{
    public class InputController : IConsoleInputController
    {
        public async Task<(int, int)> GetMoveInputAsync()
        {
            var inputTask = Task.Run(Console.ReadLine);
            var delayTask = Task.Delay(TimeSpan.FromSeconds(5));
            var completedTask = await Task.WhenAny(inputTask, delayTask);

            if (completedTask == inputTask)
            {
                string? input = await inputTask; // This is safe now.

                if (string.Equals(input?.Trim(), "hint", StringComparison.OrdinalIgnoreCase))
                {
                    throw new HintRequestedException();
                }

                var parts = input?.Split();
                if (parts is {Length: 2}
                    && int.TryParse(parts[0], out int row)
                    && int.TryParse(parts[1], out int col))
                {
                    return (row, col); // Adjusting for zero-based indexing is done by the caller.
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                    return await GetMoveInputAsync(); // Recursive call for retry
                }
            }
            else
            {
                throw new MoveTimeoutException();
            }

        }

        public string GetGameModeInput()
        {
            Console.WriteLine("Select game mode: \n1. Player vs Player (PvP)\n2. Player vs Bot (PvE)");
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "1" || input == "2")
                {
                    return input;
                }

                Console.WriteLine("Invalid input, please select 1 or 2.");
            }
        }

        public bool AskPlayAgain()
        {
            Console.WriteLine("Do you want to play another game? (yes/no)");
            while (true)
            {
                var response = Console.ReadLine();
                if (response == null)
                {
                    Console.WriteLine("Invalid input, please select from (yes/no)");
                }
                else
                {
                    return response.Trim().StartsWith("y", StringComparison.CurrentCultureIgnoreCase);
                }
            }
        }
    }
    public class HintRequestedException : Exception { }
    public class MoveTimeoutException : Exception { }
}