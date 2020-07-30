using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSalaryManager : MonoBehaviour
{
    [SerializeField] private CharacterInitialData initialData;

    [SerializeField] private int salaryInverval;

    [SerializeField] private FloatVariable moneyVariable;

    public void Start()
    {
        InvokeRepeating("PaySalary", 0, salaryInverval);
    }

    private void PaySalary()
    {
        moneyVariable.Value -= initialData.salary;
    }
}
