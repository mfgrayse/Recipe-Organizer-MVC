﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe_Organizer_MVC.Models;

namespace Recipe_Organizer_MVC.Interfaces
{
    public interface IQuery
    {
        string TextToSearchFor { get; set; }
        bool SearchInTitle { get; set; }

        /// <summary>
        /// Gets the query created from SearchText and SearchOptions
        /// </summary>
        /// <param name="queryPart">This is an optional parameter in case something needs to be added from an external class.</param>
        /// <returns></returns>
        string GetQuery(string optionalQueryPart);
    }
}
