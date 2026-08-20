namespace Brainstable.Applications.Models;

public class Bulletin
{
    public int NumberInYear { get; set; }
    public int Number { get; set; }
    public int Year { get; set; }
    public DateTime FirstDate { get; set; }
    public DateTime LastDate { get; set; }
    public int BeginPage { get; set; }
}