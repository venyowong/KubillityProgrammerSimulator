using KubillityProgrammerSimulator.Flows;
using KubillityProgrammerSimulator.Helpers;
using KubillityProgrammerSimulator.Interact;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    public class Company : ITimeSense
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 运作情况
        /// </summary>
        public decimal WorkSituation { get; set; }

        public decimal TotalAsset { get; set; }

        /// <summary>
        /// 员工规模
        /// </summary>
        public decimal StaffScale { get; set; }

        public ManageWay ManageWay { get; set; }

        public List<SalaryScheme> SalarySchemes { get; set; } = new List<SalaryScheme>();

        public QualificationType QualificationThreshold { get; set; }

        /// <summary>
        /// 工作时间段
        /// </summary>
        public List<WorkTimeZone> WorkTimes { get; set; } = new List<WorkTimeZone>();

        /// <summary>
        /// 休息模式
        /// </summary>
        public WeekEndMode WeekEndMode { get; set; }

        /// <summary>
        /// 本周最后一个工作日
        /// </summary>
        public int LastWorkDay { get; private set; } = 4;

        /// <summary>
        /// 项目模板
        /// </summary>
        public List<Project> Projects { get; set; } = new List<Project>();

        public List<Project> RunningProjects { get; set; } = new List<Project>();

        public List<Project> FinishedProjects { get; set; } = new List<Project>();

        public List<string> Staffs { get; set; } = new List<string>();

        public string Author { get; set; } = string.Empty;

        public Company Clone()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            return new Company
            {
                Id = this.Id,
                Name = this.Name,
                WorkSituation = this.WorkSituation * (1 + Convert.ToDecimal(random.NextDouble() * 0.1 - 0.05)),
                TotalAsset = this.TotalAsset * (1 + Convert.ToDecimal(random.NextDouble() * 0.1 - 0.05)),
                StaffScale = this.StaffScale * (1 + Convert.ToDecimal(random.NextDouble() * 0.1 - 0.05)),
                ManageWay = this.ManageWay,
                SalarySchemes = this.SalarySchemes,
                QualificationThreshold = this.QualificationThreshold,
                WorkTimes = this.WorkTimes,
                WeekEndMode = this.WeekEndMode,
                Projects = this.Projects,
                Author = this.Author
            };
        }

        public bool Recruit(Person person)
        {
            if (person.Qualification < this.QualificationThreshold)
            {
                AnsiConsole.MarkupLine($"[red]{this.Name}：{ContentHelper.GetLowQualificationContent()}[/]");
                return false;
            }

            var scheme = this.SalarySchemes.FirstOrDefault(x => x.Qualification == person.Qualification);
            if (scheme == null)
            {
                AnsiConsole.MarkupLine($"[yellow]{this.Name}：您学历太高了，我们公司没有合适的岗位[/]");
                return false;
            }

            person.Company = this.Id;
            person.Salary = scheme;
            this.Staffs.Add(person.Id);
            this.StaffScale++;
            AnsiConsole.MarkupLine($"[green]{this.Name}：欢迎加入 [red]{this.Name}[/] 大家庭[/]");
            return true;
        }

        public void StartNewProject()
        {
            var project = this.RunningProjects.FirstOrDefault(x => x.State == 1);
            if (project == null)
            {
                var projects = this.Projects.FindAll(p => !this.RunningProjects.Any(x => x.No == p.No)); // 可启动项目
                if (!projects.Any())
                {
                    return;
                }

                // 启动新项目
                var random = new Random((int)DateTime.Now.Ticks);
                project = projects[random.Next(projects.Count)];
                project = project.StartNewPeriod();
                AnsiConsole.MarkupLine($"{this.Name}：启动新项目 {project.Name} 第 {project.Period} 期...");
                project.Company = this.Id;
                this.RunningProjects.Add(project);
            }

            // 招募开发人员
            foreach (var person in this.Staffs)
            {
                var p = Game.Instance.GetPerson(person);
                foreach (var task in project.Tasks)
                {
                    if (p.Languages.Contains(task.Language))
                    {
                        new AcceptTaskInteract().Invoke(task);
                    }
                }
            }
        }

        public void ShowInfo()
        {
            var table = new Table();
            table.Title = new TableTitle("公司基本信息");
            table.AddColumn(new TableColumn("Key").Centered())
                .AddColumn(new TableColumn("Value").Centered());

            table.AddRow("Id", this.Id.ToString())
                .AddEmptyRow()
                .AddRow("公司名", this.Name)
                .AddEmptyRow()
                .AddRow("员工规模", this.StaffScale.ToString())
                .AddEmptyRow();
            var table2 = new Table();
            table2.AddColumn(new TableColumn("开始时间").Centered());
            table2.AddColumn(new TableColumn("结束时间").Centered());
            foreach (var time in this.WorkTimes)
            {
                table2.AddRow(time.Begin.ToString(), time.End.ToString());
            }
            table.AddRow(new Markup("工作时间"), table2.Centered());

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// 在接下来一段时间内，会有事件发生的时间点
        /// </summary>
        /// <param name="minutes">分钟数</param>
        /// <returns>当前时刻到事件发生时间点的分钟数</returns>
        public double GetKeyTime(double minutes)
        {
            var begin = Time.Instance.Current;
            var end = begin.Add(new TimeSpan(0, (int)minutes, 0));
            var times = new List<TimeSpan>();
            this.WorkTimes.ForEach(t =>
            {
                times.Add(t.Begin);
                times.Add(t.End);
            });
            var time = times.Where(t => t > begin && t < end)
                .OrderBy(t => t)
                .FirstOrDefault();
            if (time != default)
            {
                return (int)(time - begin).TotalMinutes;
            }

            return minutes;
        }

        public void TimePassed()
        {
            var now = Time.Instance.Current;

            switch (this.WeekEndMode)
            {
                case WeekEndMode.One:
                    this.LastWorkDay = 5;
                    break;
                case WeekEndMode.Switch:
                    if (Time.Instance.Week % 2 == 0)
                    {
                        this.LastWorkDay = 5;
                    }
                    else
                    {
                        this.LastWorkDay = 4;
                    }
                    break;
                case WeekEndMode.Zero:
                    this.LastWorkDay = 6;
                    break;
            }

            if (Time.Instance.WeekDay <= this.LastWorkDay)
            {
                foreach (var time in this.WorkTimes)
                {
                    if (time.Begin == now)
                    {
                        AnsiConsole.MarkupLine($"{this.Name}：开始工作了，猪猡们~");
                        foreach (var person in this.Staffs)
                        {
                            Game.Instance.GetPerson(person)!.Status = 1;
                        }
                        break;
                    }
                    if (time.End == now)
                    {
                        foreach (var person in this.Staffs)
                        {
                            Game.Instance.GetPerson(person)!.Status = 0;
                        }
                        break;
                    }
                }
            }

            // 由于公司要赚钱，所以有资源就一定会做新项目
            this.StartNewProject();

            if (Time.Instance.WeekDay == this.LastWorkDay)
            {
                // 每4周发一次工资，第4周的最后一个工作日结算
                if (Time.Instance.Week % 4 == 3)
                {
                    AnsiConsole.MarkupLine($"[yellow]{this.Name}：发工资了发工资了，下个月给我好好干活...[/]");
                    foreach (var person in this.Staffs)
                    {
                        var p = Game.Instance.GetPerson(person);
                        p!.CumulativeSalary += p.Salary.MonthlySalary;
                    }
                }

                // 每12周发一次季度奖金，第12周的最后一个工作日结算
                if (Time.Instance.Week % 12 == 11)
                {
                    AnsiConsole.MarkupLine($"[yellow]{this.Name}：发季度奖了...[/]");
                    foreach (var person in this.Staffs)
                    {
                        var p = Game.Instance.GetPerson(person);
                        p!.CumulativeSalary += p.Salary.QuarterlyBonus;
                    }
                }

                // 每48周发一次年终奖，第48周的最后一个工作日结算
                if (Time.Instance.Week % 48 == 47)
                {
                    AnsiConsole.MarkupLine($"[green]{this.Name}：来来来，发年终奖了，你们这群懒惰的土拨鼠今年干得还算可以，狠不戳...[/]");
                    foreach (var person in this.Staffs)
                    {
                        var p = Game.Instance.GetPerson(person);
                        p!.CumulativeSalary += p.Salary.AnnualBonus;
                        p.CheckGoalAchieve();
                    }
                }

                // todo: 调薪
            }
        }

        public Task? GetRunningTask(string no)
        {
            foreach (var p in this.RunningProjects)
            {
                var task = p.Tasks.FirstOrDefault(t => t.No == no);
                if (task != null)
                {
                    return task;
                }
            }

            return null;
        }

        public Project? GetRunningProject(string no) => this.RunningProjects.FirstOrDefault(p => p.No == no);
    }
}
