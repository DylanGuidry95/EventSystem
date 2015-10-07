using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventSystem : Singleton<EventSystem>
{
    /*
        This Listener class is only avialable with in the scope of the EventSystem class
    */
    class Listener
    {
        public CallBacks m_function; //creates and instance ofthe callbacks function in the Event Sytem
        public string EventMsg; //Msg that we subscribe to
        public ISub sub; //Object that is using the Sub interface
        public Listener() { } //default constructor
        public Listener(string eMsg, CallBacks func, ISub subscriber) //constructor with arguments
        {
            this.EventMsg = eMsg;
            this.sub = subscriber;
            this.m_function = func;
        }
    }


    private List<Listener> m_Subscribers = new List<Listener>();

    protected override void Awake()
    {
        base.Awake();
    }

    //When a msg is published it will alert a subcriber that is subscribed to that msg if any
    static public void Notify(string msg)
    {
        instance.NotifySubs(msg.ToLower()); //will notify the subscriber that is sub to this msg
    }

    private void NotifySubs(string msg)
    {
        //Debug.Log(m_Subscribers.Capacity);
        foreach (Listener l in m_Subscribers)
        {
            if (l.EventMsg == msg) //if the disctionary for subribers contains the msg
            {
                l.m_function();
            }
        }
    }

    //creates the subscription from combing the sting of the type of event we are looking for and the msg
    //Then passes the subscription and sub into the AddListener as arguments to be added to the dictionary
    static public void AddSub(string type, string msg, CallBacks func, ISub sub)
    {
        string t = type.ToString().ToLower(); 
        //sets the case of all characters in the string to lower case for consitency when comparing the strings 
        string m = msg.ToString().ToLower();
        string subcription = t + "->" + m; //creates a new string by merging the the type and msg together
        //Debug.Log(subcription);
        instance.AddListener(subcription, func, sub); //passes the new string and sub into the AddListener function
    }

    /*
        Will add a message and the object that uses the ISub interface to the subscriber list
    */
    private void AddListener(string msg, CallBacks func, ISub sub)
    {
        Listener l = new Listener(msg, func, sub); //creates a new object of type Listener
        m_Subscribers.Add(l); //add the Listener to the list
    }

    private void RemoveListener(string msg, CallBacks func, ISub sub)
    {
        Listener search = new Listener(msg, func, sub);

        foreach(Listener s in m_Subscribers)
        {
            if(s.sub == sub)
            {
                Debug.Log("Removing" + s.sub);
                m_Subscribers.Remove(s);
            }
        }
    }

    static public void RemoveSub(string type, string msg, CallBacks func, ISub sub)
    {
        string subsc = type.ToLower() + msg.ToLower();
        instance.RemoveListener(subsc, func, sub);
    }
}
