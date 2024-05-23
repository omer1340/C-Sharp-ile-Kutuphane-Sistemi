using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;

namespace Github1
{
    public partial class FormKitapAlma : Form
    {
        public FormKitapAlma()
        {
            InitializeComponent();
        }

        string bag = "Data Source=DESKTOP-MBNL0FO\\SQLEXPRESS;Initial Catalog=Kütüphane;Integrated Security=True";



        private void FormYazarİşlemleri_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.White;
            label3.BackColor = Color.Transparent;
            label3.ForeColor = Color.White;
            label4.BackColor = Color.Transparent;
            label4.ForeColor = Color.White;
            

            textBox1.Font = new Font("Arial", 21, FontStyle.Regular);
            comboBox1.Font = new Font("Arial", 13, FontStyle.Regular);
            comboBox1.DropDownStyle= ComboBoxStyle.DropDownList;

            // SqlCommand komut = new SqlCommand("SELECT * FROM Kitap",bag);

            using (SqlConnection  connection = new SqlConnection(bag))
            {
                connection.Open();
                string query = "Select * From Kitap";

                using (SqlCommand command = new SqlCommand(query, connection)) 
                {
                    SqlDataReader dr;

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        comboBox1.Items.Add(dr["kitap_ismi"]+ "-" +dr["yazar_ismi"]);


                    }                                             
                }

                connection.Close();

            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Rakamlara ve kontrol tuşlarına (Backspace) izin ver
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;  // Girişi reddet
            }

            // TextBox'ta zaten 11 karakter varsa ve basılan tuş Backspace değilse, yeni karakterleri engelle
            if (textBox1.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VeriKontrol veriKontrol = new VeriKontrol();

            if (veriKontrol.IsElevenDigitNumber(textBox1.Text)) //Doğru Şekilde Girilmiş TC
            {
                if (comboBox1.SelectedItem != null)
                {
                    // ComboBox'tan seçilen öğeyi al
                    string selectedValue = comboBox1.SelectedItem.ToString();

                    // Seçilen öğeyi ayırarak kitapismi ve yazarismi değişkenlerine ata
                    string[] parts = selectedValue.Split(new string[] { "-" }, StringSplitOptions.None);

                    string kitapismi = parts[0];
                    string yazarismi = parts[1];

                    string query7 = "SELECT COUNT(*) FROM Üye WHERE Tc = @tc";

                    using (SqlConnection connection = new SqlConnection(bag))
                    {
                        using(SqlCommand command8 = new SqlCommand(query7, connection))
                        {
                            command8.Parameters.AddWithValue("@tc", textBox1.Text);

                            connection.Open();

                            int uyemi = (int)command8.ExecuteScalar();

                            // Sonuca göre işlem yapma
                            if (uyemi > 0)
                            {
                                string query2 = "SELECT book_id FROM Kitap WHERE kitap_ismi = @KitapAdi AND yazar_ismi = @YazarAdi";


                                using (SqlCommand command = new SqlCommand(query2, connection))  // Book_id bulmak için 
                                {
                                    // Parametre ekleme
                                    command.Parameters.AddWithValue("@KitapAdi", kitapismi);
                                    command.Parameters.AddWithValue("@YazarAdi", yazarismi);



                                    int book_id =(int)command.ExecuteScalar();

                                   



                                     string query3 = "SELECT kitap_sayısı FROM Kitap WHERE book_id = @book_id";
                                    

                                     using (SqlCommand command2 = new SqlCommand(query3, connection)) // Seçili book_id nin kitap sayısını bulmak için 
                                     {
                                         command2.Parameters.AddWithValue("@book_id", book_id);

                                         int kitap_sayisi = Convert.ToInt32(command2.ExecuteScalar());

                                         string query4 = "SELECT COUNT(*) FROM İşlemler WHERE book_id = @book_ıd AND Alınan_tarih IS NOT NULL AND İade_tarih IS NULL";

                                         using (SqlCommand command3 = new SqlCommand(query4, connection)) // Seçili book_id kitabının kaç adet emanet edildiği
                                         {
                                             command3.Parameters.AddWithValue("@book_ıd", book_id);

                                             int elimizdeolmayankitap = Convert.ToInt32(command3.ExecuteScalar());

                                             int deneme = kitap_sayisi - elimizdeolmayankitap;
 
                                            if (kitap_sayisi - elimizdeolmayankitap > 0)
                                            {
                                                string query5 = "SELECT COUNT(*) FROM İşlemler WHERE Tc = @Tc AND Alınan_tarih IS NOT NULL AND İade_tarih IS NULL";

                                                using (SqlCommand command6 = new SqlCommand(query5, connection))
                                                {
                                                    command6.Parameters.AddWithValue("@Tc", textBox1.Text);

                                                    int kitapalmısmı = Convert.ToInt32(command6.ExecuteScalar()); // Seçili Tcdeki kişinin alıp geri getirmediği kitap sayısı

                                                    


                                                    
                                                    if (kitapalmısmı == 0)
                                                    {
                                                        string query6 = "INSERT INTO İşlemler (book_id, Tc, Alınan_Tarih) VALUES (@book_id, @tc, @Alınan_Tarih)";

                                                        using (SqlCommand command7 = new SqlCommand(query6, connection))
                                                        {
                                                            DateTime bugununTarihi = DateTime.Today;

                                                            command7.Parameters.AddWithValue("@book_id", book_id);
                                                            command7.Parameters.AddWithValue("@tc", textBox1.Text);
                                                            command7.Parameters.AddWithValue("@Alınan_Tarih", bugununTarihi);

                                                            command7.ExecuteNonQuery();

                                                            MessageBox.Show("Kitap başarıyla alınmıştır...");

                                                        }

                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Kişinin emanet aldığı kitap vardır.Yeni kitap alamaz...", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                                    }
                                                    

                                                }

                                            }
                                            else
                                            {
                                                MessageBox.Show("Kitaptan elimizde kalmamıştır...", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                            }

                                            

                                         }

                                     }
                                     

                                    connection.Close();

                                }

                            }
                            else
                            {
                                // Veritabanında TC numarasına sahip bir veri yok
                                MessageBox.Show("Belirtilen Tc numarasına ait kaydımız yoktur.Lütfen önce kayıt olunuz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }

                        connection.Close();

                    }


                }
                else
                {
                    // ComboBox boş ise
                    MessageBox.Show("Kitap seçiniz...", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Yanlış şekilde Girilen Tc!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
