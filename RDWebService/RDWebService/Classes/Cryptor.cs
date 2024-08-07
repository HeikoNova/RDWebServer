using System.Text;

namespace RDWebService.Classes
{
    public static class Cryptor
    {
        public static byte[] generateKey()
        {

            byte[] key = new byte[16];
            int day2 = (int)System.DateTime.Now.DayOfWeek;
            int day = DateTime.Now.Day;
            string s = "RM" + day + "Example";
            byte[] keypart = Encoding.UTF8.GetBytes(s);

            for (int p = 0; p < 16; p++)
            {

                if (p < s.Length)
                {
                    key[p] = keypart[p];
                }
                else
                {
                    key[p] = (byte)(p - s.Length);
                }

            }



            int i = key.Length;

            return key;
        }
        public static byte[] generateIV(string dbuser)
        {

            byte[] iv = new byte[16];
            int processID = System.Diagnostics.Process.GetCurrentProcess().Id;
            string plaintext = processID + dbuser;
            byte[] ivpart = Encoding.UTF8.GetBytes(plaintext);

            for (int p = 0; p < 16; p++)
            {

                if (p < plaintext.Length)
                {
                    iv[p] = ivpart[p];
                }
                else
                {
                    iv[p] = (byte)(p - plaintext.Length);
                }
            }

            //if (plaintext.Length > 16)
            //{
            //    int rounds = plaintext.Length - 17;

            //    for (int p = rounds; p >= 0; p--)
            //    {
            //        int index = plaintext.Length;
            //        plaintext = plaintext.Remove(index-1);
            //    }
            //}

            //if i < 16 -> fill iv
            //iv = Encoding.UTF8.GetBytes(plaintext);
            int i = iv.Length;
            return iv;
        }
        public static int GetFirstOccurance(this byte[] array, byte element)
        {

            return Array.IndexOf(array, element);

        }
    }
}
