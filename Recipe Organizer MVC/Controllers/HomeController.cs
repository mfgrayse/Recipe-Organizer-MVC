using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recipe_Organizer_MVC.Models;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";


            //ConvertOldVersion oldVer = new Recipe_Organizer_MVC.Models.ConvertOldVersion();
            //FileIOXlsx fileIO = new FileIOXlsx(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "recipes.xlsx");
            //fileIO.CreateFile();
            //fileIO.WriteToFile(oldVer.RecipeColl);
            //ISearch search = new ExcelSearch();
            //fileIO.ReadFromFile(search);
            //return View(search.TheRecipeCollection[3]);
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
            System.Web.HttpContext.Current.Session["RecipeColl"] = searchObj.TheRecipeCollection;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SelectedRecipe(string submitRecipe)
        {
            RecipeCollection coll = (RecipeCollection)System.Web.HttpContext.Current.Session["RecipeColl"];
            Recipe selected = null;

            for (int i = 0; i < coll.Count; i++)
            {
                if (coll[i].Title.Equals(submitRecipe.Replace("^^^", " ")))
                {
                    selected = coll[i];
                    break;
                }
            }

            return View("Index", selected);
        }


        //[HttpPost]
        //public ActionResult SelectedRecipe(string submitRecipe)
        //{
        //    RecipeCollection coll = (RecipeCollection)ViewData["RecipeColl"];
        //    Recipe selected = null;

        //    for (int i=0; i< coll.Count; i++)
        //    {
        //        if (coll[i].Title.Equals(submitRecipe))
        //        {
        //            selected = coll[i];
        //            break;
        //        }
        //    }

        //    return View("Index", selected);
        //}


        //[HttpPost]
        //public ActionResult SelectedRecipe(Tuple<Recipe, ISearch> recipes)
        //{
        //    ViewData["RecipeColl"] = recipes.Item2;
        //    return View("Index", recipes.Item1);
        //}
    }
}