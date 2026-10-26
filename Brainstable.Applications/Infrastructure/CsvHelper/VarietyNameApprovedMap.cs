using Brainstable.Applications.Dto;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.CsvHelper;

/// <summary>
/// Сопоставление колонок файла СОРТ_СИНОНИМ.csv со свойствами <see cref="VarietyNameApprovedDto"/>.
/// Карта используется как справочная: файл с переменным числом колонок разбирается
/// вручную в <c>LoaderCsv.LoadVarietyNameApprovedDtos</c>.
/// </summary>
public class VarietyNameApprovedMap : ClassMap<VarietyNameApprovedDto>
{
    public VarietyNameApprovedMap()
    {
        // Сорт, предыдущее название и даты изменения/одобрения.
        Map(m => m.VarietyId).Name("К_СОРТА");
        Map(m => m.ApprovedName).Name("И_СОРТА");
        Map(m => m.DateChanged).Name("ДАТА_ИЗМЕНЕНИЯ").Optional();
        Map(m => m.DateApproved).Name("ДАТА_ОДОБРЕНИЯ").Optional();
    }
}
