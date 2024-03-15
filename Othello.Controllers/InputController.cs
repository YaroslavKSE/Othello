using System.Text;
using Othello.Controllers.Interfaces;

namespace Othello.Controllers
{
    public class InputController : IConsoleInputController
    {
        public (int, int) GetMoveInput()
        {
            while (true)
            {
                //Console.WriteLine("Enter your move (row col): ");
                var input = Console.ReadLine();
                var parts = input?.Split();

                if (parts is {Length: 2}
                    && int.TryParse(parts[0], out int row)
                    && int.TryParse(parts[1], out int col))
                {
                    return (row, col);
                }

                Console.WriteLine("Invalid input, please try again.");
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
}