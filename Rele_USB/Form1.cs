using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keyboard;
using Models;

namespace Rele_USB
{


    public partial class Form1 : Form
    {
        private Grifon rele;// = new Grifon();
        public Form1()
        {
            InitializeComponent();

            rele = new Grifon();
            rele.Enable();

            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;
            if (checkBox1.Checked == true)
            {

                rele.SetRele(25,false);  
            }
            else
            {
                rele.SetRele(25,true);   
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;
            if (checkBox2.Checked == true)
            {

                rele.SetRele(26, false);
            }
            else
            {
                rele.SetRele(26, true);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;
            if (checkBox3.Checked == true)
            {
                rele.SetRele(16, false);
            }
            else
            {
                rele.SetRele(16, true);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;
            if (checkBox4.Checked == true)
            {
                rele.SetRele(17, false);
            }
            else
            {
                rele.SetRele(17, true);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;
            if (radioButton1.Checked == true)
            {
                rele.SetRele(28, true);
            }
            else
            {
                rele.SetRele(28, false);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;
            if (radioButton2.Checked == true)
            {

                rele.SetRele(29, true);
            }
            else
            {
                rele.SetRele(29, false);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!rele.Init()) MessageBox.Show("Нету подкл. девайсов!");
            rele.Enable();
            panel1.Enabled = rele.Status != DeviceStatus.Error ? true : false;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = rele.Status != DeviceStatus.Active ? false : true;

            if (radioButton3.Checked == true)
            {

                rele.SetRele(27, true);
            }
            else
            {
                rele.SetRele(27, false);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }
    }
}
