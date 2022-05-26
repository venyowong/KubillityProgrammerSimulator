using KubillityProgrammerSimulator.Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class NewGameInteract
    {
        public void Invoke()
        {
            Game.Instance.New();
            new NameInteract().Invoke();
        }
    }
}
