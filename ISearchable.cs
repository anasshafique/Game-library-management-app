using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// Interface for searchable collections
    /// Demonstrates use of interfaces as per assignment requirements
    /// </summary>
    public interface ISearchable
    {
        Player SearchById(int id);
        Player SearchByUsername(string username);
    }
}
