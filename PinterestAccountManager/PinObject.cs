using Microsoft.VisualBasic.FileIO;
using PinSharp;
using PinterestAccountManager.AppHelper;
using PinterestAccountManager.Pinterst;
using PinterestAccountManager.Pinterst.API;
using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Classes.Models;
using PinterestAccountManager.Pinterst.Classes.Models.Pins;
using PinterestAccountManager.Pinterst.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace PinterestAccountManager
{
    public class PinObject
    {
        private string ProxyString { get; set; }
        private string NewURL { get; set; }
        private string PID { get; set; }
        private string PostingAccountUsername { get; set; }
        private string ScrapingAccount { get; set; }
        private string PostingBoard{ get; set; }
        private string PostingAccount { get; set; }
        private string CSVFILE { get; set; }
        private int Delay1 { get; set; }
        private int Delay2 { get; set; }
        private FILEWriter fILEWriter;
        private DataRow TaskMangerRow;
        private DataRow AccountMangerRow;
        private static IPinApi PinApi_scarp;
        private static IPinApi PinApi_posing;
        private WebProxy PROXY_Scrapping;
        private WebProxy PROXY_Posting;
 
       private string GenerateRandom()
        {
            Random r = new Random();
            Random r2 = new Random();
            string s1, s2;
            s1 = r.Next(1114111, 999999999).ToString();
            s2 = r2.Next(5555555, 154545481).ToString();
            return (s1+s2);

        }



        public PinObject(string scrappingaccount)
        {
            this.ScrapingAccount = scrappingaccount;

        }
        public PinObject(string pid,
            string scrapingaccount ,
            string passwrod ,
            string postingaccount,
            string postingboard , 
            int delay1 ,
            int delay2 , 
            string postingaccountusername)
            {
            this.PID = pid;

        
          
            this.ScrapingAccount = scrapingaccount;
            this.PostingAccount = postingaccount;
            this.PostingAccount = postingaccountusername;
            this.PostingBoard = postingboard.Split(new string[] { "[-]" }, StringSplitOptions.None)[1];
            this.Delay1 = delay1;
            this.Delay2 = delay2;
            this.PostingAccountUsername = postingaccountusername;
            TaskMangerRow = Login.DT_TaskManger.Select("PID='" + PID + "'").FirstOrDefault();
            AccountMangerRow = Login.DT_AccountManager.Select("UserName_Email='" + PID + "'").FirstOrDefault();
            fILEWriter = new FILEWriter();

        }
        private void InitalizeProxy(string AccountUserName,ref WebProxy p) {
            string proxyString;
            AccountMangerRow = Login.DT_AccountManager.Select("UserName_Email='" + AccountUserName + "'").FirstOrDefault();
            proxyString = AccountMangerRow["Proxy"].ToString();

            try
            {
                if (ProxyString != "")
                {

                    string[] str1 = ProxyString.Split(':');
                    string IPandPortNumber, ProxyUserName, ProxyPassword;

                    IPandPortNumber = str1[0] + ":"+ str1[1];

                    p = new WebProxy(IPandPortNumber, true);
                        if (str1.Length >2)
                    {
                        ProxyUserName = str1[2];
                        ProxyPassword = str1[3];
                        p.Credentials = new NetworkCredential(ProxyUserName, ProxyPassword);
                    }
                        else
                    {
                        WebRequest.DefaultWebProxy = new WebProxy();
                    }

                }


            }
            catch ( Exception e )
            {

                Login.MainInstance.LogReport(true, AccountUserName, "InitalizeProxy " + e.Message);
                WebRequest.DefaultWebProxy = new WebProxy();
            }



        }


        public async Task<List<BoardItem>> GetBoards()
        {

            LoadAccountHTTPSession();


       var boards = await PinApi_scarp.UserProcessor.GetUserBoards(ScrapingAccount);
            return  boards.Value;
        }

        private async void LoadAccountHTTPSession()
        {
           InitalizeProxy(ScrapingAccount, ref PROXY_Scrapping);
            AccountMangerRow = Login.DT_AccountManager.Select("UserName_Email='" + ScrapingAccount + "'").FirstOrDefault();
   
            string stateFile = ScrapingAccount.GetHashCode().ToString();
            var userSession = new UserSessionData
            { UserName = ScrapingAccount,
               Password = AccountMangerRow["Password"].ToString()
        };
            var httpClientHandler = new HttpClientHandler { Proxy = PROXY_Scrapping };


            var delay = RequestDelay.FromSeconds(2, 2);
            PinApi_scarp = PinApiBuilder.CreateBuilder().SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.All)) // use logger for requests and debug messages
                   .UseHttpClientHandler(httpClientHandler)
                    .SetRequestDelay(delay)
                    .Build();
            try
            {
                if (File.Exists(stateFile))
                {
                  
                    using (var fs = File.OpenRead(stateFile))
                    {
                        PinApi_scarp.LoadStateDataFromStream(fs);
                    }
                }
                else
                {
                    var logvar = await PinApi_scarp.LoginAsync();
                    var state = PinApi_scarp.GetStateDataAsStream();
                    using (var fileStream = File.Create(stateFile))
                    {
                        state.Seek(0, SeekOrigin.Begin);
                        state.CopyTo(fileStream);
                    }



                }
             
            }
            catch (Exception ex)
            {
                Login.MainInstance.LogReport(true, ScrapingAccount, "Http Sesstion "+  ex.Message);
                Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                {
                    TaskMangerRow["Errors"] = (int.Parse(TaskMangerRow["Errors"].ToString()) + 1);
                }));

            }
        }

        private async void LoadAccountHTTPSession_posting()
        {

            InitalizeProxy(PostingAccount, ref PROXY_Posting);
            AccountMangerRow = Login.DT_AccountManager.Select("UserName_Email='" + PostingAccount + "'").FirstOrDefault();

            string stateFile = PostingAccount.GetHashCode().ToString();
            var userSession = new UserSessionData
            {
                UserName = PostingAccount,
                Password = AccountMangerRow["Password"].ToString()
            };
            var httpClientHandler = new HttpClientHandler { Proxy = PROXY_Posting };


            var delay = RequestDelay.FromSeconds(2, 2);
            PinApi_posing = PinApiBuilder.CreateBuilder().SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.All)) // use logger for requests and debug messages
                    .UseHttpClientHandler(httpClientHandler)
                    .SetRequestDelay(delay)
                    .Build();
            try
            {
                if (File.Exists(stateFile))
                {
                  
                    using (var fs = File.OpenRead(stateFile))
                    {
                        PinApi_posing.LoadStateDataFromStream(fs);
                    }
                }
                else
                {
                    var logvar = await PinApi_posing.LoginAsync();
                    var state = PinApi_posing.GetStateDataAsStream();
                    using (var fileStream = File.Create(stateFile))
                    {
                        state.Seek(0, SeekOrigin.Begin);
                        state.CopyTo(fileStream);
                    }



                }
             
            }
            catch (Exception ex)
            {
                Login.MainInstance.LogReport(true, PostingAccount, "Http Sesstion "+  ex.Message);
                Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                {
                    TaskMangerRow["Errors"] = (int.Parse(TaskMangerRow["Errors"].ToString()) + 1);
                }));

            }
        }
        private bool Prevent()
        {

            if (TaskManger.Running_Process.Contains(PID))
            {
                return false;
            }
            else
                return true;
        }

        public void Resume()
        {

            if (TaskMangerRow["Process"].ToString().Contains(TaskManger.TaskMangerProcess.ScrapSearch))
            {

                string keyword = TaskMangerRow["Process"].ToString().Replace(TaskManger.TaskMangerProcess.ScrapSearch + "_", "");
                string SLimixt = TaskMangerRow["SLimit"].ToString();

                int sa = int.Parse(SLimixt);
                SearchByKeyword(keyword, sa);

            }
            else if (TaskMangerRow["Process"].ToString().Contains(TaskManger.TaskMangerProcess.ScrapBoard))
            {

                ScrapBoard(TaskMangerRow["Scraped_Board"].ToString(), int.Parse(TaskMangerRow["SLimit"].ToString()));

            }

            else if (TaskMangerRow["Process"].ToString().Contains(TaskManger.TaskMangerProcess.Posting))
            {
                PostingPins();



            }






        }

        bool CheckLimit(long Limit)
        {

            if (int.Parse(TaskMangerRow["Scrapped_Pins"].ToString()) >= Limit)
            {

                return true;

            }
            else

                return false;

        }



        private void PushRowsToCSV()
        {

            fILEWriter.scrappingCount = fILEWriter.SearchResultTable.Rows.Count;

            Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
            {
                TaskMangerRow["Scrapped_Pins"] = int.Parse(TaskMangerRow["Scrapped_Pins"].ToString()) + fILEWriter.scrappingCount;
            }));

            fILEWriter.PUSHToFile(ScrapingAccount, TaskMangerRow["CSVFile"].ToString());
            fILEWriter.SearchResultTable.Rows.Clear();

        }

        private void FinishTask()
        {


            Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
            {
                TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Completed;
            }));
            TaskManger.Running_Process = TaskManger.Running_Process.Replace(PID, "");
            appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;




        }

        public async void SearchByKeyword(string Query, long Limit)
        {

        try
            {

                LoadAccountHTTPSession();
                var boardtask = await PinApi_scarp.UserProcessor.SearchPins(Query);
                List<PinsSearchResultItem> results = boardtask.Value.PinsSearchResultItem;
                string cursor = boardtask.Value.Cursor;
                

                if (TaskMangerRow["LastCursor"].ToString() != "")
                {
                    cursor = TaskMangerRow["LastCursor"].ToString();
                }

                string orcedurl=TaskMangerRow["ForcedURL"].ToString();
                string orglink="";


                if (TaskMangerRow["CSVFile"].ToString() == "")
                {
                    this.CSVFILE = fILEWriter.WriteCSVFile(ScrapingAccount, "Search_" + Query + "_");
                    Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["CSVFile"] = this.CSVFILE; }));
                }




                if (CheckLimit( Limit) == true)
                {PostingPins();return;}


                foreach (PinsSearchResultItem pin in results)
                {

                    if (pin.type.ToLower() == "pin") { 
                    if (orcedurl == "")
                        orglink = pin.link;
                    else
                        orglink = orcedurl + "?" +GenerateRandom();


                        fILEWriter.AddTODataTable(new string[]{
                         pin.id,
                        "https://www.pinterest.com/pin/"+ pin.id,
                        orglink,
                        pin.images.orig.url,
                        pin.title,
                        pin.description,
                        pin.board.name,
                        pin.board.url

                        }
                        );

                    }
                }



                PushRowsToCSV();






                appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;





                if (CheckLimit( Limit) == true)
                { PostingPins(); return; }


                if (Prevent() == true)
                {
                    Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Paused; }));
                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                    Login.MainInstance.simpleButton5.BeginInvoke(new Action(() => { Login.MainInstance.simpleButton5.Enabled = true; }));
                    return;
                }


            ScrapLoop: if (!(cursor == ""))
                {

                    var boardtask2= await PinApi_scarp.UserProcessor.SearchPins(Query, cursor);
                    List<PinsSearchResultItem> pins = boardtask2.Value.PinsSearchResultItem;
                    cursor = boardtask2.Value.Cursor;



                    foreach (var pin in pins)
                    {
                        if (pin.type.ToLower() == "pin")
                        {

                            if (orcedurl == "")
                                orglink = pin.link;
                            else
                                orglink = orcedurl + "?" +GenerateRandom();


                            fILEWriter.AddTODataTable(new string[]{
                        pin.id,
                        "https://www.pinterest.com/pin/"+ pin.id,
                        orglink,
                        pin?.images?.orig?.url,
                        pin?.title,
                        pin?.description,
                        pin?.board?.name,
                        pin?.board?.url

                        }
                            );

                        }
                    }



                    PushRowsToCSV();

                    if (CheckLimit( Limit) == true)
                    { PostingPins(); return; }



                    if (Prevent() == true)
                    {
                        Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Paused; }));
                        appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                        Login.MainInstance.simpleButton5.BeginInvoke(new Action(() => { Login.MainInstance.simpleButton5.Enabled = true; }));
                        return;
                    }

                   
                    goto ScrapLoop;

                }









            }
            catch (Exception ex)
            {
                Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                {
                TaskMangerRow["CSVFile"] = this.CSVFILE;
                TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Failed;
                TaskMangerRow["Errors"] = (int.Parse(TaskMangerRow["Errors"].ToString()) + 1);

                }));
                Login.MainInstance.LogReport(false, ScrapingAccount, "CSV File Saved Successfully ! ");

                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                Login.MainInstance.LogReport(true, ScrapingAccount,"ScarpW " + line +  " "+  ex.Message);
                appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;

            }


        }
        public async void ScrapBoard(string boardurl, long Limit)
        {


            var owner = await PinApi_scarp.UserProcessor.GetBoardInfo(boardurl);
            try
            {
                LoadAccountHTTPSession();

                var boardtask = await PinApi_scarp.UserProcessor.GetBoardPins(owner.Value.id.ToString());
                List<PinsSearchResultItem> results = boardtask.Value.PinsSearchResultItem;
                string cursor = boardtask.Value.Cursor;


                if (CheckLimit(Limit) == true)
                { PostingPins(); return; }


                if (TaskMangerRow["CSVFile"].ToString() == "")
                {
                    this.CSVFILE = fILEWriter.WriteCSVFile(ScrapingAccount, boardurl.Replace("/", "-") + "_");
                    Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["CSVFile"] = this.CSVFILE; }));
                }



                if (TaskMangerRow["LastCursor"].ToString() != "")
                {
                    cursor = TaskMangerRow["LastCursor"].ToString();
                }

                string orcedurl = TaskMangerRow["ForcedURL"].ToString();
                string orglink = "";


                foreach (PinsSearchResultItem pin in results)
                {

                    if (pin.type.ToLower() == "pin")
                    {
                        if (orcedurl == "")
                            orglink = pin.link;
                        else
                            orglink = orcedurl + "?" + GenerateRandom(); ;


                        fILEWriter.AddTODataTable(new string[]{
                         pin.id,
                        "https://www.pinterest.com/pin/"+ pin.id,
                        orglink,
                        pin.images.orig.url,
                        pin.title,
                        pin.description,
                        pin.board.name,
                        pin.board.url

                        }
                        );

                    }
                }



                PushRowsToCSV();
                appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;

                if (CheckLimit(Limit) == true)
                { PostingPins(); return; }















                if (Prevent() == true)
                {
                    Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Paused; }));
                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                    Login.MainInstance.simpleButton5.BeginInvoke(new Action(() => { Login.MainInstance.simpleButton5.Enabled = true; }));
                    return;
                }


            ScrapLoop: if (!(cursor == ""))
                {

                    var boardtask2 = await PinApi_scarp.UserProcessor.GetBoardPins(owner.Value.id.ToString(), cursor);
                    List<PinsSearchResultItem> pins = boardtask2.Value.PinsSearchResultItem;
                    cursor = boardtask2.Value.Cursor;





                    foreach (var pin in pins)
                    {
                        if (pin.type.ToLower() == "pin")
                        {

                            if (orcedurl == "")
                                orglink = pin.link;
                            else
                                orglink = orcedurl + "?" + GenerateRandom();


                            fILEWriter.AddTODataTable(new string[]{
                        pin.id,
                        "https://www.pinterest.com/pin/"+ pin.id,
                        orglink,
                        pin.images.orig.url,
                        pin.title,
                        pin.description,
                        pin.board.name,
                        pin.board.url

                        });

                        }
                    }



                    PushRowsToCSV();

                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;


                    if (CheckLimit(Limit) == true)
                    { PostingPins(); return; }




                    if (Prevent() == true)
                    {
                        Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Paused; }));
                        Login.MainInstance.simpleButton5.BeginInvoke(new Action(() => { Login.MainInstance.simpleButton5.Enabled = true; }));
                        return;
                    }
                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                    goto ScrapLoop;

                }

            }
            catch (Exception ex)
            {
               
                Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                {
                    TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Failed;
                    TaskMangerRow["Errors"] = (int.Parse(TaskMangerRow["Errors"].ToString()) + 1);
                }));
                appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                Login.MainInstance.LogReport(false, ScrapingAccount, "CSV File Saved Successfully ! ");
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                Login.MainInstance.LogReport(true, ScrapingAccount,"ScrapB " + line +  " " + ex.Message);

            }
        }

        public async void PostingPins()
        {
            LoadAccountHTTPSession_posting();
            Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
            {

                TaskMangerRow["Process"] = TaskManger.TaskMangerProcess.Posting;
            }));


            try
            {


         
          //  var client = new PinSharpClient(PostingAccount);

  
               

                int Posted_Count = 0;

                int StartPoint = int.Parse(TaskMangerRow["Posted_Pin"].ToString());

                int EndPoint = int.Parse(TaskMangerRow["Scrapped_Pins"].ToString());

                    System.IO.StreamReader file =new System.IO.StreamReader(TaskMangerRow["CSVFile"].ToString(),Encoding.UTF8);

                    for (int i = StartPoint; i <= (EndPoint + 1) && !file.EndOfStream; ++i)
                    {
                    string line = file.ReadLine().Replace("\"", "") ;

                        if (i ==0 ) 
                          continue;


                        string[] fields = line.Split(',');
                        if (!(fields[0] == "ID"))
                        {

                  
                        try
                        {

                            var boardtask = await PinApi_posing.PinProcessor.PostPinAsync(fields[3], PostingBoard, fields[5], fields[2], fields[4]);






                                //(string board, string imageUrl, string note, string link = null); 

                            //var PostResults = await client.Pins.CreatePinAsync(PostingBoard, fields[3], fields[5], fields[2]);
                            if (boardtask.Value ==true)
                                    {

                                        Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                                    {
                                        TaskMangerRow["Posted_Pin"] =  (int.Parse(TaskMangerRow["Posted_Pin"].ToString()) +1 );
                                    }));
                                    }

                                    if (Prevent() == true)
                                {
                                    Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Paused; }));
                                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                                    Login.MainInstance.simpleButton5.BeginInvoke(new Action(() => { Login.MainInstance.simpleButton5.Enabled = true; }));
                                    return;
                                }



                            }
                            catch (Exception ex)
                            {
                            var st = new StackTrace(ex, true);
                            // Get the top stack frame
                            var frame = st.GetFrame(0);
                            // Get the line number from the stack frame
                            var linee = frame.GetFileLineNumber();

                            Login.MainInstance.LogReport(true, PostingAccountUsername, "Posting Error " + linee + " "  + ex.Message);
                                    Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                                    {
                                        TaskMangerRow["Errors"] = (int.Parse(TaskMangerRow["Errors"].ToString()) + 1);
                                    }));


                                  
                            }

                            var randx = new Random();
                            int delay = randx.Next(Delay1, Delay2) * 1000;
                            await Task.Delay(delay);

                        }
                    }

                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                





            
            FinishTask();
            return;




            }
            catch (Exception e)
            {
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() => { TaskMangerRow["Task_Status"] = TaskManger.TaskManagerStatus.Failed; }));
                                    appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                Login.MainInstance.LogReport(true, PostingAccountUsername, "General Error "+ line + " " + e.Message);
                Login.MainInstance.GridControl_Tasks.BeginInvoke(new Action(() =>
                {
                    TaskMangerRow["Errors"] = (int.Parse(TaskMangerRow["Errors"].ToString()) + 1);
                }));


                return;
            }

















        }










    }
}
