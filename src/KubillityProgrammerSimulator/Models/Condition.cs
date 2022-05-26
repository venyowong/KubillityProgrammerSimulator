using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    public class Condition
    {
        public ConditionType Type { get; set; }

        public string Key { get; set; } = string.Empty;
        
        public string Value { get; set; } = string.Empty;
    }
}
