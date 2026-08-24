using Brainstable.Applications.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;

namespace Brainstable.Applications.Services.Implementations;

public class BulletinService
{
    private readonly Bulletin _bulletin;
    private readonly DateTime firstDate;
    private readonly DateTime lastDate;

    // Настройка путей
    private const string TEMPLATE_FOLDER = "templates//";
    private readonly string outputDirectory;
    private readonly string outputBulletins;

    public BulletinService(Bulletin bulletin)
    {
        _bulletin = bulletin;
        firstDate = bulletin.FirstDate;
        lastDate = bulletin.LastDate;
        
        outputBulletins = "Бюллетени";
        outputDirectory = Path.Combine(outputBulletins, $"{bulletin.Year}-{bulletin.Number}");



        Directory.CreateDirectory(outputBulletins);
    }

    string GetPath(string fileName) => Path.Combine(outputDirectory, fileName);

    /// <summary>
    /// Проверяет, нужно ли создавать файл (список не пустой)
    /// </summary>
    private bool ShouldGenerate<T>(IEnumerable<T> data) => data?.Any() == true;

    public void BuilderBulletin()
    {
        Directory.CreateDirectory(outputDirectory);

        var factory = new DataObjectFactory();
        var builderService = new BuilderService(factory);
        builderService.Load();

        DeleteAllFiles(outputDirectory, true);

        // ВСЕ НАЗВАНИЯ ФАЙЛОВ В ОДНОМ МЕСТЕ (используем одинаковые имена)
        string titul = "0. Титул.docx";
        string acceptedApps = "1.1 Заявки на выдачу патента.docx";
        string proposedNames = "2.1 Предложенные названия.docx";
        string approvedNames = "2.2 Одобренные названия.docx";
        string proposalsChange = "2.3 Предложения по изменению одобренных названий.docx";
        string approvedChange = "2.4 Одобренные изменения названий.docx";
        string blockClassVariety = "2.5 Сводная таблица предложенных названий.docx";
        string withdrawnApps = "3.1 Отозванные заявки.docx";
        string decisionsApps = "4.5 Сорта и гибриды, включенные в реестр охраняемых.docx";
        string rejectionApps = "4.6 Отклоненные заявки на выдачу патента.docx";
        string subjectChangeds = "5.1 Изменения состава субъектов селекционных достижений.docx";
        string licenceRegistrations = "7.1 Зарегистрированные лицензии.docx";
        string licenceCanceled = "7.2 Аннулированные лицензии.docx";
        string licenceTransfer = "7.4 Передача права.docx";
        string subjectNames = "8. Субъекты.docx";
        string endProtections = "6.1 Окончание охраны.docx";

        // 0. Титул
        var replacements = new Dictionary<string, string>();
        replacements["[n_year]"] = _bulletin.NumberInYear;
        replacements["[num]"] = _bulletin.Number;
        replacements["[year]"] = _bulletin.Year;

        var wordTitul = new WordTitleService();
        wordTitul.Create($"{TEMPLATE_FOLDER}{titul}", GetPath(titul), replacements);

        // 1.1 Принятые заявки
        var acceptedApplicationsData = builderService.GetAcceptedApplications(firstDate, lastDate);
        if (ShouldGenerate(acceptedApplicationsData))
        {
            new AcceptedApplicationWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{acceptedApps}",
                GetPath(acceptedApps),
                acceptedApplicationsData);
        }

