namespace Brainstable.Applications.Models;

public class Bulletin
{
    public string NumberInYear { get; set; }
    public string Number { get; set; }
    public string Year { get; set; }
    public DateTime FirstDate { get; set; }
    public DateTime LastDate { get; set; }
    public int BeginPage { get; set; }
}