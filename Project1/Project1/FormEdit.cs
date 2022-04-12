using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Project1.Tables;
using System.Collections.ObjectModel;

using System.Text.RegularExpressions;

namespace Project1
{
    public partial class FormEdit : Form
    {
        ReaderTables viewDB;
        database db;
        int tab = -1;
        public FormEdit()
        {
            InitializeComponent();
        }
        public FormEdit(object obj)
        {
            InitializeComponent();
            db = (database)obj;
            viewDB = new ReaderTables(db);
        }
        public FormEdit(object obj, int edit)
        {
            InitializeComponent();
            db = (database)obj;
            viewDB = new ReaderTables(db);
            tab = edit;
        }

        private void FormEdit_Load(object sender, EventArgs e)
        {
            //viewDB.UpdAllTables();

            tabControl1.TabPages[0].Text = "land_plot";
            tabControl1.TabPages[1].Text = "owners";
            tabControl1.TabPages[2].Text = "real_estate";
            tabControl1.TabPages[3].Text = "directory";

            if (tab >= 0) tabControl1.SelectedIndex = tab;

            this.Upd_land_plot();//важна последовательность
            this.Upd_owners();
            this.Upd_estate();
            this.Upd_Directory();

            //tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControl1_Selected);

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           switch (tabControl1.SelectedIndex)
           {
               case 0:
                   {
                       this.Upd_land_plot();
                       break;
                   }
               case 1:
                   {
                       this.Upd_owners();
                       break;
                   }
               case 2:
                   {
                       this.Upd_estate();
                       break;
                   }
               case 3:
                   {
                       this.Upd_Directory();
                       break;
                   }
           }
        }

        private void Upd_land_plot()
        {
            viewDB.Upd_land_plot();
            comboBox5.Items.Clear();
            comboBox5.Items.Add("Добавить новую запись");
            //чтение из таблицы 1
            {
                ReadOnlyCollection<land_plot> table = viewDB.getLandPlot();
                for (int i = 0; i < table.Count; i++)
                    comboBox5.Items.Add(table[i].name + " | " + table[i].square + " | " + table[i].date_reg);
            }
        }

        private void Upd_owners()
        {
            viewDB.Upd_owners();
            comboBox6.Items.Clear();
            comboBox6.Items.Add("Добавить новую запись");
            comboBox2.Items.Clear();
            //чтение из таблицы 2
            {
                comboBox2.Items.Add("Без партнера");
                ReadOnlyCollection<owners> table = viewDB.getOwners();
                for (int i = 0; i < table.Count; i++)
                {
                    comboBox6.Items.Add(table[i].FIO + " | " + table[i].date_of_birth + " | " + table[i].partner_FK);
                    comboBox2.Items.Add(table[i].FIO);
                }
            }
        }

        private void Upd_estate()
        {
            viewDB.Upd_real_estate();
           // viewDB.Upd_land_plot();//??
            comboBox7.Items.Clear();
            comboBox7.Items.Add("Добавить новую запись");
            //чтение из таблицы 3
            {
                ReadOnlyCollection<real_estate> table = viewDB.getEstate();
                for (int i = 0; i < table.Count; i++)
                    comboBox7.Items.Add(table[i].name + " | " + table[i].date_reg + " | " + table[i].address + " | " + table[i].type + " | " + table[i].id_land_plot);
            }
            comboBox1.Items.Clear();
            {
                ReadOnlyCollection<land_plot> table = viewDB.getLandPlot();
                for (int i = 0; i < table.Count; i++)
                    comboBox1.Items.Add(table[i].name + " | площадь: " + table[i].square);
            }

            checkBox1.Checked = false;
        }

        private void Upd_Directory()
        {
            viewDB.Upd_directory();
            comboBox8.Items.Clear();
            comboBox8.Items.Add("Добавить новую запись");
            //чтение из таблицы 4
            {
                ReadOnlyCollection<directory> table = viewDB.getDirectory();
                for (int i = 0; i < table.Count; i++)
                    comboBox8.Items.Add(table[i].id + " | " + table[i].date_contract + " | " + table[i].price);
            }
            comboBox3.Items.Clear();
            {
                ReadOnlyCollection<owners> table = viewDB.getOwners();
                for (int i = 0; i < table.Count; i++)
                    comboBox3.Items.Add(table[i].FIO);
            }
            comboBox4.Items.Clear();
            {
                ReadOnlyCollection<real_estate> table = viewDB.getEstate();
                for (int i = 0; i < table.Count; i++)
                    comboBox4.Items.Add(table[i].name + " | тип: " + table[i].type);
            }
        }

