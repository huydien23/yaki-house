namespace Yakihouse.Api.Configuration;

/// <summary>
/// CORS configuration settings
/// </summary>
public class CorsSettings
{
    public const string SectionName = "Cors";

    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
    public bool AllowCredentials { get; set; } = true;
}

/// <summary>
/// SignalR configuration settings
/// </summary>
public class SignalRSettings
{
    public const string SectionName = "SignalR";

    public int KeepAliveIntervalInSeconds { get; set; } = 15;
    public int ClientTimeoutIntervalInSeconds { get; set; } = 30;
    public int MaximumReceiveMessageSize { get; set; } = 32 * 1024; // 32 KB
}

/// <summary>
/// Database configuration settings
/// </summary>
public class DatabaseSettings
{
    public const string SectionName = "Database";

    public int CommandTimeoutInSeconds { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public bool EnableDetailedErrors { get; set; } = false;
    public int MaxRetryCount { get; set; } = 3;
    public int MaxRetryDelayInSeconds { get; set; } = 30;
}
