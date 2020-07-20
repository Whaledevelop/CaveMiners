[System.Serializable]
public class ParamsObject
{
    public object[] paramsArray;

    public ParamsObject(params object[] paramsArray)
    {
        this.paramsArray = paramsArray;
    }

    public override string ToString()
    {
        return paramsArray == null ? "Null" : "Params length " + paramsArray.Length;
    }
}
