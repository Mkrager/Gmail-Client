using System.Globalization;

namespace GmailClient.Ui.Helpers
{
    public static class DateHelper
    {
        public static string FormatEmailDate(string rawDate)
        {
            if (string.IsNullOrWhiteSpace(rawDate))
                return "Error";

            try
            {
                rawDate = rawDate.Replace(" (UTC)", "").Replace(" (GMT)", "").Trim();

                rawDate = System.Text.RegularExpressions.Regex.Replace(rawDate, @"\+(\d{2})(\d{2})", "+$1:$2");

                if (DateTimeOffset.TryParseExact(
                    rawDate,
                    "ddd, dd MMM yyyy HH:mm:ss zzz",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedDate))
                {
                    return parsedDate.ToLocalTime().ToString("dd.MM.yyyy");
                }

                return "Error";
            }
            catch
            {
                return "Error";
            }
        }
    }
}
