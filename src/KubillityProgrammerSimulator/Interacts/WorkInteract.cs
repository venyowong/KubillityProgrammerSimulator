using KubillityProgrammerSimulator.Models;
using KubillityProgrammerSimulator.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class WorkInteract
    {
        public void Invoke()
        {
            var option = new OptionPrompt("现在是上班时间，选择接下来要做的事：", 8)
                .Ask("发呆", "划水", "聊天", "刷网页", "看视频", "逛淘宝", "认真工作");
            switch (option)
            {
                case "发呆":
                    new StareBlanklyInteract().Invoke();
                    break;
                case "划水":
                    new PaddleInteract().Invoke();
                    break;
                case "聊天":
                    new ChatInteract().Invoke();
                    break;
                case "刷网页":
                    new SurfingInteract().Invoke();
                    break;
                case "看视频":
                    new WatchVideoInteract().Invoke();
                    break;
                case "逛淘宝":
                    new ShoppingInteract().Invoke();
                    break;
                case "认真工作":
                    new DoWorkInteract().Invoke();
                    break;
            }
        }
    }
}
