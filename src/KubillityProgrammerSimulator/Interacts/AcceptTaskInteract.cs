using KubillityProgrammerSimulator.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interact
{
    public class AcceptTaskInteract
    {
        public void Invoke(Models.Task task)
        {
            var lead = Game.Instance.GetLead();
            var company = Game.Instance.GetCompany(lead!.Company);
            var project = company!.GetRunningProject(task.Project);
            if (AnsiConsole.Confirm($"是否愿意参加 {project!.GetFullName()} 的 {task.Description}？"))
            {
                task.Developer = lead.Id;
                project.State = 2;
                lead.RunningTasks.Add(task.No);
                AnsiConsole.MarkupLine("[green]新任务已添加到待完成工作列表中，你可以在后续选择此项任务进行开发...[/]");
            }
        }
    }
}
