using UnityEngine;

/// <summary>
/// Реквест - это моя попытка уйти от Singleton. Это что-то похожее на GameEvent, но он связывает реквест с одним "слушателем" - резолвером, исполнителем
/// запроса. При этом реквест может вызываться из нескольких мест, а резолвер будет один. Резолвер - это MonoBehaviour, по сути, это как отдельный метод
/// Singleton, одна какая-то задача, но только в отдельном классе и без собственно синглтона. Пока в тестовом режиме
/// </summary>
[CreateAssetMenu(fileName = "Request", menuName = "Requests/Request")]
public class Request : ScriptableObject
{
    private RequestResolver resolver;    

    public void RegisterResolver(RequestResolver resolver)
    {
        this.resolver = resolver;
    }

    public bool MakeRequest(params object[] requestParams)
    {
        return resolver.Resolve(new ParamsObject(requestParams));
    }

    public bool MakeRequest(ParamsObject requestParams)
    {
        return resolver.Resolve(requestParams);
    }
}

public class Request<T> : ScriptableObject
{
    private RequestResolver<T> resolver;

    public void RegisterResolver(RequestResolver<T> resolver)
    {
        this.resolver = resolver;
    }

    public bool MakeRequest(ParamsObject requestParams, out T resolveParam)
    {
        return resolver.Resolve(requestParams, out resolveParam);
    }
}

public class Request<T1, T2> : ScriptableObject
{
    private RequestResolver<T1, T2> resolver;

    public void RegisterResolver(RequestResolver<T1, T2> resolver)
    {
        this.resolver = resolver;
    }

    public bool MakeRequest(ParamsObject requestParams, out T1 resolveParam1, out T2 resolveParam2)
    {
        return resolver.Resolve(requestParams, out resolveParam1, out resolveParam2);
    }
}
