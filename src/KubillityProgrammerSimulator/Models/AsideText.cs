using KubillityProgrammerSimulator.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    /// <summary>
    /// 旁白
    /// </summary>
    public class AsideText
    {
        public List<string> Text { get; set; } = new List<string>();

        public IEnumerable<string> Parse()
        {
            foreach (var text in this.Text)
            {
                if (!text.StartsWith('{'))
                {
                    yield return text;
                }

                var obj = text.ToObj<JObject>();
                var property = obj?.Properties().FirstOrDefault();
                if (property == null)
                {
                    continue;
                }

                if (property.Name.ToLower() == "material")
                {
                    var material = Game.Instance.GetMaterial(property.Value.Value<int>());
                    if (material != null)
                    {
                        yield return $"[yellow]{material.Text} ——{material.Author}[/]";
                    }
                }
            }
        }
    }
}
