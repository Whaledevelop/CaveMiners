using UnityEngine;
using System.Collections;

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
