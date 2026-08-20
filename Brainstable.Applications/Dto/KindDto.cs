namespace Brainstable.Applications.Dto;

public record KindDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LatineName { get; set; }
    public int BlockId { get; set; }
    public int? ClassId { get; set; }
}