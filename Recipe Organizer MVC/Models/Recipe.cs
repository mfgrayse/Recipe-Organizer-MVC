using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Recipe_Organizer_MVC.Models
{
    public class Recipe
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Notes { get; set; }
        public string CookingInstructions { get; set; }
        public List<string> MealType { get; set; }
        public string MealTypeHtml { get; set; }
        public bool IsNewRecipe { get; set; }
        public int ID { get; set; }
        public bool IsEdited { get; set; }

        public Recipe() { }

        public Recipe(DataRow row)
        {

        }





    }
}