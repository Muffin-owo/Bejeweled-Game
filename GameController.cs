public class GameController
{
    private Board board;
    private ScoreManager score;
    private bool isRunning;

    public void StartNewGame()
    {
        board = new Board();
        score = new ScoreManager();
        board.InitBoard();
        score.ResetScore();
        isRunning = true;
    }

    public bool PlayerMove(int r1, int c1, int r2, int c2)
    {
        Position pos1 = new Position(r1, c1);
        Position pos2 = new Position(r2, c2);

        if (board.TrySwap(pos1, pos2))
        {
            int cleared = board.ResolveBoard();
            score.AddPoints(cleared, 1);
            return true;
        }

        return false;
    }
    public bool IsRunning()
    {
        return isRunning;
    }

    public void CheckEndGame()
    {
        if (!board.HasAnyValidMove())
         Console.WriteLine("No moves left! Game Over.");
         isRunning = false;
    }

    public void PrintBoard()
    {
        board.PrintBoard();
    }

    public int GetScore()
    {
        return score.Score;
    }


}