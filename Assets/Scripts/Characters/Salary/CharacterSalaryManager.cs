using System.Collections;
using UnityEngine;

public class CharacterSalaryManager : CharacterManager
{
    [SerializeField] private int salaryInverval;

    [SerializeField] private IntVariable moneyVariable;

    public override void Init(Character character)
    {
        base.Init(character);
        StartCoroutine(PaySalaryEnumerator());
    }

    public IEnumerator PaySalaryEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(salaryInverval);
            moneyVariable.Value -= characterData.salary;
        }        
    }
}
