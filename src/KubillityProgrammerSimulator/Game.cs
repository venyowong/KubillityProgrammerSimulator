using KubillityProgrammerSimulator.Helpers;
using KubillityProgrammerSimulator.Models;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator
{
    public class Game : ITimeSense
    {
        public static Game Instance { get; private set; } = new Game();

        public List<Company> CompanyTemplates { get; private set; }

        private string leadKey = string.Empty;
        private Dictionary<int, Material> materials = new Dictionary<int, Material>();
        private Dictionary<string, AsideText> aside = new Dictionary<string, AsideText>();
        private Dictionary<string, Person> persons = new Dictionary<string, Person>();
        private string recordsPath = Path.Combine(Environment.CurrentDirectory, "records");
        private Dictionary<int, Company> companies = new Dictionary<int, Company>();
        private Dictionary<string, List<Plot>> plots = new Dictionary<string, List<Plot>>();

        private Game()
        {
            CompanyTemplates = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "assets/companies.json")).ToObj<List<Company>>() ?? new List<Company>();

            var materials = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "assets/materials.json")).ToObj<List<Material>>() ?? new List<Material>();
            this.materials = materials.ToDictionary(m => m.Id);
            aside = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "assets/aside.json")).ToObj<Dictionary<string, AsideText>>() ?? new Dictionary<string, AsideText>();
            plots = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "assets/plots.json")).ToObj<Dictionary<string, List<Plot>>>() ?? new Dictionary<string, List<Plot>>();
            if (!Directory.Exists(recordsPath))
            {
                Directory.CreateDirectory(recordsPath);
            }
        }

        public Material? GetMaterial(int id)
        {
            if (!materials.ContainsKey(id))
            {
                return null;
            }

            return materials[id];
        }

        public AsideText? GetAside(string key)
        {
            if (!aside.ContainsKey(key))
            {
                return null;
            }

            return aside[key];
        }

        public void New()
        {
            leadKey = Guid.NewGuid().ToString("N");
            var random = new Random((int)DateTime.Now.Ticks);
            var lead = new Person();
            lead.Id = leadKey;
            lead.Qualification = (QualificationType)random.Next(Enum.GetValues(typeof(QualificationType)).Length);
            AnsiConsole.MarkupLine(ContentHelper.MakeUpStory4Qualification(lead.Qualification));
            persons.Add(leadKey, lead);
        }

        /// <summary>
        /// 获取主角
        /// </summary>
        /// <returns></returns>
        public Person? GetLead()
        {
            if (string.IsNullOrWhiteSpace(leadKey))
            {
                return null;
            }

            return persons[leadKey];
        }

        public Person? GetPerson(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return persons[id];
        }

        public void Save(string recordName)
        {
            var record = new
            {
                LeadKey = leadKey,
                Persons = persons.Values.ToArray(),
                Companies = companies.Values.ToArray(),
                Time = Time.Instance.GetObj2Save()
            };
            File.WriteAllText(Path.Combine(recordsPath, $"{recordName}.json"), record.ToJson());
        }

        public void RemoveRecord(string recordName)
        {
            File.Delete(Path.Combine(recordsPath, $"{recordName}.json"));
        }

        /// <summary>
        /// 获取所有存档
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetRecords() => new DirectoryInfo(recordsPath)
            .GetFiles().Select(f => f.Name.Substring(0, f.Name.Length - 5));

        public bool LoadRecord(string recordName)
        {
            var record = File.ReadAllText(Path.Combine(recordsPath, $"{recordName}.json")).ToObj<JObject>();
            if (record == null)
            {
                AnsiConsole.MarkupLine("[red]存档已损坏[/]");
                return false;
            }

            try
            {
                leadKey = record["LeadKey"]!.ToString();
                persons = record["Persons"]!.Select(x => x.ToObject<Person>() ?? new Person()).ToDictionary(x => x.Id);
                Time.Instance.Resume(record["Time"]);
                var cs = record.Property("Companies");
                if (cs != null)
                {
                    companies = cs.Value.Select(x => x.ToObject<Company>() ?? new Company()).ToDictionary(x => x.Id);
                }
                return true;
            }
            catch
            {
                AnsiConsole.MarkupLine("[red]存档已损坏[/]");
                return false;
            }
        }

        public void AddCompany(Company company)
        {
            if (!this.companies.ContainsKey(company.Id))
            {
                this.companies.Add(company.Id, company);
            }
        }

        public Company? GetCompany(int id)
        {
            if (!companies.ContainsKey(id))
            {
                return null;
            }

            return companies[id];
        }

        public double GetKeyTime(double minutes)
        {
            foreach (var person in persons.Values)
            {
                var time = person.GetKeyTime(minutes);
                if (time < minutes)
                {
                    minutes = time;
                }
            }
            foreach (var company in companies.Values)
            {
                var time = company.GetKeyTime(minutes);
                if (time < minutes)
                {
                    minutes = time;
                }
            }
            return minutes;
        }

        public void TimePassed()
        {
            foreach (var company in companies.Values)
            {
                company.TimePassed();
            }
            foreach (var person in persons.Values)
            {
                person.TimePassed();
            }
        }

        public Plot? GetPlot(string key)
        {
            if (!this.plots.ContainsKey(key))
            {
                return null;
            }

            var random = new Random((int)DateTime.Now.Ticks);
            var plots = this.plots[key];
            if (!plots.Any())
            {
                return null;
            }

            return plots[random.Next(plots.Count)];
        }
    }
}
