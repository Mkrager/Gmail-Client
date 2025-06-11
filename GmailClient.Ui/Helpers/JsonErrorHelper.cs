using System.Text.Json;

namespace GmailClient.Ui.Helpers
{
    public static class JsonErrorHelper
    {
        public static string? GetErrorMessage(string json)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("error", out var errorProp))
                {
                    return errorProp.ToString();
                }
                else if (doc.RootElement.TryGetProperty("message", out var messageProp))
                {
                    return messageProp.GetString();
                }
                else if (doc.RootElement.TryGetProperty("detail", out var detailProp))
                {
                    return detailProp.GetString();
                }
                else
                {
                    return json;
                }
            }
            catch
            {
                return json;
            }
        }
    }
}
