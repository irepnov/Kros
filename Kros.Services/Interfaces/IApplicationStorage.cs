namespace Kros.Services
{
    public interface IApplicationStorage
    {
        string BaseDirectory { get; }
        
        string LogDirectory { get; }
    }
}