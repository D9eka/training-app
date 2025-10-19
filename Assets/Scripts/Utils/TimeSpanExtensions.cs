using System;

public static class TimeSpanExtensions
{
    public static string ToRussianFormattedString(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds < 1)
            return "менее секунды";

        if (timeSpan.TotalDays >= 1)
            return FormatTimeSpan(timeSpan.Days, "день", "дня", "дней");
        if (timeSpan.TotalHours >= 1)
            return FormatTimeSpan(timeSpan.Hours, "час", "часа", "часов");
        if (timeSpan.TotalMinutes >= 1)
            return FormatTimeSpan(timeSpan.Minutes, "минута", "минуты", "минут");
        return FormatTimeSpan(timeSpan.Seconds, "секунда", "секунды", "секунд");
    }

    private static string FormatTimeSpan(int value, string singular, string few, string many)
    {
        int lastDigit = value % 10;
        int lastTwoDigits = value % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
            return $"{value} {many}";
        if (lastDigit == 1)
            return $"{value} {singular}";
        if (lastDigit >= 2 && lastDigit <= 4)
            return $"{value} {few}";
        return $"{value} {many}";
    }
}