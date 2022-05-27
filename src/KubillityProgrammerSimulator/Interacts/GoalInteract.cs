using KubillityProgrammerSimulator.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class GoalInteract
    {
        private DoublePrompt prompt = new DoublePrompt(200000, double.MaxValue);

        public void Invoke()
        {
            var goal = this.prompt.Ask("请输入你的目标年薪：");
            var lead = Game.Instance.GetLead();
            lead!.Goal = goal;
            new TimePassInteract().Invoke();
        }
    }
}
