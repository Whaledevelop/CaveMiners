using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class Digger : MonoBehaviour
{
    public TileRequest digRequest;

    public DigEvent digEvent;

    private bool isDigging;
    private Vector2 digPosition;

    // Временно тут
    public MinerTools minerTools;
    public Animator animator;    
    public Rotator rotator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeDigingMode(!isDigging);
        }
    }

    public void ChangeDigingMode(bool isDigging)
    {
        if (this.isDigging != isDigging)
        {
            this.isDigging = isDigging;
            minerTools.ChangeToolMode(ToolCode.Shovel, isDigging);
            animator.SetBool("Diging", isDigging);

            // Нужно определить, в какую сторону направлена рука с лопатой, куда будет копать
            // Пока проверим на влево вправо

            TileData digTileData = digRequest.MakeRequest(transform.position, rotator.Rotations);
            digEvent.Raise(digTileData);


        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(digPosition, 0.05f);
    }

}
