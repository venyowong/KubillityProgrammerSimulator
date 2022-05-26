using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Commands
{
    public class HelpCommand : ICommand
    {
        public bool Execute(string commandText)
        {
            if (commandText != "/h" && commandText != "/help")
            {
                return false;
            }

            var table = new Table();
            table.Title = new TableTitle("通用命令");
            table.AddColumn(new TableColumn("命令").Centered());
            table.AddColumn(new TableColumn("解释").Centered());

            table.AddRow("/h", "查看所有通用命令");
            table.AddRow("/help", "查看所有通用命令");
            table.AddRow("/info", "查看人物信息");
            table.AddRow("/save", "保存游戏进度");
            table.AddRow("/remove", "删除游戏存档");

            AnsiConsole.Write(table);

            return true;
        }
    }
}
