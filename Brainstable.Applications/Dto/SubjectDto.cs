namespace Brainstable.Applications.Dto;

public record SubjectDto
{
    public int Id { get; set; }
    public int FgisId { get; set; }
    public string Name { get; set; }
    public string Inn { get; set; }
    public string Snils { get; set; }
}