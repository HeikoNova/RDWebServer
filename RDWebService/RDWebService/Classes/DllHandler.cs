using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RDWebService.Classes
{
    public class DllHandler
    {
        public string serverInst;
        public string db;
        public string user;
        public string pw;
        private string sessionId = "";

        //C:\\Users\\RD-Administator\\source\\repos\\rolemanager22\\RoleManager-Src\\bin\\x64\\RU_DE\\
        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "Request", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Request(string sessionId, int requesttype, String request, byte[] requestresult, int buffer);
        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "ActivateConnection", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ActivateConnection(int sessionID, string user);
        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "logToFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern int logToFile(string logFileMessage);
        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "GetDBConnectParams", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDBConnectParams(byte[] serverInstance, int buffer, byte[] dbName, int buffer1, byte[] dbUser, int buffer2, byte[] dbPassw, int buffer3);

        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "ActivateSession", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ActivateSession(string user, string userSessionID);

        //NEW
        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "OpenList", CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenList(string pszSessionId, string pszListId, int nCommandId, byte[] pszRet, int nRetBufSize);

        [DllImport("C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll", EntryPoint = "GetListEntries", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetListEntries(string pszSessionId, string pszListId, int startEntry, int numberEntries, byte[] pszRet, int nRetBufSize);

        public static JObject LoadDll(int id)
        {
            JObject obj = new JObject();
            //Type type1 = null;
            //string t = "RM_API";
            //string dllPath1 = "C:\\RM_API.dll";
            //Assembly assembly = null;
            int requesttype = 1;
            int actionId = 32913;
            actionId = id;

            string request = "Json:{\"action\" : \"GET_SQL_SELECT_CMD\",\"actionId\" : " + " " + actionId + "}";
            //string request1 = "";

            int buffer = 8192;

            byte[] requestresult = new byte[buffer];
            byte[] bytes = Encoding.Default.GetBytes(request);
            request = Encoding.UTF8.GetString(bytes);
            //request1 = Encoding.Unicode.GetString(bytes);
            //Console.OutputEncoding = System.Text.Encoding.UTF8;




            int iresult = 0;
            int sessionID = 61;





            string ddlPath2 = "C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll";


            try
            {
                //ActivateConnection(61, "RD-FISCHER\\RD-Fischer");
                ActivateSession("RD-FISCHER\\RD-Fischer", "1s3Rd");
                Request("1s3Rd", requesttype, request, requestresult, buffer);

                string bitString = BitConverter.ToString(requestresult);
                string utfString = Encoding.UTF8.GetString(requestresult, 0, requestresult.Length);
                //JSON Object bauen und SQL Auslesen in diesem Fall
                //Console.WriteLine($"Assembly Name: {assembly.FullName}");
                JObject json = JObject.Parse(utfString);

                dynamic resultStatement = JsonConvert.DeserializeObject(utfString);

                string ti = resultStatement.sql;
                obj = json;

                return json;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            return obj;


        }
        public static string LoadDll1(int id)
        {
            JObject obj = new JObject();
            //Type type1 = null;
            //string t = "RM_API";
            //string dllPath1 = "C:\\RM_API.dll";
            //Assembly assembly = null;
            int requesttype = 1;
            int actionId = 32913;
            actionId = id;

            string request = "Json:{\"action\" : \"GET_SQL_SELECT_CMD\",\"actionId\" : " + " " + actionId + "}";
            //string request1 = "";

            int buffer = 8192;

            byte[] requestresult = new byte[buffer];
            byte[] bytes = Encoding.Default.GetBytes(request);
            request = Encoding.UTF8.GetString(bytes);
            //request1 = Encoding.Unicode.GetString(bytes);
            //Console.OutputEncoding = System.Text.Encoding.UTF8;




            int iresult = 0;
            int sessionID = 61;





            string ddlPath2 = "C:\\Users\\RD-Administator\\source\\repos\\RoleManagerRepo\\RoleManager-Src\\bin\\x64\\RU_DE\\RM_API.dll";


            try
            {
                //ActivateConnection(61, "RD-FISCHER\\RD-Fischer");
                ActivateSession("RD-FISCHER\\RD-Fischer", "1s3Rd");
                Request("1s3Rd", requesttype, request, requestresult, buffer);

                string bitString = BitConverter.ToString(requestresult);
                string utfString = Encoding.UTF8.GetString(requestresult, 0, requestresult.Length);
                //JSON Object bauen und SQL Auslesen in diesem Fall
                //Console.WriteLine($"Assembly Name: {assembly.FullName}");
                JObject json = JObject.Parse(utfString);

                dynamic resultStatement = JsonConvert.DeserializeObject(utfString);

                string ti = resultStatement.sql;
                obj = json;

                return utfString;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            return "";


        }

        public void LoadDbSettings()
        {


            int buffer = 128;
            int buffer1 = 128;
            int buffer2 = 128;
            int buffer3 = 128;
            byte[] serverInstance = new byte[buffer];
            byte[] dbName = new byte[buffer1];
            byte[] dbUser = new byte[buffer2];
            byte[] dbPassw = new byte[buffer3];

            //result

            int len = 0;
            try
            {
                GetDBConnectParams(serverInstance, buffer, dbName, buffer1, dbUser, buffer2, dbPassw, buffer3);
                serverInst = Encoding.UTF8.GetString(serverInstance);
                //byte[] b =  serverInstance.FirstOrDefault<byte[]>("\0");
                int f = serverInstance.Length;
                //int si =serverInstance.FirstOrDefault<string>("\0");
                //serverInst.Substring(0,)

                //METHODE

                //for (int i = 0; i < serverInstance.Length; i++)
                //{
                //    if(serverInstance[i] == 0)
                //    {
                //        len = i; break;
                //    }
                //}

                //Encryptor.GetFirstOccurance(serverInstance, 0);
                //int firstIndex = Encryptor.GetFirstOccurance(serverInstance, 0);
                serverInst = Encoding.UTF8.GetString(serverInstance, 0, Cryptor.GetFirstOccurance(serverInstance, 0));
                db = Encoding.UTF8.GetString(dbName, 0, Cryptor.GetFirstOccurance(dbName, 0));
                user = Encoding.UTF8.GetString(dbUser, 0, Cryptor.GetFirstOccurance(dbUser, 0));
                pw = Encoding.UTF8.GetString(dbPassw, 0, Cryptor.GetFirstOccurance(dbPassw, 0));


            }
            catch (Exception)
            {

                throw;
            }
            //serverInst + "?" + db + "?" + user + "?" + pw

        }
        public static void setConnection()
        {
            int i = ActivateConnection(61, "RD-FISCHER\\RD-Fischer");

        }
        public static void logFileEntry(string logFileMessage)
        {
            logToFile(logFileMessage + "\n");
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            //plaintext = srDecrypt.ReadToEnd();
                            plaintext = srDecrypt.ReadToEnd();
                            if (plaintext.Substring(plaintext.Length - 1, 1) == "\0")
                            {
                                plaintext = plaintext.Substring(0, plaintext.Length - 1);
                            }
                        }
                    }
                }
            }

            return plaintext;
        }
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        public static JObject openList()
        {
            int requesttype = 1;
            int buffer = 8192;
            byte[] requestresult = new byte[buffer];
            JObject json = new JObject();

            try
            {
                //ActivateConnection(61, "RD-FISCHER\\RD-Fischer");
                string usern = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                ActivateSession(usern, Environment.ProcessId.ToString());
                OpenList(Environment.ProcessId.ToString(), "329131", 32913, requestresult, buffer);

                string utfString = Encoding.UTF8.GetString(requestresult, 0, requestresult.Length);

                json = JObject.Parse(utfString);



                return json;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            return json;
        }

        public static string getListEntries()
        {
            int requesttype = 1;
            int buffer = 81920;
            byte[] requestresult = new byte[buffer];
            byte[] requestresult2 = new byte[buffer];
            //byte[] resultValue = new byte[buffer];
            


            try
            {
                //ActivateConnection(61, "RD-FISCHER\\RD-Fischer");
                string usern = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                ActivateSession(getUsername(), "188");
                OpenList("188", "123", 32913, requestresult, buffer);
                GetListEntries("188", "123", 0, 20, requestresult2, buffer);
                //OpenList(Environment.ProcessId.ToString(), "329131", 32913, requestresult, buffer);

                string utfString = Encoding.UTF8.GetString(requestresult2, 0, requestresult2.Length);
                Console.WriteLine(utfString);
                string t = utfString.Replace(@"\", string.Empty);
                //OpenList
                string utfString1 = Encoding.UTF8.GetString(requestresult, 0, requestresult.Length);

                return t;




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            
            return "";
        }

        public static string getUsername()
        {

            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
        public static string getSessionId()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name + DateTime.Now.ToString();
        }
    }
}

