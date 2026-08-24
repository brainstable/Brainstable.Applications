using Brainstable.Applications.Models;
using Brainstable.Applications.Services.Implementations;

namespace Brainstable.Applications
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bulletin = new Bulletin();
            bulletin.Number = textBox3.Text.Trim();
            bulletin.NumberInYear = textBox2.Text.Trim();
            bulletin.Year = textBox1.Text.Trim();
            bulletin.FirstDate = dateTimePicker1.Value;
            bulletin.LastDate = dateTimePicker2.Value;

            if (bulletin.LastDate <= bulletin.FirstDate)
            {
                MessageBox.Show("Конечная дата должна быть больше начальной", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var bulletinService = new BulletinService(bulletin);
                bulletinService.BuilderBulletin();
            }
        }
    }
}