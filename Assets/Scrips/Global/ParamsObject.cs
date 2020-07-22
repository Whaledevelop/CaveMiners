using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ParamsObject
{
    public object[] paramsArray;

    public ParamsObject(params object[] paramsArray)
    {
        this.paramsArray = paramsArray;
    }

    public T GetParam<T>(int index = -1)
    {
        if (index == -1)
            return (T)paramsArray.First(param => param is T);
        else if (paramsArray.Length > index)
            return (T)paramsArray[index];
        else
            return default(T);
    }

    public List<T> GetAllOfType<T>()
    {
        List<T> typeList = new List<T>();
        foreach(object paramObject in paramsArray)
        {
            if (paramObject is T)
                typeList.Add((T)paramObject);
        }
        return typeList;
    }

    public override string ToString()
    {
        return paramsArray == null ? "Null" : "Params length " + paramsArray.Length;
    }
}
