using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubillityProgrammerSimulator
{
    public interface ITimeSense
    {
        /// <summary>
        /// 在接下来一段时间内，会有事件发生的时间点
        /// </summary>
        /// <param name="minutes">分钟数</param>
        /// <returns>当前时刻到事件发生时间点的分钟数</returns>
        double GetKeyTime(double minutes);

        /// <summary>
        /// 时间线前推后通知状态更新
        /// </summary>
        void TimePassed();
    }
}
