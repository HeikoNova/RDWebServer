using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RDWebService.Classes;
using RDWebService.Services;
using System.Collections;
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace RDWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        private readonly int sessionId = 61;
        public int isActivated = 0;
        [HttpGet("GetTableDataJSON2")]
        public async Task<ActionResult<List<Object>>> getTableDataJSON2()
        {
            //DataSet mydataset = SqlService.getDataFromSql("Select * from Roles");
            //JSON contains whole data -> need to be unique / need to be compiled only once
            string result = string.Empty;
            string tester = "select R.RoleId as [Rollen-Id],R.RoleName as [Rolle],R.CritFactor as [Kritikalität Rolle],R.AutoAssignment as [Auto],case when R.RoleStatus=0 then 'in Bearbeitung' when R.RoleStatus=1 then 'fertig gestellt' when R.RoleStatus=2 then 'abgelehnt' when R.RoleStatus=3 then 'in Freigabe' when R.RoleStatus=4 then 'freigegeben' when R.RoleStatus=5 then 'in Verwendung' when R.RoleStatus=6 then 'in Löschung' else 'unbekannt' end as [Status],R.RoleCategory as [Kategorie],R.RoleType as [Rollen-Typ],R.EntitlementCount as [Anzahl Berechtigungen],R.IDCount as [Anzahl Identitäten],R.RoleStatus as [_RoleStatus],R.isVirtual as [_isVirtual],R.validFromDate as [gültig ab],R.validToDate as [gültig bis],R.RoleExportDate as [exportiert],R.[Description] as [_Description Rolle],R.OwnerId as [EigentümerIn-Id],ISNULL(UO.SecondName,'')+', '+ISNULL(UO.FirstName,'')+', '+ISNULL(UO.UserId,'') as [EigentümerIn],R.IDCount*R.EntitlementCount as [Zuordnungen],R.RoleCreationDate+' '+ISNULL(R.CreatedByUser,'') as [erstellt],R.RoleChangeDate+' '+ISNULL(R.ChangedByUser,'') as [geändert],R.RoleVersion+' '+ISNULL(R.DefinedByUser,'') as [fertig gestellt],R.RoleReleaseDate+' '+ISNULL(R.ReleasedByUser,'') as [freigegeben],R.ApprovalResponse as [Antwort Genehmigung] from Roles as R left join Users as UO on UO.ID=R.OwnerId left join Roles as RO on RO.RoleId=R.OwnerRoleId ";

            DataTable table = new DataTable();
            table = Database.getDataFromSqlAsTable(tester);
            //var stringArr = table.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();
           

            var tableData = JsonConvert.SerializeObject(table, Formatting.None);
           // tableData += "Data";

            return Ok(tableData);





        }
        [HttpGet("GetTableDataJSON")]
        public async Task<ActionResult<List<Object>>> getTableDataJSON()
        {
            //DataSet mydataset = SqlService.getDataFromSql("Select * from Roles");
            //JSON contains whole data -> need to be unique / need to be compiled only once
            string json = string.Empty;
            try
            {
                json = JsonConvert.SerializeObject(DllHandler.LoadDll(32912));

            }
            catch (Exception e)
            {
                DllHandler.logToFile("[WEBSERVER] : Objekt konnte nicht geladen werden.");
                throw;
            }
            if (json != null)
            {
                var data = JObject.Parse(json);
                var value = data["sql"];


                if (value != null)
                {
                    DataTable table = new DataTable();
                    table = Database.getDataFromSqlAsTable(value.ToString());
                    JObject keyValuePairs = new JObject();
                    var tableData = JsonConvert.SerializeObject(table, Formatting.None);
                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(tableData);

                    return Ok(arrayList);
                }
            }
            else

                DllHandler.logToFile("[WEBSERVER] : Objekt konnte nicht geladen werden.");


            return Ok();

        }
        [HttpGet("GetHeaderDataJSON")]
        public async Task<ActionResult<List<String>>> getHeaderDataJSON()
        {
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32913));
            var data = JObject.Parse(json);


            var value = data["sql"];
            if (value != null)
            {
                //DataSet dataSet = SqlService.getDataFromSql(value.ToString());
                System.Data.DataTable table = new DataTable();
                table = Database.getDataFromSqlAsTable(value.ToString());
                ArrayList list = new ArrayList();

                foreach (DataColumn col in table.Columns)
                {
                    //if (!col.ColumnName.StartsWith("_"))
                    var s = JsonConvert.SerializeObject(col.ColumnName, Formatting.Indented);

                    //var t = "Name: " + s + " DisplayedColumn: " + col.ColumnName;
                    s = s.Replace("\"", "");
                    list.Add(s);


                }
                var tableHeaderData = JsonConvert.SerializeObject(list);
                return Ok(list);
            }
            return NotFound("Connection not established");

        }

        [HttpGet("GetRoleTableHeaders")]
        public async Task<ActionResult<List<String>>> getRoleTableHeaders()
        {
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32913));
            var data = JObject.Parse(json);

            var value = data["sql"];
            if (value != null)
            {
                //DataSet dataSet = SqlService.getDataFromSql(value.ToString());
                System.Data.DataTable table = new DataTable();
                table = Database.getDataFromSqlAsTable(value.ToString());
                ArrayList list = new ArrayList();

                foreach (DataColumn col in table.Columns)
                {
                    //if (!col.ColumnName.StartsWith("_"))
                    list.Add(col.ColumnName);


                }
                return Ok(list);
            }
            return NotFound("Connection not established");

        }
        [HttpGet("GetJSON")]
        public async Task<ActionResult<List<Object>>> getjson()
        {
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32913));
            var value = string.Empty;
            var data = new JObject();
            try
            {
                data = JObject.Parse(json);

            }
            catch (Exception)
            {

                throw;
            }
            if (data.Count > 0)
            {
                value = (string)data["sql"];
            }
            if (value != null && value != string.Empty)
            {

                DataTable table = new DataTable();
                table = Database.getDataFromSqlAsTable(value.ToString());
                string json2 = JsonConvert.SerializeObject(table, Formatting.Indented);
                ArrayList listRows1 = new ArrayList();
                foreach (DataRow rows in table.Rows)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        listRows1.Add((string)col.ColumnName + " : " + rows[col]);
                    }
                }
                return Ok(listRows1);
            }
            return Ok();

        }
        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<JObject>>> getjson1()
        {
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32913));
            var data = JObject.Parse(json);
            var value = string.Empty;
            if (data.Count > 0)
            {
                value = (string)data["sql"];

            }
            if (value != null && value != string.Empty)
            {
                //DataSet dataSet = Database.getDataFromSql(value.ToString());
                DataTable table = new DataTable();
                table = Database.getDataFromSqlAsTable(value.ToString());
                ArrayList list = new ArrayList();
                foreach (DataColumn col in table.Columns)
                {
                    foreach (DataRow rows in table.Rows)
                    {
                        ArrayList listRows = new ArrayList();
                        foreach (Object obj in rows.ItemArray)
                        {
                            listRows.Add(obj.ToString());

                        }
                        //list.Add(col.ColumnName + rows.ItemArray);
                        list.Add(listRows);

                    }
                }


                return Ok(list);
            }
            return NotFound("Connection not established");

        }
        [HttpGet("GetEntitlementTableHeaders")]
        public async Task<ActionResult<List<String>>> getEntitlementTableHeaders()
        {
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32916));
            var data = JObject.Parse(json);
            var value = string.Empty;
            if (data.Count > 0)
            {
                value = (string)data["sql"];

            }

            if (value != null)
            {
                DataSet dataSet = Database.getDataFromSql(value.ToString());
                DataTable table = new DataTable();
                table = Database.getDataFromSqlAsTable(value.ToString());
                ArrayList list = new ArrayList();

                foreach (DataColumn col in table.Columns)
                {
                    //if (!col.ColumnName.StartsWith("_"))
                    list.Add(col.ColumnName);


                }
                return Ok(list);
            }
            return NotFound("Connection not established");

        }

        [HttpGet("GetRoleTableData")]
        public async Task<ActionResult<List<String>>> getRoleTableData()
        {
            //If json is not convertable or Return Value does not fit -> ERROR 10001
            //if value is not convertable because of sql failure -> ERROR 10002
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32913));
            if (json == null)
                DllHandler.logToFile("[WebServer] Table Data could not be loaded successfully ErrorCode:10001 \n");
            if (json != null)
            {
                var data = JObject.Parse(json);
                var value = data["sql"];

                string tester = "select R.RoleId as [Rollen-Id],R.RoleName as [Rolle],R.CritFactor as [Kritikalität Rolle],R.AutoAssignment as [Auto],case when R.RoleStatus=0 then 'in Bearbeitung' when R.RoleStatus=1 then 'fertig gestellt' when R.RoleStatus=2 then 'abgelehnt' when R.RoleStatus=3 then 'in Freigabe' when R.RoleStatus=4 then 'freigegeben' when R.RoleStatus=5 then 'in Verwendung' when R.RoleStatus=6 then 'in Löschung' else 'unbekannt' end as [Status],R.RoleCategory as [Kategorie],R.RoleType as [Rollen-Typ],R.EntitlementCount as [Anzahl Berechtigungen],R.IDCount as [Anzahl Identitäten],R.RoleStatus as [_RoleStatus],R.isVirtual as [_isVirtual],R.validFromDate as [gültig ab],R.validToDate as [gültig bis],R.RoleExportDate as [exportiert],R.[Description] as [_Description Rolle],R.OwnerId as [EigentümerIn-Id],ISNULL(UO.SecondName,'')+', '+ISNULL(UO.FirstName,'')+', '+ISNULL(UO.UserId,'') as [EigentümerIn],R.IDCount*R.EntitlementCount as [Zuordnungen],R.RoleCreationDate+' '+ISNULL(R.CreatedByUser,'') as [erstellt],R.RoleChangeDate+' '+ISNULL(R.ChangedByUser,'') as [geändert],R.RoleVersion+' '+ISNULL(R.DefinedByUser,'') as [fertig gestellt],R.RoleReleaseDate+' '+ISNULL(R.ReleasedByUser,'') as [freigegeben],R.ApprovalResponse as [Antwort Genehmigung] from Roles as R left join Users as UO on UO.ID=R.OwnerId left join Roles as RO on RO.RoleId=R.OwnerRoleId ";
                if (value != null)
                {
                    //DataSet dataSet = SqlService.getDataFromSql(value.ToString());
                    DataTable table = new DataTable();
                    table = Database.getDataFromSqlAsTable(tester);
                    string jay = (String)JsonConvert.SerializeObject(table);
                    var token = JToken.Parse(jay);
                    var jTokenProperties = token.Children().OfType<JProperty>();


                    //JObject inner = token.Value<JObject>();
                    //TODO: is this safe or do we need a ordered list 
                    ArrayList list = new ArrayList();
                    //JArray arr = new JArray();

                    foreach (DataRow rows in table.Rows)
                    {
                        ArrayList listRows = new ArrayList();
                        foreach (Object obj in rows.ItemArray)
                        {
                            listRows.Add(obj.ToString());

                        }
                        list.Add(listRows);

                    }



                    DllHandler.logToFile("[WebServer] Table Data loaded successfully \n");
                    return Ok(list);
                }
                else
                {
                    DllHandler.logToFile("[WebServer] Table Data could not be loaded successfully ErrorCode: 10002\n");
                }
            }
            DllHandler.logToFile("[WebServer] Table Data could not be loaded successfully \n");
            return NotFound("Connection not established");
        }
        [HttpGet("GetEntitlementTableData")]
        public async Task<ActionResult<List<String>>> getEntitlementTableData()
        {
            //DataSet mydataset = SqlService.getDataFromSql("Select * from Roles");
            //JSON contains whole data -> need to be unique / need to be compiled only once
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32912));

            if (json != null)
            {
                var data = JObject.Parse(json);
                var value = data["sql"];

                if (value != null)
                {
                    //DataSet dataSet = SqlService.getDataFromSql(value.ToString());
                    DataTable table = new DataTable();
                    table = Database.getDataFromSqlAsTable(value.ToString());

                    //string jay = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                    //TODO: is this safe or do we need a ordered list 
                    ArrayList list = new ArrayList();

                    foreach (DataRow rows in table.Rows)
                    {
                        ArrayList listRows = new ArrayList();
                        foreach (Object obj in rows.ItemArray)
                        {
                            listRows.Add(obj.ToString());
                        }
                        list.Add(listRows);
                    }



                    return Ok(list);
                }
            }

            return NotFound("Connection not established");
        }
        [HttpGet("GetIdentityRowCount")]
        public async Task<ActionResult<List<String>>> getIdentityCount()
        {
            string json = JsonConvert.SerializeObject(DllHandler.LoadDll(32912));
            int rowcount = 0;

            if (json != null)
            {
                var data = JObject.Parse(json);
                var value = data["sql"];

                if (value != null)
                {
                    //DataSet dataSet = SqlService.getDataFromSql(value.ToString());
                    DataTable table = new DataTable();
                    table = Database.getDataFromSqlAsTable(value.ToString());

                    if (table.Rows.Count > 0)
                        rowcount = table.Rows.Count;
                }
            }
            return Ok(rowcount);
        }

        [HttpGet("AuthenticateClient")]

        public async Task<ActionResult> authenticateClient()
        {
            DllHandler handler = new DllHandler();
            handler.LoadDbSettings();
            string usern = "";
            if (string.IsNullOrEmpty(handler.user) || handler.user.Length < 2)
            {
                //handler.user = "RD-FISCHER/RD-FISCHER";
                handler.user = Environment.UserName;
                //TODO: 

                usern = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                int processID = Environment.ProcessId;//entspricht IV
            }
            //int i = DllHandler.ActivateConnection(sessionId,handler.user);
            int i = DllHandler.ActivateSession(usern, "213");
            string logMessage = "[WebServer] Unerwarteter Fehler: Interner Fehlercode: " + i + "SessionId: " + sessionId;
            if (i == 0)
            {
                logMessage = "[WebServer] Connection established, SessionId: " + sessionId;
                isActivated = 1;
            }
            else if (i == 2)
            {
                logMessage = "[WebServer] Connection not established. Server is not reachable";
            }
            else if (i == 3)
            {
                logMessage = "[WebServer] Authentication denied for user (Multi-Authentication)";
            }
            DllHandler.logFileEntry(logMessage);

            return Ok(i);
        }
        //NEW
        [HttpGet("OpenList")]
        public async Task<ActionResult> openList()
        {
            DllHandler handler = new DllHandler();
            string usern = "";
            int processID = 0;
            string returnValue = string.Empty;
            if (string.IsNullOrEmpty(handler.user) || handler.user.Length < 2)
            {
                //handler.user = "RD-FISCHER/RD-FISCHER";
                handler.user = Environment.UserName;
                //TODO: 

                usern = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                processID = Environment.ProcessId;//entspricht IV
            }
            string json = JsonConvert.SerializeObject(DllHandler.openList());
            var data = JObject.Parse(json);
            //int i = DllHandler.openList();
            //int i = DllHandler.ActivateConnection(sessionId,handler.user);
            string logMessage = "Connecting";
            string p = returnValue;
            int i = 0;
            //string logMessage = "[WebServer] Unerwarteter Fehler: Interner Fehlercode: " + i + "SessionId: " + sessionId;
            if (i == 0)
            {
                logMessage = "[WebServer] Connection established, SessionId: " + sessionId;
            }
            else if (i == 2)
            {
                logMessage = "[WebServer] Connection not established. Server is not reachable";
            }
            else if (i == 3)
            {
                logMessage = "[WebServer] Authentication denied for user (Multi-Authentication)";
            }
            DllHandler.logFileEntry(logMessage);

            return Ok(i);
        }
        [HttpGet("GetListEntries")]
        public async Task<ActionResult> getListEntries()
        {
            DllHandler handler = new DllHandler();
            string usern = "";
            int processID = 0;
            string returnValue = string.Empty;
            if (string.IsNullOrEmpty(handler.user) || handler.user.Length < 2)
            {
                //handler.user = "RD-FISCHER/RD-FISCHER";
                handler.user = Environment.UserName;
                //TODO: 

                usern = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                processID = Environment.ProcessId;//entspricht IV
            }
            string json = JsonConvert.SerializeObject(DllHandler.getListEntries());
            var data = JObject.Parse(json);
            //int i = DllHandler.openList();
            //int i = DllHandler.ActivateConnection(sessionId,handler.user);
            string logMessage = "Connecting";
            string p = returnValue;
            int i = 0;
            //string logMessage = "[WebServer] Unerwarteter Fehler: Interner Fehlercode: " + i + "SessionId: " + sessionId;
            if (i == 0)
            {
                logMessage = "[WebServer] Connection established, SessionId: " + sessionId;
            }
            else if (i == 2)
            {
                logMessage = "[WebServer] Connection not established. Server is not reachable";
            }
            else if (i == 3)
            {
                logMessage = "[WebServer] Authentication denied for user (Multi-Authentication)";
            }


            return Ok(json);
        }

        [HttpGet("LogFileEntry")]

        public async Task<ActionResult> logFileEntry()
        {
            // int i = DllHandler.ActivateConnection(61);

            DllHandler.logFileEntry("Webserver: I made a mistake");
            //Response.Cookies


            return Ok();
        }
        [HttpGet("User")]

        public async Task<ActionResult> getUserPw()
        {
            byte[] data = null;
            byte[] key = null;
            byte[] iv = null;

            key = Cryptor.generateKey();
            //ich erhalte basestring
            DllHandler handler = new DllHandler();
            handler.LoadDbSettings();
            iv = Cryptor.generateIV(handler.user);
            //cast from basestring to bytearray
            try
            {

                data = System.Convert.FromBase64String(handler.pw);
            }
            catch (Exception e)
            {

                throw (e.InnerException);
            }


            string plain = DllHandler.DecryptStringFromBytes_Aes(data, key, iv);




            return Ok(plain);
        }
        [HttpGet("DBSettings")]
        public async Task<ActionResult> getSettings()
        {
            int buffer = 128;
            int buffer1 = 128;
            int buffer2 = 128;
            int buffer3 = 128;
            byte[] serverInstance = new byte[buffer];
            byte[] dbName = new byte[buffer1];
            byte[] dbUser = new byte[buffer2];
            byte[] dbPassw = new byte[buffer3];
            DllHandler handler = new DllHandler();
            handler.LoadDbSettings();
            //string sub = infos.Substring
            return Ok();
        }
        [HttpPut("putNewUserPw")]
        public async Task<ActionResult> setPassword()
        {
            DataTable table = new DataTable();
            string crypt = "";
            ArrayList list = new ArrayList();
            table = Database.getDataFromSqlAsTable("Select Password from AccessControl where UserId = 'UB0001'");



            foreach (DataRow rows in table.Rows)
            {
                ArrayList listRows = new ArrayList();
                foreach (Object obj in rows.ItemArray)
                {
                    listRows.Add(obj.ToString());

                }
                list.Add(listRows);
            }
            crypt = list[0].ToString();
            return Ok(list);
        }
        //Möglichkeit Filter setzen und Data Abruf über getList() Func
        [HttpGet("CheckInitPw")]
        public async Task<ActionResult> checkInitPw(string userPw)
        {
            string initPw = string.Empty;
            userPw = string.Empty;

            if (string.Equals(userPw, initPw))
            {
                //User-Meldung -> Bitte geben Sie nun ihr passwort ein
                return Ok(0);
                //ANGULAR: 

            }
            else
                return Ok(1);
        }
        [HttpGet("ChangeInitPw")]
        public async Task<ActionResult> changeInitPw(string userPw)
        {
            string initPw = string.Empty;
            userPw = string.Empty;
            string userName = "";
            DataSet dataSet = Database.getInitialPw();

            if (!string.Equals(userPw, initPw))
            {


                Database.updateUserPw(userName, userPw);

                //User-Meldung -> Bitte geben Sie nun ihr passwort ein
                return Ok(0);
                //ANGULAR: 

            }
            else
                return Ok(1);
        }

    }
}
