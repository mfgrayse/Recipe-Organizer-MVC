using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Organizer_MVC.Interfaces
{
    public interface IQuery
    {
        string QueryHeader { get; set; }
        string QueryFooter { get; set; }
        string WhereClause { get; set; }
        string FullQuery { get; set; }

        void AddWhere(string column, Condition condition, IList<string> value);
        string GetMappedCondition(Condition condition);
    }

    public enum Condition
    {
        GT, //Greater than
        LT, //Less than
        GTE, //Greater than or equal
        LTE, //Less than or equal
        Like, //Like
        NotLike, //Not Like
        Equal, //Equal
        NotEqual, //Not equal
        Between, //Between
        In //In
    }
}
