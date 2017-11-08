using Recipe_Organizer_MVC.Models;

namespace Recipe_Organizer_MVC.Interfaces
{
    public interface ISearch
    {
        IQuery Query { get; set; }
        RecipeCollection TheRecipeCollection { get; set; }
        Recipe SelectedRecipe { get; set; }
    }
}
