using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Flows
{
    public class MultilineTextFlow : IFlow
    {
        /// <summary>
        /// 行与行之间展示的间隔时间，毫秒为单位
        /// </summary>
        private int interval;
        private List<IFlow> flows = new List<IFlow>();

        public MultilineTextFlow(int interval = 100)
        {
            this.interval = interval;
        }

        public IFlow AddFlow(IFlow flow)
        {
            this.flows.Add(flow);
            return this;
        }

        public IFlow AddText(string text)
        {
            this.flows.Add(new OnceTextFlow(text));
            return this;
        }

        public void Show()
        {
            foreach (var flow in this.flows)
            {
                flow.Show();
                Thread.Sleep(interval);
            }
        }
    }
}
