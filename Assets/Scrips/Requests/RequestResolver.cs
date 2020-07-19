using UnityEngine;

public abstract class RequestResolver<T> : MonoBehaviour
{
    public abstract Request<T> request { get; }

    public virtual void Start()
    {
        request.RegisterResolver(this);
    }

    public abstract T Resolve(object[] requestParams);
}
