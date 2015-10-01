using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Subscriber : MonoBehaviour, ISub
{
    [SerializeField]
    GameObject prefab;
    GameObject box;
    public void Subcribe(string type, string msg, CallBacks func)
    {
        EventSystem.AddSub(type, msg, func, this);
    }

    // Use this for initialization
    void Start()
    {
        box = Instantiate(prefab) as GameObject;
        //Subs to a event msg
        Subcribe("publisher", "Up", Up);
        Subcribe("publisher", "Down", Down);
        Subcribe("publisher", "Left", Left);
        Subcribe("publisher", "Right", Right);
    }

    // Update is called once per frame
    private void Up()
    {
        box.transform.position += new Vector3(0,1,0);
    }

    private void Down()
    {
        box.transform.position += new Vector3(0, -1, 0);
    }

    private void Left()
    {
        box.transform.position += new Vector3(-1, 0, 0);
    }

    private void Right()
    {
        box.transform.position += new Vector3(1, 0, 0);      
    }
}
