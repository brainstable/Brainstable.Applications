using Brainstable.Applications.Models;
using Brainstable.Applications.Services.Implementations;

using System.Text;
using Brainstable.Applications.Services;

namespace Brainstable.Applications;

public partial class FormViewPatents : Form
{
    private BindingSource bindingSource = new BindingSource();
    private List<PatentFutureApplication> applications = [];
    private List<PatentFutureApplication> patents = new List<PatentFutureApplication>();
    private List<PatentFutureApplication> viewApplications = [];

    private PatentFutureApplication patent;

    public FormViewPatents()
    {
        InitializeComponent();
        InitializeDataGridView();
        var factory = new DataObjectFactory();
        var builderService = new BuilderPatentFeatureService(factory);
        builderService.Load();

        applications = builderService.GetPatentFeatureApplications();

        ViewKinds(applications);
    }

    private void ViewKinds(List<PatentFutureApplication> list)
    {
        treeView1.Nodes.Clear();

        var byBlocks = list.GroupBy(x => new { x.BlockName, x.BlockOrderNumber });

        groupBox1.Text = $"Кол-во заявок = {list.Count}";

        foreach (var block in byBlocks.OrderBy(x => x.Key.BlockOrderNumber))
        {
            var nodeBlock = new TreeNode($"[{block.Key.BlockOrderNumber}] - {block.Key.BlockName} ({list.Count(x => x.BlockName == block.Key.BlockName)})");
            //nodeBlock.Tag = block.Key.BlockName;
            treeView1.Nodes.Add(nodeBlock);

            var byKinds = block.GroupBy(x => x.KindName);

            foreach (var kind in byKinds.OrderBy(x => x.Key))
            {
                var nodeKind = new TreeNode($"{kind.Key} ({list.Count(x => x.KindName == kind.Key)})");
                nodeKind.Tag = kind.Key;
                nodeBlock.Nodes.Add(nodeKind);
            }
        }
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
        var kindName = e.Node.Tag;

        if (kindName is not null)
        {
            viewApplications = applications.Where(x => x.KindName == kindName.ToString()).OrderBy(x => x.VarietyName).ToList();

            var count = applications.Count(x => x.KindName == kindName.ToString());

            LoadData(viewApplications);
        }
    }

    private void InitializeDataGridView()
    {
        // Отключаем автоматическую генерацию столбцов
        dataGridView1.AutoGenerateColumns = false;

        // Настройка внешнего вида
        dataGridView1.ReadOnly = true;
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.MultiSelect = false;

        // Очищаем существующие столбцы
        dataGridView1.Columns.Clear();

        var colId = new DataGridViewTextBoxColumn
        {
            Name = "ApplicationAdguardId",
            DataPropertyName = "ApplicationAdguardId",
            HeaderText = "№ заявки",
            Width = 80,
            ReadOnly = true
        };
        dataGridView1.Columns.Add(colId);

        // Создаем столбцы вручную
        var colVarietyId = new DataGridViewTextBoxColumn
        {
            Name = "VarietyId",
            DataPropertyName = "VarietyId",
            HeaderText = "Код сорта",
            Width = 80,
            ReadOnly = true
        };
        dataGridView1.Columns.Add(colVarietyId);

        var colVarietyName = new DataGridViewTextBoxColumn
        {
            Name = "VarietyName",
            DataPropertyName = "VarietyName",
            HeaderText = "Название сорта",
            Width = 200,
            ReadOnly = true,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        };
        dataGridView1.Columns.Add(colVarietyName);

        var colApplicatorIds = new DataGridViewTextBoxColumn
        {
            Name = "ApplicatorIds",
            DataPropertyName = "ApplicatorIds",
            HeaderText = "ID заявителей",
            Width = 150,
            ReadOnly = true,
            Visible = false
        };
        dataGridView1.Columns.Add(colApplicatorIds);

        var colAutorIds = new DataGridViewTextBoxColumn
        {
            Name = "AutorIds",
            DataPropertyName = "AutorIds",
            HeaderText = "ID авторов",
            Width = 150,
            ReadOnly = true,
            Visible = false
        };
        dataGridView1.Columns.Add(colAutorIds);

        var colDateRegistration = new DataGridViewTextBoxColumn
        {
            Name = "DateAdguardRegistration",
            DataPropertyName = "DateAdguardRegistration",
            HeaderText = "Дата регистрации",
            Width = 130,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "dd.MM.yyyy",
                Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        };
        dataGridView1.Columns.Add(colDateRegistration);

        var colDateWithdrawal = new DataGridViewTextBoxColumn
        {
            Name = "DateAdguardWithdrawal",
            DataPropertyName = "DateAdguardWithdrawal",
            HeaderText = "Дата отклонения",
            Width = 130,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "dd.MM.yyyy",
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                NullValue = "-"
            },
            Visible = false
        };
        dataGridView1.Columns.Add(colDateWithdrawal);

