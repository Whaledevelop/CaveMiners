using UnityEngine;

public abstract class UIItem<T> : MonoBehaviour
{
    public abstract void Init(T setupData);
}

public abstract class UIItem<T1, T2> : MonoBehaviour
{
    public abstract void Init(T1 setupParam1, T2 setupParam2);
}

public abstract class UIItem<T1, T2, T3> : MonoBehaviour
{
    public abstract void Init(T1 setupParam1, T2 setupParam2, T3 setupParam3);
}
