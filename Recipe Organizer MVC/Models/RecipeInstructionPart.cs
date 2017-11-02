using System;
using System.Collections.Generic;
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

        public RecipeInstructionPart(string instructions, string delimiter) : this()
        {
            if (!string.IsNullOrWhiteSpace(instructions))
            {
                string[] insArr = instructions.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                int counter = instructions.StartsWith(delimiter) ? 0 : 1;
                SectionHeader = counter == 1 ? insArr[0] : string.Empty;

                for (; counter < insArr.Length; counter++)
                    ItemList.Add(insArr[counter].Trim());
            }
        }

        public string ToStringDelimited(string delimiter)
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