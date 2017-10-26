using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using Recipe_Organizer_MVC.Extensions;

namespace Recipe_Organizer_MVC.Models
{
    public class FileIOXlsx
    {
        public const string DELIMITER = "^^^";
        private string[] ColumnHeaderArr = { "Title", "Description", "CookMethod", "MealType",
            "Ingredients1", "Ingredients2", "Ingredients3", "Ingredients4", "Ingredients5", "Instructions1",
            "Instructions2", "Instructions3", "Instructions4", "Instructions5", "Notes" };

        private string SheetName = "Recipes";

        public string FileName { get; set; }
        public string FilePath { get; set; }
        private string FullPath
        {
            get { return FilePath + "\\" + FileName; }
        }

        public FileIOXlsx(string path, string file)
        {
            FileName = file;
            FilePath = path;
        }

        private Tuple<OleDbConnection, Exception> ConnectToFile()
        {
            try
            {
                return new Tuple<OleDbConnection, Exception>(new OleDbConnection(string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;\";", FullPath)), null);
            }
            catch (Exception ex)
            {
                return new Tuple<OleDbConnection, Exception>(null, ex);
            }
        }

        private string CreateTableString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("CREATE TABLE {0} (", SheetName));
            builder.Append(CreateSqlColumnString(true) + ")");
            return builder.ToString();
        }

        public Exception CreateFile()
        {
            Tuple<OleDbConnection, Exception> connectVal = ConnectToFile();
            OleDbConnection conn = connectVal.Item1;

            try
            {
                if (connectVal.Item2 != null)
                    throw connectVal.Item2;

                if (conn != null && conn.State == ConnectionState.Closed)
                    conn.Open();

                OleDbCommand command = new OleDbCommand();
                command.Connection = conn;
                command.CommandText = CreateTableString();
                command.ExecuteNonQuery();
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

        private string CreateSqlColumnString(bool addDataTypes)
        {
            StringBuilder builder = new StringBuilder();
            
            foreach (string header in ColumnHeaderArr)
                builder.Append(header + (addDataTypes ? " LongText": string.Empty)  + ",");
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        private string CreateInsertString(Recipe recipe)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("Insert into [{0}$] (", SheetName));
            builder.Append(CreateSqlColumnString(false));
            builder.Append(") values(");

            //These need to be in order of columns in ColumnHeaderArr
            builder.Append(string.Format("'{0}',", recipe.Title.SqlSafeString(false)));
            builder.Append(string.Format("'{0}',", recipe.Description.SqlSafeString(false)));
            builder.Append(string.Format("'{0}',", recipe.CookingInstructions.SqlSafeString(false)));

            StringBuilder mealTypeBuilder = new StringBuilder();
            foreach (string item in recipe.MealType)
                mealTypeBuilder.Append(item.SqlSafeString(false) + DELIMITER);
            mealTypeBuilder.Remove(mealTypeBuilder.Length - DELIMITER.Length, DELIMITER.Length);
            builder.Append(string.Format("'{0}',", mealTypeBuilder.ToString()));
            for (int i=0; i<Recipe.INGREDIENTS_SET_MAX_SIZE; i++)
                builder.Append(string.Format("'{0}',", recipe.Ingredients[i] != null ? recipe.Ingredients[i].StringDelimited(DELIMITER).SqlSafeString(false) : string.Empty));
            for (int i=0; i<Recipe.INSTRUCTION_SET_MAX_SIZE; i++)
                builder.Append(string.Format("'{0}',", recipe.Instructions[i] != null ? recipe.Instructions[i].StringDelimited(DELIMITER).SqlSafeString(false) : string.Empty));
            builder.Append(string.Format("'{0}'", recipe.Notes.SqlSafeString(false)));
            builder.Append(")");
            return builder.ToString();
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

                OleDbCommand command = new OleDbCommand();
                command.Connection = conn;

                //*******start here....

                foreach (Recipe recipe in collection)
                {
                    if (recipe.IsDeleted)
                    {
                        //delete existing row
                    }
                    else if (recipe.IsNew)
                    {
                        //insert into new row
                        command.CommandText = CreateInsertString(recipe);
                        command.ExecuteNonQuery();
                    }
                    else if (recipe.IsEdited)
                    {
                        //edit existing row
                    }
                }





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