using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prova : MonoBehaviour
{

    public float f = 20f;
    public float t = 30f;
    public float C = 1f;
    public Transform centerOfMass;

    float horizontalInput;
    float verticalInput;
    Rigidbody RB;
    bool braking = false;



    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.centerOfMass = centerOfMass.localPosition;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButton("brake"))
            braking = true;
        else
            braking = false;
    }


    private void FixedUpdate()
    {

        if (braking)
            RB.AddForce(-RB.velocity, ForceMode.VelocityChange);    //BRAKING FORCE
        else
            RB.AddForce(transform.forward * verticalInput * f, ForceMode.Force);    //ACCELERATION



        float speed = transform.InverseTransformDirection(RB.velocity).z;
        Vector3 s = RB.velocity;

        RB.AddForce(-s.normalized * s.magnitude * s.magnitude * C, ForceMode.Force);    //DRAG FORCE


        transform.Rotate(new Vector3(0, horizontalInput * Time.fixedDeltaTime * t, 0)); //TURNING

        Debug.Log(speed);
        Debug.Log(transform.InverseTransformDirection(s));
    }

}
