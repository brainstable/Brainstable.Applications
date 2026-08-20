namespace Brainstable.Applications.Dto;

public class PatentDto
{
    public int VarietyId { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public DateTime? DateCalculateEnd { get; set; }
    public int NumberPatent { get; set; }
}