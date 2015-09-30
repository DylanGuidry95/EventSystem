using UnityEngine;
using System.Collections;
using System;

public class Subscriber : MonoBehaviour , ISub
{
    public void Recive(string msg)
    {
        Debug.Log("Sub got the mail: " + msg);
    }

    public void Subcribe(string type, string msg)
    {
        Debug.Log(type + msg + this.name);
        Observer.AddSub(type, msg, this);
    }

    // Use this for initialization
    void Start()
    {
        //Subs to a event msg
        Subcribe("publisher", "Button");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
