using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CirclesSchedule
{
    public partial class MainForm : Form
    {
        private DataGridView dataGridView;
        private Button btnAdd, btnEdit, btnDelete, btnSearch, btnRefresh;
        private TextBox txtSearch;

        public MainForm()
        {
            InitializeComponent();
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            // Таблица
            dataGridView = new DataGridView
            {
                Location = new Point(10, 50),
                Size = new Size(960, 450),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false, // Отключаем автоматическую генерацию
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Добавляем колонки вручную с русскими заголовками
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Название",
                DataPropertyName = "Name"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colLeader",
                HeaderText = "Руководитель",
                DataPropertyName = "Leader"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDays",
                HeaderText = "Дни недели",
                DataPropertyName = "DaysOfWeek"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStart",
                HeaderText = "Время начала",
                DataPropertyName = "StartTime"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEnd",
                HeaderText = "Время окончания",
                DataPropertyName = "EndTime"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colRoom",
                HeaderText = "Кабинет",
                DataPropertyName = "Room"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAge",
                HeaderText = "Возрастная группа",
                DataPropertyName = "AgeGroup"
            });
        }

        private void LoadData()
        {
            var circles = ScheduleLogic.GetAllCircles();
            dataGridView.DataSource = null;
            dataGridView.DataSource = circles;

            // Индикация пустого списка
            if (circles.Count == 0)
            {
                MessageBox.Show("Список кружков пуст. Добавьте первый кружок, нажав кнопку 'Добавить'.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new CircleEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ScheduleLogic.AddCircle(form.CircleName, form.Leader, form.Days, form.StartTime, form.EndTime, form.Room, form.AgeGroup);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;
            int index = dataGridView.CurrentRow.Index;
            var circles = ScheduleLogic.GetAllCircles();
            if (index < 0 || index >= circles.Count) return;

            var current = circles[index];
            using (var form = new CircleEditForm(current))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var updated = new CircleInfo
                    {
                        Name = form.CircleName,
                        Leader = form.Leader,
                        DaysOfWeek = form.Days,
                        StartTime = form.StartTime,
                        EndTime = form.EndTime,
                        Room = form.Room,
                        AgeGroup = form.AgeGroup
                    };
                    ScheduleLogic.UpdateCircle(index, updated);
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;
            int index = dataGridView.CurrentRow.Index;
            if (MessageBox.Show("Удалить выбранный кружок?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ScheduleLogic.DeleteCircle(index);
                LoadData();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();

            // Обработка пустого поиска
            if (string.IsNullOrEmpty(query))
            {
                LoadData();
                return;
            }

            var results = ScheduleLogic.SearchCircles(query);
            dataGridView.DataSource = null;
            dataGridView.DataSource = results;

            if (results.Count == 0)
            {
                MessageBox.Show("По вашему запросу ничего не найдено.", "Результат поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}