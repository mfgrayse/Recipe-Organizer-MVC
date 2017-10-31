using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class Recipe
    {
        public string Title { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// List of sets of ingredients. Each set can have any number ingredients.
        /// </summary>
        public List<IRecipeSectionPart<Ingredient>> Ingredients { get; set; }

        /// <summary>
        /// List of sets of instructions. Each set can have any number instructions.
        /// </summary>
        public List<IRecipeSectionPart<string>> Instructions { get; set; }
        public string Notes { get; set; }
        public string CookingInstructions { get; set; }
        public List<string> MealType { get; set; }
        public bool IsNew { get; set; }
        public bool IsEdited { get; set; }
        public bool IsDeleted { get; set; }

        public Recipe()
        {
            Instructions = new List<IRecipeSectionPart<string>>();
            Ingredients = new List<IRecipeSectionPart<Ingredient>>();
            MealType = new List<string>();
            IsNew = IsEdited = IsDeleted = false;
        }

        public Recipe(DataRow row, string titleCol, 
            string descriptionCol, string cookMethodCol, string mealTypeCol, IList<string> ingredientsCols, 
            IList<string> instructionsCols, string notesCol, string delimiter) : this()
        {
            Title = row[titleCol].ToString().Trim();
            Description = row[descriptionCol].ToString().Trim();
            CookingInstructions = row[cookMethodCol].ToString().Trim();
            MealType = row[mealTypeCol].ToString().Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

            string recipePart = string.Empty;
            for (int i = 0; i < ingredientsCols.Count; i++)
            {
                recipePart = row[ingredientsCols[i]].ToString();
                if (!string.IsNullOrWhiteSpace(recipePart))
                    Ingredients.Add(new RecipeIngredientPart(recipePart, delimiter));
            }

            for (int i = 0; i < ingredientsCols.Count; i++)
            {
                recipePart = row[instructionsCols[i]].ToString();
                if (!string.IsNullOrWhiteSpace(recipePart))
                    Instructions.Add(new RecipeInstructionPart(recipePart, delimiter));
            }

            Notes = row[notesCol].ToString().Trim();
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}