using KubillityProgrammerSimulator.Flows;
using KubillityProgrammerSimulator.Interacts;
using KubillityProgrammerSimulator.Prompts;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    public class Person : ITimeSense
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 今年以来累计薪资
        /// </summary>
        public double CumulativeSalary { get; set; }

        public SalaryScheme Salary { get; set; } = new SalaryScheme();

        public string Name { get; set; } = string.Empty;

        public int Age { get; set; } = 23;

        /// <summary>
        /// 0 休息 1 工作
        /// </summary>
        public int Status { get; set; }

        public TransactionType Transaction { get; set; }

        /// <summary>
        /// 身体健康值
        /// </summary>
        public double BodyHealthy { get; set; } = 100;

        /// <summary>
        /// 心理健康值
        /// </summary>
        public double MentalHealthy { get; set; } = 100;

        /// <summary>
        /// 每周活力值
        /// </summary>
        public double WeeklyVitality { get; set; } = 40;

        public List<Language> Languages { get; set; } = new List<Language>();

        public int Company { get; set; } = -1;

        public QualificationType Qualification { get; set; }

        /// <summary>
        /// 手中的任务
        /// </summary>
        public List<string> RunningTasks { get; set; } = new List<string>();

        /// <summary>
        /// 正在执行的任务
        /// </summary>
        public string TaskInProgress { get; set; } = string.Empty;

        /// <summary>
        /// 开始休息时间
        /// </summary>
        public TimeSpan? RestBeginTime { get; set; }

        /// <summary>
        /// 开始休息日期
        /// </summary>
        public int? RestBeginDay { get; set; }

        public void Report()
        {
            var table = new Table();
            table.Title = new TableTitle("人物当前信息");
            table.AddColumn(new TableColumn("Key").Centered())
                .AddColumn(new TableColumn("Value").Centered());

            table.AddRow("姓名", this.Name)
                .AddEmptyRow()
                .AddRow("年龄", this.Age.ToString())
                .AddEmptyRow()
                .AddRow("本周活力值", this.WeeklyVitality.ToString("0.00"))
                .AddEmptyRow()
                .AddRow("身体健康值", this.BodyHealthy.ToString("0.00"))
                .AddEmptyRow()
                .AddRow("心理健康值", this.MentalHealthy.ToString("0.00"));
            if (this.Company >= 0)
            {
                var company = Game.Instance.GetCompany(this.Company);
                table.AddEmptyRow()
                    .AddRow("公司", company!.Name)
                    .AddEmptyRow()
                    .AddRow("月薪", this.Salary.MonthlySalary.ToString("0.00"))
                    .AddEmptyRow()
                    .AddRow("今年以来累计薪资", this.CumulativeSalary.ToString("0.00"));

                if (this.RunningTasks.Any())
                {
                    var table2 = new Table();
                    table2.Title = new TableTitle("进行中的任务");
                    table2.AddColumn(new TableColumn("任务描述").Centered())
                        .AddColumn(new TableColumn("进度").Centered());
                    foreach (var task in this.RunningTasks)
                    {
                        var t = company.GetRunningTask(task);
                        table2.AddRow(t.Description, t.Progress.ToString("P"))
                            .AddEmptyRow();
                    }
                    table.AddEmptyRow()
                        .AddRow(new Markup("任务"), table2);
                }
            }

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// 在接下来一段时间内，会有事件发生的时间点
        /// </summary>
        /// <param name="minutes">分钟数</param>
        /// <returns>当前时刻到事件发生时间点的分钟数</returns>
        public double GetKeyTime(double minutes)
        {
            // 目前人物没有设计什么主动事件
            return minutes;
        }

        public void TimePassed()
        {
            if (Time.Instance.Current == new TimeSpan(0, 0, 0)) // 0 点
            {
                // 48周算一年，49周第一个工作日初始化今年以来累计薪资
                if (Time.Instance.Week % 48 == 0)
                {
                    AnsiConsole.MarkupLine($"[green]{this.Name}：新年新气象，累计收入清零...[/]");
                    this.CumulativeSalary = 0;
                    this.Age++;
                }

                #region 状态结算
                if (Time.Instance.WeekDay == 0)
                {
                    if (this.WeeklyVitality < 0)
                    {
                        // 疲惫值
                        var fatigue = -0.5 * this.WeeklyVitality;
                        this.BodyHealthy -= fatigue;
                        // 沮丧值
                        var depression = -0.009 * Math.Pow(fatigue, 2) + 0.321 * fatigue - 0.3823; // -0.009x^2+0.321x-0.3823
                        this.MentalHealthy -= depression;
                        if (this.MentalHealthy > 100)
                        {
                            this.MentalHealthy = 100;
                        }
                    }

                    if (this.BodyHealthy <= 0)
                    {
                        new GameOverInteract(2).Invoke();
                    }
                    else if (this.BodyHealthy <= 50)
                    {
                        var key = "LowBodyHealthy50";
                        if (this.BodyHealthy <= 20)
                        {
                            key = "LowBodyHealthy20";
                        }
                        else if (this.BodyHealthy <= 10)
                        {
                            key = "LowBodyHealthy10";
                        }
                        var aside = Game.Instance.GetAside(key);
                        if (aside != null)
                        {
                            var flow = new MultilineTextFlow(500);
                            foreach (var text in aside.Parse())
                            {
                                flow.AddText(text);
                            }
                            flow.Show();
                        }
                        if (this.BodyHealthy <= 20)
                        {
                            var answer = new OptionPrompt("是否选择离职？", 8).Ask("是", "否");
                            if (answer == "是")
                            {
                                new GameOverInteract(1).Invoke();
                            }
                        }
                    }

                    if (this.MentalHealthy <= 0)
                    {
                        new GameOverInteract(3).Invoke();
                    }
                    else if (this.MentalHealthy <= 20)
                    {
                        var key = "LowMentalHealthy20";
                        if (this.MentalHealthy <= 10)
                        {
                            key = "LowMentalHealthy10";
                        }
                        var aside = Game.Instance.GetAside(key);
                        if (aside != null)
                        {
                            var flow = new MultilineTextFlow(500);
                            foreach (var text in aside.Parse())
                            {
                                flow.AddText(text);
                            }
                            flow.Show();
                        }
                        if (this.MentalHealthy <= 10)
                        {
                            var answer = new OptionPrompt("是否选择离职？", 8).Ask("是", "否");
                            if (answer == "是")
                            {
                                new GameOverInteract(1).Invoke();
                            }
                        }
                    }

                    // 重置人物状态
                    this.WeeklyVitality = 40;
                }
                #endregion
            }

            if (this.Status == 0 && this.RestBeginTime == null) // 刚开始休息
            {
                this.RestBeginTime = Time.Instance.Current;
                this.RestBeginDay = Time.Instance.Day;
            }
            if (this.Status == 1 && this.RestBeginTime != null) // 刚开始上班
            {
                // 计算休息获得的恢复值
                var restHours = (Time.Instance.Current - this.RestBeginTime)!.Value.TotalHours + (Time.Instance.Day - this.RestBeginDay) * 24;
                this.WeeklyVitality += 0.35 * restHours!.Value; // 休息一小时恢复 0.35 活力值
                if (this.WeeklyVitality > 40)
                {
                    this.WeeklyVitality = 40;
                }

                this.RestBeginTime = null;
            }

            var company = Game.Instance.GetCompany(this.Company);
            var endTime = company!.WorkTimes.OrderBy(t => t.End).Last().End; // 下班时间点
            if (Time.Instance.Current == endTime) // 刚下班
            {
                AnsiConsole.MarkupLine($"[green]{this.Name}：下班啦，我免费了~~~[/]");
            }

            if (this.Status == 0)
            {
                AnsiConsole.MarkupLine($"[green]{this.Name}：正在休息...[/]");
                return;
            }

            if (this.Status == 1)
            {
                new WorkInteract().Invoke();
            }
        }

        public void CheckGoalAchieve()
        {
            AnsiConsole.MarkupLine($"[yellow]{this.Name}：收入结算中...今年能通关嘛？[/]");
            if (this.CumulativeSalary >= 500000)
            {
                new GameOverInteract(4).Invoke();
            }
            else
            {
                new MultilineTextFlow(500)
                    .AddText($"[grey]{this.Name}：似乎收入还差一丢丢...[/]")
                    .AddText($"[grey]{this.Name}：明年继续努力吧...[/]")
                    .AddText($"[grey]{this.Name}：加油！只要干不死，就往死里干，目标总能达到的，[[手动滑稽]][/]")
                    .Show();
            }
        }

        public bool MatchConditions(List<Condition> conditions)
        {
            foreach (var condition in conditions)
            {
                switch (condition.Key)
                {
                    case "Age":
                        if (!int.TryParse(condition.Value, out var age))
                        {
                            continue;
                        }

                        switch (condition.Type)
                        {
                            case ConditionType.Gt:
                                if (this.Age <= age)
                                {
                                    return false;
                                }
                                break;
                            case ConditionType.Gte:
                                if (this.Age < age)
                                {
                                    return false;
                                }
                                break;
                            case ConditionType.Lt:
                                if (this.Age >= age)
                                {
                                    return false;
                                }
                                break;
                            case ConditionType.Lte:
                                if (this.Age > age)
                                {
                                    return false;
                                }
                                break;
                            case ConditionType.Eq:
                                if (this.Age != age)
                                {
                                    return false;
                                }
                                break;
                        }
                        break;
                }
            }

            return true;
        }

        public void BeAffected(List<Influence> influences)
        {
            foreach (var influence in influences)
            {
                if (influence.ObjectType != 0)
                {
                    continue;
                }

                switch (influence.Key)
                {
                    case "CumulativeSalary":
                        if (!double.TryParse(influence.Value, out var num))
                        {
                            continue;
                        }

                        switch (influence.Type)
                        {
                            case InfluenceType.Enhance:
                                this.CumulativeSalary += num;
                                break;
                            case InfluenceType.PercentageEnhance:
                                this.CumulativeSalary *= 1 + num;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
