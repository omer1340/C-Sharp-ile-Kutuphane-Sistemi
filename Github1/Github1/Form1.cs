using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Github1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }
       
               
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Form2'nin bir örneğini oluştur
            FormÜyelikİşlemleri form2 = new FormÜyelikİşlemleri();
            // Form2'yi göster
            form2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormKitapİşlemleri form3 = new FormKitapİşlemleri();
            form3.Show();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormKitapAlma form4 = new FormKitapAlma();
            form4.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormAlınanKitaplar form5 = new FormAlınanKitaplar();
            form5.Show();
        }
    }
}
