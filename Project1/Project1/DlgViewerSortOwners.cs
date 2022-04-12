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
    public partial class DlgViewerSortOwners : Form
    {
        string[] items_columns;
        int indexColumn;
        int indexSort;
        public int getIndexColumn() { return indexColumn; }
        public int getIndexSort() { return indexSort;  }
        public DlgViewerSortOwners(string [] colums)
        {
            InitializeComponent();
            items_columns = colums;
            indexColumn = -1;
            indexSort = -1;
            
        }
        ~DlgViewerSortOwners()
        {

        }
        private void DlgViewerSortOwners_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            for (int i = 0; i < items_columns.Length;i++)
            {
                comboBox1.Items.Add(items_columns[i]);
            }
            comboBox2.Items.Add("Возрастанию");
            comboBox2.Items.Add("Убыванию");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexColumn = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            indexSort = comboBox2.SelectedIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            indexColumn = -1;
            indexSort = -1;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (indexSort != -1 && indexColumn != -1)
                this.Close();
            else MessageBox.Show("Необходимо выбрать и заполнить все", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DlgViewerSortOwners_FormClosed(object sender, FormClosedEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
        }
    }
}
