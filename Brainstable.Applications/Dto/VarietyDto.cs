namespace Brainstable.Applications.Dto;

public record VarietyDto
{
    public int  VarietyId { get; set; }
    public string VarietyName { get; set; }
    public string SelectionNumber { get; set; }
    public string LatineName { get; set; }
    public int KindId { get; set; }
}