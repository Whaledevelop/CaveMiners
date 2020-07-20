using UnityEngine;

[CreateAssetMenu(fileName = "Request", menuName = "ScriptableObjects/Request")]
public class Request : ScriptableObject
{
    [HideInInspector]
    public object[] LastRequestParams;

    private RequestResolver resolver;    

    public void RegisterResolver(RequestResolver resolver)
    {
        this.resolver = resolver;
    }

    public ParamsObject MakeRequest(params object[] requestParams)
    {
        return resolver.Resolve(requestParams);
    }
}
