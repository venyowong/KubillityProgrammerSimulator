using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Models
{
    public class Project
    {
        public string No { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 0 未启动 1 开发人员招募 2 进行中 3 完成
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 期数
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 预计耗时(天)
        /// </summary>
        public int EstimatedDays { get; set; }

        /// <summary>
        /// 实际耗时(天)
        /// </summary>
        public int PassedDays { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public decimal Bonus { get; set; }

        public List<Task> Tasks { get; set; } = new List<Task>();

        public int Company { get; set; } 

        /// <summary>
        /// 启动新一期项目
        /// </summary>
        /// <returns></returns>
        public Project StartNewPeriod()
        {
            this.Period++;
            var random = new Random((int)DateTime.Now.Ticks);
            var project = new Project
            {
                No = this.No,
                Name = this.Name,
                Description = this.Description,
                State = 1,
                Bonus = this.Bonus * (1 + Convert.ToDecimal(random.NextDouble() * 0.1 - 0.05)), // 项目奖金在模板数据的基础上 ±5% 浮动
                Period = this.Period
            };
            foreach (var task in this.Tasks)
            {
                var t = task.GenerateRandomTask();
                t.Project = project.No;
                project.Tasks.Add(t);
            }
            return project;
        }

        public string GetFullName() => $"{this.Name} 第 {this.Period} 期";
    }
}
