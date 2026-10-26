namespace Brainstable.Applications.Models;

/// <summary>
/// Реквизиты выпуска бюллетеня, введённые пользователем в форме:
/// год, номер выпуска, сквозной номер и период отбора событий.
/// </summary>
public class Bulletin
{
    /// <summary>Номер выпуска в году (в титуле — плейсхолдер [n_year]).</summary>
    public string NumberInYear { get; set; }

    /// <summary>Сквозной номер выпуска (в титуле — плейсхолдер [num]).</summary>
    public string Number { get; set; }

    /// <summary>Год выпуска (в титуле — плейсхолдер [year]).</summary>
    public string Year { get; set; }

    /// <summary>Начало периода отбора; сама дата в период не включается.</summary>
    public DateTime FirstDate { get; set; }

    /// <summary>Конец периода отбора; дата включается в период.</summary>
    public DateTime LastDate { get; set; }

    /// <summary>Начальная страница выпуска; в текущей версии не используется.</summary>
    public int BeginPage { get; set; }
}
