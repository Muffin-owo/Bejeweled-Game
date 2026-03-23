public class Match
{
    
    public List<Position> Positions { get; set; }
    public GemColor MatchColor { get; set; }

    public int Length => Positions.Count;

    public Match(List<Position> positions, GemColor color)
    {
        Positions = positions;
        MatchColor = color;
    }
}