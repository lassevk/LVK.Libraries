namespace LVK.EntityFramework.PostgreSQL;

public interface IPostgreSqlNotifications
{
    IDisposable Listen<T>(string channel, Action<T> handler);
}