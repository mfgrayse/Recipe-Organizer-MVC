namespace Recipe_Organizer_MVC.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Create an SQL safe string from user entered data.  Replace any single-quote characters (')
        /// with a pair of single-quote characters ('').  Then wrap the entire string in single-quote
        /// characters to make a SQL string.  For example, "O'Malley" becomes "'O''Malley'". 
        /// </summary>
        /// <param name="str">A string to make SQL safe.</param>
        /// <returns>The safe string.</returns>
        public static string SqlSafeString(this string str, bool wrapWithQuote)
        {
            if (str == null)
                return "NULL";

            return wrapWithQuote == true ? "'" + str.Replace("'", "''") + "'" : str.Replace("'", "''");
        }
    }
}