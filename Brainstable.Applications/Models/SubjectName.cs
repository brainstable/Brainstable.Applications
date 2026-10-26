namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы VIII.1 «Список заявителей, авторов и патентообладателей» (раздел VIII).
/// </summary>
public class SubjectName
{
    /// <summary>Номер субъекта; выводится в левой колонке.</summary>
    public int Id { get; set; }

    /// <summary>Наименование субъекта.</summary>
    public string Name { get; set; }
}
