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

        public string StringWithSeperator()
        {
            return string.Format("{0}{1}{2} {3}", QUANTITY_SEPERATOR, Quantity, QUANTITY_SEPERATOR, Item);
        }

        public override string ToString()
        {
            return Quantity + " " + Item;
        }
    }
}