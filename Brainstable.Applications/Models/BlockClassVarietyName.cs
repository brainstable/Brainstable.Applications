namespace Brainstable.Applications.Models;

/// <summary>
/// Строка сводной таблицы II.5: пара «блок классов — название сорта».
/// Список накапливается при построении разделов I.1 и II.1–II.4.
/// </summary>
public class BlockClassVarietyName : IEquatable<BlockClassVarietyName>
{
    /// <summary>Идентификатор блока классов.</summary>
    public int BlockClassId { get; set; }

    /// <summary>Название блока классов.</summary>
    public string BlockClassName { get; set; }

    /// <summary>Название сорта, попавшего в раздел.</summary>
    public string VarietyName { get; set; }

    /// <summary>
    /// Сравнение по всем трём полям, чтобы дубликаты не попадали в сводную таблицу.
    /// </summary>
    public bool Equals(BlockClassVarietyName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return BlockClassId == other.BlockClassId && BlockClassName == other.BlockClassName && VarietyName == other.VarietyName;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((BlockClassVarietyName)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(BlockClassId, BlockClassName, VarietyName);
    }
}
