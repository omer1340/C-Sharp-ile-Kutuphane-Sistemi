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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Github1
{
    public partial class FormAlınanKitaplar : Form
    {
        public FormAlınanKitaplar()
        {
            InitializeComponent();
        }

        string bag = "Data Source=DESKTOP-MBNL0FO\\SQLEXPRESS;Initial Catalog=Kütüphane;Integrated Security=True";

        private void FormAlınanKitaplar_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.White;
            label3.BackColor = Color.Transparent;
            label3.ForeColor = Color.White;
            label4.BackColor = Color.Transparent;
            label4.ForeColor = Color.White;
            label5.BackColor = Color.Transparent;
            label5.ForeColor = Color.White;
            label6.BackColor = Color.Transparent;
            label6.ForeColor = Color.White;
            label7.BackColor = Color.Transparent;
            label7.ForeColor = Color.White;
            label8.BackColor = Color.Transparent;
            label8.ForeColor = Color.White;
            label9.BackColor = Color.Transparent;
            label9.ForeColor = Color.White;
            textBox1.Font = new Font("Arial", 14, FontStyle.Regular);

            textBox2.Font = new Font("Arial", 14, FontStyle.Regular);
            textBox3.Font = new Font("Arial", 10, FontStyle.Regular);
            textBox4.Font = new Font("Arial", 14, FontStyle.Regular);
            textBox5.Font = new Font("Arial", 12, FontStyle.Regular);
            textBox6.Font = new Font("Arial", 12, FontStyle.Regular);
            textBox7.Font = new Font("Arial", 14, FontStyle.Regular);


            GetData();
            dataGridView1.ReadOnly = true;

            button2.Enabled = false;

            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;

        }

        private void GetData()
        {
            // SqlConnection oluşturma
            using (SqlConnection connection = new SqlConnection(bag))
            {
                // SQL sorgusu
                string query = "SELECT * FROM İşlemler WHERE iade_Tarih IS NULL"; // Tablo adını kendi tablonuza göre güncelleyin

                // SqlDataAdapter ve DataTable oluşturma
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                // Veri kaynağını yükleme
                adapter.Fill(dataTable);

                // DataGridView'e veri kaynağını bağlama
                dataGridView1.DataSource = dataTable;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Eğer TextBox'ta metin varsa
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                // Metindeki her karakter bir rakam değilse
                if (!textBox1.Text.All(char.IsDigit))
                {
                    // Sadece sayıları al ve metni güncelle
                    textBox1.Text = new string(textBox1.Text.Where(char.IsDigit).ToArray());
                    // Metni temizle ve en fazla 11 karakter kabul et
                    textBox1.Text = textBox1.Text.Substring(0, Math.Min(textBox1.Text.Length, 11));
                    // Cursor'ı metnin sonuna taşı
                    textBox1.SelectionStart = textBox1.Text.Length;
                }
                else if (textBox1.Text.Length > 11) // Eğer metin 11 karakterden uzunsa
                {
                    // Metni en fazla 11 karaktere kısalt
                    textBox1.Text = textBox1.Text.Substring(0, 11);
                    // Cursor'ı metnin sonuna taşı
                    textBox1.SelectionStart = textBox1.Text.Length;
                }
            }

            // TextBox'taki metnin yalnızca rakamlardan oluşup oluşmadığını kontrol et
            if (textBox1.Text.Length == 11)
            {

            }
            else
            {
                button2.Enabled = false; // Aksi halde butonu devre dışı bırak
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();

            button2.Enabled = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            button2.Enabled = false;

            string query = "SELECT COUNT(*) FROM Üye WHERE Tc = @tc";  // Kullanıcı üye mi diye bakalım.

            using (SqlConnection connection = new SqlConnection(bag))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tc", textBox1.Text);

                    connection.Open();

                    int uyemi = (int)command.ExecuteScalar();

                    // Sonuca göre işlem yapma
                    if (uyemi > 0)
                    {
                        string query5 = "SELECT COUNT(*) FROM İşlemler WHERE tc = @tc AND İade_Tarih IS NULL";
                        SqlCommand command5 = new SqlCommand(query5, connection);
                        command5.Parameters.AddWithValue("@tc", textBox1.Text);
                        int count = (int)command5.ExecuteScalar();

                        if (count > 0)
                        {
                            string query2 = "SELECT isim, email FROM Üye WHERE Tc = @tc";

                            using (SqlCommand command2 = new SqlCommand(query2, connection))
                            {

                                command2.Parameters.AddWithValue("@tc", textBox1.Text);

                                using (SqlDataReader reader = command2.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        // Sonuçlardan isim ve email değerlerini al
                                        textBox2.Text = reader["isim"].ToString();
                                        textBox3.Text = reader["email"].ToString();
                                    }
                                }
                            }

                            string query3 = "SELECT Alınan_Tarih, book_id FROM İşlemler WHERE tc = @tc AND İade_Tarih IS NULL";

                            using (SqlCommand command3 = new SqlCommand(query3, connection))
                            {
                                command3.Parameters.AddWithValue("@tc", textBox1.Text);
                                using (SqlDataReader reader1 = command3.ExecuteReader())
                                {
                                    if (reader1.Read())
                                    {
                                        // Sonuçlardan isim ve email değerlerini al
                                        textBox4.Text = reader1["Alınan_Tarih"].ToString();
                                        string book_id = reader1["book_id"].ToString();


                                        reader1.Close();


                                        string query4 = "SELECT kitap_ismi, yazar_ismi, sayfa_sayısı FROM Kitap WHERE book_id = @book_id";

                                        SqlCommand command4 = new SqlCommand(query4, connection);
                                        command4.Parameters.AddWithValue("@book_id", book_id);

                                        SqlDataReader reader2 = command4.ExecuteReader();


                                        if (reader2.Read())
                                        {
                                            textBox5.Text = reader2["kitap_ismi"].ToString();
                                            textBox6.Text = reader2["yazar_ismi"].ToString();
                                            textBox7.Text = reader2["sayfa_sayısı"].ToString();
                                        }
                                        reader2.Close();

                                        button2.Enabled = true;


                                    }
                                }
                            }

                        }
                        else
                        {
                            // Kayıt yoksa farklı bir mesaj göster
                            MessageBox.Show("Belirtilen Tc numarasının emanet aldığı kitap yoktur...", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button2_Click(object sender, EventArgs e)
        {

            string query = "UPDATE İşlemler SET İade_Tarih = @iade_Tarih WHERE Tc = @tc AND İade_Tarih IS NULL";

            using (SqlConnection connection = new SqlConnection(bag))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tc",textBox1.Text);
                    command.Parameters.AddWithValue("@iade_Tarih", DateTime.Now);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Güncelleme başarılı
                        MessageBox.Show("Kayıt başarıyla güncellendi.");
                        GetData();
                    }
                    else
                    {
                        // Güncelleme başarısız
                        MessageBox.Show("Güncellenecek kayıt bulunamadı.");
                    }

                }
            }
        }


    }
}
