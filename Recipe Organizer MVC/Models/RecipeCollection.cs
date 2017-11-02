using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class RecipeCollection : List<Recipe>
    {
        public Exception ErrorMessage { get; private set; }

        public RecipeCollection() { }

        public RecipeCollection(Recipe theRecipe)
        {
            this.Add(theRecipe);
        }

        public RecipeCollection(DataTable table, string titleCol,
            string descriptionCol, string cookMethodCol, string mealTypeCol, IList<string> ingredientsCols,
            IList<string> instructionsCols, string notesCol, string delimiter)
        {
            foreach (DataRow row in table.Rows)
            {
                this.Add(new Recipe(row, titleCol, descriptionCol, cookMethodCol, mealTypeCol, 
                    ingredientsCols, instructionsCols, notesCol, delimiter));
            }
        }
    }
}