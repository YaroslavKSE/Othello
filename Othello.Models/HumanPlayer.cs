namespace Othello.Models
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(CellState color) : base(color) { }

        public override (int, int) MakeMove(Board board)
        {
            // Implementation would involve getting user input for the move
            // For demonstration purposes, let's return a dummy value
            // In a real application, you would get input from the console or UI

            Console.WriteLine($"Player {Color}, enter your move as 'row col': ");
            var input = Console.ReadLine().Split();
            var row = int.Parse(input[0]);
            var col = int.Parse(input[1]);

            return (row, col);
        }
    }
}