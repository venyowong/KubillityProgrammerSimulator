using KubillityProgrammerSimulator.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class GameOverInteract
    {
        private string result;
        private int type;

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="type">0 提示结束 1 辞职 2 猝死 3 自杀 4 通关</param>
        /// <param name="result"></param>
        public GameOverInteract(int type, string result = "")
        {
            this.type = type;
            this.result = result;
        }

        public void Invoke()
        {
            var lead = Game.Instance.GetLead();
            lead!.Report();
            if (!string.IsNullOrWhiteSpace(this.result))
            {
                if (this.type < 4)
                {
                    AnsiConsole.MarkupLine($"[red]{this.result}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[green]{this.result}[/]");
                }
            }
        }
    }
}
