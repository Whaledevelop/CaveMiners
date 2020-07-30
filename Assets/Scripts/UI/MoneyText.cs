using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    [SerializeField] private Text moneyText;

    public void OnChangeMoney(float money)
    {
        moneyText.text = money.ToString();
    }
}
