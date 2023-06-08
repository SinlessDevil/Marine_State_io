namespace Scripts.Infrastructure.Services.Factories
{
    public interface IStateFactory
    {
        T GetState<T>() where T : class, IExitable;
    }
}