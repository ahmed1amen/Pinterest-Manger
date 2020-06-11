    using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PinterestAccountManager.AppHelper;
using PinSharp;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using static PinterestAccountManager.AppHelper.TaskManger;
using PinterestAccountManager.Pinterst.Classes;
using System.Net.Http;
using PinterestAccountManager.Pinterst;
using PinterestAccountManager.Pinterst.Logger;
using PinterestAccountManager.Pinterst.API;

namespace PinterestAccountManager
{
    public partial class Login : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public List<BackgroundWorker> TaskList = new List<BackgroundWorker> { };
        public static DataTable DT_AccountManager = new DataTable();
        public static DataTable DT_TaskManger = new DataTable();
        public string Scad = "{0}-[({1})-({2})]";       //Days,{00:00:00 AM - 01:30:00 PM }
        void setscad()
        {
            string days = "";
            if (chk_SAT.Checked == true)
                days += chk_SAT.Text + " ";
            if (chk_SUN.Checked == true)
                days += chk_SUN.Text + " ";
            if (chk_MON.Checked == true)
                days += chk_MON.Text + " ";
            if (chk_TUE.Checked == true)
                days += chk_TUE.Text + " ";
            if (chk_WED.Checked == true)
                days += chk_WED.Text + " ";
            if (chk_THU.Checked == true)
                days += chk_THU.Text + " ";
            if (chk_FRI.Checked == true)
                days += chk_FRI.Text + " ";

            Scad = string.Format(Scad, days, timeSpanEdit1.TimeSpan.ToString(), timeSpanEdit2.TimeSpan.ToString());

            string[] nax = DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames;
            string DayToday = nax[(int)DateTime.Now.DayOfWeek];

            if (Scad.Contains(DayToday))
            { }
            Regex rx = new Regex("\\(.*?\\)");
            string t1 = rx.Matches(Scad)[0].Value.Replace("(", "").Replace(")", "");
            string t2 = rx.Matches(Scad)[1].Value.Replace("(", "").Replace(")", "");



            TimeSpan Time1 = TimeSpan.Parse(t1);
            TimeSpan Time2 = TimeSpan.Parse(t2);


            TimeSpan start = timeSpanEdit1.TimeSpan;
            TimeSpan end = timeSpanEdit2.TimeSpan;


            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
                //      MessageBox.Show("X");
            }



        }
        private static Login _instance = null;
        private static IPinApi PinApi;
        private WebProxy loginproxy;

        public static Login MainInstance
        {
            
            get
            {
          
                return _instance;
            }
        }

        private void InitalizeProxy(string ProxyString,string username)
        {


            
            try
            {
                if (ProxyString != "")
                {

                    string[] str1 = ProxyString.Split(':');
                    string IPandPortNumber, ProxyUserName, ProxyPassword;

                    IPandPortNumber = str1[0] + ":" + str1[1];

                    loginproxy = new WebProxy(IPandPortNumber, true);
                    if (str1.Length > 2)
                    {
                        ProxyUserName = str1[2];
                        ProxyPassword = str1[3];
                        loginproxy.Credentials = new NetworkCredential(ProxyUserName, ProxyPassword);
                    }
                    else
                    {
                        WebRequest.DefaultWebProxy = new WebProxy();
                    }

                }


            }
            catch (Exception e)
            {

                Login.MainInstance.LogReport(true, username, "InitalizeProxy " + e.Message);
                WebRequest.DefaultWebProxy = new WebProxy();
            }



        }
        public Login()

        {



            InitializeComponent();
            {
                if (_instance == null)
                    _instance = this;
            }

            //DT Account Mananger
            DT_AccountManager.Columns.Add("UserName_Email");
            DT_AccountManager.Columns.Add("Password");
            DT_AccountManager.Columns.Add("Status");
            DT_AccountManager.Columns.Add("AccessToken");
            DT_AccountManager.Columns.Add("Followingg");
            DT_AccountManager.Columns.Add("Followerss");
            DT_AccountManager.Columns.Add("Pins");
            DT_AccountManager.Columns.Add("FullName");
            DT_AccountManager.Columns.Add("Proxy");

            //DT Task Manger
            DT_TaskManger.Columns.Add("Scraping_Account");
            DT_TaskManger.Columns.Add("Posting_Account");
            DT_TaskManger.Columns.Add("Process");
            DT_TaskManger.Columns.Add("Scraped_Board");
            DT_TaskManger.Columns.Add("Posting_Board");
            DT_TaskManger.Columns.Add("Schedule");
            DT_TaskManger.Columns.Add("Scrapped_Pins");
            DT_TaskManger.Columns.Add("Posted_Pin");
            DT_TaskManger.Columns.Add("Errors");
            DT_TaskManger.Columns.Add("Task_Status");
            DT_TaskManger.Columns.Add("LastRun");
            
            DT_TaskManger.Columns.Add("LastCursor");
            DT_TaskManger.Columns.Add("PID");
            DT_TaskManger.Columns.Add("ForcedURL");
            DT_TaskManger.Columns.Add("CSVFile");
            DT_TaskManger.Columns.Add("acctoken");
            DT_TaskManger.Columns.Add("accPassword");
            DT_TaskManger.Columns.Add("t1delay");
            DT_TaskManger.Columns.Add("t2delay");
            DT_TaskManger.Columns.Add("SLimit");
            DT_TaskManger.Columns.Add("PostingAccountUsername");
            //

            //
            if (System.IO.File.Exists("UserDataTable.dll") == true)
            {
                DT_AccountManager = appHelper.SetUserDataTable_Control;
            }

            if (System.IO.File.Exists("TaskMangerDataTable.dll") == true)
            {
                DT_TaskManger = appHelper.SetTaskMangerDataTable_Control;
            }

            GridControl_Accounts.DataSource = DT_AccountManager;
            GridControl_Tasks.DataSource = DT_TaskManger;


            for (int i = 0; i < GridView_Tasks.DataRowCount; i++)
            {
                GridView_Tasks.SetRowCellValue(i, "Errors", "0");

                if (GridView_Tasks.GetRowCellValue(i, "Task_Status").ToString() .Contains (TaskManagerStatus.Running) ) ;
                GridView_Tasks.SetRowCellValue(i, "Task_Status", TaskManagerStatus.Paused);

            }



        }
        public void LogReport(bool IsError, string usernameF, string messageOf)
        {


            if (IsError == true)
                List_Log.Items.Add(String.Format("[{0}] >> [{1}] error : {2} ", DateTime.Now, usernameF, messageOf));
            else
                List_Log.Items.Add(String.Format("[{0}] >> [{1}] : {2} ", DateTime.Now, usernameF, messageOf));

        }


        private async void Btn_Add_ClickAsync(object sender, EventArgs e)
        {
                
            ProgWait.Visible = true;
            try
            {



                //   var client = new PinSharpClient(txt_accesstoken.Text);


                //  var user = await client.Me.GetUserAsync();

                string stateFile = txt_Username.Text.GetHashCode().ToString();
                var userSession = new UserSessionData
                {
                    UserName = txt_Username.Text,
                    Password = txt_password.Text
                };

                InitalizeProxy(txt_proxy.Text, txt_Username.Text);
                var httpClientHandler = new HttpClientHandler { Proxy = loginproxy };


                var delay = RequestDelay.FromSeconds(2, 2);
                PinApi = PinApiBuilder.CreateBuilder().SetUser(userSession)
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
                            PinApi.LoadStateDataFromStream(fs);
                        }
                    }
                    else
                    {
                        var logvar = await PinApi.LoginAsync();
                        var state = PinApi.GetStateDataAsStream();
                        using (var fileStream = File.Create(stateFile))
                        {
                            state.Seek(0, SeekOrigin.Begin);
                            state.CopyTo(fileStream);
                        }



                    }

                }
                catch (Exception ex)
                {
               LogReport(true, txt_Username.Text, "Http Sesstion " + ex.Message);
        
                  

                }
                var user = await PinApi.UserProcessor.GetCurrentUserAsync();

                DataRow dr = DT_AccountManager.NewRow();

                DT_AccountManager.Rows.Add(user.Value.username, txt_password.Text, "ok", txt_accesstoken.Text, "Following", "Followers", "Pins", user.Value.first_name+ " " + user.Value.last_name);

                appHelper.SetUserDataTable_Control = DT_AccountManager;



            }
            catch (Exception ex)
            {

                LogReport(true, txt_Username.Text, ex.Message);


            }




            ProgWait.Visible = false;

        }

        private void LookUpEdit1_Enter(object sender, EventArgs e)
        {




        }

        private void Login_Load(object sender, EventArgs e)
        {


            backgroundWorker1.RunWorkerAsync();
            Lookup_ScrapingAccount.Properties.DataSource = GridControl_Accounts.DataSource;
            Lookup_ScrapingAccount.Properties.DisplayMember = "UserName_Email";
            Lookup_ScrapingAccount.Properties.ValueMember = "Password";
            Lookup_ScrapingAccount.Properties.ValueMember = "AccessToken";



            Lookup_PostingAccount.Properties.DataSource = GridControl_Accounts.DataSource;
            Lookup_PostingAccount.Properties.DisplayMember = "UserName_Email";
            Lookup_PostingAccount.Properties.ValueMember = "Password";
            Lookup_PostingAccount.Properties.ValueMember = "AccessToken";


            lookUpEdit2.Properties.DataSource = GridControl_Accounts.DataSource;
            lookUpEdit2.Properties.DisplayMember = "UserName_Email";
            lookUpEdit2.Properties.ValueMember = "Password";
            lookUpEdit2.Properties.ValueMember = "AccessToken";
            WindowsFormsSettings.AnimationMode = AnimationMode.EnableAll;

        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            dynamic ScrapingAccount = Lookup_ScrapingAccount.GetSelectedDataRow();
            dynamic PostingAccount = Lookup_PostingAccount.GetSelectedDataRow();


            if (Lookup_ScrapingAccount.GetSelectedDataRow() == null || Lookup_PostingAccount.GetSelectedDataRow() == null)
            {
                MessageBox.Show("Please Select The scraping Account And The Posting Accoint! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (Combo_CurentBoard.SelectedItem == null)
            {
                MessageBox.Show("Please Select an Board ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ScrapingAccount.Row.ItemArray[1].ToString() == "")
            {
                MessageBox.Show("You Must Enter The Password of Scraping Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BackgroundWorker t = new BackgroundWorker();
            t.WorkerSupportsCancellation = true;
            string pid = t.GetHashCode().ToString();

            t.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs a)
            {
PinObject pinObject = new PinObject(pid, ScrapingAccount.Row.ItemArray[0].ToString(), ScrapingAccount.Row.ItemArray[1].ToString(), PostingAccount.Row.ItemArray[3].ToString(), Combo_CurentBoard.SelectedItem.ToString(), ((int)num1.Value), ((int)num2.Value) , PostingAccount.Row.ItemArray[0].ToString());         
pinObject.SearchByKeyword(txt_keyword.Text, ((int)Search_Limit.Value));

            });

            TaskList.Add(t);
            //t.RunWorkerAsync();

            TaskManger.Running_Process += t.GetHashCode().ToString();

            setscad();

            DT_TaskManger.Rows.Add(
                ScrapingAccount.Row.ItemArray[0].ToString(),
                PostingAccount.Row.ItemArray[0].ToString(),
                TaskManger.TaskMangerProcess.ScrapSearch + "_" + txt_keyword.Text,
                txt_BoardURL.Text,
                Combo_CurentBoard.SelectedItem.ToString(),
                Scad,
                "0",
                "0",
                "0",
                TaskManger.TaskManagerStatus.Ready,
                DateTime.Now.ToString(),
                "",
                t.GetHashCode().ToString(),
                txt_forcechangeurl.Text,
                "",
                PostingAccount.Row.ItemArray[3].ToString(),
                ScrapingAccount.Row.ItemArray[1].ToString(),
                num1.Value,
                num2.Value,
                Search_Limit.Value,
                PostingAccount.Row.ItemArray[0].ToString());



             Lookup_ScrapingAccount.Enabled = true;







        }

        private void Label10_Click(object sender, EventArgs e)
        {

        }

        private void ComboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TabNavigationPage3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Btn_addusernameToBlock_Click(object sender, EventArgs e)
        {
            listBoxControl1.Items.Add(txt_blockusername.Text);
        }

        private void Btn_addImageUrlToblocklist_Click(object sender, EventArgs e)
        {
            listBoxControl1.Items.Add(txt_blockurlimage.Text);
        }

        private async void SimpleButton7_Click(object sender, EventArgs e)
        {

            try
            {
                if (lookUpEdit2.GetSelectedDataRow() == null)
                {
                    MessageBox.Show("Please Select an account ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (comboBoxEdit3.SelectedText == null)
                {
                    MessageBox.Show("Please Select an Board ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                string boardID = Combo_CurentBoard.SelectedItem.ToString().Split(new string[] { "[-]" }, StringSplitOptions.None)[1];

       
                dynamic x = lookUpEdit2.GetSelectedDataRow();
                var client = new PinSharpClient(x.Row.ItemArray[3].ToString());



                if (radioGroup1.SelectedIndex == 1)
                {
                    if (opf_selectImage.ShowDialog() == DialogResult.OK)
                    {

                        ImageToBase64(opf_selectImage.FileName);
                    }

                    var user = await client.Pins.CreatePinFromBase64Async(comboBoxEdit3.SelectedText, ImageToBase64(opf_selectImage.FileName), Txt_Description.Text, Txt_Link.Text);
                    MessageBox.Show("Pin Posted", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {


                    var user = await client.Pins.CreatePinAsync(comboBoxEdit3.SelectedText, txt_ImageUrl.Text, Txt_Description.Text, Txt_Link.Text);
                    MessageBox.Show("Pin Posted", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {

                LogReport(true, txt_Username.Text, ex.Message);


            }


        }

        private async void LookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {


        }

        private async Task SimpleButton4_ClickAsync(object sender, EventArgs e)
        {
            var client = new PinSharpClient("");

            var pins = await client.Boards.GetPinsAsync("");

            foreach (var pin in pins)
            {



            }

        }


        public string ImageToBase64(string path)
        {

            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        private void ComboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void LookUpEdit2_QueryCloseUp(object sender, CancelEventArgs e)
        {


        }

        private async void SimpleButton8_Click(object sender, EventArgs e)
        {

            if (lookUpEdit2.GetSelectedDataRow() == null)
            {
                MessageBox.Show("Please Select an account ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dynamic x = lookUpEdit2.GetSelectedDataRow();
            try
            {
                Lookup_ScrapingAccount.Enabled = false;
                comboBoxEdit3.Enabled = false;
                var client = new PinSharpClient(x.Row.ItemArray[3].ToString());

                var boards = await client.Me.GetBoardsAsync();

                comboBoxEdit3.Properties.Items.Clear();
                foreach (var bo in boards)
                {
                    comboBoxEdit3.Properties.Items.Add(bo.Name + "[-]" + bo.Id);


                }


            }
            catch (Exception ex)
            {

                LogReport(true, x.Row.ItemArray[0].ToString(), ex.Message);
                Lookup_ScrapingAccount.Enabled = true;
                comboBoxEdit3.Enabled = true;
            }
            Lookup_ScrapingAccount.Enabled = true;
            comboBoxEdit3.Enabled = true;

        }

        private void ListBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TabNavigationPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GroupControl4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TextEdit2_EditValueChanged(object sender, EventArgs e)
        {
        }

        private async void SimpleButton9_Click(object sender, EventArgs e)
        {

            txt_BoardURL.Text = txt_BoardURL.Text.ToLower().Replace("https://www.pinterest.com/", "").Replace("http://www.pinterest.com/", "").Replace("pinterest.com/", "");
            if (txt_BoardURL.Text.EndsWith("/"))
                txt_BoardURL.Text = txt_BoardURL.Text.TrimEnd(txt_BoardURL.Text[txt_BoardURL.Text.Length - 1]);

            if (Lookup_ScrapingAccount.GetSelectedDataRow() == null || Lookup_PostingAccount.GetSelectedDataRow() == null)
            {
                MessageBox.Show("Please Select The scraping Account And The Posting Accoint! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Combo_CurentBoard.SelectedItem == null)
            {
                MessageBox.Show("Please Select an Board ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            dynamic ScrapingAccount = Lookup_ScrapingAccount.GetSelectedDataRow();
            dynamic PostingAccount = Lookup_PostingAccount.GetSelectedDataRow();
            if (ScrapingAccount.Row.ItemArray[1].ToString() == "")
            {
                MessageBox.Show("You Must Enter The Password of Scraping Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            BackgroundWorker t = new BackgroundWorker();
            t.WorkerSupportsCancellation = true;
            string pid = t.GetHashCode().ToString();

            t.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs a)
            {
                PinObject pinObject = new PinObject(pid, ScrapingAccount.Row.ItemArray[0].ToString(), ScrapingAccount.Row.ItemArray[1].ToString(), PostingAccount.Row.ItemArray[3].ToString(), Combo_CurentBoard.SelectedItem.ToString(), ((int)num1.Value), ((int)num2.Value), PostingAccount.Row.ItemArray[0].ToString());
                pinObject.ScrapBoard(txt_BoardURL.Text, ((int)ScrapBoardLimit.Value));
            });

            TaskList.Add(t);
            //t.RunWorkerAsync();

            TaskManger.Running_Process += t.GetHashCode().ToString();

            FILEWriter fILEWriter = new FILEWriter();
            setscad();

            DT_TaskManger.Rows.Add(
                ScrapingAccount.Row.ItemArray[0].ToString(),
                PostingAccount.Row.ItemArray[0].ToString(),
                TaskManger.TaskMangerProcess.ScrapBoard + "_" + txt_keyword.Text,
                txt_BoardURL.Text,
              Combo_CurentBoard.SelectedItem.ToString(),
                Scad, 
                "0",  
                "0",
                "0", 
                TaskManger.TaskManagerStatus.Ready, 
                DateTime.Now.ToString(), 
                "", 
                t.GetHashCode().ToString(), 
                txt_forcechangeurl.Text, 
                "",
                PostingAccount.Row.ItemArray[3].ToString(),
                ScrapingAccount.Row.ItemArray[1].ToString(), 
                num1.Value ,
                num2.Value ,
                ScrapBoardLimit.Value,
                PostingAccount.Row.ItemArray[0].ToString());

            Lookup_ScrapingAccount.Enabled = true;

        }
        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            int[] selectedRows = GridView_Tasks.GetSelectedRows();
            foreach (int rowHandle in selectedRows)
            {
                if (rowHandle >= 0)
                {
                    String ID = GridView_Tasks.GetRowCellValue(rowHandle, "PID").ToString();
                    String status = GridView_Tasks.GetRowCellValue(rowHandle, "Task_Status").ToString();

                    if (status == TaskManger.TaskManagerStatus.Running)

                    {
                        XtraMessageBox.Show("Cannot Remove Running Task !", "TaskManger", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


                    foreach (BackgroundWorker t in TaskList)
                    {

                        if (t.GetHashCode().ToString() == ID)
                        {
                            TaskManger.Running_Process = TaskManger.Running_Process.Replace(t.GetHashCode().ToString(), "");
                            GridView_Tasks.SetRowCellValue(rowHandle, "Task_Status", TaskManger.TaskManagerStatus.Paused);

                            DataRow dxr = DT_TaskManger.Select("PID='" + ID + "'").FirstOrDefault();
                            DT_TaskManger.Rows.Remove(dxr);
                            appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
                        }

                    }



                }




            }













        }

        private void GroupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {
            setscad();

        }

        private void Tab_Login_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GridControl_Tasks_Click(object sender, EventArgs e)
        {

        }

        private void SimpleButton4_Click(object sender, EventArgs e)
        {
            int[] selectedRows = GridView_Tasks.GetSelectedRows();
            foreach (int rowHandle in selectedRows)
            {

                if (rowHandle >= 0)
                {
                    String ID = GridView_Tasks.GetRowCellValue(rowHandle, "PID").ToString();
                    String status = GridView_Tasks.GetRowCellValue(rowHandle, "Task_Status").ToString();

                    if (status == TaskManger.TaskManagerStatus.Running)
                    { XtraMessageBox.Show("Already Running ! ", "TaskManger", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    if (status == TaskManger.TaskManagerStatus.Failed)
                    { /*XtraMessageBox.Show("an Error shown in this process . please check error log!", "TaskManger", MessageBoxButtons.OK, MessageBoxIcon.Error); return; */}


                    if (status == TaskManger.TaskManagerStatus.Paused)
                    { XtraMessageBox.Show("You Cannot Start A Paused Task , Only Resume", "TaskManger", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


                    if (status == TaskManger.TaskManagerStatus.Ready)
                    { 

                    foreach (BackgroundWorker t in TaskList)
                    {

                        if (t.GetHashCode().ToString() == ID)
                        {

                            TaskManger.Running_Process += t.GetHashCode().ToString() + " ";
                            t.RunWorkerAsync();


                            GridView_Tasks.SetRowCellValue(rowHandle, "Task_Status", TaskManagerStatus.Running);

                        }

                    }

                    }

                }




            }
            appHelper.SetTaskMangerDataTable_Control = DT_TaskManger;

        }

        private async void SimpleButton5_Click(object sender, EventArgs e)
        {






    int[] selectedRows = GridView_Tasks.GetSelectedRows();
    foreach (int rowHandle in selectedRows)
    {


        if (rowHandle >= 0)
        {
            String ID = GridView_Tasks.GetRowCellValue(rowHandle, "PID").ToString();
            String status = GridView_Tasks.GetRowCellValue(rowHandle, "Task_Status").ToString();
            String Scraping_Account = GridView_Tasks.GetRowCellValue(rowHandle, "Scraping_Account").ToString();
            String Scraped_Board = GridView_Tasks.GetRowCellValue(rowHandle, "Scraped_Board").ToString();
            String Posting_Board = GridView_Tasks.GetRowCellValue(rowHandle, "Posting_Board").ToString();
            String acctoken = GridView_Tasks.GetRowCellValue(rowHandle, "acctoken").ToString();
            String accPassword = GridView_Tasks.GetRowCellValue(rowHandle, "accPassword").ToString();
            String t1delay = GridView_Tasks.GetRowCellValue(rowHandle, "t1delay").ToString();
            String t2delay = GridView_Tasks.GetRowCellValue(rowHandle, "t2delay").ToString();
            String PostingaccountUsername= GridView_Tasks.GetRowCellValue(rowHandle, "PostingAccountUsername").ToString();









                    if (status.Contains( TaskManger.TaskManagerStatus.Paused) || status.Contains(TaskManger.TaskManagerStatus.Failed))
            {

                BackgroundWorker t = new BackgroundWorker();
                t.WorkerSupportsCancellation = true;
                string pid = t.GetHashCode().ToString();

                t.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs a)
                {
                    PinObject pinObject = new PinObject(pid,
                            Scraping_Account,
                            accPassword,
                            acctoken,
                            Posting_Board,
                            int.Parse(t1delay), 
                            int.Parse(t2delay),
                            PostingaccountUsername

                        );




                    pinObject.Resume();

                });

                        DataRow dxr = DT_TaskManger.Select("PID='" + ID + "'").FirstOrDefault();
                        dxr["PID"] = pid;
                        GridView_Tasks.SetRowCellValue(rowHandle, "PID", pid);
                        ID = pid;

                      

                        appHelper.SetTaskMangerDataTable_Control = DT_TaskManger;
                        GridControl_Tasks.Focus();
                        GridControl_Tasks.Refresh();
                     

                        TaskList.Add(t);
                //t.RunWorkerAsync();

                TaskManger.Running_Process += t.GetHashCode().ToString();




                foreach (BackgroundWorker ts in TaskList)
                {

                    if (ts.GetHashCode().ToString() == ID)
                    {

                        TaskManger.Running_Process += ts.GetHashCode().ToString() + " ";
                        ts.RunWorkerAsync();


                        GridView_Tasks.SetRowCellValue(rowHandle, "Task_Status", TaskManagerStatus.Running);

                    }

                }


                        return;




            }


                foreach (BackgroundWorker t in TaskList)
            {

                if (t.GetHashCode().ToString() == ID)
                {
                    TaskManger.Running_Process = TaskManger.Running_Process.Replace(t.GetHashCode().ToString(), "");
                            simpleButton5.Enabled = false;
                }

            }



        }




    }

    appHelper.SetTaskMangerDataTable_Control = DT_TaskManger;



        }

        private void Chk_ForceChangeUrl_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ForceChangeUrl.Checked == false)
            {
                txt_forcechangeurl.Text = "";
                txt_forcechangeurl.Enabled = false;
            }
            else
            {

                txt_forcechangeurl.Enabled = true;


            }
        }

        private void SimpleButton6_Click(object sender, EventArgs e)
        {


            appHelper.SetUserDataTable_Control = DT_AccountManager;




        }

        private void Combo_CurentBoard_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void Btn_GetBoardsAfterScrap_Click(object sender, EventArgs e)
        {


            if (Lookup_PostingAccount.GetSelectedDataRow() == null)
            {
                MessageBox.Show("Please Select an account ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dynamic x = Lookup_PostingAccount.GetSelectedDataRow();
            try
                {

                Lookup_ScrapingAccount.Enabled = false;
                Lookup_PostingAccount.Enabled = false;
                Combo_CurentBoard.Enabled = false;
                General_GroupBoxScrapSearch.Enabled = false;
                General_GroupBoxScrapBoard.Enabled = false;

                PinObject pinObject = new PinObject(x.Row.ItemArray[0].ToString());
                 var axx = await pinObject.GetBoards();
              
               Combo_CurentBoard.Properties.Items.Clear();
                foreach (var bo in axx)
                {
                    string xa = bo.url.ToLower().Replace("https://www.pinterest.com/", "");
                    if (xa.EndsWith("/"))
                        xa = xa.TrimEnd(bo.url[bo.url.Length - 1]);
                        xa = xa.TrimStart(bo.url[bo.url.Length - 1]);
                    Combo_CurentBoard.Properties.Items.Add(xa + "[-]" + bo.id);
                }


            }
            catch (Exception ex)
            {

                LogReport(true, x.Row.ItemArray[0].ToString(), ex.Message);
                Lookup_ScrapingAccount.Enabled = true;
                Combo_CurentBoard.Enabled = true;
                Lookup_PostingAccount.Enabled = true;
                General_GroupBoxScrapSearch.Enabled = true;
                General_GroupBoxScrapBoard.Enabled = true;

            }
            Lookup_PostingAccount.Enabled = true;
            Lookup_ScrapingAccount.Enabled = true;
            Combo_CurentBoard.Enabled = true;
            General_GroupBoxScrapSearch.Enabled = true;
            General_GroupBoxScrapBoard.Enabled = true;








        }

        private void LabelControl3_Click(object sender, EventArgs e)
        {





        }
        string readerof;
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isAvailable = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (isAvailable)
            {
                try
                {
                    HttpWebRequest request = ((HttpWebRequest)(WebRequest.Create("http://pastebin.com/raw.php?i=F5CGG5JP")));
                    request.Method = "GET";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                    HttpWebResponse response = ((HttpWebResponse)(request.GetResponse()));
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    request.ContentType = "application/x-www-form-urlencoded";
                    this.readerof = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Connecting To The Internet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                }


                //0482716356

            }
            else
            {

                MessageBox.Show("Error Connecting To The Internet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();

            }





        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (!(this.readerof.Contains("online")))
            {
                Application.Exit();

            }

        }

        private void SimpleButton3_Click(object sender, EventArgs e)
        {

        }

        private void LabelControl5_Click(object sender, EventArgs e)
        {
           

        }

        private async void SimpleButton10_Click(object sender, EventArgs e)
        {

            try
            {
                btn_CreateBoard.Enabled = false;
                if (lookUpEdit2.GetSelectedDataRow() == null)
                {
                    MessageBox.Show("Please Select an account ! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btn_CreateBoard.Enabled = true;
                    return;
                }




              
                dynamic x = lookUpEdit2.GetSelectedDataRow();
                var client = new PinSharpClient(x.Row.ItemArray[3].ToString());




                var user = await client.Boards.CreateBoardAsync(txt_boardname.Text, txt_boarddes.Text);
                    MessageBox.Show("Board Created", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                

            }
            catch (Exception ex)
            {

                LogReport(true, txt_Username.Text, ex.Message);


            }
            txt_boarddes.Text = "";
            txt_boardname.Text = "";

            btn_CreateBoard.Enabled = true;




        }

        private void LabelControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void XtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

         
        }

        private void GridControl_Accounts_Click(object sender, EventArgs e)
        {

        }

        private void GridControl_Tasks_EmbeddedNavigator_Click(object sender, EventArgs e)
        {

            appHelper.SetTaskMangerDataTable_Control = Login.DT_TaskManger;
        }
    }
 }

    

