using KubillityProgrammerSimulator.Prompts;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class LoadRecordInteract
    {
        private OptionPrompt optionPrompt = new OptionPrompt("请选择存档：", 8);

        public void Invoke()
        {
            var choices = Game.Instance.GetRecords().ToList();
            if (!choices.Any())
            {
                AnsiConsole.MarkupLine("当前无任何存档，请先创建新游戏...");
                new NewGameInteract().Invoke();
                return;
            }

            while (true)
            {
                var record = this.optionPrompt.Ask(choices.ToArray());
                if (Game.Instance.LoadRecord(record))
                {
                    new ResumeInteract().Invoke();
                    return;
                }
            }
        }
    }
}
