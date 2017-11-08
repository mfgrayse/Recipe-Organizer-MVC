using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class ExcelSearch : ISearch
    {
        public IQuery Query { get; set; }
        public RecipeCollection TheRecipeCollection { get; set; }
        public Recipe SelectedRecipe { get; set; }

        public ExcelSearch() : this(new ExcelQuery()) { }

        public ExcelSearch(ExcelQuery query)
        {
            Query = query;
        }
    }
}