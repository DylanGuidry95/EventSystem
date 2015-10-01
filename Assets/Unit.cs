using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour, IPub
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

    enum UnitStates
    {
        e_Init,
        e_Idle,
        e_Combat, 
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
    }

    void AddTransitions()
    {
        _fsm.AddTransition(UnitStates.e_Init, UnitStates.e_Idle);
        _fsm.AddTransition(UnitStates.e_Idle, UnitStates.e_Combat);
        _fsm.AddTransition(UnitStates.e_Idle, UnitStates.e_Dead);
        _fsm.AddTransition(UnitStates.e_Combat, UnitStates.e_Idle);
        _fsm.AddTransition(UnitStates.e_Combat, UnitStates.e_Dead);
    }

    string prevState;
    void Update()
    {
        CheckState();
    }

    delegate void Transition();
    Transition _transition;

    void CheckState()
    {
        switch(_fsm.state)
        {
            case UnitStates.e_Init:
                new WaitForSeconds(2f);
                _fsm.Transition(_fsm.state, UnitStates.e_Idle);
                break;

            case UnitStates.e_Idle:
                Publish("Unit->" + UnitStates.e_Idle);
                new WaitForSeconds(2f);
                _fsm.Transition(_fsm.state, UnitStates.e_Combat);
                break;

            case UnitStates.e_Combat:
                Publish("Unit->" + UnitStates.e_Combat);
                break;

            case UnitStates.e_Dead:
                break;

            default:
                break;
        }
    }

    public void Publish(string msg)
    {
        EventSystem.Notify(msg);
    }
}
