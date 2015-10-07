using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CombatSystem : MonoBehaviour , IPub, ISub
{
    FSM<CombatStates> _fsm;

    [SerializeField]
    List<Players> AllyTeam;
    [SerializeField]
    List<Enemy> EnemyTeam;

    public bool test;
    public int i;
    public int j;
    static private CombatSystem Instance;
    static public CombatSystem _instance
    {
        get
        {
            return Instance;
        }
    }

    [SerializeField]
    string state;

    enum CombatStates
    {
        e_Init,
        e_ActionChoice,
        e_PerformAction,
        e_CheckForResolution,
        e_ExitCombat
    }

    void Awake()
    {
        _fsm = new FSM<CombatStates>();
        AddState();
        AddTransitions();
        Instance = this;
        Players[] allies = FindObjectsOfType<Players>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Players ally in allies)
        {
            AllyTeam.Add(ally);
        }

        foreach (Enemy en in enemies)
        {
            EnemyTeam.Add(en);
        }

        test = true;
        StartCoroutine(transition());
        Subscribe("GUI","Attacked", AttackOrder);
    }

    void Start()
    {
        AttackOrder();
    }

    IEnumerator transition()
    {
        while(WinCondition() == false)
        {
            if (WinCondition() == true)
            {
                _fsm.Transition(_fsm.state, CombatStates.e_ExitCombat);
                StopCoroutine(transition());
            }
            CheckState();

            yield return null;
        }
        Publish("Combat->" + "e_ExitCombat");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            InitToSelectAction();
        }

        if (AllyTeam[i] == null)
        {
            AllyTeam.Remove(AllyTeam[i]);
        }
        if (EnemyTeam[j] == null)
        {
            EnemyTeam.Remove(EnemyTeam[j]);
        }

        state = _fsm.state.ToString();
    }

    bool WinCondition()
    {
        if(AllyTeam.Count != 0 && EnemyTeam.Count == 0)
        {
            Debug.Log("winner: ally");
            foreach (Players p in AllyTeam)
            {
                Publish("CombatWin->" + p.name);
            }
            _fsm.Transition(_fsm.state, CombatStates.e_ExitCombat);
            StopCoroutine(transition());
            return true;
        }
        else if (EnemyTeam.Count != 0 && AllyTeam.Count == 0)
        {
            Debug.Log("winner: enemy");
            foreach (Enemy e in EnemyTeam)
            {
                Publish("CombatWin->" + e.name);
            }
            _fsm.Transition(_fsm.state, CombatStates.e_ExitCombat);
            StopCoroutine(transition());
            return true;
        }
        return false;
    }

    void CheckState()
    {
        switch (_fsm.state)
        {
            case CombatStates.e_Init:
                //_fsm.Transition(_fsm.state, CombatStates.e_ActionChoice);
                break;
            case CombatStates.e_ActionChoice:
                test = false;
                SelectAction();
                //AttackOrder();
                break;
        }
    }

    void AddState()
    {
        _fsm.AddState(CombatStates.e_Init);
        _fsm.AddState(CombatStates.e_ActionChoice);
        _fsm.AddState(CombatStates.e_PerformAction);
        _fsm.AddState(CombatStates.e_CheckForResolution);
        _fsm.AddState(CombatStates.e_ExitCombat);
    }

    void AddTransitions()
    {
        _fsm.AddTransition(CombatStates.e_Init, CombatStates.e_ActionChoice);
        _fsm.AddTransition(CombatStates.e_ActionChoice, CombatStates.e_PerformAction);
        _fsm.AddTransition(CombatStates.e_PerformAction, CombatStates.e_ActionChoice);
        _fsm.AddTransition(CombatStates.e_PerformAction,CombatStates.e_CheckForResolution);
        //This transition is to transition between each player choosing and performing there actions
        _fsm.AddTransition(CombatStates.e_ActionChoice,CombatStates.e_ExitCombat);
    }

    void SelectAction()
    {
        Publish("Combat->" + _fsm.state.ToString());
    }

    void AttackOrder()
    {
        if (_fsm.state == CombatStates.e_ActionChoice)
        {
            if (i == AllyTeam.Count)
            {
                i = 0;
            }
            else if (i <= AllyTeam.Count)
            {
                Publish("Combat->" + AllyTeam[i].name);
                i++;
                if (i == AllyTeam.Count)
                    i = 0;
            }

            if (j == EnemyTeam.Count)
            {
                j = 0;
            }
            else if (j <= EnemyTeam.Count)
            {
                Publish("Combat->" + EnemyTeam[j].name);
                j++;
                if (j == EnemyTeam.Count)
                    j = 0;
            }
        }

    }

    void InitToSelectAction()
    {
        _fsm.Transition(_fsm.state, CombatStates.e_ActionChoice);
        Publish("Combat->" + "e_ActionSelect");
    }

    void ActionSelected()
    {
        _fsm.Transition(_fsm.state, CombatStates.e_PerformAction);
        InitToSelectAction();
    }

    public void Publish(string msg)
    {
        EventSystem.Notify(msg);
    }

    public void Subscribe(string type, string msg, CallBacks func)
    {
        EventSystem.AddSub(type, msg, func, this);
    }
}
