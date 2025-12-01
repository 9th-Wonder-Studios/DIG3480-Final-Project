using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 1f, 0f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);

        transform.LookAt(Camera.main.transform);
    }
}
