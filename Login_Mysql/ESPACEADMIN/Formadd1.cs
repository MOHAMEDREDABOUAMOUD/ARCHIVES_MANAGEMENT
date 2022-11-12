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
    public partial class Formadd1 : Form
    {
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        int cp;
        public static string date, day, month, year, s;

        public Formadd1()
        {
            InitializeComponent();
        }

        private void Formadd1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "yyyy mm dd";
            comboBox1.Items.Add("admin");
            comboBox1.Items.Add("chef division");
            comboBox1.Items.Add("chef service");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="" && textBox10.Text!="" && textBox11.Text!="" && textBox2.Text!="" && textBox4.Text!="" && textBox7.Text!="" && textBox8.Text!="" && comboBox1.SelectedIndex!=-1)
            {
                if(textBox7.Text.Contains("@"))
                {
                    date = dateTimePicker1.Text;
                    cp = 0; s = "";
                    for (int i = 0; i < date.Length;i++)
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
                        switch (comboBox1.SelectedItem.ToString())
                        {
                            case "admin":
                                s = "1"; break;
                            case "chef division":
                                s = "2"; break;
                            case "chef service":
                                s = "3"; break;
                        }
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("insert into utilisateur(nom,prenom,date_naissance,adresse,grade,IDrole,email,num_Tel,login,pwd) values('" + textBox1.Text + "','" + textBox2.Text + "','" + year + "-" + month + "-" + day + "','" + textBox4.Text + "','" + comboBox1.SelectedItem.ToString() + "'," + s + ",'" + textBox7.Text + "','" + textBox8.Text + "','" + textBox10.Text + "','" + textBox11.Text + "')", connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                    OpenChildForm(new Formusr1());
                }
                else
                {
                    MessageBox.Show("le format de l'email est incorrect");
                }
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
