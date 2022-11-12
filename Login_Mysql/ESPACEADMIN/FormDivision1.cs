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
    public partial class FormDivision1 : Form
    {
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        int cp=0,cp1,ind;
        string grade,id,idc1,idc2,intit,iddivision;
        string[] tab;
        public FormDivision1(string g,string i)
        {
            tab=new string[50];
            InitializeComponent();
            initdatagrid();
            grade = g;
            id = i;
            if(grade!="admin")
            {
                //MessageBox.Show(grade+" "+i);
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button6.Enabled = false;
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select intitule,nom,prenom from division D inner join utilisateur U on U.id=D.id_chef_div where D.id="+id, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
                }
                connexion.Close();
            }
        }

        private void FormDivision1_Load(object sender, EventArgs e)
        {
            //initdatagrid();
        }
        private void initdatagrid()
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Intitulé";
            dataGridView1.Columns[0].Width = 477;
            dataGridView1.Columns[1].Name = "Nom du chef";
            dataGridView1.Columns[1].Width = 400;
            dataGridView1.Columns[2].Name = "Prenom du chef";
            dataGridView1.Columns[2].Width = 400;
            dataGridView1.RowCount = 1;
            dataGridView1.RowHeadersVisible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select intitule,nom,prenom from division D inner join utilisateur U on U.id=D.id_chef_Div", connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (rdr.Read())
            {
                dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
            }
            connexion.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select intitule,nom,prenom from division D inner join utilisateur U on U.id=D.id_chef_Div where intitulé like '" + textBox1.Text + "'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
                }
                connexion.Close();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                string intit = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Intitulé"].Value.ToString();
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id from division where intitule like '"+intit+"'",connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    iddivision = rdr.GetInt32(0).ToString();
                }
                connexion.Close();
                ind = 0;
                connexion.Open();
                cmd = new MySqlCommand("select id from archivage where id_division =" + iddivision, connexion);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tab[ind] = rdr.GetInt32(0).ToString();
                    ind++;
                }
                connexion.Close();
                for (int i = 0; i < ind; i++)
                {
                    connexion.Open();
                    cmd = new MySqlCommand("delete from document where id_archive =" + tab[i], connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                }
                connexion.Open();
                cmd = new MySqlCommand("delete from archivage where id_division =" + iddivision , connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("delete from service where id_division = " + iddivision, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("delete from division where id =" + iddivision, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            }
            else
            {
                MessageBox.Show("veuiller selectionner une division !");
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
            Label label2 = (Label)EA.Controls["label2"];
            label2.Text = "gestion des services";
            childForm.BringToFront();
            childForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select id,nom,prenom from utilisateur where grade like 'chef division' and id not in (select id_chef_div from division)", connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr.GetInt32(0) + "-" + rdr.GetString(1) + " " + rdr.GetString(2));
            }
            connexion.Close();
            groupBox2.Location = new Point(3,7);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Intitulé"].Value.ToString();
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id,nom,prenom from utilisateur where grade like 'chef division' and id not in (select id_chef_div from division)", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr.GetString(1) == dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Nom du chef"].Value.ToString() && rdr.GetString(2) == dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Prenom du chef"].Value.ToString()) cp1 = cp;
                    cp++;
                    comboBox3.Items.Add(rdr.GetInt32(0) + "-" + rdr.GetString(1) + " " + rdr.GetString(2));
                }
                connexion.Close();
                comboBox3.SelectedIndex = cp1;
                intit = textBox2.Text;
                groupBox1.Location = new Point(3, 7);
            }
            else
            {
                MessageBox.Show("veuiller selectionner une division !");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id from division where intitule like '" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Intitulé"].Value.ToString() + "'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    OpenChildForm(new FormService1(rdr.GetInt32(0).ToString()));
                }
                connexion.Close();
            }
            else
            {
                MessageBox.Show("veuiller selectionner une division !");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!="" && comboBox3.SelectedIndex!=-1)
            {
                idc1 = comboBox3.SelectedItem.ToString();
                for (int i = 0; i < idc1.Length; i++)
                {
                    if (idc1[i] == '-')
                    {
                        i = idc1.Length;
                    }
                    else
                    {
                        idc2 += idc1[i];
                    }
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("update division set intitule='" + textBox2.Text + "',id_chef_Div=" + idc2 + " where intitule like '" + intit + "'", connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                groupBox1.Location = new Point(12, 679);
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(textBox3.Text!="" && comboBox1.SelectedIndex!=-1)
            {
                idc1 = comboBox1.SelectedItem.ToString();
                for (int i = 0; i < idc1.Length; i++)
                {
                    if (idc1[i] == '-')
                    {
                        i = idc1.Length;
                    }
                    else
                    {
                        idc2 += idc1[i];
                    }
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("insert into division(id_chef_div,intitule) values(" + idc2 + ",'" + textBox3.Text + "')", connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                groupBox2.Location = new Point(12, 879);
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }
    }
}
