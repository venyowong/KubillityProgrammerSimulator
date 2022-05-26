using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Commands
{
    public interface ICommand
    {
        bool Execute(string commandText);
    }
}
