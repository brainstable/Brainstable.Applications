namespace Brainstable.Applications.Models;

public class SubjectChanged
{
    public string BlockName { get; set; }
    public int BlockOrderNumber { get; set; }
    public string KindName { get; set; }
    public string KindLatineName { get; set; }
    public int VarietyId { get; set; }
    public string VarietyName { get; set; }
    public int ApplicationId { get; set; }

    public List<SubjectChangedItem> SubjectChangedItems { get; set; } = [];
}

public class SubjectChangedItem : IEquatable<SubjectChangedItem>
{
    public string ExcludedSubjectId { get; set; }
    public string IncludedSubjectId { get; set; }
    public int TypeSubject { get; set; }
    public DateTime DateChanged { get; set; }


    public bool Equals(SubjectChangedItem? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ExcludedSubjectId == other.ExcludedSubjectId && IncludedSubjectId == other.IncludedSubjectId && TypeSubject == other.TypeSubject && DateChanged.Equals(other.DateChanged);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SubjectChangedItem)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ExcludedSubjectId, IncludedSubjectId, TypeSubject, DateChanged);
    }
}