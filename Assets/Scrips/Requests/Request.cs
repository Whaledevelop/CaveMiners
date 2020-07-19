using UnityEngine;


public class Request<T> : ScriptableObject
{
    private RequestResolver<T> resolver;
    public void RegisterResolver(RequestResolver<T> resolver)
    {
        this.resolver = resolver;
    }

    public T MakeRequest(params object[] requestParams)
    {
        return resolver.Resolve(requestParams);
    }
}
