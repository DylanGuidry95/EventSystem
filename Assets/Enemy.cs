using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ISub
{
    int MaxHealth; //Max health any player object can have at any time
    [SerializeField]
    int CurrentHealth;  //current health a playte robject has
    [SerializeField]
    int AttackPower; //how much damage a player object can do before arrmor calculations
    [SerializeField]
    int Potions; //number of potions a player object has
    [SerializeField]
    int Arrmor; //Arrmor rating of a player object how much dmg he can absorb

    public Slider hp;

    public bool blocking;

    [SerializeField]
    List<Players> Target;

    void Awake()
    {
        Subscribe("Combat", gameObject.name, InCombat);
        Subscribe("CombatWin", gameObject.name, Winner);
    }

    void Start()
    {
        MaxHealth = 200; //sets players max HP to 100
        CurrentHealth = MaxHealth; //sets the starting current hp to the max
        AttackPower = Random.Range(30, 40); //generates a random number between 10 and 20 to be his attack power
        Potions = Random.Range(1, 5); //generate random number between 1 adn 5 to see how many potions the player has a start
        Arrmor = Random.Range(1, 10); //generate random number between 1,10 for how much dmg the player can block
        hp.maxValue = MaxHealth;
        hp.value = CurrentHealth;
        gameObject.GetComponent<Unit>().toIdle();
    }

    void InCombat()
    {
        Debug.Log("Fighting: " + gameObject.name.ToString());
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        gameObject.GetComponent<Unit>().ToCombat();
        Attack();
    }

    public void Subscribe(string type, string msg, CallBacks func)
    {
        EventSystem.AddSub(type, msg, func, this);
    }

    public void TakeDamage(int dmg)
    {
        if (!blocking)
            CurrentHealth -= dmg;
        else
        {
            CurrentHealth -= (dmg - Arrmor);
            blocking = false;
        }
        hp.value = CurrentHealth;
    }

    void Update()
    {
        if (CurrentHealth <= 0)
        {
            gameObject.GetComponent<Unit>().ToDead();
            UnSub("CombatWin", gameObject.name, Winner);
            UnSub("Combat", gameObject.name, InCombat);
        }
    }

    void Winner()
    {
        gameObject.GetComponent<Unit>().ToVictory();
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    void UnSub(string type, string msg, CallBacks func)
    {
        EventSystem.RemoveSub(type, msg, func, this);
    }

    void Attack()
    {
        int chance = Random.Range(0, Target.Count);
        if (gameObject.GetComponent<Unit>().getState().ToString() == "e_Combat")
        {
            Target[chance].TakeDamage(AttackPower);
            if(Target[chance] == null)
            {
                Target.RemoveAt(chance);
            }
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            gameObject.GetComponent<Unit>().toIdle();
        }
    }
}
