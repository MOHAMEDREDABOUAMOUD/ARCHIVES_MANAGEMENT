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
    public partial class Formusr1 : Form
    {
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        int cp;
        string idchefdiv,idchefserv;
        public static string date, day, month, year, s,name,nom="",prenom="";

        public Formusr1()
        {
            InitializeComponent();
        }

        private void Formusr1_Load(object sender, EventArgs e)
        {
            initdatagrid();
        }

        private void initdatagrid()
        {
            dataGridView1.ColumnCount = 10;
            dataGridView1.Columns[0].Name = "Nom";
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Name = "Prenom";
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Name = "Date de naissance";
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Name = "Adresse";
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Name = "grade";
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[5].Name = "email";
            dataGridView1.Columns[5].Width = 300;
            dataGridView1.Columns[6].Name = "num_tele";
            dataGridView1.Columns[6].Width = 200;
            dataGridView1.Columns[7].Name = "ID";
            dataGridView1.Columns[8].Width = 100;
            dataGridView1.Columns[8].Name = "Login";
            dataGridView1.Columns[8].Width = 100;
            dataGridView1.Columns[9].Name = "Password";
            dataGridView1.Columns[9].Width = 110;
            dataGridView1.RowCount = 1;
            dataGridView1.RowHeadersVisible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(textBox12.Text!="")
            {
                name = textBox12.Text;
                bool trouve = false;
                for (int i = 0; i < name.Length; i++)
                {
                    if (name[i] == ' ')
                    {
                        trouve = true;
                        nom = s; s = "";
                    }
                    else
                    {
                        s = s + name[i];
                    }
                }
                if (trouve == false)
                {
                    nom = s;
                    prenom = "";
                }
                else
                {
                    prenom = s;
                }
                s = "";
                if (prenom != "")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from utilisateur where nom like '" + nom + "' and prenom like '" + prenom + "'", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    while (rdr.Read())
                    {
                        dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetString(9), rdr.GetString(10));
                    }
                    connexion.Close();
                }
                else
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from utilisateur where nom like '" + nom + "'", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    while (rdr.Read())
                    {
                        dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetString(9), rdr.GetString(10));
                    }
                    connexion.Close();
                }
                nom = ""; prenom = "";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString();
                if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["grade"].Value.ToString()=="chef division")
                {
                    bool trouve;
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select id from division where id_chef_div="+dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString(), connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read()) trouve = true;
                    else trouve = false;
                    connexion.Close();
                    if(trouve==true)
                    {
                        connexion.Open();
                        cmd = new MySqlCommand("select id,prenom,nom from utilisateur where grade like 'chef division' and id not in (select id_chef_div from division)", connexion);
                        rdr = cmd.ExecuteReader();
                        comboBox1.Items.Clear();
                        while (rdr.Read())
                        {
                            comboBox1.Items.Add(rdr.GetInt32(0).ToString()+"-"+rdr.GetString(1)+" "+rdr.GetString(2));
                        }
                        connexion.Close();
                        groupBox1.Location = new Point(1,5);
                    }
                    else
                    {
                        connexion.Open();
                        cmd = new MySqlCommand("delete from utilisateur where id = " + id, connexion);
                        cmd.ExecuteNonQuery();
                        connexion.Close();
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                    }
                }
                else if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["grade"].Value.ToString() == "chef service")
                {
                    bool trouve;
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select id from service where id_chef_serv=" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString(), connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read()) trouve = true;
                    else trouve = false;
                    connexion.Close();
                    if (trouve == true)
                    {
                        connexion.Open();
                        cmd = new MySqlCommand("select id,prenom,nom from utilisateur where grade like 'chef service' and  id not in (select id_chef_serv from service)" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString(), connexion);
                        rdr = cmd.ExecuteReader();
                        comboBox1.Items.Clear();
                        while (rdr.Read())
                        {
                            comboBox1.Items.Add(rdr.GetInt32(0).ToString() + "-" + rdr.GetString(1) + " " + rdr.GetString(2));
                        }
                        connexion.Close();
                        groupBox1.Location = new Point(1,5);
                    }
                    else
                    {
                        connexion.Open();
                        cmd = new MySqlCommand("delete from utilisateur where id = " + id, connexion);
                        cmd.ExecuteNonQuery();
                        connexion.Close();
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                    }
                }
            }
            else
            {
                MessageBox.Show("veuiller selectionner un utilisateur !");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
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

        private void button10_Click(object sender, EventArgs e)
        {
            groupBox1.Location = new Point(1040, 43);
            OpenChildForm(new Formadd1());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select * from utilisateur", connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (rdr.Read())
            {
                dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetString(9), rdr.GetString(10));
            }
            connexion.Close();
        }

        private void OpenChildForm2()
        {
            Formmodify fm1 = new Formmodify();
            fm1.TopLevel = false;
            fm1.FormBorderStyle = FormBorderStyle.None;
            fm1.Dock = DockStyle.Fill;
            ESPACE_ADMIN EA = (ESPACE_ADMIN)Application.OpenForms["ESPACE_ADMIN"];
            Panel panel2 = (Panel)EA.Controls["panel2"];
            panel2.Controls.Add(fm1);
            panel2.Tag = fm1;
            //Formmodify fm = (Formmodify)Application.OpenForms["Formmodify"];
            TextBox txt1 =(TextBox)fm1.Controls["textBox1"];
            txt1.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Nom"].Value.ToString();
            TextBox txt2 = (TextBox)fm1.Controls["textBox2"];
            txt2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Prenom"].Value.ToString();
            TextBox txt8 = (TextBox)fm1.Controls["textBox8"];
            txt8.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["num_tele"].Value.ToString();
            TextBox txt7 = (TextBox)fm1.Controls["textBox7"];
            txt7.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["email"].Value.ToString();
            TextBox txt4 = (TextBox)fm1.Controls["textBox4"];
            txt4.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Adresse"].Value.ToString();
            TextBox txt10 = (TextBox)fm1.Controls["textBox10"];
            txt10.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Login"].Value.ToString();
            ComboBox cb = (ComboBox)fm1.Controls["comboBox1"];
            cb.Items.Add("admin");
            cb.Items.Add("chef division");
            cb.Items.Add("chef service");
            switch (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["grade"].Value.ToString())
            {
                case "admin":
                    cb.SelectedIndex = 0;break;
                case "chef division":
                    cb.SelectedIndex = 1;break;
                case "chef service":
                    cb.SelectedIndex = 2;break;
            }
            DateTimePicker dt = (DateTimePicker)fm1.Controls["DateTimePicker1"];
            dt.Value = System.DateTime.Parse(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Date de naissance"].Value.ToString());
            fm1.BringToFront();
            fm1.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                OpenChildForm2();
            }
            else
            {
                MessageBox.Show("veuiller selectionner un utilisateur !");
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            name = textBox12.Text;
            bool trouve = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    trouve = true;
                    nom = s; s = "";
                }
                else
                {
                    s = s + name[i];
                }
            }
            if (trouve == false)
            {
                nom = s;
                prenom = "";
            }
            else
            {
                prenom = s;
            }
            s = "";
            if (prenom != "")
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select * from utilisateur where nom like '" + nom + "' and prenom like '" + prenom + "'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetString(9), rdr.GetString(10));
                }
                connexion.Close();
            }
            else
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select * from utilisateur where nom like '" + nom + "'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetString(9), rdr.GetString(10));
                }
                connexion.Close();
            }
            nom = ""; prenom = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex!=-1)
            {
                MySqlCommand cmd;
                if(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["grade"].Value.ToString()=="chef division")
                {
                    int i = 0; idchefdiv = "";
                    while (comboBox1.Text[i] != '-')
                    {
                        idchefdiv += comboBox1.Text[i]; i++;
                    }
                    connexion.Open();
                    cmd = new MySqlCommand("update division set id_chef_div=" + idchefdiv + " where id_chef_div=" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString(), connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                }
                else
                {
                    int i = 0; idchefserv = "";
                    while (comboBox1.Text[i] != '-')
                    {
                        idchefserv += comboBox1.Text[i]; i++;
                    }
                    connexion.Open();
                    cmd = new MySqlCommand("update service set id_chef_serv=" + idchefserv + "where id_chef_serv =" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString(), connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                }
                connexion.Open();
                cmd = new MySqlCommand("delete from utilisateur where id =" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["ID"].Value.ToString(), connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select * from utilisateur", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(6), rdr.GetString(7), rdr.GetInt32(8), rdr.GetString(9), rdr.GetString(10));
                }
                connexion.Close();
                groupBox1.Location = new Point(1040,43);
            }
        }


    }
}
