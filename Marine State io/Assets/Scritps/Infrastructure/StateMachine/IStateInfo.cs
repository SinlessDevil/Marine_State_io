using System;

namespace Scripts.Infrastructure.StateMachine
{
    public interface IStateInfo
    {
        Type StateType { get; }
        void Enter();
        void Update();
        void Exit();
    }
}