namespace ServicesRegistration.Core
{
    /// <summary>
    /// Life time supported in .NET Dependency Injected
    /// </summary>
    public enum ServiceLifetime
    {
        Transient,
        Scoped,
        Singleton
    }
}
