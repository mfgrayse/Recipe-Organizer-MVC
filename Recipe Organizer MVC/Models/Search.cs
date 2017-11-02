using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class Search
    {
        public IQuery Query { get; set; }
        public RecipeCollection TheRecipeCollection { get; set; }

        public Search() { }

    }
}