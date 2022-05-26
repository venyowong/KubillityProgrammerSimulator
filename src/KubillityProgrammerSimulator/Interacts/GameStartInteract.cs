using KubillityProgrammerSimulator.Prompts;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class GameStartInteract
    {
        public void Invoke()
        {
            var option = new OptionPrompt("开始游戏", 8).Ask("1. 开始新游戏", "2. 读取存档");
            switch (option)
            {
                case "1. 开始新游戏":
                    new NewGameInteract().Invoke();
                    break;
                case "2. 读取存档":
                    new LoadRecordInteract().Invoke();
                    break;
            }
        }
    }
}
