using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Digger : MonoBehaviour
{
    public Request digRequest;

    public Vector2Event startDiggingEvent;

    // Временно тут
    public MinerTools minerTools;
    public Animator animator;    
    public Rotator rotator;

    public void ChangeDigingMode()
    {
        // Нужно определить, в какую сторону направлена рука с лопатой, куда будет копать
        // Пока проверим на влево вправо
        digRequest.MakeRequest(OnDiggingAllowed, OnDiggingNotAllowed, transform.position, new Vector2Int(rotator.RightLeftMultiplier, 0));
    }

    public void OnDiggingAllowed(object[] diggingParams)
    {
        startDiggingEvent.Raise((Vector3)(Vector3Int)diggingParams[0]);
    }

    public void OnDiggingNotAllowed(object[] diggingParams)
    {

    }
}
