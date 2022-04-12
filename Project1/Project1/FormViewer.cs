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
    public partial class FormViewer : Form
    {
        database db;
       /* public FormViewer()
        {
            InitializeComponent();
        }*/
        public FormViewer(object obj)
        {
            InitializeComponent();
            db = (database)obj;
            listView1.GridLines = true;//разделение столбцов
            listView1.FullRowSelect = true;//выделение всей строки
            listView1.View = View.Details;//для отображения текста

        }

        private void PrintList(string sql_cmd, string[] columns)
        {
            listView1.Clear();

            for (int i=0; i<columns.Length; i++)
                listView1.Columns.Add(columns[i]);

            List<string> table = db.ReadOrderData(sql_cmd);
            for (int i = 0; i < table.Count; i += columns.Length)
            {
                string[] elements = new string[columns.Length];
                for (int j = 0; j < elements.Length; j++)
                    elements[j] = table[i + j];
                listView1.Items.Add(new ListViewItem(elements));
            }
            table.Clear();

            for (int i=0; i<columns.Length; i++)//resize
                listView1.AutoResizeColumn(i,ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        //test, 8 задание
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            /*
            PrintList("SELECT real_estate.id,real_estate._name,real_estate.date_reg,real_estate.address,real_estate._type, land_plot._name, land_plot._square, land_plot.date_reg FROM real_estate JOIN land_plot ON (land_plot.id=real_estate.id_land_plot)",
                new string[]{"id","Название","Дата регистрации","Адрес","Тип","Земельный участок","Площадь земли",
                    "Дата регистрации участка"
                });*/
            PrintList("SELECT * FROM getEstates();", 
                new string[]{"id","Название","Дата регистрации","Адрес","Тип","Земельный участок","Площадь земли",
                    "Дата регистрации участка"});

        }
        //задание 2.a
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            PrintList("SELECT owners.id, owners.FIO,owners.date_of_birth, "+
	        "CASE " +
		        "WHEN (SELECT COUNT(*) FROM directory WHERE directory.id_owner=owners.id)>2 THEN 'более 2 бизнесов' "+
		        "WHEN (SELECT COUNT(*) FROM directory WHERE directory.id_owner=owners.id)=2 THEN '2 бизнеса' "+
		        "WHEN (SELECT COUNT(*) FROM directory WHERE directory.id_owner=owners.id)=1 THEN '1 бизнес' "+
		        "ELSE 'Не имеет бизнесов' "+
	        "END "+
	        "business FROM owners",
            new string[]{"id","FIO","Дата рождения","Количество бизнесов"});

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }

        private void FormViewer_Load(object sender, EventArgs e)
        {

        }

        //задание 2.c.1
        private void toolStripButton6_Click(object sender, EventArgs e)
        {//выводит содержимое справочника и считает количество владельцев(людей) в базе. Исключительно для задания
            PrintList("SELECT owners.FIO, real_estate._name, (SELECT COUNT(*) FROM owners) ofAllOwners, (SELECT COUNT(*) FROM directory as select_directory WHERE owners.id=select_directory.id_owner) CountOwners FROM  directory JOIN owners ON (directory.id_owner=owners.id) JOIN real_estate ON (directory.id_estate=real_estate.id);",
                new string[] { "ФИО", "Название недвижимости", "Из всех владельцев", "Количество имуществ" });
        }
        //задание 2.c.2
        private void toolStripButton7_Click(object sender, EventArgs e)
        {//
            PrintList("SELECT _owners.id, _owners.FIO,_owners.date_of_birth FROM (SELECT * FROM owners as _table) _owners;",
                new string[]{"id","ФИО","Дата рождения"});
        }
        //задание 2.c.3
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            PrintList("SELECT land_plot.* FROM land_plot WHERE land_plot.id in (SELECT real_estate.id_land_plot FROM real_estate WHERE real_estate._type = 'Квартира' OR real_estate._type='квартира' );",
                new string[] { "id", "Название", "Площадь", "Дата регистрации" });
        }
        //задание 2.d
        private void toolStripButton9_Click(object sender, EventArgs e)
        {//d.Многотабличный запрос, содержащий группировку записей,
            //--статистика покупок недвижимости по датам(для сделок дороже 10)
            PrintList("SELECT directory.date_contract, SUM(directory.price) as summ FROM directory GROUP BY directory.date_contract HAVING AVG(directory.price)>10;",
                new string[] { "Дата", "Сумма покупок за день"});
        }

        //Задание 2.e
        private void toolStripButton10_Click(object sender, EventArgs e)
        {//--вывести недвижимости, которыми никто не владеет
            PrintList("SELECT real_estate._name, real_estate._type, real_estate.date_reg, real_estate.address FROM real_estate WHERE NOT real_estate.id = ANY (SELECT directory.id_estate FROM directory);",
                new string[] { "Название", "Вид имущества", "Дата регистрации", "Адрес" });
        }

        //Задание 2.b
        private void toolStripButton4_Click(object sender, EventArgs e)
        {//вывести (топовых бизнесменов) владельцев с их ценой имущества по указанной сортировке
            string[] columns = new string[] { "id", "ФИО", "Дата рождения", "Стоимость имуществ" };
            DlgViewerSortOwners dlg = new DlgViewerSortOwners(columns);
            dlg.ShowDialog();
            if (dlg.getIndexColumn()!=-1)
            {
                string query = "SELECT * FROM top_owners ORDER BY top_owners.";
                switch (dlg.getIndexColumn())
                {
                    case 0:
                        query += "id";
                        break;
                    case 1:
                        query += "FIO";
                        break;
                    case 2:
                        query += "date_of_birth";
                        break;
                    case 3:
                        query += "Sum_Estate";
                        break;
                }
                if (dlg.getIndexSort() == 1) query += " DESC";


                PrintList(query, columns);
            }
            dlg.Dispose();
            dlg = null;
        }
    }
}
