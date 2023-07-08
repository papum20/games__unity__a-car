using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public Transform wheel_BL;
    public Transform wheel_BR;
    public Transform wheel_FL;
    public Transform wheel_FR;

    public WheelCollider collider_BL;
    public WheelCollider collider_BR;
    public WheelCollider collider_FL;
    public WheelCollider collider_FR;


    public Transform centerOfMass;
    Rigidbody _rigidbody;


    [SerializeField] float motorTorque = 100f;
    float maxSteerAngle = 20f;



    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }




    private void FixedUpdate()
    {
        collider_BL.motorTorque = Input.GetAxis("Vertical") * motorTorque;
        collider_BR.motorTorque = Input.GetAxis("Vertical") * motorTorque;

        collider_FL.steerAngle = Input.GetAxis("Horizontal") * maxSteerAngle;
        collider_FR.steerAngle = Input.GetAxis("Horizontal") * maxSteerAngle;

    }



    private void Update()
    {
        Vector3 pos;
        Quaternion rot;

        collider_BL.GetWorldPose(out pos, out rot);
        wheel_BL.position = pos;
        wheel_BL.rotation = rot;

        collider_BR.GetWorldPose(out pos, out rot);
        wheel_BR.position = pos;
        wheel_BR.rotation = rot;

        collider_FL.GetWorldPose(out pos, out rot);
        wheel_FL.position = pos;
        wheel_FL.rotation = rot;

        collider_FR.GetWorldPose(out pos, out rot);
        wheel_FR.position = pos;
        wheel_FR.rotation = rot;
    }

}











/*
{

    Rigidbody RB;

    Vector3 direction;

    float accelerationTime;
    float decelerationTime;
    float speed;
    [SerializeField] float speedMultiplier;
    float maxSpeed = 80f;
    float maxSpeedBack = -30f;


    public Transform frontPivot;
    public Transform backPivot;

    float turningAngle;
    float turningDirection;
    float maxTurning = 45f;
    float turningMultiplier = 45f/2f;





    private void Start()
    {
        RB = GetComponent<Rigidbody>();

        direction = new Vector3(0, 0, 0);
        accelerationTime = 0f;
        decelerationTime = 0f;
        speed = 0f;

        turningAngle = 0f;
        turningDirection = 0f;
    }

    private void Update()
    {
        //FORWARD MOVEMENT INPUT

        direction.z = Input.GetAxis("Vertical");
        if (direction.z != 0f)
        {
            accelerationTime += Time.deltaTime;
            decelerationTime = 0f;
        }
        else
        {
            accelerationTime = 0f;
            decelerationTime += Time.deltaTime;
        }


        //TURNING INPUT

        direction.x = Input.GetAxis("Horizontal");
        if (direction.x != 0f)
            turningDirection = direction.x;

    
    }


    private void FixedUpdate()
    {
        //FORWARD MOVEMENT

        if (accelerationTime != 0f)
        {
            speed = accelerationTime * accelerationTime * speedMultiplier * direction.z;
            if (speed > maxSpeed)
                speed = maxSpeed;
            else if (speed < maxSpeedBack)
                speed = maxSpeedBack;
        }
        else
        {
            speed -= decelerationTime * decelerationTime * speedMultiplier;
            if (speed < 0)
                speed = 0;
        }

        Debug.Log(speed);

        RB.AddForce(new Vector3(0, 0, speed * RB.mass - RB.velocity.z), ForceMode.Force);
        //RB.AddForce(new Vector3(0, 0, speed), ForceMode.Impulse);


        //TURNING

        if (direction.x != 0f)
        {
            turningAngle += direction.x * Time.deltaTime * turningMultiplier;
            if (turningAngle > maxTurning)
                turningAngle = maxTurning;
            else if (turningAngle < -maxTurning)
                turningAngle = -maxTurning;
        }
        else
        {
            turningAngle -= turningDirection * Time.deltaTime * turningMultiplier * 2;
            if ( (turningDirection > 0 && turningAngle < 0) || (turningDirection < 0 && turningAngle > 0) )
                turningAngle = 0;
        }

        if (direction.z > 0)
            transform.RotateAround(backPivot.position, Vector3.up, turningAngle);
        else if(direction.z < 0)
            transform.RotateAround(frontPivot.position, Vector3.up, turningAngle);


    }



}
*/