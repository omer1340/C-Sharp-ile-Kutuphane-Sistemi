using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Github1
{
     class VeriKontrol{
        public bool Boşad(string input)
        {
                  if (string.IsNullOrWhiteSpace(input))  //İsim Kısmı Boşsa Buraya
                  {
                   return false;
                  }
                  else // İsim Kısmı Dolu
                  {

                   return true;

                  }


        }

        public bool IsElevenDigitNumber(string input) //tc 11 haneli ve sadece sayımı
        {
            return input.Length == 11 && long.TryParse(input, out long _);
        }

        public bool IsNumber(string input)
        {
            return int.TryParse(input, out int _);
        }

       public bool Email(string input)
        {
            if(string.IsNullOrWhiteSpace(input)) //Email Kısmı Boşsa buraya girer.
            {
                return false;
            }
            else  //Email Kısmı doluysa buraya girer.
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";  
                bool değişken1 = Regex.IsMatch(input, pattern);  // Doğru bir epostamı sorgulama.

                if (değişken1 == true) //Doğru bir eposta.
                {
                    return true;
                }
                else //Yanlış bir eposta.
                {
                    return false;
                }

               
            }


        }

    }
}
