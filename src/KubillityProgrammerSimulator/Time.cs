using Newtonsoft.Json.Linq;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator
{
    internal class Time
    {
        public static Time Instance { get; private set; } = new Time();
        
        public int Day { get; private set; } = 1;

        public int Week
        {
            get => (int)Math.Ceiling(this.Day / 7.0);
        }

        public int WeekDay
        {
            get => this.Day % 7;
        }

        public TimeSpan Current { get; private set; } = new TimeSpan(0, 0, 0);

        private static readonly TimeSpan _dayEnd = new TimeSpan(24, 0, 0);

        /// <summary>
        /// 时间流逝
        /// </summary>
        /// <param name="minutes">计划时间</param>
        public double Pass(double minutes)
        {
            var remainMinutes = (_dayEnd - this.Current).TotalMinutes;
            if (minutes > remainMinutes)
            {
                minutes = remainMinutes;
            }

            var time = Game.Instance.GetKeyTime(minutes);
            this.Current = this.Current.Add(new TimeSpan(0, (int)time, 0));
            if (this.Current >= _dayEnd)
            {
                AnsiConsole.Write(new Rule());
                this.Day++;
                this.Current = new TimeSpan(0, 0, 0);
            }
            return time;
        }

        public object GetObj2Save()
        {
            return new
            {
                this.Day,
                this.Current
            };
        }

        public void Resume(JToken jToken)
        {
            this.Day = jToken.Value<int>("Day");
            this.Current = TimeSpan.Parse(jToken["Current"]!.ToString());
        }
    }
}
