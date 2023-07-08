using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SphereController : MonoBehaviour
{

    [SerializeField] GameObject sphere;

    float horizontalInput;
    float verticalInput;

    [SerializeField] float turningSpeed;

    float mass = 0f;
    float gravity = 9.8f;

    float acceleration = 20f;   //EXPRESSED IN km/h
    float deceleration = 10f;   //BACKWARDS ACCELERATION, EXPRESSED IN km/h
    float maxSpeed = 200f;  //EXPRESSED IN km/h
    float minSpeed = -80f;  //EXPRESSED IN km/h

//    float accelerationTime = 0f;
//    float decelerationTime = 0f;    //BACKWARDS ACCELERATION TIME
    float currentSpeed = 0f;
    float currentForce = 0f;    //FORCE TO ADD




    private void Start()
    {
        sphere.transform.parent = null;

        mass = sphere.GetComponent<Rigidbody>().mass;
        gravity = Physics.gravity.magnitude;

        maxSpeed /= 3.6f;   //EXPRESSED IN m/s
        minSpeed /= 3.6f;   //EXPRESSED IN m/s
        acceleration /= 3.6f;   //EXPRESSED IN m/s
        deceleration /= 3.6f;   //EXPRESSED IN m/s
    }


    private void Update()
    {
        //Debug.Log(sphere.GetComponent<Rigidbody>().velocity.magnitude);


        //GET INPUT

        horizontalInput = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        verticalInput = Input.GetAxis("Vertical");



        //CAR FOLLOWS SPHERE

        transform.position = sphere.transform.position;
    }



    private void FixedUpdate()
    {

        /*
        //ACCELERATION

        if (verticalInput > 0)
        {
            accelerationTime += Time.deltaTime;
            decelerationTime = 0f;
            currentSpeed = verticalInput * accelerationTime * acceleration;
        }
        else if (verticalInput < 0)
        {
            accelerationTime = 0f;
            decelerationTime += Time.deltaTime;
            currentSpeed = verticalInput * decelerationTime * deceleration;
        }
        else
        {
            accelerationTime = 0f;
            decelerationTime = 0f;
        }
*/
        // currentSpeed = Mathf.Clamp(currentSpeed * mass, minSpeed * mass, maxSpeed * mass);

        currentSpeed = sphere.GetComponent<Rigidbody>().velocity.magnitude;

        //APPLIED FORCE
        currentForce = verticalInput * ((currentSpeed > 0) ? acceleration : deceleration);
        //RESISTANCE FORCE
        //currentForce -= Mathf.Abs(verticalInput) * currentSpeed * ((currentSpeed > 0) ? acceleration : deceleration) / maxSpeed;

        //Debug.Log(currentForce);

        currentForce *= mass;

        //Debug.Log(verticalInput);
        //Debug.Log(acceleration);
        Debug.Log(currentSpeed);
        //Debug.Log(currentForce);


        //ACCELERATION

        sphere.GetComponent<Rigidbody>().AddForce(transform.forward * currentForce, ForceMode.Force);


        //TURNING
        
        if (currentSpeed > 0)
            transform.Rotate(0, horizontalInput, 0, Space.World);
        else if (currentSpeed < 0)
            transform.Rotate(0, -horizontalInput, 0, Space.World);
    }



}









/*
public class SphereController : MonoBehaviour
{

    [SerializeField] GameObject sphere;

    float horizontalInput;
    float verticalInput;

    [SerializeField] float speedForward;
    [SerializeField] float speedBackwards;
    [SerializeField] float speedTurning;

    float mass = 0f;
    float friction = 0.5f;
    float gravity = 9.8f;

    float acceleration = 20f;   //EXPRESSED IN km/h
    float deceleration = 10f;   //BACKWARDS ACCELERATION, EXPRESSED IN km/h
    float maxSpeed = 200f;  //EXPRESSED IN km/h
    float minSpeed = -80f;  //EXPRESSED IN km/h

    float accelerationTime = 0f;
    float decelerationTime = 0f;    //BACKWARDS ACCELERATION TIME
    float currentSpeed = 0f;

    //DRAG
    float radius = 1f;  //CAR SIMPLIFIED AS A SPHERE TO APPLY STOKES' LAW
    double airViscosity = 0.0000183;




    private void Start()
    {
        sphere.transform.parent = null;

        mass = sphere.GetComponent<Rigidbody>().mass;
        gravity = Physics.gravity.magnitude;

        maxSpeed /= 3.6f;   //EXPRESSED IN m/s
        minSpeed /= 3.6f;   //EXPRESSED IN m/s
        acceleration /= 3.6f;   //EXPRESSED IN m/s
        deceleration /= 3.6f;   //EXPRESSED IN m/s
    }


    private void Update()
    {
        Debug.Log(sphere.GetComponent<Rigidbody>().velocity.magnitude);


        //GET INPUT

        horizontalInput = Input.GetAxis("Horizontal") * speedTurning * Time.deltaTime;
        verticalInput = Input.GetAxis("Vertical");
        //verticalInput *= (verticalInput > 0) ? speedForward : speedBackwards;


        //ACCELERATION

        if(verticalInput > 0)
        {
            accelerationTime += Time.deltaTime;
            decelerationTime = 0f;
            currentSpeed = verticalInput * accelerationTime * acceleration;
        }
        else if(verticalInput < 0)
        {
            accelerationTime = 0f;
            decelerationTime += Time.deltaTime;
            currentSpeed = verticalInput * decelerationTime * deceleration;
        }
        else
        {
            accelerationTime = 0f;
            decelerationTime = 0f;
        }

        currentSpeed = Mathf.Clamp(currentSpeed * mass, minSpeed * mass, maxSpeed * mass);
        //Debug.Log(currentSpeed);

        //DRAG

        float dragForce = (float)(6 * 3.14 * radius * airViscosity * currentSpeed);

        //FRICTION

        if (currentSpeed > 0)
            currentSpeed = Mathf.Max(0, currentSpeed - (friction * gravity * mass + dragForce) );
        else if (currentSpeed < 0)
            currentSpeed = Mathf.Min(0, currentSpeed + (friction * gravity * mass + dragForce) );



        //CAR FOLLOWS SPHERE

        transform.position = sphere.transform.position;
    }



    private void FixedUpdate()
    {
        //ACCELERATION

        //sphere.GetComponent<Rigidbody>().AddForce(transform.forward * verticalInput, ForceMode.Force);
        sphere.GetComponent<Rigidbody>().AddForce(transform.forward * currentSpeed, ForceMode.Force);


        //TURNING

        if (verticalInput > 0)
            transform.Rotate(0, horizontalInput, 0, Space.World);
        else if(verticalInput < 0)
            transform.Rotate(0, -horizontalInput, 0, Space.World);
    }



}
*/