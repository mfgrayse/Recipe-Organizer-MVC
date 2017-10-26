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
        public const int INSTRUCTION_SET_MAX_SIZE = 5;
        public const int INGREDIENTS_SET_MAX_SIZE = 5;

        public string Title { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Array that is INGREDIENTS_SET_MAX_SIZE sets of ingredients. Each set can have any number ingredients.
        /// </summary>
        public IRecipeSectionPart<Ingredient>[] Ingredients { get; set; }

        /// <summary>
        /// Array that is INSTRUCTION_SET_MAX_SIZE sets of instructions. Each set can have any number instructions.
        /// </summary>
        public IRecipeSectionPart<string>[] Instructions { get; set; }
        public string Notes { get; set; }
        public string CookingInstructions { get; set; }
        public List<string> MealType { get; set; }
        public bool IsNew { get; set; }
        public bool IsEdited { get; set; }
        public bool IsDeleted { get; set; }

        public Recipe()
        {
            Instructions = new RecipeInstructionPart[INSTRUCTION_SET_MAX_SIZE];
            Ingredients = new RecipeIngredientPart[INGREDIENTS_SET_MAX_SIZE];
            MealType = new List<string>();
            IsNew = IsEdited = IsDeleted = false;
        }

        public Recipe(DataRow row) : this()
        {

        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}