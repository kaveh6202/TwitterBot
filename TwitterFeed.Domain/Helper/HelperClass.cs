namespace TwitterFeed.Infra.Helper;

public static class HelperClass
{
    public static string GetRelativeTimeInFarsi(this TimeSpan ts)
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        double delta = Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "یک ثانیه پیش" : ts.Seconds + " ثانیه پیش";

        if (delta < 2 * MINUTE)
            return "یک دقیقه پیش";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " دقیقه پیش";

        if (delta < 90 * MINUTE)
            return "یک ساعت پیش";

        if (delta < 24 * HOUR)
            return ts.Hours + " ساعت پیش";

        if (delta < 48 * HOUR)
            return "دیروز";

        if (delta < 30 * DAY)
            return ts.Days + " روز قبل";

        if (delta < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "یک ماه پیش" : months + " ماه پیش";
        }
        else
        {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "یک سال پیش" : years + " سال پیش";
        }
    }
    
}