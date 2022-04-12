using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{

    class ControlProcedures
    {
        database db;
        public ControlProcedures(database obj)
        {
            db=obj;
        }
        ~ControlProcedures()
        {

        }

        public void Add_land_plot(string name, string square, string date_reg)
        {
            db.Query("EXECUTE Add_land_plot @_name='" + name + "', @_square='" + square + "', @date_reg='" + date_reg + "'");
        }

        public void Upd_land_plot(int id,string name, string square, string date_reg)
        {
            db.Query("EXECUTE Upd_land_plot @id="+id+", @_name='" + name + "', @_square='" + square + "', @date_reg='" + date_reg + "'");
        }

        public void Del_land_plot(int id)
        {
            db.Query("EXECUTE Del_land_plot @id=" + id);
        }

        public void Add_owner(string FIO, string date_of_birth, int partner_FK)
        {
            string partner = "NULL";
            if (partner_FK != 0) partner = partner_FK.ToString();
            //@FIO nvarchar(MAX), @date_of_birth date, @partner_FK int
            db.Query("EXECUTE Add_owners @FIO='" + FIO + "', @date_of_birth='" + date_of_birth + "', @partner_FK=" + partner);
        }

        //Upd_owners(@id int, @FIO nvarchar(MAX), @date_of_birth date, @partner_FK int)
        public void Upd_owners(int id, string FIO, string date_of_birth, int partner_FK)
        {
            string partner = "NULL";
            if (partner_FK != 0) partner = partner_FK.ToString();
            db.Query("EXECUTE Upd_owners @id=" + id + ", @FIO='" + FIO + "', @date_of_birth='" + date_of_birth + "', @partner_FK=" + partner);
        }
        public void Del_owners(int id)
        {
            db.Query("EXECUTE Del_owners @id=" + id);
        }
        //Add_real_estate(@name nvarchar(MAX), @date_reg date, @_address nvarchar(MAX), @_type nvarchar(MAX), @id_land_plot int)
        public void Add_real_estate(string name, string date_reg, string address, string type, int id_land_plot)
        {
            //'" + date_reg + "'
            db.Query("EXECUTE Add_real_estate @name='" + name + "', @date_reg=NULL, @_address='" + address + "', @_type='" + type + "', @id_land_plot=" + id_land_plot);
        }
        public void Upd_real_estate(int id, string name, string date_reg, string address, string type, int id_land_plot)
        {
            db.Query("EXECUTE Upd_real_estate @id="+id+", @name='" + name + "', @date_reg='" + date_reg + "', @_address='" + address + "', @_type='" + type + "', @id_land_plot=" + id_land_plot);
        }
        public void Del_real_estate(int id)
        {
            db.Query("EXECUTE Del_real_estate @id=" + id);
        }

        public void Try_ChangeEstate_LandPlot(int id, int id_land_plot)
        {
            db.Query("EXECUTE Try_ChangeEstate_LandPlot @id=" + id + ", @id_land_plot=" + id_land_plot);
        }

        //Add_directory(@id_owner int, @id_estate int, @date_contract date, @price float)
        public void Add_directory(int id_owner, int id_estate, string date_contract, string price)
        {
            db.Query("EXECUTE Add_directory @id_owner=" + id_owner + ", @id_estate=" + id_estate + ", @date_contract='" + date_contract + "', @price='" + price+"'");
        }
        public void Upd_directory(int id, int id_owner, int id_estate, string date_contract, string price)
        {
            db.Query("EXECUTE Upd_directory @id=" + id + ", @id_owner=" + id_owner + ", @id_estate=" + id_estate + ", @date_contract='" + date_contract + "', @price='" + price+"'");
        }
        public void Del_directory(int id)
        {
            db.Query("EXECUTE Del_directory @id=" + id);
        }

    }
}
