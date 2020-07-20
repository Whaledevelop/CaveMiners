using UnityEngine;

public abstract class RequestResolver : MonoBehaviour
{
    [SerializeField] private Request request;

    public virtual void Start()
    {
        request.RegisterResolver(this);
    }

    public abstract bool Resolve(object[] requestParams, out object[] resolveParams);
}
