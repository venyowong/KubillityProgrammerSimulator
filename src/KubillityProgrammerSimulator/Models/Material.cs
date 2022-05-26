using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    /// <summary>
    /// 素材
    /// </summary>
    public class Material
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;
    }
}
