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
    public partial class StartMenu : Form
    {
        database coursework;
        public StartMenu()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StartMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (coursework!=null)
            coursework.CloseConnect();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {//пароли, гость: User:1234     MySQL:MySQL
            coursework = new database("coursework", textBox1.Text, textBox2.Text);
            if (coursework.isOpen())
            {
                List<string> result=coursework.ReadOrderData("USE [coursework] IF IS_ROLEMEMBER ('db_owner', '" + textBox1.Text + "') = 1 SELECT '1' AS result ELSE SELECT '0' AS result");
                bool isAdmin=false;
                if (result.Count>0&&result[0]=="1") isAdmin=true;
                result.Clear();
                (new MenuForm(isAdmin, coursework, this)).Show();
                this.Hide();
                //MessageBox.Show("Подключились!");
            }
            else
            {
                MessageBox.Show("Ошибка авторизации!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
