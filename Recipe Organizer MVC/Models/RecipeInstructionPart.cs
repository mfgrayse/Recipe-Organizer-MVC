using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recipe_Organizer_MVC.Interfaces;

namespace Recipe_Organizer_MVC.Models
{
    public class RecipeInstructionPart : IRecipeSectionPart<string>
    {
        public string SectionHeader { get; set; }
        public List<string> ItemList { get; set; }

        public RecipeInstructionPart()
        {
            ItemList = new List<string>();
        }

        public string StringDelimited(string delimiter)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append(SectionHeader + delimiter);
            foreach (string item in ItemList)
                builder.Append(item + delimiter);
            builder.Remove(builder.Length - delimiter.Length, delimiter.Length);

            return builder.ToString();
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(SectionHeader) ? "Blank Instruction Header" : "Section: " + SectionHeader;
        }
    }
}