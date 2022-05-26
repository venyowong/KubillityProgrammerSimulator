using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Flows
{
    public interface IFlow
    {
        IFlow AddText(string text);

        IFlow AddFlow(IFlow flow);

        void Show();
    }
}
