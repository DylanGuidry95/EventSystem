using UnityEngine;
using System.Collections;
using System;

public class Publisher : MonoBehaviour, IPub
{
    public GameObject prefab;

    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SpawnStuff);    
    }

    public void Publish(string msg)
    {
        Observer.Notify(msg);
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
    public void SpawnStuff()
    {
        Instantiate(prefab);
        Publish("publisher:" + gameObject.name);
    }

	// Update is called once per frame
	void Update ()
    {

	}
}
