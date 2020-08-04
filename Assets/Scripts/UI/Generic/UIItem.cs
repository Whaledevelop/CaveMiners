using UnityEngine;

public abstract class UIItem<T> : MonoBehaviour
{
    public abstract void Init(T setupData);
}
