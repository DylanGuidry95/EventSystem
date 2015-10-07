using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GUI : MonoBehaviour ,IPub, ISub
{
    public void Publish(string msg)
    {
        EventSystem.Notify(msg);
    }

    // Use this for initialization
    void Awake()
    {
        DeactivateButtons();
        Subscribe("Combat", "e_ActionChoice", DisplayButton);
        Subscribe("Combat", "e_ExitCombat", TurnOffButton);
    }

    void Start()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void ButtonMessage()
    {
        Publish("GUI->" + gameObject.name);
        Publish("GUI->Attacked");
    }

    void DisplayButton()
    {
            gameObject.GetComponent<Button>().interactable = true;
    }

    void TurnOffButton()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void DeactivateButtons()
    {
            GetComponent<Button>().onClick.AddListener(ButtonMessage);
            gameObject.GetComponent<Button>().interactable = false;
    }

    public void Subscribe(string type, string msg, CallBacks func)
    {
        EventSystem.AddSub(type, msg, func, this);
    }
}
