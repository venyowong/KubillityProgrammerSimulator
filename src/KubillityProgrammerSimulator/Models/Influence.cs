using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    /// <summary>
    /// 影响结果
    /// </summary>
    public class Influence
    {
        /// <summary>
        /// 影响对象类型 0 人物 1 公司
        /// </summary>
        public int ObjectType { get; set; }

        public InfluenceType Type { get; set; }

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
