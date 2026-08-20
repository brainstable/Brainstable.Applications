using System.Globalization;
using System.Text;

using Brainstable.Applications.Dto;
using Brainstable.Applications.Infrastructure.CsvHelper;

using CsvHelper;
using CsvHelper.Configuration;

namespace Brainstable.Applications.Infrastructure.Reader;

public static class LoaderCsv
{
    private static CsvConfiguration conf; 

    static LoaderCsv()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        conf = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "|",
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Не выбрасывать исключение при отсутствии полей
            HeaderValidated = null, // Не проверять соответствие заголовков
            BadDataFound = null
        };
    }
    
    public static List<SubjectDto> LoadSubjects(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<SubjectMap>();
        return csv.GetRecords<SubjectDto>().ToList();
    }
    public static List<BlockDto> LoadBlocks(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<BlockMap>();
        return csv.GetRecords<BlockDto>().ToList();
    }

    public static List<KindDto> LoadKinds(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<KindMap>();
        return csv.GetRecords<KindDto>().ToList();
    }

    public static List<VarietyDto> LoadVarieties(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<VarietyMap>();
        return csv.GetRecords<VarietyDto>().ToList();
    }
    public static List<VarietyAutorDto> LoadVarietiesAutors(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<VarietyAutorMap>();
        return csv.GetRecords<VarietyAutorDto>().ToList();
    }

    public static List<ApplicationDto> LoadApplicaitons(string filePath, int filterType = 1)
    {
        var results = new List<ApplicationDto>();

        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<ApplicationMap>();

        // Читаем и фильтруем на лету
        foreach (var record in csv.GetRecords<ApplicationDto>())
        {
            if (record.Type == filterType)
            {
                results.Add(record);
            }
        }

        return results;
    }

    public static List<SubjectChangeDto> LoadSubjectChangeDtos(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<SubjectChangeDtoMap>();
        return csv.GetRecords<SubjectChangeDto>().ToList();
    }

    public static List<ApplicationApplicatorDto> LoadApplicationsApplicators(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<ApplicationApplicatorMap>();
        return csv.GetRecords<ApplicationApplicatorDto>().ToList();
    }

    public static List<VarietyNameApprovedDto> LoadVarietyNameApprovedDtos(string filePath)
    {
        var csvContent = File.ReadAllText(filePath, Encoding.GetEncoding(1251));
        return LoadVarietyNameApprovedDtosFromString(csvContent);
    }

    private static List<VarietyNameApprovedDto> LoadVarietyNameApprovedDtosFromString(string csvContent)
    {
        var records = new List<VarietyNameApprovedDto>();
        var lines = csvContent.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // Пропускаем заголовок (первая строка)
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split('|');

            // Если колонок 8 (как в проблемных строках), объединяем
            if (parts.Length == 8)
            {
                // parts[0] = К_СОРТА
                // parts[1] = И_СОРТА_20
                // parts[2] = И_СОРТА
                // parts[3] = RG 405
                // parts[4] = 02
                // parts[5] = 2008-1-11
                // parts[6] = 2008-7-16
                // parts[7] = ""

                var record = new VarietyNameApprovedDto()
                {
                    VarietyId = int.Parse(parts[0].Trim()),
                    ApprovedName = parts[2].Trim(), // И_СОРТА
                    DateChanged = ParseDate(parts[5].Trim()),
                    DateApproved = ParseNullableDate(parts[6].Trim())
                };
                records.Add(record);
            }
            else if (parts.Length >= 6)
            {
                // Нормальные строки с 6 колонками
                var record = new VarietyNameApprovedDto
                {
                    VarietyId = int.Parse(parts[0].Trim()),
                    ApprovedName = parts[2].Trim(), // И_СОРТА
                    DateChanged = ParseDate(parts[4].Trim()),
                    DateApproved = ParseNullableDate(parts[5].Trim())
                };
                records.Add(record);
            }
        }

        return records;
    }

    public static List<BlockClassDto> LoadFromCsvFile(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<BlockClassDtoMap>();
        return csv.GetRecords<BlockClassDto>().ToList();
    }

    public static List<PatentDto> LoadPatentDtos(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<PatentDtoMap>();
        return csv.GetRecords<PatentDto>().ToList();
    }

    public static List<LicenceDto> LoadLicenceDtos(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<LicenceDtoMap>();
        return csv.GetRecords<LicenceDto>().ToList();
    }

    public static List<ContractDto> LoadContractDtos(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.GetEncoding(1251));
        using var csv = new CsvReader(reader, conf);

        csv.Context.RegisterClassMap<ContractDtoMap>();
        return csv.GetRecords<ContractDto>().ToList();
    }

    private static DateTime ParseDate(string dateStr)
    {
        if (DateTime.TryParseExact(dateStr, new[] { "yyyy-M-d", "yyyy-MM-dd" },
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        return DateTime.MinValue;
    }

    private static DateTime? ParseNullableDate(string dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr))
            return null;

        if (DateTime.TryParseExact(dateStr, new[] { "yyyy-M-d", "yyyy-MM-dd" },
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        return null;
    }

    private static string[] SplitCsvLine(string line)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == '|' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        result.Add(current.ToString());
        return result.ToArray();
    }
}