using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// Main entry point - Kept minimal as per assignment requirements
    /// All logic is encapsulated in MenuManager and other classes
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            MenuManager menu = new MenuManager();
            menu.Run();
        }
    }
}
