using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_Mysql
{
    public partial class ESPACE_ADMIN : Form
    {
        
        public static string grade,id;

        public ESPACE_ADMIN(string g,string i)
        {
            InitializeComponent();
            //MessageBox.Show(i);
            grade = g;
            id = i;
            if(grade == "admin")
            {
                label1.Text="Administator Access";
            }
            else if (grade == "chef division")
            {
                label1.Text = "Division Access";
                button2.Enabled = false;
                button6.Enabled = false;
            }
            else if (grade == "chef service")
            {
                label1.Text = "Service Access";
                button2.Enabled = false;
                button3.Enabled = false;
                button6.Enabled = false;
            }
            OpenChildForm(new FormDir(g,i));
        }

        private void espace_admin_load(object sender, EventArgs e)
        {

        }
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void OpenChildForm(Form childForm)
        {

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(childForm);
            this.panel2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            label2.Text = "gestion des archives";
            OpenChildForm(new FormDir(grade,id));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            label2.Text = "Dash Board";
            OpenChildForm(new ESPACEADMIN.dashboard());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void ESPACE_ADMIN_Load_1(object sender, EventArgs e)
        {
            InitTimer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "Gestion des utilisateurs";
            OpenChildForm(new Formusr1());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "Gestion des divisions";
            OpenChildForm(new FormDivision1(grade,id));
        }


        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("timer run");
        }

    }
}
