using System.Collections.Generic;

namespace Recipe_Organizer_MVC.Interfaces
{
    public interface IRecipeSectionPart<T>
    {
        //Properties
        string SectionHeader { get; set; }
        List<T> ItemList { get; set; }

        //Methods
        string ToStringDelimited(string delimiter);
    }
}
