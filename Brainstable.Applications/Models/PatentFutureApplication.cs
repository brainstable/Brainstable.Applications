namespace Brainstable.Applications.Models;

public class PatentFutureApplication
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public int ApplicationAdguardId { get; set; }
    public string ApplicatorIds { get; set; }
    public List<string> ApplicatorNames { get; set; } = [];
    public string AutorIds { get; set; }
    public List<string> AutorNames { get; set; } = [];
    public DateTime DateAdguardRegistration { get; set; }
    public DateTime? DateAdguardWithdrawal { get; set; }
    public DateTime? DateAdguardExpertise { get; set; }
}

// DTO для отображения в DataGridView