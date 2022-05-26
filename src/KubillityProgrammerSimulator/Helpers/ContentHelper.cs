using KubillityProgrammerSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator.Helpers
{
    public static class ContentHelper
    {
        public static string MakeUpStory4Qualification(QualificationType qualification)
        {
            List<string>? stories = null;
            switch (qualification)
            {
                case QualificationType.HighSchool:
                    stories = Game.Instance.GetAside("HighSchoolQualification")?.Parse().ToList();
                    break;
                case QualificationType.Undergraduate2:
                    stories = Game.Instance.GetAside("Undergraduate2Qualification")?.Parse().ToList();
                    break;
                case QualificationType.Undergraduate:
                    stories = Game.Instance.GetAside("UndergraduateQualification")?.Parse().ToList();
                    break;
                case QualificationType.Plan211:
                    stories = Game.Instance.GetAside("Plan211Qualification")?.Parse().ToList();
                    break;
                case QualificationType.Plan985:
                    stories = Game.Instance.GetAside("Plan985Qualification")?.Parse().ToList();
                    break;
                case QualificationType.Master:
                    stories = Game.Instance.GetAside("MasterQualification")?.Parse().ToList();
                    break;
                case QualificationType.Double985Master:
                    stories = Game.Instance.GetAside("Double985MasterQualification")?.Parse().ToList();
                    break;
                case QualificationType.Doctor:
                    stories = Game.Instance.GetAside("DoctorQualification")?.Parse().ToList();
                    break;
            }
            if (stories == null)
            {
                return string.Empty;
            }

            var random = new Random((int)DateTime.Now.Ticks);
            return stories[random.Next(stories.Count)];
        }

        public static string GetLowQualificationContent()
        {
            var aside = Game.Instance.GetAside("LowQualification")!.Parse().ToList();
            var random = new Random((int)DateTime.Now.Ticks);
            return aside[random.Next(aside.Count)];
        }
    }
}
