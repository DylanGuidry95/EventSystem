using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GUI : MonoBehaviour ,IPub, ISub
{
    public List<Button> DaButtons;

    public void Publish(string msg)
    {
        EventSystem.Notify(msg);
    }

    // Use this for initialization
    void Awake()
    {
        foreach(Button b in DaButtons)
        {
            b.onClick.AddListener(ButtonMessage);
            b.enabled = false;
        }


    }

    void Start()
    {
        Subcribe("Unit", "e_Combat", DisplayButton);
        Subcribe("Unit", "e_Idle", DisplayButton);
    }

    void ButtonMessage()
    {
        Debug.Log("add");
        Publish("gui->" + gameObject.name);
    }

    void DisplayButton()
    {
        Debug.Log("Hit");
        foreach(Button b in DaButtons)
        {
            if (b.enabled == false)
            {
                Debug.Log("true");
                b.enabled = true;
            }

            if (b.enabled == true)
            {
                Debug.Log("false");
                b.enabled = false;
            }

        }
    }

    public void Subcribe(string type, string msg, CallBacks func)
    {
        EventSystem.AddSub(type, msg, func, this);
    }
}
