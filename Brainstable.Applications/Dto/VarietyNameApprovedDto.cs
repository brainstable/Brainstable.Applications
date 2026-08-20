namespace Brainstable.Applications.Dto;

public record VarietyNameApprovedDto
{
    public int VarietyId { get; set; }
    public string ApprovedName { get; set; }
    public DateTime? DateApproved { get; set; }
    public DateTime DateChanged { get; set; }
}