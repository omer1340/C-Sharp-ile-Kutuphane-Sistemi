using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Github1
{
    public partial class FormKitapİşlemleri : Form
    {
        public FormKitapİşlemleri()
        {
            InitializeComponent();
        }

        private void FormKitapİşlemleri_Load(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Label label)
                {
                    label.BackColor = Color.Transparent;
                    label.ForeColor = Color.White;
                }
            }


            textBox1.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox2.Font = new Font("Arial", 15, FontStyle.Regular);
            dateTimePicker1.Font = new Font(dateTimePicker1.Font.FontFamily, 12);
            textBox3.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox4.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox5.Font = new Font("Arial", 15, FontStyle.Regular);

            textBox6.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox7.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox8.Font = new Font("Arial", 15, FontStyle.Regular);

            textBox8.ReadOnly = true;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VeriKontrol veriKontrol = new VeriKontrol();
           
            if (veriKontrol.Boşad(textBox1.Text))  //İsim Kısmı Doluysa
            {
                if (veriKontrol.Boşad(textBox2.Text)) //Yazar adı kısmı doluysa
                {
                    if (veriKontrol.IsNumber(textBox3.Text)) //Sayfa sayısı doluysa
                    {
                        if (veriKontrol.IsNumber(textBox4.Text)) //Kitap sayısı doluysa
                        {

                            string bag = "Data Source=DESKTOP-MBNL0FO\\SQLEXPRESS;Initial Catalog=Kütüphane;Integrated Security=True";
                            bool eslesmeVarMi = false;

                            // Veritabanı bağlantısı oluşturma
                            using (SqlConnection connection = new SqlConnection(bag))
                            {
                                //Bağlantı açma
                                connection.Open();
                                string query = "SELECT COUNT(*) FROM Kitap WHERE kitap_ismi = @kitap_ismi AND yazar_ismi = @yazar_ismi";

                                using(SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@kitap_ismi", textBox1.Text);
                                    command.Parameters.AddWithValue("@yazar_ismi", textBox2.Text);                                 

                                    //Sorguyu çalıştır ve sonucu al
                                    int rowCount = (int)command.ExecuteScalar();
                                    eslesmeVarMi = (rowCount > 0);

                                    if (eslesmeVarMi) //AYNI YAZAR VE KİTAP ADINDA VERİ VAR
                                    {
                                        //SADECE KİTAP SAYISINI GÜNCELLEYECEĞİZ.
                                        string query2 = "SELECT kitap_sayısı FROM Kitap WHERE kitap_ismi = @kitap_ismi AND yazar_ismi = @yazar_ismi";
                                        using (SqlCommand command2 = new SqlCommand(query2, connection))
                                        {
                                            command2.Parameters.AddWithValue("@kitap_ismi", textBox1.Text);
                                            command2.Parameters.AddWithValue("@yazar_ismi", textBox2.Text);

                                            int mevcutkitapsayısı = (int)command2.ExecuteScalar();
                                            int eklenenkitapsayısı = int.Parse(textBox4.Text);
                                            int toplamkitapsayısı = mevcutkitapsayısı + eklenenkitapsayısı;

                                            string query3 = "UPDATE Kitap SET kitap_sayısı = @Yenikitapsayısı WHERE kitap_ismi = @kitap_ismi AND yazar_ismi = @yazar_ismi";
                                            using (SqlCommand command3 = new SqlCommand(query3, connection))
                                            {
                                                command3.Parameters.AddWithValue("@Yenikitapsayısı", toplamkitapsayısı);
                                                command3.Parameters.AddWithValue("@kitap_ismi", textBox1.Text);
                                                command3.Parameters.AddWithValue("@yazar_ismi", textBox2.Text);

                                                command3.ExecuteNonQuery();
                                            }
                                        }

                                        MessageBox.Show("Aynı kitap bulunduğundan kitap sayısı güncellenmiştir.", "Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                    else //BÖYLE BİR KİTAP YOK
                                    {
                                        string query4 = "INSERT INTO Kitap(kitap_ismi,yazar_ismi,yayın_tarihi,sayfa_sayısı,kitap_sayısı,raf_no) VALUES(@kitap_ismi,@yazar_ismi,@yayın_tarihi,@sayfa_sayısı,@kitap_sayısı,@raf_no)";
                                        using(SqlCommand command4 = new SqlCommand(query4, connection)) 
                                        {
                                            command4.Parameters.AddWithValue("@kitap_ismi",textBox1.Text);
                                            command4.Parameters.AddWithValue("@yazar_ismi",textBox2.Text);
                                            command4.Parameters.AddWithValue("@yayın_tarihi",dateTimePicker1.Value);
                                            command4.Parameters.AddWithValue("@sayfa_sayısı",textBox3.Text);
                                            command4.Parameters.AddWithValue("@kitap_sayısı",textBox4.Text);
                                            command4.Parameters.AddWithValue("@raf_no",textBox5.Text);

                                            command4.ExecuteNonQuery();




                                        }

                                        MessageBox.Show("Kitap girişi yapılmıştır.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }


                                }

                                connection.Close();

                            }
                           


                        }
                        else
                        {
                            MessageBox.Show("Kitap sayısı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Sayfa sayısı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }


                }
                else // Yazar adı kısmı boşsa
                {
                    MessageBox.Show("Yazar adı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else // İsim Kısmı Boşsa
            {
                MessageBox.Show("Kitap adı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Rakamlara ve kontrol tuşlarına (Backspace) izin ver
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;  // Girişi reddet
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Rakamlara ve kontrol tuşlarına (Backspace) izin ver
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;  // Girişi reddet
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            string bag = "Data Source=DESKTOP-MBNL0FO\\SQLEXPRESS;Initial Catalog=Kütüphane;Integrated Security=True";

            string query = "SELECT raf_no FROM Kitap WHERE kitap_ismi = @kitapismi AND yazar_ismi = @yazarismi";

            using (SqlConnection connection = new SqlConnection(bag))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kitapismi",textBox6.Text);
                    command.Parameters.AddWithValue("@yazarismi",textBox7.Text);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox8.Text = reader["raf_no"].ToString();
                            
                        }
                        else
                        {
                            MessageBox.Show("Kitap bulunamadı.");
                        }
                    }
                }
            }


        }
    }
}
