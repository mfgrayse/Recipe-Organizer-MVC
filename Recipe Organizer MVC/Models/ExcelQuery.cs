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
        public bool SearchTitle { get; set; }
        public bool SearchDescription { get; set; }
        public bool SearchIngredients { get; set; }
        public bool SearchInstructions { get; set; }
        public bool SearchCookingMethod { get; set; }
        public bool SearchNotes { get; set; }
        public bool SearchMTDinner { get; set; }
        public bool SearchMTLunch { get; set; }
        public bool SearchMTBreakfast { get; set; }
        public bool SearchMTDessert { get; set; }
        public bool SearchMTDrink { get; set; }
        public bool SearchMTOther { get; set; }
        
        public ExcelQuery()
        {
            TextToSearchFor = string.Empty;
        }

        public ExcelQuery(string textToSearchFor)
        {
            TextToSearchFor = textToSearchFor;
        }

        private CheckStatus GetCheckStatus()
        {
            bool otherThanMTChecked = SearchTitle || SearchDescription || SearchIngredients || SearchInstructions || SearchCookingMethod || SearchNotes;
            bool mtChecked = SearchMTDinner || SearchMTLunch || SearchMTBreakfast || SearchMTDessert || SearchMTDrink || SearchMTOther;

            if (!otherThanMTChecked && mtChecked)
                return CheckStatus.MealTypeOnly;

            if (!otherThanMTChecked && !mtChecked)
                return CheckStatus.Nothing;

            if (otherThanMTChecked && mtChecked)
                return CheckStatus.Both;

            return CheckStatus.OtherThanMealTypeOnly;
        }

        /// <summary>
        /// Gets the query for Excel. If nothing checked, defaults to title search. If no serach text, then gets all.
        /// If meal type(s) is checked it will either get all for meal type(s) or other search criteria for meal type(s).
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public string GetQuery(string sheetName)
        {
            StringBuilder builder = new StringBuilder();
            CheckStatus checkStatus = GetCheckStatus();
            string header = " and {0} like '%{1}%'";

            if (checkStatus == CheckStatus.MealTypeOnly || checkStatus == CheckStatus.Both)
            {
                if (SearchMTDinner)
                    builder.Append(string.Format(header, FileIOXlsx.MEALTYPE_COL, Recipe.MT_DINNER));
                if (SearchMTLunch)
                    builder.Append(string.Format(header, FileIOXlsx.MEALTYPE_COL, Recipe.MT_LUNCH));
                if (SearchMTBreakfast)
                    builder.Append(string.Format(header, FileIOXlsx.MEALTYPE_COL, Recipe.MT_BREAKFAST));
                if (SearchMTDessert)
                    builder.Append(string.Format(header, FileIOXlsx.MEALTYPE_COL, Recipe.MT_DESSERT));
                if (SearchMTDrink)
                    builder.Append(string.Format(header, FileIOXlsx.MEALTYPE_COL, Recipe.MT_DRINK));
                if (SearchMTOther)
                    builder.Append(string.Format(header, FileIOXlsx.MEALTYPE_COL, Recipe.MT_OTHER));
            }

            if (!string.IsNullOrWhiteSpace(TextToSearchFor))
            {
                if (checkStatus == CheckStatus.OtherThanMealTypeOnly || checkStatus == CheckStatus.Both)
                {
                    if (SearchDescription)
                        builder.Append(string.Format(header, FileIOXlsx.DESCRIPTION_COL, TextToSearchFor));

                    if (SearchIngredients)
                    {
                        builder.Append(string.Format(" and ({0} like '%{1}%' or {2} like '%{1}%' or {3} like '%{1}%' or {4} like '%{1}%' or {5} like '%{1}%')",
                            FileIOXlsx.INGREDIENTS1_COL, TextToSearchFor, FileIOXlsx.INGREDIENTS2_COL, FileIOXlsx.INGREDIENTS3_COL, FileIOXlsx.INGREDIENTS4_COL, FileIOXlsx.INGREDIENTS5_COL));
                    }
                    if (SearchInstructions)
                    {
                        builder.Append(string.Format(" and ({0} like '%{1}%' or {2} like '%{1}%' or {3} like '%{1}%' or {4} like '%{1}%' or {5} like '%{1}%')",
                            FileIOXlsx.INSTRUCTIONS1_COL, TextToSearchFor, FileIOXlsx.INSTRUCTIONS2_COL, FileIOXlsx.INSTRUCTIONS3_COL, FileIOXlsx.INSTRUCTIONS4_COL, FileIOXlsx.INSTRUCTIONS5_COL));
                    }

                    if (SearchCookingMethod)
                        builder.Append(string.Format(header, FileIOXlsx.COOKMETHOD_COL, TextToSearchFor));
                    if (SearchNotes)
                        builder.Append(string.Format(header, FileIOXlsx.NOTES_COL, TextToSearchFor));
                }

                if (builder.Length == 0 || SearchTitle || checkStatus == CheckStatus.MealTypeOnly)
                    builder.Append(string.Format(header, FileIOXlsx.TITLE_COL, TextToSearchFor));
            }

            string foo = string.Format("Select * from [{0}$] where title not like '%aaaaaaaaaaaaaaaaaaaa%'{1}",
                sheetName, builder.ToString());

            return string.Format("Select * from [{0}$] where title not like '%aaaaaaaaaaaaaaaaaaaa%'{1}",
                sheetName, builder.ToString());
        }
    }

    internal enum CheckStatus
    {
        MealTypeOnly,
        OtherThanMealTypeOnly,
        Nothing,
        Both
    }
}