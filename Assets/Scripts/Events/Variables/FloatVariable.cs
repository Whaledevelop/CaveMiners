using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }


[CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable")]
public class FloatVariable : ScriptableVariable<float>
{
    public void Plus(float plus)
    {
        Value += plus;
    }
}
