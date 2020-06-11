
using Newtonsoft.Json;
using PinSharp;
using PinterestAccountManager.Pinterst;
using PinterestAccountManager.Pinterst.API;
using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Classes.Models;
using PinterestAccountManager.Pinterst.Classes.Models.Pins;
using PinterestAccountManager.Pinterst.Logger;
using System;
using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
using System.IO;
using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinterestAccountManager
{
    public partial class Form1 : Form
    {
        private static IPinApi PinApi;
        public Form1()
        {
            InitializeComponent();
        }

        async Task loadUserifAsync()
        {
             string stateFile =textBox1.Text.GetHashCode().ToString();
            var userSession = new UserSessionData
            {
                UserName = textBox1.Text,
                Password = textBox2.Text
            };
            var delay = RequestDelay.FromSeconds(2, 2);
            PinApi = PinApiBuilder.CreateBuilder().SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.All)) // use logger for requests and debug messages
                    .SetRequestDelay(delay)
                    .Build();


                    try
                                {
                if (File.Exists(stateFile))
                {
                    Console.WriteLine("Loading state from file");
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
            catch (Exception exa)
            {
                Console.WriteLine(exa);
            }



        }

        private async  void Button1_ClickAsync(object sender, EventArgs e)
        {




   //       
//

           await loadUserifAsync();
            button1.Enabled = true;
            button2.Enabled = true; 
            button3.Enabled = true;

       //     var client = new PinSharpClient("Ahq9K_zjHK-9iTy9IgHdr5akV3zbFeR79C0oYK9GZIxO6AC0rQnZADAAAvyzRmsHfj5gv5YAAAAA");


       //     var board = await client.Me.SearchPinsAsync("", "", 500);

            

       ////     var inof = await client.Me.GetUserAsync();

       //     string pathh = @"C:\Users\Dr.Ahmed Amen\Desktop\541961_7924574_1557225_2c065fea_image.png";
       //     Image image = Image.FromFile(pathh);
       //     MemoryStream memoryStream = new MemoryStream();
       //     image.Save(memoryStream, image.RawFormat);
       //     byte[] imagebytes = memoryStream.ToArray();
       //     string base64string = Convert.ToBase64String(imagebytes);

       //    // var inofsx = await client.Pins.CreatePinFromBase64Async(board.First().Id, base64string, "blaaablaa");






        }

        private async void Button2_ClickAsync(object sender, EventArgs e)
        {
       
            var boardtask =  await PinApi.UserProcessor.SearchPins("dogs", "");
            List<PinsSearchResultItem> results = boardtask.Value.PinsSearchResultItem;
           string cc  =  boardtask.Value.Cursor;
            foreach (PinsSearchResultItem board in results)
            {
                MessageBox.Show(board.images.orig.url);
            }



            //var Boardtask = await PinApi.UserProcessor.GetUserBoards();
            //List<BoardItem> boardlist = Boardtask.Value;

            //foreach (BoardItem board in boardlist)
            //{
            //   dataGridView1.Rows.Add(board.name , board.id);

            //}

        }
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog ()== DialogResult.OK)
            {
                txt_ImageUrl.Text = openFileDialog1.FileName;

            }
        }

        private async void Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count== 0 || dataGridView1.Rows.Count==0)
            {
                MessageBox.Show("Please Select Any Board","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);


            }
            else
            { 
            if (txt_ImageUrl.Text=="")
            {
                MessageBox.Show("Error Uri Or Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

              


            }
            else
                {
                    this.button3.Enabled = false;
                    var post = await PinApi.PinProcessor.PostPinAsync(txt_ImageUrl.Text, dataGridView1.SelectedRows[0].Cells[1].Value.ToString(), txt_description.Text, txt_link.Text, txt_titel.Text);

                    this.button3.Enabled = true;

                    MessageBox.Show("Your Pin,successfully Posted ", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }

        
    }
}
