using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recipe_Organizer_MVC.Models;

namespace Recipe_Organizer_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";
            Recipe foo = new Recipe()
            {
                Title = "Pizza Crust",
                Description = "Pizza or Calzone Crust",
                CookingInstructions = "Bake at 550 as Pizza about 10 mins or done",
                //Ingredients = new List<List<string>> { new List<string> { "1 1/2 cups Water (luke-warm)", "1 package Yeast", "1 cup Wheat Flour", "3 cups Bread Flour", "1/8 tsp Salt", "1/8 tsp white pepper", "1/8 tsp Garlic Powder", "Parmesan Cheese", "1 tbsp Olive Oil" } },
                //Instructions = new List<List<string>> { new List<string> { "1. Bloom yeast in luke-warm water, about 10 mins.", "2. Combine dry ingredients and cheese.", "3. Make well, pour in yeast water, combine until liquid absorbed.", "4. Knead on table 10-15 times.", "5. Coat rising bowl with olive oil, let dough rise about an hour.", "6. Punch down, knead 5-7 times, quarter, let rise another 10-15 mins.", "7. Create pizza/calzone crust by hand." } },
                MealType = new List<string> { "Dinner", "Lunch" },
                Notes = "These are the notes."

            };

            return View(foo);
            //return View();
        }

        public ActionResult About()
        {
            return View();
        }

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}