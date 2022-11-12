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
    public partial class Form1 : Form
    {
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=
        string idserv, iddiv;
        public Form1()
        {
            InitializeComponent();
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (username_txt.Text != "" && pwd_txt.Text != "")
            {
                iddiv = ""; idserv = "";
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select D.id from division D inner join utilisateur U on U.id=D.id_chef_div where U.login like '"+username_txt.Text+"' and U.pwd like '"+pwd_txt.Text+"'",connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    iddiv = rdr.GetInt32(0).ToString();
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select S.id from service S inner join utilisateur U on U.id=S.id_chef_serv where U.login like '" + username_txt.Text + "' and U.pwd like '" + pwd_txt.Text + "'", connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    idserv = rdr.GetInt32(0).ToString();
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select login,pwd,grade from utilisateur", connexion);
                rdr = cmd.ExecuteReader();
                bool trouve = false;
                while (rdr.Read())
                {
                    if (rdr.GetString(0) == username_txt.Text)
                    {
                        trouve = true;
                        if (rdr.GetString(1) == pwd_txt.Text)
                        {
                            if (rdr.GetString(2)=="admin")
                            {
                                Hide();
                                ESPACE_ADMIN f2 = new ESPACE_ADMIN(rdr.GetString(2),"");
                                f2.Show();
                            }
                            else if(rdr.GetString(2)=="chef division")
                            {
                                if(iddiv!="")
                                {
                                    Hide();
                                    ESPACE_ADMIN f2 = new ESPACE_ADMIN(rdr.GetString(2), iddiv);
                                    f2.Show();
                                }
                            }
                            else if(rdr.GetString(2)=="chef service")
                            {
                                if(idserv!="")
                                {
                                    Hide();
                                    ESPACE_ADMIN f2 = new ESPACE_ADMIN(rdr.GetString(2), idserv);
                                    f2.Show();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("mot de pass incorecte");
                        }
                    }
                }
                if (trouve == false)
                {
                    MessageBox.Show("utilisateur introuvable");
                }
                connexion.Close();
            }
            else
            {
                MessageBox.Show("veuiler remplir les deux champs");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pwd_txt_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = login_btn;
        }            
            
     }

        
    
}
