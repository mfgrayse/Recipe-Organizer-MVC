using System;
using System.Web.Mvc;
using Recipe_Organizer_MVC.Interfaces;
using Recipe_Organizer_MVC.Models;

namespace Recipe_Organizer_MVC.Controllers
{
    public class HomeController : Controller
    {
        public const string BUTTON_SPACE_REPLACE = "^^^";

        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExcelSearch(ExcelSearch searchObj)
        {
            FileIOXlsx fileIO = new FileIOXlsx(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "recipes.xlsx");
            fileIO.ReadFromFile(searchObj);
            System.Web.HttpContext.Current.Session["SearchObject"] = searchObj;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SelectedRecipe(string submitRecipe)
        {
            RecipeCollection coll = ((ISearch)System.Web.HttpContext.Current.Session["SearchObject"]).TheRecipeCollection;
            Recipe selected = null;

            for (int i = 0; i < coll.Count; i++)
            {
                //This parses the chosen (via click) recipe button's value that is automatically passed as a string.
                //The button's value is the name of the recipe with spaces replaced by ^^^ since there can't be spaces
                //in the button's value property.
                if (coll[i].Title.Equals(submitRecipe.Replace(BUTTON_SPACE_REPLACE, " ")))
                {
                    selected = coll[i];
                    break;
                }
            }

            return View("Index", selected);
        }
    }
}