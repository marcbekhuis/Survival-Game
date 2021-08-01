using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public delegate void StateCall(State state);
    public event StateCall OnStart;
    public event StateCall OnUpdate;
    public event StateCall OnEnd;

    public StateMachine stateMachine;

    [SerializeField] private Behaviour[] activeBehaviours;
    [SerializeField] private GameObject[] activeGameObjects;
    [SerializeField] private State[] nextStates;

    public virtual void StartState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        foreach (var activeGameObject in activeGameObjects)
        {
            activeGameObject.SetActive(true);
        }

        foreach (var activeBehaviour in activeBehaviours)
        {
            activeBehaviour.enabled = true;
        }

        OnStart.Invoke(this);
    }

    public virtual void EndState()
    {
        foreach (var activeGameObject in activeGameObjects)
        {
            activeGameObject.SetActive(false);
        }

        foreach (var activeBehaviour in activeBehaviours)
        {
            activeBehaviour.enabled = false;
        }

        OnEnd.Invoke(this);
    }

    public virtual void UpdateState()
    {
        OnUpdate.Invoke(this);
        foreach (var nextState in nextStates)
        {
            if (!nextState.ActivateCondition()) continue;
            nextState.StartState(stateMachine);
            EndState();
            return;
        }
    }

    public abstract bool ActivateCondition();
}
