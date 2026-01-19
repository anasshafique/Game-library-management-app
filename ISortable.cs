using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// Interface for sortable collections
    /// Demonstrates use of interfaces as per assignment requirements
    /// </summary>
    public interface ISortable
    {
        List<Player> SortByHours();
        List<Player> SortByScore();
    }
}
