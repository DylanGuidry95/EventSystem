using UnityEngine;
using System.Collections;
using System;

public class Publisher : MonoBehaviour, IPub
{
    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Move); 
        //fuck this AddListener function and all its confusion
    }

    public void Publish(string msg)
    {
        EventSystem.Notify(msg);
        //calls the notify method in the EventSystem class and passes the strign msg as the argument
    }

    // Use this for initialization
    void Start ()
    {
	
	}

    public void Move()
    {
        Publish("publisher:" + gameObject.name);
    }

    // Update is called once per frame
    void Update ()
    {

	}
}
