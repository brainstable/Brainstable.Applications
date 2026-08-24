namespace Brainstable.Applications
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(272, 349);
            button1.Name = "button1";
            button1.Size = new Size(231, 63);
            button1.TabIndex = 5;
            button1.Text = "Сформировать";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(227, 85);
            label1.Name = "label1";
            label1.Size = new Size(81, 20);
            label1.TabIndex = 1;
            label1.Text = "Номер №:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(154, 133);
            label2.Name = "label2";
            label2.Size = new Size(154, 20);
            label2.TabIndex = 3;
            label2.Text = "Номер за все время:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(272, 40);
            label3.Name = "label3";
            label3.Size = new Size(36, 20);
            label3.TabIndex = 5;
            label3.Text = "Год:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(162, 190);
            label4.Name = "label4";
            label4.Size = new Size(146, 20);
            label4.TabIndex = 7;
            label4.Text = "Начальная дата (>):";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(160, 236);
            label5.Name = "label5";
            label5.Size = new Size(148, 20);
            label5.TabIndex = 8;
            label5.Text = "Конечная дата (<=):";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(329, 185);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(198, 27);
            dateTimePicker1.TabIndex = 3;
            dateTimePicker1.Value = new DateTime(2025, 5, 20, 0, 0, 0, 0);
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(329, 231);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(198, 27);
            dateTimePicker2.TabIndex = 4;
            dateTimePicker2.Value = new DateTime(2025, 6, 30, 0, 0, 0, 0);
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Location = new Point(329, 37);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(198, 27);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Location = new Point(329, 82);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(198, 27);
            textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Location = new Point(329, 130);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(198, 27);
            textBox3.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ФОРМИРОВАНИЕ БЮЛЛЕТЕНЕЙ";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
    }
}
