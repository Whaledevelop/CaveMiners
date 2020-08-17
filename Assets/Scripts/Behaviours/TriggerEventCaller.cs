using UnityEngine;
using System.Collections;

/// <summary>
/// Скрипт, вызывающий определенный триггер при попадании объекта в него
/// </summary>
public class TriggerEventCaller : MonoBehaviour
{
    public string triggerTag;

    public GameEvent triggerEvent;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == triggerTag)
        {
            triggerEvent.Raise();
        }
    }
}
