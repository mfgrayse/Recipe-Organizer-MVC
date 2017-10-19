using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

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

        public RecipeCollection(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                this.Add(new Recipe(row));
            }
        }

        public RecipeCollection(Exception ex)
        {
            ErrorMessage = ex;
        }

    }
}