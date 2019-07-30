using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;

public class AccountsUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI balanceValue;

    public float valueLerpMultiplier = 2f;

    void Start()
    {
        
    }

    float currentBalanceValue;
    void Update()
    {

        currentBalanceValue = Mathf.Lerp(currentBalanceValue, Accounts.Instance.Balance, valueLerpMultiplier * Time.unscaledDeltaTime * (1f - ((currentBalanceValue - Accounts.Instance.Balance) / Accounts.Instance.Balance)));
        balanceValue.text = "Balance: " + currentBalanceValue.ToString("C", CultureInfo.CurrentCulture);    
    }
}