        var colDateExpertise = new DataGridViewTextBoxColumn
        {
            Name = "DateAdguardExpertise",
            DataPropertyName = "DateAdguardExpertise",
            HeaderText = "Дата экспертизы",
            Width = 130,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "dd.MM.yyyy",
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                NullValue = "-"
            }
        };
        dataGridView1.Columns.Add(colDateExpertise);

        // Привязываем BindingSource к DataGridView
        dataGridView1.DataSource = bindingSource;

        // Подписываемся на событие клика по ячейке
        dataGridView1.CellClick += DataGridView1_CellClick;
        dataGridView1.RowEnter += DataGridView1_RowEnter;
    }
    public void LoadData(List<PatentFutureApplication> data)
    {
        patents = data;
        bindingSource.DataSource = patents;
        bindingSource.ResetBindings(false);
    }

    // Метод для обновления списка
    public void UpdateData(List<PatentFutureApplication> newData)
    {
        patents.Clear();
        patents.AddRange(newData);
        bindingSource.ResetBindings(false);
    }

    // Обработка клика по ячейке
    private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            patent = bindingSource[e.RowIndex] as PatentFutureApplication;
            if (patent != null)
            {
                ShowDetails(patent);
            }
        }
    }

    // Обработка смены строки
    private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            patent = bindingSource[e.RowIndex] as PatentFutureApplication;
            if (patent != null)
            {
                ShowDetails(patent);
            }
        }
    }

    // Отображение деталей в текстовом окне
    private void ShowDetails(PatentFutureApplication patent)
    {
        var details = new StringBuilder();
        details.AppendLine($"Культура: {patent.KindName} ({patent.KindLatineName})");
        details.AppendLine($"Сорт: {patent.VarietyName} (Номер заявки: {patent.ApplicationAdguardId}, Код сорта: {patent.VarietyId})");
        details.AppendLine($"Дата регистрации: {patent.DateAdguardRegistration:dd.MM.yyyy}");

        if (patent.DateAdguardExpertise.HasValue)
            details.AppendLine($"Дата экспертизы: {patent.DateAdguardExpertise.Value:dd.MM.yyyy}");
        else
            details.AppendLine($"Дата экспертизы: -");

        details.AppendLine();
        details.AppendLine($"Заявители:");
        details.AppendLine($"{string.Join(", ", patent.ApplicatorNames ?? new List<string>())}");
        details.AppendLine();
        details.AppendLine($"Авторы:");
        details.AppendLine($"{string.Join(", ", patent.AutorNames ?? new List<string>())}");

        textBox1.Text = details.ToString();
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        if (patent == null)
            return;
        
        var dict = new Dictionary<string, string>();
        dict["[number]"] = "";
        dict["[kind_name]"] = patent.KindName;
        dict["[kind_name_lat]"] = patent.KindLatineName;
        dict["[variety_name]"] = patent.VarietyName;
        dict["[patent_owner]"] = string.Join(Environment.NewLine, patent.ApplicatorNames);
        dict["[autors]"] = string.Join(Environment.NewLine, patent.AutorNames);
        dict["[var_id]"] = patent.VarietyId.ToString();
        dict["[date]"] = patent.DateAdguardRegistration.ToString("dd.MM.yyyy");

       
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
        {
            // Настройка диалога сохранения
            saveFileDialog.Filter = "Word Document (*.docx)|*.docx";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.DefaultExt = "docx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "Сохранить в Word";
            saveFileDialog.FileName = $"Отчет_по_заявкам_{DateTime.Now:yyyyMMdd}";
            saveFileDialog.FileName = $"{patent.VarietyName.Replace("/", "_").Replace("\\", "_")}.docx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var word = new WordPatentService();
                    word.Create("templates\\patent_template.docx", saveFileDialog.FileName, dict);

                    MessageBox.Show($"Документ успешно сохранен!\nПуть: {saveFileDialog.FileName}",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Предложение открыть файл
                    if (MessageBox.Show("Открыть сохраненный документ?", "Открыть файл",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        OpenWordDocument(saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении документа: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    /// <summary>
    /// Открытие Word документа
    /// </summary>
    private void OpenWordDocument(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Файл не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось открыть документ: {ex.Message}",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}