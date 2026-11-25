using System;
using System.Drawing;
using System.Windows.Forms;

namespace CirclesSchedule
{
    public class CircleEditForm : Form
    {
        private TextBox txtName, txtLeader, txtDays, txtStart, txtEnd, txtRoom, txtAge;

        public string CircleName { get { return txtName.Text.Trim(); } }
        public string Leader { get { return txtLeader.Text.Trim(); } }
        public string Days { get { return txtDays.Text.Trim(); } }
        public string StartTime { get { return txtStart.Text.Trim(); } }
        public string EndTime { get { return txtEnd.Text.Trim(); } }
        public string Room { get { return txtRoom.Text.Trim(); } }
        public string AgeGroup { get { return txtAge.Text.Trim(); } }

        public CircleEditForm() : this(null) { }

        public CircleEditForm(CircleInfo existing)
        {
            this.Text = existing == null ? "Добавить кружок" : "Редактировать кружок";
            this.Size = new Size(400, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int y = 20;
            int labelWidth = 150;
            int inputWidth = 200;

            txtName = AddField("Название кружка:", ref y, labelWidth, inputWidth, existing != null ? existing.Name : "");
            txtLeader = AddField("Руководитель (ФИО):", ref y, labelWidth, inputWidth, existing != null ? existing.Leader : "");
            txtDays = AddField("Дни недели (Пн, Ср, Пт):", ref y, labelWidth, inputWidth, existing != null ? existing.DaysOfWeek : "");
            txtStart = AddField("Время начала (15:00):", ref y, labelWidth, inputWidth, existing != null ? existing.StartTime : "");
            txtEnd = AddField("Время окончания (16:30):", ref y, labelWidth, inputWidth, existing != null ? existing.EndTime : "");
            txtRoom = AddField("Кабинет:", ref y, labelWidth, inputWidth, existing != null ? existing.Room : "");
            txtAge = AddField("Возрастная группа:", ref y, labelWidth, inputWidth, existing != null ? existing.AgeGroup : "");

            var btnOk = new Button { Text = "ОК", DialogResult = DialogResult.None, Location = new Point(100, y + 10), Size = new Size(80, 30) };
            var btnCancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, Location = new Point(200, y + 10), Size = new Size(80, 30) };

            btnOk.Click += BtnOk_Click;

            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // Валидация времени начала
            DateTime startTimeParsed;
            if (!DateTime.TryParse(txtStart.Text.Trim(), out startTimeParsed))
            {
                MessageBox.Show("Некорректный формат времени начала. Используйте формат ЧЧ:ММ (например, 15:00).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStart.Focus();
                return;
            }

            // Валидация времени окончания
            DateTime endTimeParsed;
            if (!DateTime.TryParse(txtEnd.Text.Trim(), out endTimeParsed))
            {
                MessageBox.Show("Некорректный формат времени окончания. Используйте формат ЧЧ:ММ (например, 16:30).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEnd.Focus();
                return;
            }

            // Проверка, что время окончания позже времени начала
            if (endTimeParsed <= startTimeParsed)
            {
                MessageBox.Show("Время окончания должно быть позже времени начала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEnd.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private TextBox AddField(string label, ref int y, int labelWidth, int inputWidth, string value)
        {
            var lbl = new Label { Text = label, Location = new Point(10, y + 3), AutoSize = true };
            var txt = new TextBox { Location = new Point(labelWidth + 15, y), Size = new Size(inputWidth, 20), Text = value ?? "" };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            y += 35;
            return txt;
        }
    }
}