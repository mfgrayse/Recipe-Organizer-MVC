using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class ConvertOldVersion
    {
        public ConvertOldVersion()
        {
            ConnectToFile();
        }

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
        public RecipeCollection RecipeColl { get; private set; }
        private const string DEFAULT_FILE = "Recipes.html";
        private static string DEFAULT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private Recipe ParseStringToProps(string htmlRecipe)
        {
            Recipe theRecipe = new Recipe();
            theRecipe.Title = TagSubString(htmlRecipe, TITLE_START_TAG, TITLE_END_TAG);
            theRecipe.Description = TagSubString(htmlRecipe, RECIPE_DESCRIPTION_START_TAG, RECIPE_DESCRIPTION_END_TAG);
            theRecipe.Ingredients = ConvertIngredients(TagSubString(htmlRecipe, RECIPE_INGREDIENTS_START_TAG, RECIPE_INGREDIENTS_END_TAG));
            theRecipe.Instructions = ConvertInstructions(TagSubString(htmlRecipe, RECIPE_INSTRUCTIONS_START_TAG, RECIPE_INSTRUCTIONS_END_TAG));
            theRecipe.Notes = TagSubString(htmlRecipe, RECIPE_NOTES_START_TAG, RECIPE_NOTES_END_TAG);
            theRecipe.CookingInstructions = TagSubString(htmlRecipe, COOK_TYPE_START_TAG, COOK_TYPE_END_TAG);

            string tempItem = string.Empty;
            //theRecipe.MealType = new List<string>();

            foreach (string item in TagSubString(htmlRecipe, MEAL_TYPE_START_TAG, MEAL_TYPE_END_TAG).Split(','))
            {
                tempItem = item.Replace("<!--", string.Empty);
                tempItem = tempItem.Replace("-->", string.Empty);
                theRecipe.MealType.Add(tempItem.Trim());
            }
            return theRecipe;
        }

        private IRecipeSectionPart<Ingredient>[] ConvertIngredients(string oldString)
        {
            IRecipeSectionPart<Ingredient>[] ingredients = new RecipeIngredientPart[Recipe.INGREDIENTS_SET_MAX_SIZE];

            //Split on </li><li></li><li> to get sections
            string[] sectionArr = oldString.Split(new string[] { "</li><li></li><li>" }, StringSplitOptions.None);
            string[] ingredientArr, ingredientItemArr;
            int counter, quantCounter;
            string quantity, item;

            for (int i=0; i<sectionArr.Length; i++)
            {
                //Replace </li><li> with other seperator so we can remove extra tags
                //Remove remaining <li> and </li> tags and any \t chars
                //Split on other seperator to get items within section
                sectionArr[i] = sectionArr[i].Replace("</li><li>", "^^^");
                sectionArr[i] = sectionArr[i].Replace("<li>", string.Empty);
                sectionArr[i] = sectionArr[i].Replace("</li>", string.Empty);
                sectionArr[i] = sectionArr[i].Replace("<b>", string.Empty);
                sectionArr[i] = sectionArr[i].Replace("</b>", string.Empty);
                sectionArr[i] = sectionArr[i].Replace("<i>", string.Empty);
                sectionArr[i] = sectionArr[i].Replace("</i>", string.Empty);
                sectionArr[i] = sectionArr[i].Replace(@"\t", string.Empty);
                ingredientArr = sectionArr[i].Split(new string[] { "^^^" }, StringSplitOptions.None);
                ingredients[i] = new RecipeIngredientPart();

                if (sectionArr.Length > 1)
                {
                    //If first item in array starts with a number, then header is string.empty, otherwise first item is header
                    ingredients[i].SectionHeader = Char.IsDigit(ingredientArr[0][0]) ? string.Empty : ingredientArr[0];
                }

                counter = string.IsNullOrWhiteSpace(ingredients[i].SectionHeader) ? 0 : 1;
                for (; counter < ingredientArr.Length; counter++)
                {
                    //Parse old ingredient line into seperate quantity, item

                    //split on space, go through each arr item until not start with number, take next item as well
                    //and that should be quantity. the rest will be the item.

                    ingredientItemArr = ingredientArr[counter].Split(' ');
                    quantity = string.Empty;
                    item = string.Empty;
                    quantCounter = 0;

                    for (; quantCounter < ingredientItemArr.Length; quantCounter++)
                    {
                        if (Char.IsDigit(ingredientItemArr[quantCounter][0]))
                            quantity += ingredientItemArr[quantCounter] + " ";
                        else
                        {
                            //if no quantity, only an item, then move on....
                            if (!string.IsNullOrWhiteSpace(quantity))
                            {
                                quantity += ingredientItemArr[quantCounter] + " ";
                                quantCounter++;
                            }
                            break;
                        }
                    }

                    for (; quantCounter < ingredientItemArr.Length; quantCounter++)
                        item += ingredientItemArr[quantCounter] + " ";

                    if (!string.IsNullOrWhiteSpace(item))
                        item = item.Remove(item.Length - 1, 1);
                    ingredients[i].ItemList.Add(new Ingredient(quantity, item));
                }
            }
            return ingredients;
        }

        private IRecipeSectionPart<string>[] ConvertInstructions(string oldString)
        {
            IRecipeSectionPart<string>[] instructions = new RecipeInstructionPart[Recipe.INSTRUCTION_SET_MAX_SIZE];
            instructions[0] = new RecipeInstructionPart();
            oldString = oldString.Replace("</p><p>", "^^^");
            oldString = oldString.Replace(@"</p>\r\n\t\t<p>", string.Empty);
            oldString = oldString.Replace("<p>", string.Empty);
            oldString = oldString.Replace("</p>", string.Empty);
            oldString = oldString.Replace(@"\t", string.Empty);
            oldString = oldString.Replace("<b>", string.Empty);
            oldString = oldString.Replace("</b>", string.Empty);
            oldString = oldString.Replace("<i>", string.Empty);
            oldString = oldString.Replace("</i>", string.Empty);

            //Look for a number followed by a '.' and a ' ' (removes instruction counters)
            oldString = Regex.Replace(oldString, @"[0-9+]\. ", string.Empty);
            string[] instructionsArr = oldString.Split(new string[] { "^^^" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in instructionsArr)
                instructions[0].ItemList.Add(item);

            return instructions;
        }

        private string TagSubString(string text, string startTag, string endTag)
        {
            int start = text.IndexOf(startTag) + startTag.Length;
            int end = text.IndexOf(endTag);

            return text.Substring(start, end - start);
        }

        private bool ConnectToFile()
        {

            if (string.IsNullOrWhiteSpace(FilePath))
                FilePath = DEFAULT_PATH;

            if (string.IsNullOrWhiteSpace(FileName))
                FileName = DEFAULT_FILE;

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
                //RecipeColl = ParseRecipe(fileText);
                RecipeColl = new RecipeCollection();
                ParseRecipe(fileText);

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error loading file into ram: " + ex.Message);
                return false;
            }

            return true;
        }

        //        private RecipeCollection ParseRecipe(string fileText)
        private void ParseRecipe(string fileText)
        {
            int index = 0;
            int start = 0;
            int end = 0;
//            RecipeCollection recipeDict = new RecipeCollection();
            Recipe tempConcrete = null;
            bool endOfFile = false;

            while (!endOfFile)
            {
                start = fileText.IndexOf(RECIPE_START_TAG, index);
                end = fileText.IndexOf(RECIPE_END_TAG, index + 1) + RECIPE_END_TAG.Length;

                if (start >= 0 && end >= 0)
                {
                    tempConcrete = ParseStringToProps(fileText.Substring(start, end - start));
                    //                    recipeDict.Add(tempConcrete);
                    RecipeColl.Add(tempConcrete);
                    index = end;
                }
                else
                    endOfFile = true;
            }

  //          return recipeDict;
        }
    }
}