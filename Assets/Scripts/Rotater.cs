using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{

    [SerializeField] float speed;

    private void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f));
    }

}
