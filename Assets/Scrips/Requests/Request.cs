using UnityEngine;

[CreateAssetMenu(fileName = "Request", menuName = "ScriptableObjects/Request")]
public class Request : ScriptableObject
{
    private RequestResolver resolver;

    public void RegisterResolver(RequestResolver resolver)
    {
        this.resolver = resolver;
    }

    public void MakeRequest(ObjectArrayDelegate onSuccess, ObjectArrayDelegate onFailure, params object[] requestParams)
    {
        if (resolver.Resolve(requestParams, out object[] resolveParams))
        {
            onSuccess?.Invoke(resolveParams);
        }
        else
        {
            onFailure?.Invoke(resolveParams);
        }
    }
}
