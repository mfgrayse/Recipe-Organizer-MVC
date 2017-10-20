using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Recipe_Organizer_MVC.Models
{
    public class ConvertOldVersion
    {



        public ConvertOldVersion()
        {
            ConnectToFile();
        }


        //****this is only temporary:
        public const string RECIPE_START_TAG = "<!--RecipeStartTag-->";
        public const string RECIPE_END_TAG = "<!--RecipeEndTag-->";
        public const string TITLE_START_TAG = "<!--TitleStartTag-->";
        public const string TITLE_END_TAG = "<!--TitleEndTag-->";
        public const string RECIPE_DESCRIPTION_START_TAG = "<!--RecipeDescriptionStartTag-->";
        public const string RECIPE_DESCRIPTION_END_TAG = "<!--RecipeDescriptionEndTag-->";
        public const string MEAL_TYPE_START_TAG = "<!--MealTypeStartTag-->";
        public const string MEAL_TYPE_END_TAG = "<!--MealTypeEndTag-->";
        public const string RECIPE_INGREDIENTS_START_TAG = "<!--RecipeIngredientsStartTag-->";
        public const string RECIPE_INGREDIENTS_END_TAG = "<!--RecipeIngredientsEndTag-->";
        public const string COOK_TYPE_START_TAG = "<!--CookTypeStartTag-->";
        public const string COOK_TYPE_END_TAG = "<!--CookTypeEndTag-->";
        public const string RECIPE_INSTRUCTIONS_START_TAG = "<!--RecipeInstructionsStartTag-->";
        public const string RECIPE_INSTRUCTIONS_END_TAG = "<!--RecipeInstructionsEndTag-->";
        public const string RECIPE_NOTES_START_TAG = "<!--RecipeNotesStartTag-->";
        public const string RECIPE_NOTES_END_TAG = "<!--RecipeNotesEndTag-->";
        private string FilePath { get; set; }
        private string FileName { get; set; }
        //private Dictionary<string, Recipe> RecipeDict { get; set; }
        public RecipeCollection RecipeColl { get; private set; }
        private const string DEFAULT_FILE = "Recipes.html";
        private static string DEFAULT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


        //****this is only temporary:
        private Recipe ParseStringToProps(string htmlRecipe)
        {
            Recipe theRecipe = new Recipe();

            theRecipe.Title = TagSubString(htmlRecipe, TITLE_START_TAG, TITLE_END_TAG);
            theRecipe.Description = TagSubString(htmlRecipe, RECIPE_DESCRIPTION_START_TAG, RECIPE_DESCRIPTION_END_TAG);

            ////theRecipe.Ingredients = TagSubString(htmlRecipe, RECIPE_INGREDIENTS_START_TAG, RECIPE_INGREDIENTS_END_TAG);

            ////if (theRecipe.Ingredients.Contains("</li><li>"))
            ////    theRecipe.Ingredients = theRecipe.Ingredients.Replace("</li><li>", "\r\n");

            ////theRecipe.Ingredients = theRecipe.Ingredients.Replace("<li>", string.Empty);
            ////theRecipe.Ingredients = theRecipe.Ingredients.Replace("</li>", string.Empty);
            ////theRecipe.Ingredients = theRecipe.Ingredients.Replace("\t", string.Empty);

            ////theRecipe.Instructions = TagSubString(htmlRecipe, RECIPE_INSTRUCTIONS_START_TAG, RECIPE_INSTRUCTIONS_END_TAG);

            ////if (theRecipe.Instructions.Contains("</p><p>"))
            ////    theRecipe.Instructions = theRecipe.Instructions.Replace("</p><p>", "\r\n\r\n");

            ////else if (theRecipe.Instructions.Contains("</p>\r\n\t\t<p>"))
            ////    theRecipe.Instructions = theRecipe.Instructions.Replace("</p>\r\n\t\t<p>", "\r\n\r\n");

            ////theRecipe.Instructions = theRecipe.Instructions.Replace("<p>", string.Empty);
            ////theRecipe.Instructions = theRecipe.Instructions.Replace("</p>", string.Empty);

            theRecipe.Notes = TagSubString(htmlRecipe, RECIPE_NOTES_START_TAG, RECIPE_NOTES_END_TAG);
            theRecipe.CookingInstructions = TagSubString(htmlRecipe, COOK_TYPE_START_TAG, COOK_TYPE_END_TAG);

            string tempItem = string.Empty;
            theRecipe.MealType = new List<string>();

            foreach (string item in TagSubString(htmlRecipe, MEAL_TYPE_START_TAG, MEAL_TYPE_END_TAG).Split(','))
            {
                tempItem = item.Replace("<!--", string.Empty);
                tempItem = tempItem.Replace("-->", string.Empty);
                theRecipe.MealType.Add(tempItem.Trim());
            }
            return theRecipe;
        }

        //****this is only temporary:
        private string TagSubString(string text, string startTag, string endTag)
        {
            int start = text.IndexOf(startTag) + startTag.Length;
            int end = text.IndexOf(endTag);

            return text.Substring(start, end - start);
        }

        //****this is only temporary:
        private bool ConnectToFile()
        {

            if (string.IsNullOrWhiteSpace(FilePath))
                FilePath = DEFAULT_PATH;

            string textBuffer = string.Empty;

            try
            {
                textBuffer = ReadFromFile();
            }
            catch (Exception ex)
            {
                return false;
            }

            if (!LoadFileTextToDict(textBuffer))
            {
                //MessageBox.Show("Error loading file contents into memory.");
                return false;
            }

            return true;
        }
        private string FullPath
        {
            get { return FilePath + "\\" + FileName; }
        }

        private string ReadFromFile()
        {
            return System.IO.File.ReadAllText(FullPath, Encoding.UTF8);
        }

        private bool LoadFileTextToDict(string fileText)
        {
            try
            {
                RecipeColl = ParseRecipe(fileText);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error loading file into ram: " + ex.Message);
                return false;
            }

            return true;
        }

        private RecipeCollection ParseRecipe(string fileText)
        {
            int index = 0;
            int start = 0;
            int end = 0;
            RecipeCollection recipeDict = new RecipeCollection();
            Recipe tempConcrete = null;
            bool endOfFile = false;

            while (!endOfFile)
            {
                start = fileText.IndexOf(RECIPE_START_TAG, index);
                end = fileText.IndexOf(RECIPE_END_TAG, index + 1) + RECIPE_END_TAG.Length;

                if (start >= 0 && end >= 0)
                {
                    tempConcrete = ParseStringToProps(fileText.Substring(start, end - start));
                    recipeDict.Add(tempConcrete);
                    index = end;
                }
                else
                    endOfFile = true;
            }

            return recipeDict;
        }
    }
}