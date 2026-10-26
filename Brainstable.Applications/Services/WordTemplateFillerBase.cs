using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services
{
    /// <summary>
    /// Абстрактный базовый класс для генерации Word-документов из шаблонов
    /// </summary>
    /// <typeparam name="T">Тип объектов, которые будут выводиться в документ</typeparam>
    /// <remarks>
    /// Предоставляет общую логику для создания структурированных документов Word с группировкой данных и таблицами.
    /// Для использования необходимо унаследовать класс и реализовать абстрактные методы.
    /// Шаблон уже содержит заголовок раздела и шапку таблицы; класс добавляет в конец документа
    /// заголовки групп и строки данных, поэтому шапку нужно править вместе с набором колонок.
    /// </remarks>
    public abstract class WordTemplateFillerBase<T>
    {
        #region Константы форматирования

        /// <summary>Название шрифта, используемого в документе</summary>
        public const string FontName = "Times New Roman";

        /// <summary>Размер шрифта для обычного текста (12pt)</summary>
        public const string FontSizeNormal = "24";

        /// <summary>Размер шрифта для мелкого текста (10pt)</summary>
        public const string FontSizeSmall = "20";

        /// <summary>Размер шрифта для крупного текста (14pt)</summary>
        public const string FontSizeLarge = "28";

        #endregion

        #region Публичные методы

        /// <summary>
        /// Основной метод для заполнения шаблона Word данными
        /// </summary>
        /// <param name="templatePath">Путь к файлу-шаблону</param>
        /// <param name="outputPath">Путь для сохранения результата</param>
        /// <param name="items">Список объектов для вывода</param>
        /// <remarks>
        /// Метод выполняет следующие шаги:
        /// 1. Копирует шаблон в выходной файл
        /// 2. Открывает документ для редактирования
        /// 3. Группирует элементы согласно логике группировки
        /// 4. Для каждой группы создает заголовки и таблицы с данными
        /// </remarks>
        public void FillWordTemplate(string templatePath, string outputPath, List<T> items)
        {
            // Копируем шаблон в выходной файл
            File.Copy(templatePath, outputPath, true);

            // Открываем документ для редактирования
            using var wordDocument = WordprocessingDocument.Open(outputPath, true);
            var mainPart = wordDocument.MainDocumentPart;
            var body = mainPart!.Document.Body;

            // Получаем группы элементов (может быть переопределено)
            var groups = GroupItems(items);

            foreach (var group in groups)
            {
                // Создаем заголовок группы
                var headerText = GetGroupHeader(group);
                if (!string.IsNullOrEmpty(headerText))
                {
                    body!.AppendChild(CreateParagraph(headerText,
                        isBold: true,
                        isUnderline: true,
                        fontSize: FontSizeLarge,
                        afterSpacing: "120"));
                }

                // Получаем подгруппы внутри текущей группы
                var subGroups = GetSubGroups(group);
                foreach (var subGroup in subGroups)
                {
                    // Создаем заголовок подгруппы
                    var subHeaderText = GetSubGroupHeader(subGroup);
                    if (!string.IsNullOrEmpty(subHeaderText))
                    {
                        body!.AppendChild(CreateParagraph(subHeaderText,
                            isBold: true,
                            isItalic: true,
                            isUnderline: true,
                            fontSize: FontSizeNormal,
                            afterSpacing: "240"));
                    }

                    // Получаем элементы для отображения в таблице
                    var dataItems = GetDataItems(subGroup);
                    if (dataItems.Any())
                    {
                        // Создаем таблицу с данными
                        body!.AppendChild(CreateTable(dataItems));
                        // Добавляем пустую строку для разделения таблиц
                        body!.AppendChild(CreateEmptyParagraph());
                    }
                }
            }
        }

        #endregion

        #region Абстрактные методы (должны быть переопределены в наследниках)

        /// <summary>
        /// Определяет логику группировки элементов
        /// </summary>
        /// <param name="items">Список всех элементов</param>
        /// <returns>Коллекция групп элементов</returns>
        /// <remarks>
        /// По умолчанию все элементы помещаются в одну группу.
        /// Переопределите этот метод для кастомной группировки.
        /// </remarks>
        protected virtual IEnumerable<IGrouping<object, T>> GroupItems(List<T> items)
        {
            return items.GroupBy(x => new { });
        }

        /// <summary>
        /// Возвращает текст заголовка для группы
        /// </summary>
        /// <param name="group">Группа элементов</param>
        /// <returns>Текст заголовка</returns>
        protected abstract string GetGroupHeader(IGrouping<object, T> group);

        /// <summary>
        /// Определяет подгруппы внутри основной группы
        /// </summary>
        /// <param name="group">Основная группа</param>
        /// <returns>Коллекция подгрупп</returns>
        /// <remarks>
        /// По умолчанию все элементы остаются в одной группе.
        /// Переопределите для создания вложенной структуры.
        /// </remarks>
        protected virtual IEnumerable<IGrouping<object, T>> GetSubGroups(IGrouping<object, T> group)
        {
            return group.GroupBy(x => new { });
        }

        /// <summary>
        /// Возвращает текст заголовка для подгруппы
        /// </summary>
        /// <param name="subGroup">Подгруппа элементов</param>
        /// <returns>Текст заголовка подгруппы</returns>
        protected abstract string GetSubGroupHeader(IGrouping<object, T> subGroup);

        /// <summary>
        /// Извлекает элементы из подгруппы для отображения в таблице
        /// </summary>
        /// <param name="subGroup">Подгруппа элементов</param>
        /// <returns>Коллекция элементов для таблицы</returns>
        protected abstract IEnumerable<T> GetDataItems(IGrouping<object, T> subGroup);

        /// <summary>
        /// Определяет структуру колонок таблицы
        /// </summary>
        /// <returns>Список определений колонок</returns>
        /// <remarks>
        /// Каждая колонка определяется именем, шириной в процентах и заголовком.
        /// Ширина задается в процентах от общей ширины таблицы.
        /// </remarks>
        protected abstract List<TableColumnDefinition> GetTableColumns();

        /// <summary>
        /// Получает данные для ячейки таблицы по имени колонки
        /// </summary>
        /// <param name="item">Элемент данных</param>
        /// <param name="columnName">Имя колонки</param>
        /// <returns>Список строк для отображения в ячейке (каждая строка на новой строке)</returns>
        protected abstract List<string> GetCellData(T item, string columnName);

        /// <summary>
        /// Определяет форматирование для конкретной ячейки
        /// </summary>
        /// <param name="item">Элемент данных</param>
        /// <param name="columnName">Имя колонки</param>
        /// <param name="cellText">Текст ячейки</param>
        /// <returns>Объект с настройками форматирования</returns>
        /// <remarks>
        /// По умолчанию возвращает стандартное форматирование.
        /// Переопределите для кастомного форматирования отдельных ячеек.
        /// </remarks>
        protected virtual CellFormatting GetCellFormatting(T item, string columnName, string cellText)
        {
            return new CellFormatting();
        }

        #endregion

        #region Вспомогательные методы для создания элементов Word

        /// <summary>
        /// Создает параграф (абзац) с текстом и заданным форматированием
        /// </summary>
        /// <param name="text">Текст параграфа</param>
        /// <param name="isBold">Жирный шрифт</param>
        /// <param name="isItalic">Курсив</param>
        /// <param name="isUnderline">Подчеркивание</param>
        /// <param name="fontSize">Размер шрифта</param>
        /// <param name="afterSpacing">Отступ после параграфа в twips (1/20 пункта)</param>
        /// <param name="alignment">Выравнивание текста</param>
        /// <returns>Объект параграфа</returns>
        protected Paragraph CreateParagraph(string text,
            bool isBold = false,
            bool isItalic = false,
            bool isUnderline = false,
            string fontSize = null,
            string afterSpacing = "0",
            EnumValue<JustificationValues> alignment = null)
        {
            var paragraph = new Paragraph();
            paragraph.ParagraphProperties = new ParagraphProperties();

            // Настройка отступов
            var spacing = new SpacingBetweenLines();
            spacing.After = afterSpacing;
            paragraph.ParagraphProperties.AppendChild(spacing);

            // Настройка выравнивания
            if (alignment != null)
            {
                var justification = new Justification();
                justification.Val = alignment;
                paragraph.ParagraphProperties.AppendChild(justification);
            }

            // Создание текстового блока (Run)
            var run = new Run();
            var textElement = new Text(text);
            run.AppendChild(textElement);

            // Применение форматирования к тексту
            var runProps = CreateRunProperties(isBold, isItalic, isUnderline, fontSize ?? FontSizeNormal);
            run.RunProperties = runProps;

            paragraph.AppendChild(run);
            return paragraph;
        }

        /// <summary>
        /// Создает свойства форматирования для текстового блока
        /// </summary>
        /// <param name="isBold">Жирный шрифт</param>
        /// <param name="isItalic">Курсив</param>
        /// <param name="isUnderline">Подчеркивание</param>
        /// <param name="fontSize">Размер шрифта</param>
        /// <returns>Объект со свойствами форматирования</returns>
        protected RunProperties CreateRunProperties(bool isBold = false,
            bool isItalic = false,
            bool isUnderline = false,
            string fontSize = null)
        {
            var runProps = new RunProperties();

            // Установка шрифта
            var runFonts = new RunFonts();
            runFonts.Ascii = FontName;
            runFonts.HighAnsi = FontName;
            runFonts.ComplexScript = FontName;
            runProps.AppendChild(runFonts);

            // Установка размера шрифта
            var fontSizeElement = new FontSize();
            fontSizeElement.Val = fontSize ?? FontSizeNormal;
            runProps.AppendChild(fontSizeElement);

            // Применение стилей
            if (isBold)
                runProps.AppendChild(new Bold());

            if (isUnderline)
            {
                var underline = new Underline();
                underline.Val = UnderlineValues.Single;
                runProps.AppendChild(underline);
            }

            if (isItalic)
                runProps.AppendChild(new Italic());

            return runProps;
        }

        /// <summary>
        /// Создает таблицу с данными на основе переданных элементов.
        /// Строка заголовков не создаётся: шапка таблицы уже находится в .docx-шаблоне.
        /// </summary>
        /// <param name="items">Коллекция элементов для отображения</param>
        /// <returns>Объект таблицы</returns>
        protected Table CreateTable(IEnumerable<T> items)
        {
            var table = new Table();
            var columns = GetTableColumns();

            // Настройка свойств таблицы
            var tableProperties = new TableProperties();
            tableProperties.TableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

            // Отключение границ таблицы
            var borders = new TableBorders();
            borders.TopBorder = new TopBorder() { Val = BorderValues.None };
            borders.BottomBorder = new BottomBorder() { Val = BorderValues.None };
            borders.LeftBorder = new LeftBorder() { Val = BorderValues.None };
            borders.RightBorder = new RightBorder() { Val = BorderValues.None };
            borders.InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.None };
            borders.InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.None };
            tableProperties.AppendChild(borders);

            // Настройка ширины колонок
            var tableGrid = new TableGrid();
            foreach (var column in columns)
            {
                var gridColumn = new GridColumn();
                gridColumn.Width = column.Width;
                tableGrid.AppendChild(gridColumn);
            }
            tableProperties.AppendChild(tableGrid);

            table.AppendChild(tableProperties);

            // Добавление строк данных
            foreach (var item in items)
            {
                table.AppendChild(CreateDataRow(item, columns));
            }

            return table;
        }

        /// <summary>
        /// Создает строку таблицы с данными из элемента
        /// </summary>
        /// <param name="item">Элемент данных</param>
        /// <param name="columns">Определения колонок</param>
        /// <returns>Объект строки таблицы</returns>
        protected TableRow CreateDataRow(T item, List<TableColumnDefinition> columns)
        {
            var row = new TableRow();

            foreach (var column in columns)
            {
                // Получаем данные для текущей колонки
                var cellData = GetCellData(item, column.Name);
                var cellText = string.Join("\n", cellData);
                var formatting = GetCellFormatting(item, column.Name, cellText);

                // Создаем ячейку и добавляем в строку
                row.AppendChild(CreateTableCell(
                    cellText,
                    column.Width,
                    formatting));
            }

            return row;
        }

        /// <summary>
        /// Создает ячейку таблицы с текстом и форматированием
        /// </summary>
        /// <param name="text">Текст ячейки (может содержать переносы строк \n)</param>
        /// <param name="width">Ширина ячейки в процентах</param>
        /// <param name="formatting">Настройки форматирования</param>
        /// <returns>Объект ячейки таблицы</returns>
        protected TableCell CreateTableCell(string text,
            string width = "auto",
            CellFormatting formatting = null)
        {
            var cell = new TableCell();

            // Настройка свойств ячейки
            var cellProperties = new TableCellProperties();
            cellProperties.TableCellWidth = new TableCellWidth()
            {
                Width = width,
                Type = TableWidthUnitValues.Pct
            };

            // Отключение границ ячейки
            cellProperties.AppendChild(new TopBorder() { Val = BorderValues.None });
            cellProperties.AppendChild(new BottomBorder() { Val = BorderValues.None });
            cellProperties.AppendChild(new LeftBorder() { Val = BorderValues.None });
            cellProperties.AppendChild(new RightBorder() { Val = BorderValues.None });

            cell.AppendChild(cellProperties);

            // Создание параграфа внутри ячейки
            var paragraph = new Paragraph();
            paragraph.ParagraphProperties = new ParagraphProperties();

            // Установка выравнивания
            if (formatting?.Alignment != null)
            {
                var justification = new Justification();
                justification.Val = formatting.Alignment;
                paragraph.ParagraphProperties.AppendChild(justification);
            }

            // Настройка межстрочного интервала
            var spacing = new SpacingBetweenLines();
            spacing.After = "200";
            spacing.Line = "240";
            spacing.LineRule = LineSpacingRuleValues.Auto;
            paragraph.ParagraphProperties.AppendChild(spacing);

            // Разбиваем текст на строки и создаем отдельные текстовые блоки
            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var run = new Run();
                var textElement = new Text(lines[i]);
                run.AppendChild(textElement);

                // Базовое форматирование для всей ячейки
                var runProps = CreateRunProperties(
                    isBold: formatting?.IsBold ?? false,
                    isItalic: formatting?.IsItalic ?? false,
                    isUnderline: formatting?.IsUnderline ?? false,
                    fontSize: formatting?.FontSize ?? FontSizeNormal
                );

                // Специальное форматирование для конкретной строки
                if (formatting != null && formatting.LineFormatting.TryGetValue(i, out var lineFormat))
                {
                    if (lineFormat.IsBold)
                        runProps.AppendChild(new Bold());
                    if (lineFormat.IsItalic)
                        runProps.AppendChild(new Italic());
                    if (lineFormat.IsUnderline)
                    {
                        var underline = new Underline();
                        underline.Val = UnderlineValues.Single;
                        runProps.AppendChild(underline);
                    }
                    if (!string.IsNullOrEmpty(lineFormat.FontSize))
                    {
                        var fontSize = new FontSize();
                        fontSize.Val = lineFormat.FontSize;
                        runProps.AppendChild(fontSize);
                    }
                }

                run.RunProperties = runProps;
                paragraph.AppendChild(run);

                // Добавляем перенос строки после каждой строки кроме последней
                if (i < lines.Length - 1)
                {
                    paragraph.AppendChild(new Break());
                }
            }

            cell.AppendChild(paragraph);
            return cell;
        }

        /// <summary>
        /// Создает пустой параграф для вертикального отступа
        /// </summary>
        /// <returns>Пустой параграф</returns>
        protected Paragraph CreateEmptyParagraph()
        {
            var paragraph = new Paragraph();
            paragraph.ParagraphProperties = new ParagraphProperties();

            // Убираем все отступы
            var spacing = new SpacingBetweenLines();
            spacing.After = "0";
            spacing.Before = "0";
            paragraph.ParagraphProperties.AppendChild(spacing);

            // Создаем пустой текстовый блок
            var run = new Run();
            var textElement = new Text("");
            run.AppendChild(textElement);
            paragraph.AppendChild(run);

            return paragraph;
        }

        #endregion
    }

    /// <summary>
    /// Определение колонки таблицы
    /// </summary>
    public class TableColumnDefinition
    {
        /// <summary>Уникальное имя колонки для идентификации</summary>
        public string Name { get; set; }

        /// <summary>Ширина колонки в процентах (например, "30" для 30%)</summary>
        public string Width { get; set; }

        /// <summary>Текст заголовка колонки</summary>
        public string Header { get; set; }

        /// <summary>
        /// Создает новое определение колонки
        /// </summary>
        /// <param name="name">Уникальное имя колонки</param>
        /// <param name="width">Ширина в процентах</param>
        /// <param name="header">Заголовок колонки (если не указан, используется имя)</param>
        public TableColumnDefinition(string name, string width, string header = null)
        {
            Name = name;
            Width = width;
            Header = header ?? name;
        }
    }

    /// <summary>
    /// Настройки форматирования ячейки таблицы
    /// </summary>
    public class CellFormatting
    {
        private string _fontSize = "24"; // Размер шрифта по умолчанию (12pt)

        /// <summary>Размер шрифта для всей ячейки</summary>
        public string FontSize
        {
            get => _fontSize;
            set => _fontSize = value;
        }

        /// <summary>Применять жирное начертание ко всей ячейке</summary>
        public bool IsBold { get; set; } = false;

        /// <summary>Применять курсив ко всей ячейке</summary>
        public bool IsItalic { get; set; } = false;

        /// <summary>Применять подчеркивание ко всей ячейке</summary>
        public bool IsUnderline { get; set; } = false;

        /// <summary>Выравнивание текста в ячейке</summary>
        public EnumValue<JustificationValues> Alignment { get; set; }

        /// <summary>Специальное форматирование для отдельных строк внутри ячейки</summary>
        /// <remarks>Ключ - номер строки (начиная с 0), значение - настройки форматирования для этой строки</remarks>
        public Dictionary<int, CellLineFormatting> LineFormatting { get; set; } = new();
    }

    /// <summary>
    /// Форматирование отдельной строки внутри ячейки таблицы
    /// </summary>
    public class CellLineFormatting
    {
        /// <summary>Применять жирное начертание к строке</summary>
        public bool IsBold { get; set; } = false;

        /// <summary>Применять курсив к строке</summary>
        public bool IsItalic { get; set; } = false;

        /// <summary>Применять подчеркивание к строке</summary>
        public bool IsUnderline { get; set; } = false;

        /// <summary>Размер шрифта для строки (если не указан, используется размер ячейки)</summary>
        public string FontSize { get; set; }
    }
}