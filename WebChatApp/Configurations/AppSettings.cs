public class AppSettings
{
    public LoggingSettings Logging { get; set; }
    public string AllowedHosts { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class LoggingSettings
{
    public LogLevelSettings LogLevel { get; set; }
}

public class LogLevelSettings
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}
