using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    public class Task
    {
        public string No { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Language Language { get; set; }

        /// <summary>
        /// 预计耗时(小时)
        /// </summary>
        public double EstimatedTime { get; set; }

        /// <summary>
        /// 实际耗时(小时)
        /// </summary>
        public double PassedTime { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// 前置任务
        /// </summary>
        public string? Previous { get; set; }

        public string? Developer { get; set; }

        public string Project { get; set; } = string.Empty;

        public Task GenerateRandomTask()
        {
            var task = new Task
            {
                No = this.No,
                Description = this.Description,
                Language = this.Language,
                Previous = this.Previous
            };
            var random = new Random((int)DateTime.Now.Ticks);
            task.EstimatedTime = this.EstimatedTime * (1 + random.NextDouble() * 0.1 - 0.05); // 预计耗时在模板数据的基础上 ±5% 浮动
            return task;
        }

        public double Handle(double minutes)
        {
            var remainMinutes = this.EstimatedTime * (1 - this.Progress) * 60;
            if (minutes > remainMinutes)
            {
                minutes = (int)Math.Ceiling(remainMinutes);
            }

            AnsiConsole.MarkupLine("疯狂敲代码中...");
            AnsiConsole.MarkupLine($"进行了 {minutes} 分钟 {this.Description}...");
            this.PassedTime += minutes / 60;
            this.Progress = this.PassedTime / 1.0 / this.EstimatedTime;
            if (this.Progress >= 1)
            {
                this.Progress = 1;
                var person = Game.Instance.GetPerson(this.Developer);
                person!.RunningTasks.Remove(this.No);
                var company = Game.Instance.GetCompany(person.Company);
                var project = company!.GetRunningProject(this.Project);
                if (!project!.Tasks.Any(t => t.Progress < 100))
                {
                    project.State = 3;
                    company.RunningProjects.Remove(project);
                    company.FinishedProjects.Add(project);
                }
            }
            return minutes;
        }
    }
}
