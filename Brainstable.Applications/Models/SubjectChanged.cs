namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы V.1 «Изменение состава субъектов селекционных достижений» (раздел V):
/// сорт и список изменений его субъектов (замена, добавление, удаление).
/// </summary>
public class SubjectChanged
{
    /// <summary>Название блока реестра — заголовок верхнего уровня.</summary>
    public string BlockName { get; set; }

    /// <summary>Порядковый номер блока реестра для сортировки.</summary>
    public int BlockOrderNumber { get; set; }

    /// <summary>Русское название рода/вида.</summary>
    public string KindName { get; set; }

    /// <summary>Латинское название рода/вида.</summary>
    public string KindLatineName { get; set; }

    /// <summary>Идентификатор сорта; выводится в колонке «Номер заявки».</summary>
    public int VarietyId { get; set; }

    /// <summary>Название сорта.</summary>
    public string VarietyName { get; set; }

    /// <summary>Номер заявки, по которой изменился состав субъектов.</summary>
    public int ApplicationId { get; set; }

    /// <summary>Изменения состава субъектов по этому сорту.</summary>
    public List<SubjectChangedItem> SubjectChangedItems { get; set; } = [];
}

/// <summary>
/// Одно изменение состава субъектов: кто исключён и кто включён.
/// Для добавления исключаемый субъект пуст, для удаления — пуст включаемый.
/// </summary>
public class SubjectChangedItem : IEquatable<SubjectChangedItem>
{
    /// <summary>Исключаемый субъект (пустая строка при добавлении).</summary>
    public string ExcludedSubjectId { get; set; }

    /// <summary>Включаемый субъект (пустая строка при удалении).</summary>
    public string IncludedSubjectId { get; set; }

    /// <summary>Тип субъекта: 1 — заявитель, 2 — автор, 6 — патентообладатель.</summary>
    public int TypeSubject { get; set; }

    /// <summary>Дата изменения состава.</summary>
    public DateTime DateChanged { get; set; }

    /// <summary>
    /// Сравнение по всем полям, чтобы одно и то же изменение не добавлялось дважды.
    /// </summary>
    public bool Equals(SubjectChangedItem? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ExcludedSubjectId == other.ExcludedSubjectId && IncludedSubjectId == other.IncludedSubjectId && TypeSubject == other.TypeSubject && DateChanged.Equals(other.DateChanged);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SubjectChangedItem)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(ExcludedSubjectId, IncludedSubjectId, TypeSubject, DateChanged);
    }
}
