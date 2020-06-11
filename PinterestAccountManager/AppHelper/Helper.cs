using Microsoft.VisualBasic;
using PinSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinterestAccountManager.AppHelper
{
    public class appHelper
    {


        
        public static DataTable SetUserDataTable_Control
        {

            set  {
                string path = @"UserDataTable.dll";

                Stream stream = File.Create(path);
                using (stream)
                {
                    try
                    {
                         
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, value);
                        
                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Problem writing TweetsDataTable to disk: " + Environment.NewLine + e.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

                    }
                }
                }

          get  {
                string path = @"UserDataTable.dll";
                Stream stream = File.Open(path, FileMode.Open);
                using (stream)
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        
                        return (DataTable)formatter.Deserialize(stream);


                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Problem reading UserDataTable from disk: " + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new DataTable();
                    }
                }



            }

        }






        public static DataTable SetTaskMangerDataTable_Control
        {

            set
            {
                string path = @"TaskMangerDataTable.dll";

                Stream stream = File.Create(path);
                using (stream)
                {
                    try
                    {

                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, value);

                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Problem writing TaskMangerDataTable to disk: " + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }

            get
            {
                string path = @"TaskMangerDataTable.dll";
                Stream stream = File.Open(path, FileMode.Open);
                using (stream)
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        return (DataTable)formatter.Deserialize(stream);


                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Problem reading TaskMangerDataTable from disk: " + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new DataTable();
                    }
                }



            }

        }



        public static PinSharpClient LoadUserSession (string path = "")
        {
            {

                Stream stream = File.Create(path);
                using (stream)
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, path);
                       }

                    catch (Exception e)
                    {
                        MessageBox.Show("Problem writing Session File to disk: " + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }



            }


            {

                Stream stream = File.Open(path, FileMode.Open);
                using (stream)
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, path);

                        return (PinSharpClient)formatter.Deserialize(stream);
                    }

                    catch (Exception e)
                    {
                        MessageBox.Show("Problem reading Session File from disk: " + Environment.NewLine + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new PinSharpClient();
                    }
                }



            }


        }



    }
}
