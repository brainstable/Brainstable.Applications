using Brainstable.Applications.Models;
using Brainstable.Applications.Services.Implementations;

namespace Brainstable.Applications
{
    /// <summary>
    /// Главная форма приложения: ввод реквизитов выпуска и запуск формирования бюллетеня.
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик кнопки «Сформировать»: проверяет период и запускает генерацию выпуска.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            var bulletin = new Bulletin();

            // Реквизиты выпуска из полей формы.
            bulletin.Number = textBox3.Text.Trim();       // Номер за все время (сквозной)
            bulletin.NumberInYear = textBox2.Text.Trim(); // Номер № (в году)
            bulletin.Year = textBox1.Text.Trim();         // Год выпуска

            // Границы периода отбора: начальная дата не включается, конечная включается.
            bulletin.FirstDate = dateTimePicker1.Value;
            bulletin.LastDate = dateTimePicker2.Value;

            if (bulletin.LastDate <= bulletin.FirstDate)
            {
                MessageBox.Show("Конечная дата должна быть больше начальной", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Генерация синхронная: на большой выгрузке окно будет недоступно до её окончания.
                var bulletinService = new BulletinService(bulletin);
                bulletinService.BuilderBulletin();
            }
        }
    }
}
