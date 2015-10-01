using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//uses the singleton class created to ensure there can only be one instance of this class at any given time
public class Observer : Singleton<Observer>
{
    static Dictionary<string, ISub> Subscribers; //stores all the messages that subscribers are looking for

    protected override void Awake() //Overrides the awake the unity function
    {
        base.Awake();
        Subscribers = new Dictionary<string, ISub>();
    }

    //When a msg is published it will alert a subcriber that is subscribed to that msg if any
    static public void Notify(string msg)
    {
        instance.NotifySubs(msg); //will notify the subscriber that is sub to this msg
    }

    private void NotifySubs(string msg)
    {
        foreach(KeyValuePair<string, ISub> s in Subscribers)
        {
            if (Subscribers.ContainsKey(msg.ToString().ToLower())) //if the disctionary for subribers contains the msg
            {
                //s.Value.Recive(msg.ToString().ToLower()); // the subscriber will then recive the msg
            }
        }
    }

    //creates the subscription from combing the sting of the type of event we are looking for and the msg
    //Then passes the subscription and sub into the AddListener as arguments to be added to the dictionary
    static public void AddSub(string type, string msg, ISub sub)
    {
        string t = type.ToString();
        string m = msg. ToString();
        string subcription = t + ":" + m;
        instance.AddListener(subcription.ToLower(), sub);
    }

    /*
        Will add a message and the object that uses the ISub interface to the subscriber list
    */
    private void AddListener(string msg, ISub sub)
    {
        Subscribers.Add(msg.ToString(), sub);
    }
}
