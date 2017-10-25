using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class RecipeIngredientPart : IRecipeSectionPart<Ingredient>
    {
        public string SectionHeader { get; set; }
        public List<Ingredient> ItemList { get; set; }

        public RecipeIngredientPart()
        {
            ItemList = new List<Ingredient>();
        }

        public string StringDelimited(string delimiter)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append(SectionHeader + delimiter);
            foreach (Ingredient item in ItemList)
                builder.Append(item.StringWithSeperator() + delimiter);
            builder.Remove(builder.Length - delimiter.Length, delimiter.Length);

            return builder.ToString();
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(SectionHeader) ? "Blank Ingredient Header" : "Section: " + SectionHeader; 
        }
    }
}