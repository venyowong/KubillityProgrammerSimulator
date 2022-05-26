using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Flows
{
    public class OnceTextFlow : IFlow
    {
        private string text;

        public OnceTextFlow(string text = "")
        {
            this.text = text;
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
            AnsiConsole.MarkupLine(this.text);
        }
    }
}
