using System;

public class Board
{
    private const int rows = 8;
    private const int cols = 8;
    private Gem[,] grid;
    private Random random = new Random();

    public Board()
    {
        grid = new Gem[rows, cols];
    }

    public void InitBoard()
    {
        for (int r = 0; r < rows ; r++)
        {
            for (int c = 0; c < cols ; c++)
            {
                do
                {
                    grid[r, c] = CreateRandomGem();
                } while (!CanPlaceGem(r, c, grid[r, c].Color));

            }
        }
    }

    private bool CanPlaceGem(int row, int col, GemColor color)
    {
        if(col >= 2)
        {
            if (grid[row, col - 1].Color == color && grid[row, col - 2].Color == color)
                return false;
        }
        if(row >= 2)
        {
            if (grid[row - 1, col].Color == color && grid[row - 2, col].Color == color)
                return false;
        }
        return true;
    }

    private Gem CreateRandomGem()
    {
        GemColor color = (GemColor)random.Next(0, 6);
        return new Gem(color);
    }

    public bool CheckAdjacent(Position pos1, Position pos2)
    {
        if(pos1.Row == pos2.Row && Math.Abs(pos1.Column - pos2.Column) == 1)
            return true;

        if(pos1.Column == pos2.Column && Math.Abs(pos1.Row - pos2.Row) == 1)
            return true;

        return false;
    }

    public void SwapGems(Position pos1, Position pos2)
    {
        Gem temp = grid[pos1.Row, pos1.Column];
        grid[pos1.Row, pos1.Column] = grid[pos2.Row, pos2.Column];
        grid[pos2.Row, pos2.Column] = temp;
    }



    public List<Match> FindAllMatches()
    {
        List<Match> matches = new List<Match>();
       
        FindHorizontalMatches(matches);
        FindVerticalMatches(matches);

        return matches;
    }

    private void FindHorizontalMatches(List<Match> matches)
    {
        for (int r = 0; r < rows; r++)
        {
            int count = 1;

            for (int c = 1; c < cols; c++)
            {
                if (grid[r, c].Color == grid[r, c - 1].Color)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        List<Position> positions = new List<Position>();
                        for (int i = 0; i < count; i++)
                        {
                            positions.Add(new Position(r, c - 1 - i));
                        }
                        matches.Add(new Match(positions, grid[r, c - 1].Color));
                    }
                    count = 1;
                }
            }
            if (count >= 3)
            {
                List<Position> positions = new List<Position>();
                for (int i = 0; i < count; i++)
                {
                    positions.Add(new Position(r, cols - 1 - i));
                }
                matches.Add(new Match(positions, grid[r, cols - 1].Color));
            }

        }
    }


    private void FindVerticalMatches(List<Match> matches)
    {
        for (int c = 0; c < cols; c++)
        {
            int count = 1;

            for (int r = 1; r < rows; r++)
            {
                if (grid[r, c].Color == grid[r - 1, c].Color)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        List<Position> positions = new List<Position>();
                        for (int i = 0; i < count; i++)
                        {
                            positions.Add(new Position(r - 1 - i, c));
                        }
                        matches.Add(new Match(positions, grid[r - 1, c].Color));
                    }
                    count = 1;
                }
            }
            if (count >= 3)
            {
                List<Position> positions = new List<Position>();
                for (int i = 0; i < count; i++)
                {
                    positions.Add(new Position(rows - 1 - i, c));
                }
                matches.Add(new Match(positions, grid[rows - 1, c].Color));
            }

        }
       
    }

    public int ClearMatches(List<Match> matches)
    {
        HashSet<Position> uniquePositions = new HashSet<Position>();

        foreach (var match in matches)
        {
            foreach (var pos in match.Positions)
            {
                uniquePositions.Add(pos);
            }
        }

        foreach (var position in uniquePositions)
        {
            grid[position.Row, position.Column] = null;
        }
        return uniquePositions.Count;

    }
    public void ApplyGravity()
    {
        for (int c = 0; c < cols; c++)
        {
            int writeRow = rows - 1;

            for (int r = rows - 1; r >= 0; r--)
            {
                if (grid[r, c] != null)
                {
                    grid[writeRow, c] = grid[r, c];

                    if (r != writeRow)
                    {
                        grid[r, c] = null;
                    }

                    writeRow--;
                }
            }
        }
    }


    public void RefillNewGems()
    {
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if (grid[r, c] == null)
                {
                    grid[r, c] = CreateRandomGem();
                }
            }
        }
    }

    public int ResolveBoard()
    {
         List<Match> matches = FindAllMatches();

         int totalCleared = 1;

         while (matches.Count > 0)
            {
                int cleared = ClearMatches(matches);
                totalCleared += cleared;

                ApplyGravity();
                RefillNewGems();

                matches = FindAllMatches();
            }
        return totalCleared;
    }


    public bool WouldCreateMatch(Position pos1, Position pos2)
    {
        SwapGems(pos1, pos2);
        List<Match> matches = FindAllMatches();
        bool hasMatch = matches.Count > 0;
        SwapGems(pos1, pos2);
        return hasMatch;
    }
    
    public bool TrySwap(Position pos1, Position pos2)
    {
        if (!CheckAdjacent(pos1, pos2))
            return false;

        if (!WouldCreateMatch(pos1, pos2))
            return false;

        SwapGems(pos1, pos2);
        ResolveBoard();
        return true;
    }

    public bool HasAnyValidMove()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Position pos = new Position(r, c);

                if (c < cols - 1)
                {
                    Position rightPos = new Position(r, c + 1);
                    if (WouldCreateMatch(pos, rightPos))
                        return true;
                }

                if (r < rows - 1)
                {
                    Position downPos = new Position(r + 1, c);
                    if (WouldCreateMatch(pos, downPos))
                        return true;
                }
            }
        }
        return false;
    }


    public void PrintBoard()
        {
            Console.WriteLine("Current Board:");
            Console.Write("  "); 
            for (int c = 0; c < cols; c++)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine();

            for (int r = 0; r < rows; r++)
            {
                
                Console.Write(r + " ");

                for (int c = 0; c < cols; c++)
                {
                    if (grid[r, c] == null)
                        Console.Write(". ");
                    else
                        Console.Write(grid[r, c].Color.ToString()[0] + " ");
                }

                Console.WriteLine();
            }
        }
}