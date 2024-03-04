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

                if (parts != null && parts.Length == 2 
                                  && int.TryParse(parts[0], out int row) 
                                  && int.TryParse(parts[1], out int col))
                {
                    return (row, col);
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }
        }
    }
}