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
    public partial class FormService1 : Form
    {
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        string id, id1, iddiv, idservice, archives;
        string[] tab;
        int ind = 0;
        public FormService1(string i)
        {
            tab = new string[50];
            InitializeComponent();
           iddiv = i;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select nom_serv,nom,prenom from service S inner join utilisateur U on U.id=S.id_chef_serv", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (rdr.Read())
                {
                    dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
                }
                connexion.Close();
            }
        }

        private void initdatagrid()
        {
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Intitulé";
            dataGridView1.Columns[0].Width = 370;
            dataGridView1.Columns[1].Name = "Nom du chef";
            dataGridView1.Columns[1].Width = 370;
            dataGridView1.Columns[2].Name = "Prenom du chef";
            dataGridView1.Columns[2].Width = 370;
            dataGridView1.RowCount = 1;
            dataGridView1.RowHeadersVisible = false;

        }

        private void FormService1_Load(object sender, EventArgs e)
        {
            initdatagrid();
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select nom_serv,nom,prenom from service S inner join utilisateur U on U.id=S.id_chef_serv where S.id_division ="+iddiv, connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (rdr.Read())
            {
                dataGridView1.Rows.Add(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
            }
            connexion.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connexion.Open();
            MySqlCommand cmd = new MySqlCommand("select id,nom,prenom from utilisateur where grade like 'chef service' and id not in (select id_chef_serv from service)", connexion);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr.GetString(0) + "-" + rdr.GetString(2) + " " + rdr.GetString(1));
            }
            connexion.Close();
            connexion.Open();
            cmd = new MySqlCommand("select intitule,id from division where id="+iddiv, connexion);
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                comboBox3.Items.Add(rdr.GetInt32(1).ToString()+"-"+rdr.GetString(0));
                comboBox3.Text = rdr.GetInt32(1).ToString() + "-" + rdr.GetString(0);
            }
            connexion.Close();
            groupBox1.Location = new Point(12, -6);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                string intit = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Intitulé"].Value.ToString();
                textBox3.Text = intit;
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id,nom,prenom from utilisateur where grade like 'chef service' and id not in(select id_chef_serv from service)", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    comboBox2.Items.Add(rdr.GetInt32(0).ToString() + "-" + rdr.GetString(1) + " " + rdr.GetString(2));
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select id,intitule from division where id =" + iddiv, connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    comboBox4.Items.Add(rdr.GetInt32(0).ToString() + "-" + rdr.GetString(1));
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select S.id_chef_serv,U.nom,U.prenom,S.id_division,D.intitule from service S inner join utilisateur U inner join division D on S.id_division=D.id and U.id=S.id_chef_serv where S.nom_serv like '" + intit + "'", connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    comboBox2.Text = rdr.GetInt32(0).ToString() + "-" + rdr.GetString(1) + " " + rdr.GetString(2);
                    comboBox4.Text = rdr.GetInt32(3).ToString() + "-" + rdr.GetString(4);
                }
                connexion.Close();
                groupBox2.Location = new Point(12, -6);
            }
            else
            {
                MessageBox.Show("veuiller selectionner un service !");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells.Count>0)
            {
                string intit = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Intitulé"].Value.ToString();
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id from service where nom_serv like '"+intit+"'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    idservice = rdr.GetInt32(0).ToString();
                }
                connexion.Close();
                ind = 0;
                connexion.Open();
                cmd = new MySqlCommand("select id from archivage where id_service =" + idservice, connexion);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tab[ind] = rdr.GetInt32(0).ToString();
                    ind++;
                }
                connexion.Close();
                for (int i = 0; i < ind;i++)
                {
                    connexion.Open();
                    cmd = new MySqlCommand("delete from document where id_archive =" + tab[i], connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                }
                connexion.Open();
                cmd = new MySqlCommand("delete from archivage where id_service ="+idservice, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("delete from service where nom_serv like '" + intit + "'", connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            }
            else
            {
                MessageBox.Show("veuiller selectionner un service !");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex!=-1 && textBox2.Text!="" && comboBox3.SelectedIndex!=-1)
            {
                for (int i = 0; i < comboBox1.SelectedItem.ToString().Length; i++)
                {
                    if (comboBox1.Text[i] == '-')
                    {
                        i = comboBox1.Text.Length;
                    }
                    else
                    {
                        id += comboBox1.Text[i];
                    }
                }
                for (int i = 0; i < comboBox3.Text.Length; i++)
                {
                    if (comboBox3.Text[i] == '-')
                    {
                        i = comboBox3.Text.Length;
                    }
                    else
                    {
                        id1 += comboBox3.Text[i];
                    }
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("insert into service(id_chef_serv,nom_serv,id_division) values(" + id + ",'" + textBox2.Text + "'," + id1 + ");", connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                id = "";
                id1 = "";
                groupBox1.Location = new Point(12, 852);
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string nom=dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Intitulé"].Value.ToString();
            if(comboBox2.SelectedIndex!=-1 && textBox3.Text!="" && comboBox4.SelectedIndex!=-1)
            {
                for (int i = 0; i < comboBox2.SelectedItem.ToString().Length; i++)
                {
                    if (comboBox2.Text[i] == '-')
                    {
                        i = comboBox2.Text.Length;
                    }
                    else
                    {
                        id += comboBox2.Text[i];
                    }
                }
                for (int i = 0; i < comboBox4.Text.Length; i++)
                {
                    if (comboBox4.Text[i] == '-')
                    {
                        i = comboBox4.Text.Length;
                    }
                    else
                    {
                        id1 += comboBox4.Text[i];
                    }
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("update service set id_chef_serv=" + id + ",nom_serv='" + textBox3.Text + "',id_division=" + id1+ " where nom_serv like '"+nom+"'", connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                id = "";
                id1 = "";
                groupBox2.Location = new Point(12, 852);
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }
    }
}
