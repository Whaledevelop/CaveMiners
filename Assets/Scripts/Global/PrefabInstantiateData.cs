using UnityEngine;

[System.Serializable]
public class PrefabInstantiateData
{
    public GameObject prefab;
    public Transform parent;

    private GameObject instance;

    public bool IsActive => instance != null && instance.activeSelf;

    public void ChangeMode(bool mode)
    {
        if (mode)
            Instantiate();
        else
            Destroy();
    }

    public GameObject Instantiate()
    {
        if (!IsActive)
            instance = Object.Instantiate(prefab, parent);
        return instance;

    }

    public void Destroy()
    {
        if (IsActive)
            Object.Destroy(instance);
    }
}