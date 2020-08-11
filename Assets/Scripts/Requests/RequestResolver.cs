using UnityEngine;

public abstract class RequestResolver : MonoBehaviour
{
    [SerializeField] private Request request;

    public virtual void Awake()
    {
        request.RegisterResolver(this);
    }

    public abstract bool Resolve(ParamsObject requestParams);
}

public abstract class RequestResolver<T> : MonoBehaviour
{
    public abstract Request<T> Request { get; }

    public virtual void Awake()
    {
        Request.RegisterResolver(this);
    }

    public abstract bool Resolve(ParamsObject requestParams, out T resolveParams);
}

public abstract class RequestResolver<T1, T2> : MonoBehaviour
{
    public abstract Request<T1, T2> Request { get; }

    public virtual void Awake()
    {
        Request.RegisterResolver(this);
    }

    public abstract bool Resolve(ParamsObject requestParams, out T1 resolveParam1, out T2 resolveParam2);
}
