using KubillityProgrammerSimulator.Models;
using KubillityProgrammerSimulator.Prompts;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Interacts
{
    public class InductionInteract
    {
        private OptionPrompt optionPrompt = new OptionPrompt("请选择希望入职的公司，输入公司 Id：", 8);

        public void Invoke()
        {
            AnsiConsole.MarkupLine("正在挑选候选公司...");
            List<Company> companies = Game.Instance.CompanyTemplates.OrderBy(c => c.Id).ToList();
            var random = new Random((int)DateTime.Now.Ticks);
            while (companies.Count > 10)
            {
                var i = random.Next(companies.Count);
                companies.RemoveAt(i);
            }

            var name = string.Empty;
            while (string.IsNullOrWhiteSpace(name))
            {
                if (!companies.Any())
                {
                    new GameOverInteract(0, "入职失败...").Invoke();
                    return;
                }

                companies.ForEach(c =>
                {
                    c.ShowInfo();
                    Thread.Sleep(5000 / companies.Count);
                });
                name = this.optionPrompt.Ask(companies.Select(c => c.Name).ToArray());
                var company = companies.First(c => c.Name == name).Clone();
                if (!company.Recruit(Game.Instance.GetLead()))
                {
                    companies = companies.FindAll(c => c.Id != company.Id);
                    name = string.Empty;
                }
                else
                {
                    Game.Instance.AddCompany(company);
                }
            }

            new GoalInteract().Invoke();
        }
    }
}
