using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{


    [SerializeField] Rigidbody carRB;
    [SerializeField] Car2 carScript;
    public LayerMask groundMask;


    //SUSPENSIONS

    [SerializeField] float restSpringLength; //default spring length
    [SerializeField] float springTravel;    //how far the spring can get from the default length
    float maxSpringLength;
    float minSpringLength;

    [SerializeField] float springStiffness; //k
    [SerializeField] float damperStiffness;

    float currentSpringLength;
    float lastSpringLength;
    float springVelocity;

    float springForce;
    float damperForce;
    Vector3 suspensionForce;


    //WHEELS

    [SerializeField] float wheelRadius;
    [SerializeField] Transform wheelTransform;





    private void Start()
    {
        maxSpringLength = restSpringLength + springTravel;
        minSpringLength = restSpringLength - springTravel;
    }


    private void LateUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, maxSpringLength + wheelRadius, groundMask))
            carScript.grounded = true;
    }


    private void FixedUpdate()
    {
        
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxSpringLength + wheelRadius, groundMask))
        {
            lastSpringLength = currentSpringLength;
            currentSpringLength = Mathf.Clamp(hit.distance - wheelRadius, minSpringLength, maxSpringLength);

            wheelTransform.position = transform.position - transform.up * currentSpringLength;


            springVelocity = (lastSpringLength - currentSpringLength) / Time.fixedDeltaTime;

            //F = - k x, minus damperForce
            springForce = springStiffness * (restSpringLength - currentSpringLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;

            carRB.AddForceAtPosition(suspensionForce, hit.point);
        }

        else
        {            
            currentSpringLength = maxSpringLength;
            lastSpringLength = currentSpringLength;
            wheelTransform.position = transform.position - transform.up * currentSpringLength;
        }

    }



}
