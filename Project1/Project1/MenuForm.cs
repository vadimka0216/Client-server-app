using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class MenuForm : Form
    {
        database coursework;
        Form MainForm;
        public MenuForm(bool isAdmin, object db, object form)
        {
            InitializeComponent();
            setBoxAction(isAdmin);
            coursework = (database)db;
            MainForm = (Form)form;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             
        }

        private void setBoxAction(bool high = false)
        {
            comboBox1.Items.Clear();
            if (high)
            {
                comboBox1.Items.Add("Добавить/изменить/удалить земельный участок");
                comboBox1.Items.Add("Добавить/изменить/удалить владельца");
                comboBox1.Items.Add("Добавить/изменить/удалить недвижимость");
                comboBox1.Items.Add("Добавить/изменить/удалить свидетельство");
                comboBox1.Items.Add("Просмотр таблиц");
            }
            else
            {
                comboBox1.Items.Add("Просмотр таблиц");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите действие!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox1.Items.Count > 1)
            {//если админ
                if (comboBox1.SelectedIndex == 4) (new FormViewer(coursework)).ShowDialog();
                else
                (new FormEdit(coursework, comboBox1.SelectedIndex)).ShowDialog();
            }
            else (new FormViewer(coursework)).ShowDialog();

        }

        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Close();
        }
    }
}