        private void onKeyPressAll(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 34 || e.KeyChar == 39)
            {
                e.Handled = true;
                MessageBox.Show("Введен запрещенный знак!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button1_Click(object sender, EventArgs e)//add or upd land_plot
        {
            if (comboBox5.SelectedIndex >= 1)
            {
                int id = int.Parse(textBox12.Text);//comboBox5.SelectedIndex
                (new ControlProcedures(db)).Upd_land_plot(id, textBox1.Text, textBox2.Text, textBox5.Text);
            }
            else
            {
                (new ControlProcedures(db)).Add_land_plot(textBox1.Text, textBox2.Text, textBox5.Text);
            }
            Upd_land_plot();
        }

        private void button5_Click(object sender, EventArgs e)//delete land_plot
        {
            if (comboBox5.SelectedIndex >= 1)
            {
                int id = int.Parse(textBox12.Text);//comboBox5.SelectedIndex
                (new ControlProcedures(db)).Del_land_plot(id);
                Upd_land_plot();
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {//заполнение полей данными(чтение из таблицы 1)
            if (comboBox5.SelectedIndex >= 1)
            {
                int i=comboBox5.SelectedIndex-1;
                ReadOnlyCollection<land_plot> table = viewDB.getLandPlot();
                textBox1.Text = table[i].name;
                textBox2.Text = table[i].square;
                textBox5.Text = table[i].date_reg;
                textBox12.Text = table[i].id;
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox5.Text = "";
                textBox12.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int partner = 0;
            if (comboBox2.SelectedIndex > 0) partner = int.Parse(viewDB.getOwners()[comboBox2.SelectedIndex-1].id);
            int id = -1;
            if (textBox13.Text!="") id=int.Parse(textBox13.Text);
            if (partner==id)
            {
                MessageBox.Show("Нельзя выбрать в качестве партнера самого себя!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox6.SelectedIndex >= 1)
            {
                (new ControlProcedures(db)).Upd_owners(id, textBox4.Text, textBox3.Text, partner);
            }
            else
            {
                (new ControlProcedures(db)).Add_owner(textBox4.Text, textBox3.Text, partner);
            }
            this.Upd_owners();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex >= 1)
            {
                int id = int.Parse(textBox13.Text);//comboBox5.SelectedIndex
                (new ControlProcedures(db)).Del_owners(id);
                Upd_owners();
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex >= 1)
            {
                int i = comboBox6.SelectedIndex - 1;
                ReadOnlyCollection<owners> table = viewDB.getOwners();
                textBox13.Text = table[i].id;
                textBox4.Text = table[i].FIO;
                textBox3.Text = table[i].date_of_birth;
                int j = -1;
                //MessageBox.Show(table[i].partner_FK);
                if (table[i].partner_FK!="")
                {
                    for (j = 0; j < table.Count; j++)
                        if (table[i].partner_FK==table[j].id)
                            break;
                }
                comboBox2.SelectedIndex = j+1;
            }
            else
            {
                textBox13.Text = "";
                textBox4.Text = "";
                textBox3.Text = "";
                comboBox2.SelectedIndex = -1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //7,6,8,9
            int id_land_plot=0;
            if (comboBox1.SelectedIndex>=0) id_land_plot=int.Parse(viewDB.getLandPlot()[comboBox1.SelectedIndex].id);
            else
            {
                MessageBox.Show("Необходимо выбрать земельный участок для недвижимости!", "Предупреждение",MessageBoxButtons.OK, MessageBoxIcon.Warning );
                return;
            }
            if (!checkBox1.Checked)
            {
                if (comboBox7.SelectedIndex >= 1)
                {
                    int id = int.Parse(textBox14.Text);
                    (new ControlProcedures(db)).Upd_real_estate(id, textBox7.Text, textBox6.Text, textBox8.Text, textBox9.Text, id_land_plot);
                }
                else
                {
                    (new ControlProcedures(db)).Add_real_estate(textBox7.Text, textBox6.Text, textBox8.Text, textBox9.Text, id_land_plot);
                }
            }
            else 
            {
                int id = int.Parse(textBox14.Text);
                (new ControlProcedures(db)).Try_ChangeEstate_LandPlot(id, id_land_plot);
            }
            this.Upd_estate();
        }

        private void button7_Click(object sender, EventArgs e)//delete estate
        {
            if (comboBox7.SelectedIndex >= 1)
            {
                int id = int.Parse(textBox14.Text);
                (new ControlProcedures(db)).Del_real_estate(id);
                this.Upd_estate();
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateTabEstate()
        {
            int i = comboBox7.SelectedIndex - 1;
            ReadOnlyCollection<real_estate> table = viewDB.getEstate();
            textBox7.Text = table[i].name;
            textBox6.Text = table[i].date_reg;
            textBox8.Text = table[i].address;
            textBox9.Text = table[i].type;
            textBox14.Text = table[i].id;
            int j = 0;
            ReadOnlyCollection<land_plot> table2 = viewDB.getLandPlot();
            for (j = 0; j < table2.Count; j++)
                if (table[i].id_land_plot == table2[j].id)
                {
                    break;
                }
            comboBox1.SelectedIndex = j;
        }

        private void TurnTextBoxTabEstate(bool flag)
        {
            textBox7.Enabled = flag;
            //textBox6.Enabled = flag;
            textBox8.Enabled = flag;
            textBox9.Enabled = flag;
            textBox14.Enabled = flag;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            TurnTextBoxTabEstate(true);
            checkBox1.Checked = false;
            if (comboBox7.SelectedIndex >= 1)
            {
                UpdateTabEstate();
            }
            else
            {
                textBox7.Text = "";
                textBox6.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                textBox14.Text = "";
                comboBox1.SelectedIndex = -1;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (comboBox7.SelectedIndex >= 1)
                {
                    UpdateTabEstate();
                    TurnTextBoxTabEstate(false);
                }
                else
                {
                    MessageBox.Show("Не выбрана запись для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBox1.Checked = false;
                }
            }
            else
            {
                TurnTextBoxTabEstate(true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id_estate = 0;
            int id_owner = 0;
            if (comboBox4.SelectedIndex >= 0) id_estate = int.Parse(viewDB.getEstate()[comboBox4.SelectedIndex].id);
            if (comboBox3.SelectedIndex >= 0) id_owner = int.Parse(viewDB.getOwners()[comboBox3.SelectedIndex].id);
            if (id_estate == 0 || id_owner == 0)
            {
                MessageBox.Show("Необходимо выбрать недвижимость и владельца!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string price;
            if (new Regex("^(\\d+),(\\d+)$").IsMatch(textBox10.Text))
            {
                price = textBox10.Text.Replace(',', '.');
            }
            else if (textBox10.Text != "" && ((new Regex("^(\\d+)$")).IsMatch(textBox10.Text) || (new Regex("^(\\d+)\\.(\\d+)$")).IsMatch(textBox10.Text)))
            {
                price = textBox10.Text;
            }
            else
            {
                MessageBox.Show("Необходимо указать цену корректно! (Пример: 111.11)", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox8.SelectedIndex >= 1)
            {
                int id = int.Parse(textBox15.Text);
                (new ControlProcedures(db)).Upd_directory(id, id_owner, id_estate, textBox11.Text, price);
            }
            else
            {
                List<string> count_fails=db.ReadOrderData("SELECT COUNT(*) FROM directory WHERE directory.id_owner=" + id_owner + " AND directory.id_estate=" + id_estate);
                if (count_fails[0] == "0")
                {
                    (new ControlProcedures(db)).Add_directory(id_owner, id_estate, textBox11.Text, price);
                }
                else
                {
                    MessageBox.Show("Такая недвижимость уже имеется у владельца!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                count_fails.Clear();
            }
            this.Upd_Directory();
        }

        private void button8_Click(object sender, EventArgs e)//delete directory 
        {
            if (comboBox8.SelectedIndex >= 1)
            {
                int id = int.Parse(textBox15.Text);
                (new ControlProcedures(db)).Del_directory(id);
                this.Upd_Directory();
            }
            else
            {
                MessageBox.Show("Выберите запись!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex >= 1)
            {
                int i = comboBox8.SelectedIndex - 1;
                ReadOnlyCollection<directory> table = viewDB.getDirectory();
                textBox11.Text = table[i].date_contract;
                textBox10.Text = table[i].price;
                textBox15.Text = table[i].id;
                int j = 0;
                {
                    ReadOnlyCollection<real_estate> table2 = viewDB.getEstate();
                    for (j = 0; j < table2.Count; j++)
                        if (table[i].id_estate == table2[j].id)
                            break;
                }
                comboBox4.SelectedIndex = j;
                {
                    ReadOnlyCollection<owners> table2 = viewDB.getOwners();
                    for (j = 0; j < table2.Count; j++)
                        if (table[i].id_owner == table2[j].id)
                            break;
                }
                comboBox3.SelectedIndex = j;
            }
            else
            {
                textBox11.Text = "";
                textBox10.Text = "";
                textBox15.Text = "";
                comboBox4.SelectedIndex = comboBox3.SelectedIndex = -1;
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {

        }
        
    }
}
