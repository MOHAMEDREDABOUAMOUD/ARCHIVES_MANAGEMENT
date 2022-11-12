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

namespace Login_Mysql
{
    public partial class Formmodify : Form
    {
        string date, year, month, day, s;
        int cp, id;
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        
        public Formmodify()
        {
            InitializeComponent();
        }

        private void Formmodify_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "yyyy mm dd";
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select id from utilisateur where nom like '" + textBox1.Text + "' and prenom like '" + textBox2.Text + "' and email like '"+textBox7.Text+"'", connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
            }
            connexion.Close();
        }

        private void OpenChildForm(Form childForm)
        {

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            ESPACE_ADMIN EA = (ESPACE_ADMIN)Application.OpenForms["ESPACE_ADMIN"];
            Panel panel2 = (Panel)EA.Controls["panel2"];
            panel2.Controls.Add(childForm);
            panel2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Formusr1());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="" && textBox10.Text!="" && textBox2.Text!="" && textBox4.Text!="" && textBox7.Text!="" && textBox8.Text!="" && comboBox1.SelectedIndex!=-1)
            {
                date = dateTimePicker1.Text;
                cp = 0; s = "";
                for (int i = 0; i < date.Length; i++)
                {
                    if (date[i] == '/')
                    {
                        if (cp == 0)
                        {
                            day = s; cp++; s = "";
                        }
                        else if (cp == 1)
                        {
                            month = s; cp++; s = "";
                        }
                    }
                    else s += date[i];
                }
                year = s;
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        s = "1"; break;
                    case 1:
                        s = "2"; break;
                    case 2:
                        s = "3"; break;
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("update utilisateur set nom='" + textBox1.Text + "',prenom='" + textBox2.Text + "',date_naissance='" + year + "-" + month + "-" + day + "',num_Tel='" + textBox8.Text + "',email='" + textBox7.Text + "',adresse='" + textBox4.Text + "',IDrole=" + s + ",grade='" + comboBox1.SelectedItem.ToString() + "',login='" + textBox10.Text + "' where id =" + id, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                OpenChildForm(new Formusr1());
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }
    }
}
