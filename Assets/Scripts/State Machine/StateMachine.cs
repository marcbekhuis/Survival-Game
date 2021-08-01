using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State activeState;

    [SerializeField] private State startingState;

    // Start is called before the first frame update
    void Start()
    {
        startingState.StartState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GamePaused) return;
        activeState.UpdateState();
    }
}