        // 2.1 Предложенные названия
        var proposedNamesData = builderService.GetProposedNames(firstDate, lastDate);
        if (ShouldGenerate(proposedNamesData))
        {
            new ProposedNamesWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{proposedNames}",
                GetPath(proposedNames),
                proposedNamesData);
        }

        // 2.2 Одобренные названия
        var approvedNamesData = builderService.GetApprovedNames(firstDate, lastDate);
        if (ShouldGenerate(approvedNamesData))
        {
            new ApprovedNamesWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{approvedNames}",
                GetPath(approvedNames),
                approvedNamesData);
        }

        // 2.3 Предложения по изменению одобренных названий
        var proposalsChangeData = builderService.GetProposalsChangeApprovedNames(firstDate, lastDate);
        if (ShouldGenerate(proposalsChangeData))
        {
            new ProposalsChangeApprovedNamesWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{proposalsChange}",
                GetPath(proposalsChange),
                proposalsChangeData);
        }

        // 2.4 Одобренные изменения названий
        var approvedChangeData = builderService.GetApprovedChangeApprovedNames(firstDate, lastDate);
        if (ShouldGenerate(approvedChangeData))
        {
            new ApprovedChangeApprovedNamesWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{approvedChange}",
                GetPath(approvedChange),
                approvedChangeData);
        }

        // 2.5 Сводная таблица предложенных названий (всегда генерируем, так как это справочная таблица)
        var blockClassVarietyData = builderService.GetBlockClassVarietyNames();
        new BlockClassVarietyNamesWordFiller().FillWordTemplate(
            $"{TEMPLATE_FOLDER}{blockClassVariety}",
            GetPath(blockClassVariety),
            blockClassVarietyData);

        // 3.1 Отозванные заявки
        var withdrawnAppsData = builderService.GetWithdrawnApplications(firstDate, lastDate);
        if (ShouldGenerate(withdrawnAppsData))
        {
            new WithdrawnApplicationWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{withdrawnApps}",
                GetPath(withdrawnApps),
                withdrawnAppsData);
        }

        // 4.5 Сорта и гибриды, включенные в реестр охраняемых
        var decisionsAppsData = builderService.GetDecisionsApplications(firstDate, lastDate);
        if (ShouldGenerate(decisionsAppsData))
        {
            new DecisionsApplicationWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{decisionsApps}",
                GetPath(decisionsApps),
                decisionsAppsData);
        }

        // 4.6 Отклоненные заявки на выдачу патента
        var rejectionAppsData = builderService.GetRejectionApplications(firstDate, lastDate);
        if (ShouldGenerate(rejectionAppsData))
        {
            new RejectionApplicationWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{rejectionApps}",
                GetPath(rejectionApps),
                rejectionAppsData);
        }

        // 5.1 Изменения состава субъектов селекционных достижений
        var subjectChangedsData = builderService.GetSubjectChangeds(firstDate, lastDate);
        if (ShouldGenerate(subjectChangedsData))
        {
            new SubjectChangedWordFiller().FillWordTemplate(
                $"{TEMPLATE_FOLDER}{subjectChangeds}",
                GetPath(subjectChangeds),
                subjectChangedsData);
        }

        // 6.1 Отклоненные патенты
        var endApplications = builderService.GetProtectionEndApplications(firstDate, lastDate);
        if (ShouldGenerate(endApplications))
        {
            new ProtectionEndApplicationWordFiller().FillWordTemplate($"{TEMPLATE_FOLDER}{endProtections}",
                GetPath(endProtections),
                endApplications);
        }

        // 7.1 Зарегистрированные лицензии
        var licences = builderService.GetLicences(firstDate, lastDate);
        if (ShouldGenerate(licences))
        {
            new LicencesWordFiller().FillWordTemplate($"{TEMPLATE_FOLDER}{licenceRegistrations}",
                GetPath(licenceRegistrations),
                licences);
        }

        // 7.2 Аннулированные лицензии.docx
        licences = builderService.GetCanceledLicences(firstDate, lastDate);
        if (ShouldGenerate(licences))
        {
            new LicencesCanceledWordFiller().FillWordTemplate($"{TEMPLATE_FOLDER}{licenceCanceled}",
                GetPath(licenceCanceled),
                licences);
        }

        // 7.4 Передача права.docx
        var transferLicences = builderService.GetTransferLicences(firstDate, lastDate);
        if (ShouldGenerate(transferLicences))
        {
            new LicencesTransferWordFiller().FillWordTemplate($"{TEMPLATE_FOLDER}{licenceTransfer}",
                GetPath(licenceTransfer),
                transferLicences);
        }

        // 8. Субъекты (всегда генерируем, так как это справочная таблица)
        var subjectNamesData = builderService.GetSubjectNames();
        new SubjectNamesWordFiller().FillWordTemplate(
            $"{TEMPLATE_FOLDER}{subjectNames}",
            GetPath(subjectNames),
            subjectNamesData);

        // Объединение файлов
        var isMerge = MergeFiles(outputDirectory, outputBulletins);

        if (isMerge)
        {
            MessageBox.Show($"Бюллетень №{_bulletin.Number} ({_bulletin.NumberInYear}) за {_bulletin.Year} год успешно сформирован", "Создание бюллетеня", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Бюллетень №{_bulletin.Number} ({_bulletin.NumberInYear}) за {_bulletin.Year} год не сформирован", "Создание бюллетеня", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private bool MergeFiles(string outputDirectory, string outputBulletins)
    {
        try
        {
            var fileName =
                $"{outputBulletins}//Бюллетень {_bulletin.Year}-{_bulletin.NumberInYear}({_bulletin.Number}).docx";
            var merge = new WordDocumentMerger();
            merge.MergeDirectory(outputDirectory, fileName);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Удаляет все файлы из директории с возможностью удаления поддиректорий
    /// </summary>
    private static int DeleteAllFiles(string directoryPath, bool deleteSubdirectories = false)
    {
        if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
            return 0;

        int deletedCount = 0;

        try
        {
            // Удаляем файлы
            var files = Directory.GetFiles(directoryPath);
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                    deletedCount++;
                }
                catch { }
            }

            // Если нужно удалить поддиректории
            if (deleteSubdirectories)
            {
                var directories = Directory.GetDirectories(directoryPath);
                foreach (var dir in directories)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                        deletedCount++;
                    }
                    catch { }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return deletedCount;
    }
}