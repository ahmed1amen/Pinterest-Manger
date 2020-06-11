using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinterestAccountManager
{
    class FILEWriter
    {

        public int scrappingCount=0;
    public    DataTable SearchResultTable = new DataTable();



        public FILEWriter()
    {



            SearchResultTable.Columns.AddRange(new DataColumn[] {
                new DataColumn("ID", typeof(string)),
                new DataColumn("PIN_Url", typeof(string)),
                new DataColumn("PIN_Link", typeof(string)),
                new DataColumn("IMAGE", typeof(string)),
                new DataColumn("Title", typeof(string)),
                new DataColumn("Description", typeof(string)),
                new DataColumn("Board_Name", typeof(string)),
                new DataColumn("Board_URL", typeof(string)),

            });




        }

        public void AddTODataTable(string [] arr)
        {
            //  "ID",
            //"PIN_Url",
            //"PIN_Link",
            //"IMAGE",
            //  "Note"
            SearchResultTable.Rows.Add(arr);
            


        }

        public void PUSHToFile(string account, string path)
        {


            try
            {

            


                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + account) == false)
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + account);
                }



                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    
                        Rfc4180Writer.WriteDataTable(SearchResultTable, writer,false);
                }

                }




            catch (Exception ex)
            {
                
            }


        }

        public string WriteCSVFile(string account,string perfix= "",string PathIFExist ="")
        {


            try {

                string filename = DateTime.Now.Day.ToString() + "-"+ DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
                if (PathIFExist != "")
                    filename = PathIFExist;


                if ( Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + account) == false)
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + account);
                }



             using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory+ account + "\\" + perfix + filename + ".csv" ,true))
            {
                    if (PathIFExist != "")
                        Rfc4180Writer.WriteDataTable(SearchResultTable, writer, false);
                    else
                        Rfc4180Writer.WriteDataTable(SearchResultTable, writer, true);
            }

                return (AppDomain.CurrentDomain.BaseDirectory + account + "\\" + perfix + filename + ".csv");
            }




            catch (Exception ex)
            {
                return "";
            }




        }

    }
}
