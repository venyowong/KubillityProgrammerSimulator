using KubillityProgrammerSimulator.Models;
using KubillityProgrammerSimulator.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class NameInteract
    {
        private StringPrompt prompt = new StringPrompt(5, 20);

        public void Invoke()
        {
            var name = this.prompt.Ask("请输入角色昵称：");
            var lead = Game.Instance.GetLead();
            lead!.Name = name;
            new LanguageInteract().Invoke();
        }
    }
}
