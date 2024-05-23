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
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;


namespace Github1
{
    public partial class FormÜyelikİşlemleri : Form
    {
        public FormÜyelikİşlemleri()
        {
            InitializeComponent();
        }
       

        private void FormÜyelikİşlemleri_Load(object sender, EventArgs e)
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
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Add("Erkek");
            comboBox1.Items.Add("Kadın");
            textBox3.Font = new Font("Arial", 10, FontStyle.Regular);
            textBox4.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox5.Font = new Font("Arial", 15, FontStyle.Regular);
            textBox6.Font = new Font("Arial", 10, FontStyle.Regular);

            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;

        }



        public void button1_Click(object sender, EventArgs e)
        {

            VeriKontrol veriKontrol = new VeriKontrol();   
               // İsim değişkeni için method çalıştırıp return ile değer alıyoruz.


  
                if (veriKontrol.Boşad(textBox1.Text))  //İsim Kısmı Dolu Buraya (true değeri alır.)
                {
                   
                     if (veriKontrol.IsElevenDigitNumber(textBox2.Text)) //Doğru Şekilde Girilmiş TC
                     {

                           if(comboBox1.SelectedItem != null) //combobox seçimi yapılmış
                           {
                              if (veriKontrol.Email(textBox3.Text)) //E mail doğru
                              {
                                   string bag = "Data Source=DESKTOP-MBNL0FO\\SQLEXPRESS;Initial Catalog=Kütüphane;Integrated Security=True";
                                 
                                   bool varMi = false;
                               
                                

                                  // Veritabanı bağlantısı oluşturma
                                  using (SqlConnection connection = new SqlConnection(bag))
                                  {
                                     // Veritabanı sorgusu
                                     string query = "SELECT COUNT (*) FROM Üye WHERE Tc =@Tc";

                                    // Sorguyu ve bağlantıyı SqlCommand nesnesine atanması
                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                      // Parametre ekleme
                                      command.Parameters.AddWithValue("@Tc", textBox2.Text);

                                      // Bağlantıyı açma
                                       connection.Open();

                                      // Sorguyu çalıştırma ve sonucu alma
                                      int rowCount = (int)command.ExecuteScalar();

                                      // Sonuca göre varMi değerini belirleme
                                       varMi = (rowCount > 0);

                                        // Bağlantıyı kapama
                                        connection.Close();
                                    }
                                  }
                                  if (varMi)  // AYNI TC'Lİ ÜYE VARSA BURAYA GİRER
                                  {
                                   MessageBox.Show("Aynı Tc'ye sahip üye bulunmaktadır..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                  }
                                  else // BÜTÜN KONTROLLERDEN GEÇİP VERİTABANINA EKLEME İŞLEMİ
                                  {
                                     string query2 = "INSERT INTO Üye(İsim, Tc, DoğumTarihi, Cinsiyet, email) VALUES(@İsim, @Tc, @DoğumTarihi, @Cinsiyet, @email)";
                                     using ( SqlConnection connection = new SqlConnection(bag))   
                                     {
                                       using(SqlCommand command = new SqlCommand(query2, connection))
                                       {
                                        connection.Open();
                                        command.Parameters.AddWithValue("@İsim", textBox1.Text);
                                        command.Parameters.AddWithValue("@Tc", textBox2.Text);
                                        command.Parameters.AddWithValue("@DoğumTarihi", dateTimePicker1.Value);
                                        command.Parameters.AddWithValue("@Cinsiyet", comboBox1.Text);
                                        command.Parameters.AddWithValue("@email", textBox3.Text);

                                        command.ExecuteNonQuery();
                                        connection.Close();

                                        MessageBox.Show("Kayıt Başarılı");

                                       }
                                     }
                                  }
                              }
                              else  // Yanlış E posta
                              {
                                    MessageBox.Show("E mail adresi hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                              }


                           }
                           else  // Cinsiyet seçimi yapılmamış.
                           {

                                MessageBox.Show("Cinsiyet Seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);


                           }   
                     }
                     else //TC Kısmı Boş
                     {
                        MessageBox.Show("Yanlış şekilde Girilen Tc!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }                  
                }
                else // İsim Kısmı Boş
                {
                   MessageBox.Show("İsim kısmı boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) //Tc Girişi Kontrol 
        {
           
                // Rakamlara ve kontrol tuşlarına (Backspace) izin ver
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;  // Girişi reddet
                }

                // TextBox'ta zaten 11 karakter varsa ve basılan tuş Backspace değilse, yeni karakterleri engelle
                if (textBox2.Text.Length >= 11 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Rakamlara ve kontrol tuşlarına (Backspace) izin ver
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;  // Girişi reddet
            }

            // TextBox'ta zaten 11 karakter varsa ve basılan tuş Backspace değilse, yeni karakterleri engelle
            if (textBox4.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            textBox6.Clear();


            VeriKontrol veriKontrol = new VeriKontrol();
            // İsim değişkeni için method çalıştırıp return ile değer alıyoruz.


            if (veriKontrol.IsElevenDigitNumber(textBox4.Text)) //Doğru Şekilde Girilmiş TC
            {
                string bag = "Data Source=DESKTOP-MBNL0FO\\SQLEXPRESS;Initial Catalog=Kütüphane;Integrated Security=True";

                bool varMi = false;

                // Veritabanı bağlantısı oluşturma
                using (SqlConnection connection = new SqlConnection(bag))
                {
                    // Veritabanı sorgusu
                    string query = "SELECT COUNT (*) FROM Üye WHERE Tc =@Tc";

                    // Sorguyu ve bağlantıyı SqlCommand nesnesine atanması
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekleme
                        command.Parameters.AddWithValue("@Tc", textBox4.Text);

                        // Bağlantıyı açma
                        connection.Open();

                        // Sorguyu çalıştırma ve sonucu alma
                        int rowCount = (int)command.ExecuteScalar();

                        // Sonuca göre varMi değerini belirleme
                        varMi = (rowCount > 0);

                        
                    }

                    if (!varMi)
                    {
                        MessageBox.Show("Bu Tc kimlik numarasına ait üyemiz bulunmamaktadır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Üye bilgileri gösterme
                        string query2 = "SELECT isim, email FROM Üye WHERE Tc = @tc";
                        using (SqlCommand command2 = new SqlCommand(query2, connection))
                        {
                            command2.Parameters.AddWithValue("@tc",textBox4.Text);

                            using (SqlDataReader reader = command2.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Sonuçlardan isim ve email değerlerini al
                                    textBox5.Text = reader["isim"].ToString();
                                    textBox6.Text = reader["email"].ToString();
                                }
                            }



                        }



                    }

                    // Bağlantıyı kapama
                    connection.Close();
                }
                
            }
           
            else
            {
                MessageBox.Show("Yanlış şekilde Girilen Tc!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
