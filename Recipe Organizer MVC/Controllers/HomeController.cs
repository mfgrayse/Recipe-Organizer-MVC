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
        public ActionResult Update(Recipe recipe)
        {
            ISearch searchObj = (ISearch)System.Web.HttpContext.Current.Session["SearchObject"];
            searchObj.SelectedRecipe = recipe;

            //***update excel here....then grab recipe from excel and display


            return View("Index", searchObj.SelectedRecipe);
        }

        [HttpPost]
        public ActionResult Edit(string buttonVal)
        {
            ISearch searchObj = (ISearch)System.Web.HttpContext.Current.Session["SearchObject"];

            if (buttonVal.Equals("edit") && searchObj != null && searchObj.SelectedRecipe != null)
                return View(searchObj.SelectedRecipe);

            return View(new Recipe());
        }

        [HttpPost]
        public ActionResult ExcelSearch(ExcelSearch searchObj)
        {
            FileIOXlsx fileIO = new FileIOXlsx(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "recipes.xlsx");
            fileIO.ReadFromFile(searchObj);
            searchObj.TheRecipeCollection.Sort((r1,r2)=>r1.Title.CompareTo(r2.Title));
            searchObj.SelectedRecipe = searchObj.TheRecipeCollection.Count > 0 ? searchObj.TheRecipeCollection[0] : null;
            System.Web.HttpContext.Current.Session["SearchObject"] = searchObj;
            return View("Index", searchObj.SelectedRecipe);
        }

        [HttpPost]
        public ActionResult SelectedRecipe(string buttonVal)
        {
            ISearch searchObj = (ISearch)System.Web.HttpContext.Current.Session["SearchObject"];

            if (searchObj == null || searchObj.TheRecipeCollection == null || searchObj.TheRecipeCollection.Count == 0)
                return View("Index", null);

            //Find the recipe based on button text
            for (int i = 0; i < searchObj.TheRecipeCollection.Count; i++)
            {
                if (searchObj.TheRecipeCollection[i].Title.Equals(buttonVal))
                {
                    searchObj.SelectedRecipe = searchObj.TheRecipeCollection[i];
                    break;
                }
            }

            return View("Index", searchObj.SelectedRecipe);
        }
    }
}