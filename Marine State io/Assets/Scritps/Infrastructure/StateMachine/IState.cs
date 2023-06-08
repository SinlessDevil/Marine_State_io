using Scripts.Infrastructure.Services.Factories;

namespace Scripts.Infrastructure.StateMachine
{
    public interface IState : IExitable
    {
        void Enter();
    }
}