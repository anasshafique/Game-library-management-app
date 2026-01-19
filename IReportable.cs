using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// Interface for report generation
    /// Demonstrates use of interfaces as per assignment requirements
    /// </summary>
    public interface IReportable
    {
        void GenerateTopScoresReport(int count);
        void GenerateMostActivePlayersReport(int count);
    }
}
