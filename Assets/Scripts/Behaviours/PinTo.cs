using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTo : MonoBehaviour
{
    public Transform pinToObject;

    public Vector3 pinAxis;

    // Update is called once per frame
    void Update()
    {
        float x = pinAxis.x != 0 ? pinAxis.x * pinToObject.position.x : transform.position.x;
        float y = pinAxis.y != 0 ? pinAxis.y * pinToObject.position.y : transform.position.y;
        float z = pinAxis.z != 0 ? pinAxis.z * pinToObject.position.z : transform.position.z;
        transform.position = new Vector3(x, y, z);
    }
}
