using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Players : MonoBehaviour, ISub
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

    bool blocking;

    public Slider hp;


    [SerializeField]
    string state;

    [SerializeField]
    Enemy Target;

    void Awake()
    {
        Subscribe("GUI", "Attack", Attack);
        Subscribe("GUI", "Defend", Defend);
        Subscribe("GUI", "Heal", Heal);
        Subscribe("GUI", "Wait", Wait);
        Subscribe("Combat", gameObject.name, InCombat);
        Subscribe("CombatWin", gameObject.name, Winner);
    }
    void Start()
    {
        MaxHealth = 100; //sets players max HP to 100
        CurrentHealth = MaxHealth; //sets the starting current hp to the max
        AttackPower = Random.Range(10,20); //generates a random number between 10 and 20 to be his attack power
        Potions = Random.Range(1, 5); //generate random number between 1 adn 5 to see how many potions the player has a start
        Arrmor = Random.Range(1, 10); //generate random number between 1,10 for how much dmg the player can block
        Target = FindObjectOfType<Enemy>();
        hp.maxValue = MaxHealth;
        hp.value = CurrentHealth;
        gameObject.GetComponent<Unit>().toIdle();
    }

    void Update()
    {
        if (CurrentHealth <= 0)
        {
            UnSub("CombatWin", gameObject.name, Winner);
            UnSub("GUI", "Attack", Attack);
            UnSub("GUI", "Defend", Defend);
            UnSub("GUI", "Heal", Heal);
            UnSub("GUI", "Wait", Wait);
            UnSub("Combat", gameObject.name, InCombat);
            gameObject.GetComponent<Unit>().ToDead();

        }
        state = gameObject.GetComponent<Unit>().getState().ToString();
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

    public void Subscribe(string type, string msg, CallBacks func)
    {
        EventSystem.AddSub(type, msg, func, this);
    }

    void InCombat()
    {
        Debug.Log("Fighting: " + gameObject.name);
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        gameObject.GetComponent<Unit>().ToCombat();
    }

    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
        if(blocking)
        {
            CurrentHealth -= (dmg - Arrmor);
            blocking = false;
        }
        hp.value = CurrentHealth;
    }

    void Attack()
    {
        if (gameObject.GetComponent<Unit>().getState().ToString() == "e_Combat")
        {
            Target.TakeDamage(AttackPower);
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            gameObject.GetComponent<Unit>().toIdle();
        }

    }

    void Defend()
    {
        if (gameObject.GetComponent<Unit>().getState() == "e_Combat")
        {
            blocking = true;
            gameObject.GetComponent<Renderer>().material.color = Color.white;
            gameObject.GetComponent<Unit>().toIdle();
        }

    }

    void Heal()
    {
        if (gameObject.GetComponent<Unit>().getState() == "e_Combat")
        {
            if(Potions > 0)
            {
                CurrentHealth += 10;
                Potions -= 1;
            }
            gameObject.GetComponent<Unit>().toIdle();
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

    }

    void Wait()
    {
        if (gameObject.GetComponent<Unit>().getState() == "e_Combat")
        {
            gameObject.GetComponent<Unit>().toIdle();
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }

    }
}
