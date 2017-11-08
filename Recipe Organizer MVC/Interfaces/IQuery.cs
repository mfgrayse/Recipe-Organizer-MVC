using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe_Organizer_MVC.Models;

namespace Recipe_Organizer_MVC.Interfaces
{
    public interface IQuery
    {
//    SearchItems =  "Title", "Meal Type", "Description", "Ingredients", "Instructions", "Cooking Method", "Notes" };
  //  MealTypes = "Dinner", "Lunch", "Breakfast", "Dessert", "Drink", "Other" };
  
        string TextToSearchFor { get; set; }
        bool SearchTitle { get; set; }
        bool SearchDescription { get; set; }
        bool SearchIngredients { get; set; }
        bool SearchInstructions { get; set; }
        bool SearchCookingMethod { get; set; }
        bool SearchNotes { get; set; }
        bool SearchMTDinner { get; set; }
        bool SearchMTLunch { get; set; }
        bool SearchMTBreakfast { get; set; }
        bool SearchMTDessert { get; set; }
        bool SearchMTDrink { get; set; }
        bool SearchMTOther { get; set; }


        /// <summary>
        /// Gets the query created from SearchText and SearchOptions
        /// </summary>
        /// <param name="queryPart">This is an optional parameter in case something needs to be added from an external class.</param>
        /// <returns></returns>
        string GetQuery(string optionalQueryPart);
    }
}
