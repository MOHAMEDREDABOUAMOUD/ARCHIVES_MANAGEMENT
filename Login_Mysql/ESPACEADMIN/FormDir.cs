using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace Login_Mysql
{
    public partial class FormDir : Form
    {
        string keywrd, id, id1, idserv, idarchive, generatedid,taille,ids,filePath,date,day,month,year,s,iddv,grade,idgrade,name;
        int entern,cp,x,y;
        bool enter = false,isFile = false,visgb5=false,visgb6=false;
        public static MySqlConnection connexion = new MySqlConnection("Server=localhost;Database=gestion_archive;port=3306;User Id=root;password=");//database=gestion_archive; server=localhost; user=root; pwd=

        public FormDir(string g,string i)
        {
            InitializeComponent();
            grade = g;
            idgrade = i;
            if (grade == "admin")
            {
                comboBox5.Items.Add("Nom Service");
                comboBox5.Items.Add("Nom Division");
                comboBox5.Items.Add("Nom Archive");
                comboBox5.Items.Add("Nom Document");
                comboBox5.Items.Add("Key word Archive");
                comboBox5.Items.Add("Sujet Archive");
            }
            else if (grade == "chef division")
            {
                comboBox5.Items.Add("Nom Service");
                comboBox5.Items.Add("Nom Archive");
                comboBox5.Items.Add("Nom Document");
                comboBox5.Items.Add("Key word Archive");
                comboBox5.Items.Add("Sujet Archive");
            }
            else if (grade == "chef service")
            {
                comboBox5.Items.Add("Nom Archive");
                comboBox5.Items.Add("Nom Document");
                comboBox5.Items.Add("Key word Archive");
                comboBox5.Items.Add("Sujet Archive");
            }
        }

        public void loadarchives()
        {
            if(grade=="admin")
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select name,id from archivage", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                }
                connexion.Close();
            }
            else if(grade=="chef division")
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                }
                connexion.Close();
            }
            else if(grade=="chef service")
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                }
                connexion.Close();
            }
        }

        private void FormDir_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            loadarchives();
        }

        public void loadButtonAction()
        {

            /*loadFilesAndDirectories();
            isFile = false;*/
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(visgb5)
            {
                groupBox5.Location = new Point(1457, 202); visgb5 = false;
            }
            else if (visgb6)
            {
                groupBox6.Location = new Point(1457, 418); visgb6 = false;
            }
            keywrd = e.Item.Text;
            if (isFile)
            {
                id = "";
                for (int i = 0; i < keywrd.Length; i++)
                {
                    if (keywrd[i] == '-')
                    {
                        i = keywrd.Length;
                    }
                    else
                    {
                        id += keywrd[i];
                    }
                }
            }
            else
            {
                id1 = ""; id = "";
                for (int i = 0; i < keywrd.Length; i++)
                {
                    if (keywrd[i] == '-')
                    {
                        i = keywrd.Length;
                    }
                    else
                    {
                        id1 += keywrd[i];
                    }
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(visgb5==true)
            {
                groupBox5.Location = new Point(1457, 202); visgb5 = false;
            }
            else if(visgb6==true)
            {
                groupBox6.Location = new Point(1457, 418); visgb6 = false;
            }
            FileAttributes fileAttr;
            if(isFile)
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select path from document where id = " + id, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    filePath = rdr.GetString(0);
                }
                connexion.Close();
                FileInfo fileDetails = new FileInfo(filePath);
                fileAttr = File.GetAttributes(filePath);
                Process.Start(filePath);
            }
            else
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where A.id = " + id1, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                listView1.Items.Clear();
                while (rdr.Read())
                {
                    listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                }
                connexion.Close();
                isFile = true;
                button6.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(enter==true)
            {
                if(entern==1)
                {
                    groupBox1.Location = new Point(1343, 50);
                }
                else if(entern==2)
                {
                    groupBox2.Location = new Point(1348, 100);
                }
                else
                {
                    groupBox3.Location = new Point(1400, 44);
                }
                enter = false;
            }
            else
            {
                listView1.Items.Clear();
                button2.Enabled = false;
                button3.Enabled = false;
                button6.Enabled = true;
                loadarchives();
                isFile = false;
            }
        }

        public void UploadFile(string fpath,string ext,string name,float size)
        {
                connexion.Open();

                FileStream fstream = File.OpenRead(fpath);
                byte[] contents = new byte[fstream.Length];
                fstream.Read(contents, 0, (int)fstream.Length);
                fstream.Close();

                using (MySqlCommand cmd = new MySqlCommand("insert into document(id_archive,files,path,type,name,size) values(@id_archive,@files,@path,@type,@name,@size)", connexion))
                {
                    cmd.Parameters.AddWithValue("@id_archive", id1);
                    cmd.Parameters.AddWithValue("@files", contents);
                    cmd.Parameters.AddWithValue("@path", fpath);
                    cmd.Parameters.AddWithValue("@type", ext);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@size", size);
                    cmd.ExecuteNonQuery();
                }
                connexion.Close();
                connexion.Open();
                MySqlCommand cmd1 = new MySqlCommand("update archivage set taille=taille + "+size+" where id="+id1,connexion);
                cmd1.ExecuteNonQuery();
                connexion.Close();
                MessageBox.Show("Upload Successful", "file", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(isFile)
            {
                DateTime todaye = DateTime.Today;
                date = todaye.ToString();
                cp = 0; s = "";
                for (int i = 0; i < date.Length; i++)
                {
                    if (date[i] == ' ')
                    {
                        i = date.Length;
                    }
                    else if (date[i] == '/')
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
                    else
                    {
                        s += date[i];
                    }
                }
                cp = 0;
                year = s; s = "";
                date = year + "-" + month + "-" + day;
                using (OpenFileDialog dlg = new OpenFileDialog() { Filter = "Text Documents (*.pdf;*.rar) |*.pdf;*.rar", ValidateNames = true })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DialogResult dialog = MessageBox.Show("Are you sure you want to upload this files ?", "file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialog == DialogResult.Yes)
                        {
                            string fpath = dlg.FileName;
                            FileInfo fi = new FileInfo(fpath);
                            UploadFile(fpath, fi.Extension, fi.Name, fi.Length);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("update archivage set date_modif='" + date + "' where id =" + id1, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                date = "";
                listView1.Items.Clear();
                connexion.Open();
                cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where A.id = " + id1, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                listView1.Items.Clear();
                while (rdr.Read())
                {
                    listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                }
                connexion.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isFile)
            {
                if(listView1.SelectedItems.Count>0)
                {
                    DateTime todaye = DateTime.Today;
                    date = todaye.ToString();
                    cp = 0;
                    for (int i = 0; i < date.Length; i++)
                    {
                        if (date[i] == ' ')
                        {
                            i = date.Length;
                        }
                        else if (date[i] == '/')
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
                        else
                        {
                            s += date[i];
                        }
                    }
                    cp = 0;
                    year = s;
                    date = year + "-" + month + "-" + day;
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select size from document where id ="+id,connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if(rdr.Read())
                    {
                        taille = rdr.GetFloat(0).ToString();
                    }
                    connexion.Close();

                    connexion.Open();
                    cmd = new MySqlCommand("delete from document where id ="+id, connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();

                    connexion.Open();
                    cmd = new MySqlCommand("update archivage set taille =taille - "+taille+" where id = "+id1,connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();

                    listView1.Items.Clear();
                    connexion.Open();
                    cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where A.id = " + id1, connexion);
                    rdr = cmd.ExecuteReader();
                    listView1.Items.Clear();
                    while (rdr.Read())
                    {
                        listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                    }
                    connexion.Close();
                    connexion.Open();
                    cmd = new MySqlCommand("update archivage set date_modif='" + date + "' where id =" + id1, connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                    date = "";
                    isFile = true;

                }
                else
                {
                    MessageBox.Show("veuiller selectionner un document !");
                }
            }
            else
            {
                if(listView1.SelectedItems.Count>0)
                {
                    string[] tab=new string[500];
                    int cp = 0;
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select id from document where id_archive = " + id1, connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        tab[cp] = rdr.GetString(0); cp++;
                    }
                    connexion.Close();

                    for(int i=0;i<cp;i++)
                    {
                        connexion.Open();
                        cmd = new MySqlCommand("delete from document where id = " + tab[i], connexion);
                        cmd.ExecuteNonQuery();
                        connexion.Close();
                    }

                    connexion.Open();
                    cmd = new MySqlCommand("delete from archivage where id = " + id1, connexion);
                    cmd.ExecuteNonQuery();
                    connexion.Close();
                    listView1.Items.Clear();
                    loadarchives();
                }
                else
                {
                    MessageBox.Show("veuiller selectionner un archive !");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(!isFile)
            {
                groupBox4.Location = new Point(700, 700);
                groupBox1.Location = new Point(10, 0);
                if(grade=="admin")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select nom_serv from service", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        comboBox1.Items.Add(rdr.GetString(0));
                    }
                    connexion.Close();
                }
                else if(grade == "chef division")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select nom_serv from service where id_service="+idgrade, connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        comboBox1.Items.Add(rdr.GetString(0));
                    }
                    connexion.Close();
                }
                else if(grade == "chef service")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select nom_serv from service where id ="+idgrade, connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        comboBox1.Items.Add(rdr.GetString(0));
                    }
                    connexion.Close();
                }
                enter = true;
                entern = 1;
            }
                
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="" && textBox2.Text!="" && textBox5.Text!="" && comboBox1.SelectedIndex!=-1)
            {
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id,id_division from service where nom_serv like '" + comboBox1.SelectedItem.ToString()+"'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ids = rdr.GetString(0);
                    iddv = rdr.GetString(1);
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select S.nom_serv,D.intitule from service S inner join division D on S.id_division=D.id where S.id=" + ids, connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    generatedid = rdr.GetString(1) + '-' + rdr.GetString(0) + '-' + textBox5.Text;
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("insert into archivage(sujet,mot_cle,id_service,name,genid,id_division) values ('" + textBox1.Text + "','"+textBox2.Text+"',"+ids+",'"+textBox5.Text+"','"+generatedid+"',"+iddv+")", connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                generatedid = "";
                groupBox1.Location = new Point(1343, 50);
                groupBox4.Location = new Point(10, 0);
                listView1.Clear();
                loadarchives();
                enter = false;
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }

        public void downloadfile(string file)
        {
            connexion.Open();
            bool em = false;

            using (MySqlCommand cmd = new MySqlCommand("select files from document where id = '" + id + "'", connexion))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    if (reader.Read())
                    {
                        em = true;
                        byte[] fileData = (byte[])reader.GetValue(0);
                        using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fs))
                            {
                                bw.Write(fileData);
                                bw.Close();
                            }
                        }
                        MessageBox.Show("download Successful", "file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (em == false)
                    {
                        MessageBox.Show("NO DATA FOUND", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    reader.Close();
                }
            }
            connexion.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(isFile && listView1.SelectedItems.Count>0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "text document (*.pdf;)|*.pdf", ValidateNames = true})
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        DialogResult dialog = MessageBox.Show("Are you sure you want to download this files ?", "file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialog == DialogResult.Yes)
                        {
                            string filename = sfd.FileName;
                            downloadfile(filename);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("veuiller selectionner un document !");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(isFile)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select id,mot_cle from archivage", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        comboBox3.Items.Add(rdr.GetString(0) + "-" + rdr.GetString(1));
                    }
                    connexion.Close();
                    connexion.Open();
                    cmd = new MySqlCommand("select D.id_archive,A.mot_cle,D.name from document D inner join archivage A on A.id=D.id_archive where D.id = " + id, connexion);
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        comboBox3.Text = rdr.GetInt32(0) + "-" + rdr.GetString(1);
                        textBox6.Text = rdr.GetString(2);
                    }
                    connexion.Close();
                    groupBox4.Location = new Point(700, 700);
                    groupBox3.Location = new Point(10, 0);
                    enter = true;
                    entern = 3;
                }
                else
                {
                    MessageBox.Show("veuiller selectionner in document !");
                }
            }
            else if(!isFile)
            {
                if(listView1.SelectedItems.Count>0)
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select nom_serv from service", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        comboBox2.Items.Add(rdr.GetString(0));
                    }
                    connexion.Close();
                    connexion.Open();
                    cmd = new MySqlCommand("select S.nom_serv,A.sujet,A.mot_cle,A.name from archivage A inner join service S on A.id_service=S.id where A.id =" + id1, connexion);
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        textBox4.Text = rdr.GetString(1);
                        textBox3.Text = rdr.GetString(2);
                        comboBox2.Text = rdr.GetString(0);
                        textBox7.Text = rdr.GetString(3);
                    }
                    connexion.Close();
                    groupBox4.Location = new Point(700, 700);
                    groupBox2.Location = new Point(10, 0);
                    enter = true;
                    entern = 2;
                }
                else
                {
                    MessageBox.Show("veuiller selectionner un archive !");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(textBox3.Text!="" && textBox4.Text!="" && textBox7.Text!="" && comboBox2.SelectedIndex!=-1)
            {
                DateTime todaye = DateTime.Today;
                date = todaye.ToString();
                cp = 0;
                for (int i = 0; i < date.Length; i++)
                {
                    if (date[i] == ' ')
                    {
                        i = date.Length;
                    }
                    else if (date[i] == '/')
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
                    else
                    {
                        s += date[i];
                    }
                }
                year = s; s = "";
                date = year + "-" + month + "-" + day;
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("select id from service where nom_serv like '" + comboBox2.Text + "'", connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    idserv = rdr.GetInt32(0).ToString();
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("select S.nom_serv,D.intitule from service S inner join division D on S.id_division = D.id where S.id=" + idserv, connexion);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    generatedid = rdr.GetString(1) + '-' + rdr.GetString(0) + '-' + textBox7.Text;
                }
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("update archivage set id_service=" + idserv + ",sujet='" + textBox4.Text + "',mot_cle='" + textBox3.Text + "',name='" + textBox7.Text + "',genid='" + generatedid + "' where id =" + id1, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("update archivage set date_modif='" + date + "' where id =" + id1, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                date = "";
                generatedid = "";
                listView1.Clear();
                loadarchives();
                groupBox2.Location = new Point(1348, 100);
                groupBox4.Location = new Point(10, 0);
                enter = false;
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(comboBox3.SelectedIndex!=-1 && textBox6.Text!="")
            {
                DateTime todaye = DateTime.Today;
                date = todaye.ToString();
                cp = 0;
                for (int i = 0; i < date.Length; i++)
                {
                    if (date[i] == ' ')
                    {
                        i = date.Length;
                    }
                    else if (date[i] == '/')
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
                    else
                    {
                        s += date[i];
                    }
                }
                year = s; s = "";
                date = year + "-" + month + "-" + day;
                for (int i = 0; i < comboBox3.Text.Length; i++)
                {
                    if (comboBox3.Text[i] == '-')
                    {
                        i = comboBox3.Text.Length;
                    }
                    else
                    {
                        idarchive += comboBox3.Text[i];
                    }
                }
                connexion.Open();
                MySqlCommand cmd = new MySqlCommand("update document set id_archive=" + idarchive + ",name='" + textBox6.Text + "' where id ="+id, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                connexion.Open();
                cmd = new MySqlCommand("update archivage set date_modif='" + date + "' where id =" + id1, connexion);
                cmd.ExecuteNonQuery();
                connexion.Close();
                date = "";
                groupBox3.Location = new Point(1400, 44);
                groupBox4.Location = new Point(10, 0);
                enter = false;
                listView1.Items.Clear();
                connexion.Open();
                cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where A.id = " + id1, connexion);
                MySqlDataReader rdr = cmd.ExecuteReader();
                listView1.Items.Clear();
                while (rdr.Read())
                {
                    listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                }
                connexion.Close();
            }
            else
            {
                MessageBox.Show("veuiller remplir tout les champs !");
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            label27.Visible = false;
            listView1.Items.Clear();
            if(!isFile)
            {
                if (comboBox4.SelectedItem.ToString().Equals("Name"))
                {
                    if(grade=="admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage order by name ASC;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if(grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade+" order by name ASC;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if(grade== "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade+" order by name ASC;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox4.SelectedItem.ToString().Equals("Date Modified"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage order by date_modif; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade+" order by date_modif; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service = "+idgrade+" order by date_modif; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox4.SelectedItem.ToString().Equals("Size"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage order by taille; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division = "+idgrade+" order by taille; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade+" order by taille; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                }
            }
            else
            {
                if (comboBox4.SelectedItem.ToString().Equals("Name"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from document order by name ASC;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive=A.id where A.id_division="+idgrade+" order by name ASC;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive=A.id where A.id_service=" + idgrade + " order by name ASC;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox4.SelectedItem.ToString().Equals("Size"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from document order by size; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive=A.id where A.id_division=" + idgrade + " order by size; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive=A.id where A.id_service=" + idgrade + " order by size; ", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                }
            }
        }

        private void textBox8_MouseDown(object sender, MouseEventArgs e)
        {
            textBox8.Text = "";
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (comboBox5.SelectedItem == null)
            {
                if (grade == "admin")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select name,id from archivage where mot_cle  like '" + textBox8.Text + "%' ;", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                    }
                    connexion.Close();
                }
                else if (grade == "chef division")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade+" and mot_cle  like '" + textBox8.Text + "%' ;", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                    }
                    connexion.Close();
                }
                else if (grade == "chef service")
                {
                    connexion.Open();
                    MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade+" and mot_cle  like '" + textBox8.Text + "%' ;", connexion);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                    }
                    connexion.Close();
                }
            }
            else
            {
                if (comboBox5.SelectedItem.ToString().Equals("Key word Archive"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where mot_cle  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade+" and mot_cle  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade+" and mot_cle  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox5.SelectedItem.ToString().Equals("Nom Archive"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where name  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade+" and name  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade+" and name  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox5.SelectedItem.ToString().Equals("Nom Document"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where D.name like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where A.id_division ="+idgrade+" and D.name like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select D.name,D.id from document D inner join archivage A on D.id_archive= A.id where id_service="+idgrade+" and D.name like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 1);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox5.SelectedItem.ToString().Equals("Sujet Archive"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where sujet  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_division="+idgrade+" and sujet  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,id from archivage where id_service="+idgrade+" and sujet  like '" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                }
                else if (comboBox5.SelectedItem.ToString().Equals("Nom Service"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.id from archivage A  inner join service S on A.id_service = S.id where S.nom_serv like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.id from archivage A  inner join service S on A.id_service = S.id where A.id_division=" + idgrade + " and S.nom_serv like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef service")
                    {
                        /*connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.id from archivage A  inner join service S on A.id_service = S.id where A.id_service=" + idgrade + " and S.nom_serv like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();*/
                    }
                }
                else if (comboBox5.SelectedItem.ToString().Equals("Nom Division"))
                {
                    if (grade == "admin")
                    {
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.id from archivage A inner join division D on A.id_division = D.id where D.intitule like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();
                    }
                    else if (grade == "chef division")
                    {
                        /*connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.id from archivage A inner join division D on A.id_division = D.id where A.id_division=" + idgrade + " and D.intitule like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();*/
                    }
                    else if (grade == "chef service")
                    {
                        /*connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.id from archivage A inner join division D on A.id_division = D.id where A.id_service=" + idgrade + " and D.intitule like '%" + textBox8.Text + "%' ;", connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            listView1.Items.Add(rdr.GetInt32(1) + "-" + rdr.GetString(0), 0);
                        }
                        connexion.Close();*/
                    }
                }
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(isFile)
            {
                if (e.Button == MouseButtons.Right)
                {
                    var focusedItem = listView1.FocusedItem;
                    if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                    {
                        if(e.Location.X>=681)
                        {
                            x=e.Location.X-270;
                        }
                        else
                        {
                            x = e.Location.X;
                        }
                        if (e.Location.Y+70>=275)
                        {
                            y = e.Location.Y + 70 - 260;
                        }
                        else
                        {
                            y = e.Location.Y +70;
                        }
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select name,size,type from document where id =" + id, connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            label25.Text = rdr.GetString(0);
                            label24.Text = rdr.GetFloat(1).ToString();
                            label23.Text = rdr.GetString(2);
                        }
                        connexion.Close();
                        //MessageBox.Show(Cursor.Position.X.ToString());
                        groupBox6.Location = new Point(x,y);
                        visgb6 = true;
                        //MessageBox.Show(groupBox5.Location.X.ToString(), groupBox5.Location.Y.ToString());
                    }
                } 
                else
                {
                    if (visgb5 == true)
                    {
                        groupBox5.Location = new Point(1457, 202); visgb5 = false;
                    }
                    else if (visgb6 == true)
                    {
                        groupBox6.Location = new Point(1457, 418); visgb6 = false;
                    }
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    var focusedItem = listView1.FocusedItem;
                    if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                    {
                        if (e.Location.X >= 681)
                        {
                            x = e.Location.X - 270;
                        }
                        else
                        {
                            x = e.Location.X;
                        }
                        if (e.Location.Y + 70 >= 275)
                        {
                            y = e.Location.Y + 70 - 260;
                        }
                        else
                        {
                            y = e.Location.Y + 70;
                        }
                        connexion.Open();
                        MySqlCommand cmd = new MySqlCommand("select A.name,A.sujet,A.taille,A.date_Arch,A.date_modif,count(D.id) from archivage A inner join document D on D.id_archive = A.id where A.id =" + id1, connexion);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            label16.Text = rdr.GetString(0);
                            label17.Text = rdr.GetString(1);
                            label18.Text = rdr.GetFloat(2).ToString();
                            label19.Text = rdr.GetString(3);
                            label20.Text = rdr.GetString(4);
                            label21.Text = rdr.GetInt32(5).ToString();
                        }
                        connexion.Close();
                        //MessageBox.Show(Cursor.Position.X.ToString());
                        groupBox5.Location = new Point(x, y);
                        visgb5 = true;
                        //MessageBox.Show(groupBox5.Location.X.ToString(), groupBox5.Location.Y.ToString());
                    }
                }
                else
                {
                    if (visgb5 == true)
                    {
                        groupBox5.Location = new Point(1457, 202); visgb5 = false;
                    }
                    else if (visgb6 == true)
                    {
                        groupBox6.Location = new Point(1457, 418); visgb6 = false;
                    }
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            label26.Visible = false;
        }
    }
}
