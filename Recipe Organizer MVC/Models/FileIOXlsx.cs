using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace Recipe_Organizer_MVC.Models
{
    public class FileIOXlsx
    {
        private string[] ColumnHeaderArr = { "ID", "Title", "Description", "CookMethod", "MealType",
            "Ingredients1", "Ingredients2", "Ingredients3", "Ingredients4", "Ingredients5", "Instructions1",
            "Instructions2", "Instructions3", "Instructions4", "Instructions5", "Notes" };
        public string FileName { get; set; }
        public string FilePath { get; set; }
        private string FullPath
        {
            get { return FilePath + "\\" + FileName; }
        }

        public FileIOXlsx(string file, string path)
        {
            FileName = file;
            FilePath = path;
        }

        private Tuple<OleDbConnection, Exception> ConnectToFile()
        {
            try
            {
                return new Tuple<OleDbConnection, Exception>(new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", FullPath)), null);
            }
            catch (Exception ex)
            {
                return new Tuple<OleDbConnection, Exception>(null, ex);
            }
        }

        private Exception CreateFile()
        {
            //*****
            return null;
        }

        public RecipeCollection ReadFromFile(string query)
        {
            Tuple<OleDbConnection, Exception> connectVal = ConnectToFile();
            OleDbConnection conn = connectVal.Item1;

            try
            {
                if (connectVal.Item2 != null)
                    throw connectVal.Item2;

                if (conn != null && conn.State == ConnectionState.Closed)
                    conn.Open();

                //                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("Select * from [G2WAttendee_xls$]", conn);
                //****do we need to always filter out column header info or just when 'select *'?????
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);

                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return new RecipeCollection(dataSet.Tables[0]);
            }
            catch (Exception ex)
            {
                return new RecipeCollection(ex);
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public Exception WriteToFile(RecipeCollection collection)
        {
            Tuple<OleDbConnection, Exception> connectVal = ConnectToFile();
            OleDbConnection conn = connectVal.Item1;

            try
            {
                if (connectVal.Item2 != null)
                    throw connectVal.Item2;

                if (conn != null && conn.State == ConnectionState.Closed)
                    conn.Open();


                //*******start here....



                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }




            /*        /// <summary>
        /// Attempts to save a new XLS file to the location the GoToWebinar Attendee report was read from. If the user
        /// does not have write access to that folder, it attempts to save the file in their My Documents folder.
        /// </summary>
        /// <returns>True if file successfully saved</returns>
        private bool SaveToFile()
        {
            outputFile = attendeeReportPath + "\\DTI_" + Path.GetFileNameWithoutExtension(txtAttendeeReport.Text) + ".xls";

            if (!ExcelWorkbookUtil.Create(excelTable, outputFile))
            {
                MessageBox.Show("There was an error saving to " + outputFile + " attempting to save to My Documents.");
                outputFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DTI_" + Path.GetFileNameWithoutExtension(txtAttendeeReport.Text) + ".xls";
                
                if (!ExcelWorkbookUtil.Create(excelTable, outputFile))
                    MessageBox.Show("There was an error saving to My Documents, please contact IT for further " + 
                        "assistance.", "Can't Save", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }
*/






        }

    }
}