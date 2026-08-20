namespace Brainstable.Applications.Dto;

public record SubjectChangeDto
{
    public int ApplicationId { get; set; }
    public int TypeApplication { get; set; }
    public int VarietyId { get; set; }
    public int? OldSubjectId { get; set; }
    public int TypeSubject { get; set; }
    public int CauseChange { get; set; }
    public DateTime DateChanged { get; set; }
    public int? NewSubjectId { get; set; }
}