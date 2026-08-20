namespace Brainstable.Applications.Models;

public class BlockClassVarietyName : IEquatable<BlockClassVarietyName>
{
    public int BlockClassId { get; set; }
    public string BlockClassName { get; set; }
    public string VarietyName { get; set; }

    public bool Equals(BlockClassVarietyName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return BlockClassId == other.BlockClassId && BlockClassName == other.BlockClassName && VarietyName == other.VarietyName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BlockClassVarietyName)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BlockClassId, BlockClassName, VarietyName);
    }
}