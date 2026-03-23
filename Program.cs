using System;

public class Program
{
    public static void Main(string[] args)
    {
        GameController game = new GameController();
        game.StartNewGame();

        while (game.IsRunning())
        {
            Console.WriteLine("\nCurrent Board:");
            game.PrintBoard();

            Console.WriteLine("\nEnter first gem row:");
            int r1;
            if (!int.TryParse(Console.ReadLine(), out r1))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            Console.WriteLine("Enter first gem column:");
            int c1;
            if (!int.TryParse(Console.ReadLine(), out c1))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            Console.WriteLine("Enter second gem row:");
            int r2;
            if (!int.TryParse(Console.ReadLine(), out r2))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            Console.WriteLine("Enter second gem column:");
            int c2;
            if (!int.TryParse(Console.ReadLine(), out c2))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            bool moved = game.PlayerMove(r1, c1, r2, c2);

            if (moved)
            {
                Console.WriteLine("Move successful!");
            }
            else
            {
                Console.WriteLine("Invalid move!");
            }

            Console.WriteLine("Score: " + game.GetScore());
        }
        Console.WriteLine("Final Score: " + game.GetScore());
    }
}