namespace Brainstable.Applications.Models;

public class PatentFutureApplicationView
{
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public string ApplicatorIds { get; set; }
    public string AutorIds { get; set; }
    public DateTime DateAdguardRegistration { get; set; }
    public DateTime? DateAdguardWithdrawal { get; set; }
    public DateTime? DateAdguardExpertise { get; set; }

    // Преобразование
    public static List<PatentFutureApplicationView> ToViewList(List<PatentFutureApplication> patents)
    {
        return patents.Select(p => new PatentFutureApplicationView
        {
            VarietyId = p.VarietyId,
            VarietyName = p.VarietyName,
            ApplicatorIds = p.ApplicatorIds,
            AutorIds = p.AutorIds,
            DateAdguardRegistration = p.DateAdguardRegistration,
            DateAdguardWithdrawal = p.DateAdguardWithdrawal,
            DateAdguardExpertise = p.DateAdguardExpertise
        }).ToList();
    }
}