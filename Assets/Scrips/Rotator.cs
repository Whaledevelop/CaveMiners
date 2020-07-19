﻿using UnityEngine;
using System.Collections;

public enum Rotation
{
    None,
    Top,
    Right,
    Left,
    Down
}

public class Rotator : MonoBehaviour
{
    public Rotation TopDownRotation => transform.localScale.y > 0 ? Rotation.Down : Rotation.Top;

    public Rotation RightLeftRotation => transform.localScale.x > 0 ? Rotation.Right : Rotation.Left;

    public int TopDownMultiplier => transform.localScale.y > 0 ? 1 : -1;
    public int RightLeftMultiplier => transform.localScale.x > 0 ? 1 : -1;
    public Vector2Int Rotations => new Vector2Int(RightLeftMultiplier, TopDownMultiplier);

    public void Rotate(Vector2 direction)
    {
        if (direction.x != 0)
            Rotate(direction.x > 0 ? Rotation.Right : Rotation.Left);
        if (direction.y != 0)
            Rotate(direction.y > 0 ? Rotation.Top : Rotation.Down);
    }

    public void Rotate(Rotation direction)
    {
        switch(direction)
        {
            case Rotation.Right:
                if (transform.localScale.x != 1)
                    transform.localScale = new Vector2(1, transform.localScale.y);                          
                break;

            case Rotation.Left:
                if (transform.localScale.x != -1)
                    transform.localScale = new Vector2(-1, transform.localScale.y);
                break;

            //case Rotation.Down:
            //    if (transform.localScale.y != 1)
            //        transform.localScale = new Vector2(transform.localScale.x, 1);
            //    break;

            //case Rotation.Top:
            //    if (transform.localScale.y != -1)
            //        transform.localScale = new Vector2(transform.localScale.x, -1);
            //    break;

        }
    }
}
