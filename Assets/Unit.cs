using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour
{
    FSM<UnitStates> _fsm;

    static private Unit Instance;
    static public  Unit _instance
    {
        get
        {
            return Instance;
        }
    }

    public string getState()
    {
        return _fsm.state.ToString();
    }

    enum UnitStates
    {
        e_Init,
        e_Idle,
        e_Combat,
        e_Victor, 
        e_Dead
    }

    void Awake()
    {
        _fsm = new FSM<UnitStates>();
        AddState();
        AddTransitions();
        Instance = this;
    }

    void Start()
    {

    }

    void AddState()
    {
        _fsm.AddState(UnitStates.e_Init);
        _fsm.AddState(UnitStates.e_Idle);
        _fsm.AddState(UnitStates.e_Combat);
        _fsm.AddState(UnitStates.e_Dead);
        _fsm.AddState(UnitStates.e_Victor);
    }

    void AddTransitions()
    {
        _fsm.AddTransition(UnitStates.e_Init, UnitStates.e_Idle);
        _fsm.AddTransition(UnitStates.e_Idle, UnitStates.e_Combat);
        _fsm.AddTransition(UnitStates.e_Idle, UnitStates.e_Dead);
        _fsm.AddTransition(UnitStates.e_Combat, UnitStates.e_Idle);
        _fsm.AddTransition(UnitStates.e_Combat, UnitStates.e_Dead);
        _fsm.AddTransition(UnitStates.e_Combat, UnitStates.e_Victor);
        _fsm.AddTransition(UnitStates.e_Idle, UnitStates.e_Victor);
    }

    public void ToCombat()
    {
        _fsm.Transition(_fsm.state, UnitStates.e_Combat);
    }

    public void toIdle()

    {
        _fsm.Transition(_fsm.state, UnitStates.e_Idle);
    }

    public void ToVictory()
    {
        _fsm.Transition(_fsm.state, UnitStates.e_Victor);
    }

    public void ToDead()
    {
        _fsm.Transition(_fsm.state, UnitStates.e_Dead);
        Destroy(this.gameObject);
    }
}
