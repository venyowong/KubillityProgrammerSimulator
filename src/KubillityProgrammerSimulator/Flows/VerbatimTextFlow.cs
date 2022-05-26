using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Flows
{
    public class VerbatimTextFlow : IFlow
    {
        /// <summary>
        /// 字与字之间展示的间隔时间，毫秒为单位
        /// </summary>
        private int interval;
        private string text = string.Empty;

        public VerbatimTextFlow(int interval = 100)
        {
            this.interval = interval;
        }

        public IFlow AddFlow(IFlow flow)
        {
            throw new NotImplementedException();
        }

        public IFlow AddText(string text)
        {
            this.text += text;
            return this;
        }

        public void Show()
        {
            foreach (var c in this.text)
            {
                AnsiConsole.Markup(c.ToString());
                Thread.Sleep(interval);
            }
        }
    }
}
