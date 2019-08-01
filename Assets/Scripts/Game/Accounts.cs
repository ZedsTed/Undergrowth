using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accounts : SingletonDontCreate<Accounts>
{
    [Tooltip("Current balance of money.")]
    [SerializeField]
    protected float balance;
    public float Balance { get { return balance; } protected set { balance = value; } }


    
    protected void Start()
    {
        balance = 4000f;        
    }

    
    protected void Update()
    {
        
    }

    public bool BuyItem(float cost)
    {
        if ((balance - cost) < 0f)
            return false;

        balance -= cost;

        return true;
    }

    public bool SellItem(float cost)
    {
        balance += cost;

        return true;
    }
}
