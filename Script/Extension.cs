using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushEightProject
{
    public static class Extension
    {
        public static String TimeString(this int totalSeconds)
        {
            totalSeconds = Math.Max(totalSeconds, 0);

            var days = Math.Floor(totalSeconds / (3600.0f * 24));
            totalSeconds -= (int)(days * (3600.0f * 24));

            var hours = Math.Floor(totalSeconds / 3600.0f);
            totalSeconds -= (int)(hours * 3600.0f);

            var minutes = Math.Floor(totalSeconds / 60.0f);
            totalSeconds -= (int)(minutes * 60.0f);

            StringBuilder Builder = new StringBuilder();
            Builder.Length = 0;

            if (days > 0)
            {
                Builder.Append(days);
                Builder.Append("일 ");
            }

            if (hours > 0)
            {
                Builder.Append(hours);
                Builder.Append("시간 ");
            }

            if (minutes > 0)
            {
                Builder.Append(minutes);
                Builder.Append("분 ");
            }

            if (totalSeconds > 0)
            {
                Builder.Append(totalSeconds);
                Builder.Append("초");
            }

            return Builder.ToString();
        }
    }
}
