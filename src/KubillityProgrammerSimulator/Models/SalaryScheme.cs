using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    /// <summary>
    /// 薪资方案
    /// </summary>
    public class SalaryScheme
    {
        public QualificationType Qualification { get; set; }

        public double QuarterlyBonus { get; set; }

        public double AnnualBonus { get; set; }

        public double MonthlySalary { get; set; }
    }
}
