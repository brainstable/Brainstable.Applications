namespace Brainstable.Applications.Dto;

public record ApplicationApplicatorDto
{
    public int ApplicationId { get; set; }
    public int ApplicatorId { get; set; }
    public int Priority { get; set; }
}