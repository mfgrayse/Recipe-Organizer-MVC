using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recipe_Organizer_MVC.Models
{
    public class Ingredient
    {
        public const string QUANTITY_SEPERATOR = "<IngAmount>";

        public string Quantity { get; set; }
        public string Item { get; set; }

        public Ingredient() { }
        public Ingredient(string quantity, string item)
        {
            Quantity = quantity.Trim();
            Item = item.Trim();
        }

        public Ingredient(string ingredientString)
        {
            string[] tempArr = ingredientString.Split(new string[] { QUANTITY_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);

            if (tempArr.Length > 1)
            {
                Quantity = tempArr[0].Trim();
                Item = tempArr[1].Trim();
            }
            else
            {
                Quantity = string.Empty;
                Item = ingredientString.Trim();
            }

            if (Item.Contains(QUANTITY_SEPERATOR))
                Item = Item.Replace(QUANTITY_SEPERATOR, string.Empty);
        }

        public string StringWithSeperator()
        {
            return string.Format("{0}{1} {2}", Quantity, QUANTITY_SEPERATOR, Item);
        }

        public override string ToString()
        {
            return Quantity + " " + Item;
        }
    }
}