using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Login_Mysql.ESPACEADMIN
{
    public partial class dashboard : Form
    {
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        
        public dashboard()
        {
            InitializeComponent();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select intitule from division",connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                comboBox1.Items.Add(rdr.GetString(0));
            }
            connexion.Close();
            label4.Text = "";
            label6.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Today;
            string div = comboBox1.Text;
            comboBox2.Items.Clear();
            comboBox2.Text = "";
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select nom_serv from service S inner join division D on S.id_division=D.id where D.intitule like '"+div+"'",connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                comboBox2.Items.Add(rdr.GetString(0));
            }
            connexion.Close();

            string year = dateTime.ToString("yyyy"), month = dateTime.ToString("MM");

            connexion.Open();
            cmd = new MySqlCommand("select count(A.id) from archivage A inner join division D on D.id=A.id_division where D.intitule like '"+div+"' and A.date_Arch like '"+year+"%'",connexion);
            rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                label6.Text = rdr.GetInt32(0).ToString();
            }
            connexion.Close();

            connexion.Open();
            cmd = new MySqlCommand("select count(A.id) from archivage A inner join division D on D.id=A.id_division where D.intitule like '"+div+"' and A.date_Arch like '" + year +"-"+month+"%'", connexion);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                label4.Text = rdr.GetInt32(0).ToString();
            }
            connexion.Close();
            string[] tab={"janv","fev","mar","avr","mai","juin","juil","aout","sept","oct","nov","dec"};
            chart1.Series["archives"].Points.Clear();
            chart1.Series["archives"].Points.AddXY("", 0);
            for (int i = 1; i <= 12;i++)
            {
                connexion.Open();
                if(i<10) cmd = new MySqlCommand("select count(A.id) from archivage A inner join division D on D.id=A.id_division where D.intitule like '" + div + "' and A.date_Arch like '" + year + "-0" + i + "%'", connexion);
                else cmd = new MySqlCommand("select count(A.id) from archivage A inner join division D on D.id=A.id_division where D.intitule like '" + div + "' and A.date_Arch like '" + year + "-" + i + "%'", connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    chart1.Series["archives"].Points.AddXY(tab[i-1],rdr.GetInt32(0));
                }
                else
                {
                    chart1.Series["archives"].Points.AddXY(tab[i - 1], 0);
                }
                connexion.Close();
                MessageBox.Show(tab[i - 1]);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Today;
            string year = dateTime.ToString("yyyy"), month = dateTime.ToString("MM");
            string serv = comboBox2.Text;

            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select count(A.id) from archivage A inner join service S on S.id=A.id_service where S.nom_serv like '" + serv + "' and A.date_Arch like '" + year + "%'", connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                label6.Text = rdr.GetInt32(0).ToString();
            }
            connexion.Close();

            connexion.Open();
            cmd = new MySqlCommand("select count(A.id) from archivage A inner join service S on S.id=A.id_service where S.nom_serv like '" + serv + "' and A.date_Arch like '" + year +"-"+month+ "%'", connexion);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                label4.Text = rdr.GetInt32(0).ToString();
            }
            connexion.Close();

            string[] tab = { "janv", "fev", "mar", "avr", "mai", "juin", "juil", "aout", "sept", "oct", "nov", "dec" };
            chart1.Series["archives"].Points.Clear();
            chart1.Series["archives"].Points.AddXY("", 0);
            for (int i = 1; i <= 12; i++)
            {
                connexion.Open();
                if (i < 10) cmd = new MySqlCommand("select count(A.id) from archivage A inner join service S on S.id=A.id_service where S.nom_serv like '" + serv + "' and A.date_Arch like '" + year + "-0" + i + "%'", connexion);
                else cmd = new MySqlCommand("select count(A.id) from archivage A inner join service S on S.id=A.id_service where S.nom_serv like '" + serv + "' and A.date_Arch like '" + year + "-" + i + "%'", connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    chart1.Series["archives"].Points.AddXY(tab[i - 1], rdr.GetInt32(0));
                }
                else
                {
                    chart1.Series["archives"].Points.AddXY(tab[i - 1], 0);
                }
                connexion.Close();
            }
        }
    }
}
