using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    /// <summary>
    /// 剧情
    /// </summary>
    public class Plot
    {
        public AsideText Content { get; set; } = new AsideText();

        public List<Condition> Conditions { get; set; } = new List<Condition>();

        public List<Influence> Influences { get; set; } = new List<Influence>();
    }
}
