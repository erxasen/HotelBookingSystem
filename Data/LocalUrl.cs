namespace HotelBookingSystem.Data;

public static class LocalUrl
{
    /// <summary>
    /// Защита от open redirect: пропускает только локальные адреса вида "/path",
    /// всё остальное заменяет на "/".
    /// </summary>
    public static string Sanitize(string? url)
    {
        if (string.IsNullOrEmpty(url) || url[0] != '/')
            return "/";

        if (url.Length > 1 && (url[1] == '/' || url[1] == '\\'))
            return "/";

        return url;
    }
}
