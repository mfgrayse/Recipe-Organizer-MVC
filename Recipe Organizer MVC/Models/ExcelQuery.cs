using System;
using Recipe_Organizer_MVC.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Recipe_Organizer_MVC.Models
{
    public class ExcelQuery : IQuery
    {
        public string TextToSearchFor { get; set; }
        public bool SearchInTitle { get; set; }
        //add others....

        public ExcelQuery()
        {
            TextToSearchFor = string.Empty;
        }

        public ExcelQuery(string textToSearchFor)
        {
            TextToSearchFor = textToSearchFor;
        }

        public string GetQuery(string sheetName)
        {
            StringBuilder builder = new StringBuilder();


            builder.Append(string.Format("Select * from [{0}$] where title not like '%aaaaaaaaaaaaaaaaaaaa%'{1}",
                sheetName, (string.IsNullOrEmpty(TextToSearchFor) ? string.Empty : " and Title like '%"+ TextToSearchFor +"%'")));

            //        string query = string.Format("Select * from [{0}$] where title not like '%aaaaaaaaaaaaaaaaaaaa%'{1}",
            //SheetName, (string.IsNullOrWhiteSpace(whereQueryPart) ? string.Empty : " and " + whereQueryPart));


            return builder.ToString();
        }
        
    }
}