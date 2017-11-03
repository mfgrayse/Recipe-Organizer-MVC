using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using Recipe_Organizer_MVC.Extensions;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class FileIOXlsx
    {
        public const string DELIMITER = "^^^";
        public const string TITLE_COL = "Title";
        public const string DESCRIPTION_COL = "Description";
        public const string COOKMETHOD_COL = "Cookmethod";
        public const string MEALTYPE_COL = "Mealtype";
        public const string INGREDIENTS1_COL = "Ingredients1";
        public const string INGREDIENTS2_COL = "Ingredients2";
        public const string INGREDIENTS3_COL = "Ingredients3";
        public const string INGREDIENTS4_COL = "Ingredients4";
        public const string INGREDIENTS5_COL = "Ingredients5";
        public const string INSTRUCTIONS1_COL = "Instructions1";
        public const string INSTRUCTIONS2_COL = "Instructions2";
        public const string INSTRUCTIONS3_COL = "Instructions3";
        public const string INSTRUCTIONS4_COL = "Instructions4";
        public const string INSTRUCTIONS5_COL = "Instructions5";
        public const string NOTES_COL = "Notes";
        public const int INSTRUCTION_SET_MAX_SIZE = 5;
        public const int INGREDIENTS_SET_MAX_SIZE = 5;

        private string[] ColumnHeaderArr = { TITLE_COL, DESCRIPTION_COL, COOKMETHOD_COL, MEALTYPE_COL,
            INGREDIENTS1_COL, INGREDIENTS2_COL, INGREDIENTS3_COL, INGREDIENTS4_COL, INGREDIENTS5_COL,
            INSTRUCTIONS1_COL, INSTRUCTIONS2_COL, INSTRUCTIONS3_COL, INSTRUCTIONS4_COL,
            INSTRUCTIONS5_COL, NOTES_COL };
        
        public static string SheetName = "Recipes";

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

        /// <summary>
        /// Creates an XLSX file with the correct column headers. Note that due to the limitations of the
        /// OleDB provider for .NET, that LongText (columns that allow more than 255 chars) columns cannot be created
        /// here. You must first manually open the file and manually insert a string with more than 255 chars into
        /// each column. LongText inserts will then work properly provided the seed is not removed until real data
        /// that is longer than 255 chars is inserted.
        /// </summary>
        /// <returns></returns>
        public void CreateFile()
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
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public void ReadFromFile(ISearch searchObj)
        {
            Tuple<OleDbConnection, Exception> connectVal = ConnectToFile();
            OleDbConnection conn = connectVal.Item1;

            try
            {
                if (connectVal.Item2 != null)
                    throw connectVal.Item2;

                if (conn != null && conn.State == ConnectionState.Closed)
                    conn.Open();

                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(searchObj.Query.GetQuery(SheetName), conn);
                string foo = searchObj.Query.GetQuery(SheetName);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                searchObj.TheRecipeCollection = new RecipeCollection(dataSet.Tables[0], TITLE_COL, DESCRIPTION_COL, COOKMETHOD_COL, MEALTYPE_COL,
                    new string[] { INGREDIENTS1_COL, INGREDIENTS2_COL, INGREDIENTS3_COL, INGREDIENTS4_COL, INGREDIENTS5_COL },
                    new string[] { INSTRUCTIONS1_COL, INSTRUCTIONS2_COL, INSTRUCTIONS3_COL, INSTRUCTIONS4_COL, INSTRUCTIONS5_COL }, 
                    NOTES_COL, DELIMITER );
            }
            catch (Exception ex)
            {
                throw ex;
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
            for (int i = 0; i < INGREDIENTS_SET_MAX_SIZE; i++)
                builder.Append(string.Format("'{0}',", recipe.Ingredients.Count > i && recipe.Ingredients[i] != null ? recipe.Ingredients[i].ToStringDelimited(DELIMITER).SqlSafeString(false) : string.Empty));
            for (int i=0; i<INSTRUCTION_SET_MAX_SIZE; i++)
                builder.Append(string.Format("'{0}',", recipe.Instructions.Count > i && recipe.Instructions[i] != null ? recipe.Instructions[i].ToStringDelimited(DELIMITER).SqlSafeString(false) : string.Empty));
            builder.Append(string.Format("'{0}'", recipe.Notes.SqlSafeString(false)));
            builder.Append(")");
            return builder.ToString();
        }

        public void WriteToFile(RecipeCollection collection)
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

                //*******start here....test reading (full and partial) and do an update

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

        }

    }
}