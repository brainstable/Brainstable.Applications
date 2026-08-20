namespace Brainstable.Applications.Dto;

public record ApplicationDto
{
    public int Id { get; set; }
    public int VarietyId { get; set; }
    public int Type { get; set; }
    public DateTime DateRegistration { get; set; }
    public DateTime? DateRejection { get; set; }
    public int? CauseRejection { get; set; }
    public DateTime? DateExpertise { get; set; }
    public int ApplicaitorId { get; set; }
    public DateTime? DateInclusion { get; set; }
    public int? NumberCaudeAdguardEnd { get; set; }
}