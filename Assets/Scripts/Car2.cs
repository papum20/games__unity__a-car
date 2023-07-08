using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2 : MonoBehaviour
{


    [Header("Objects")]

    [SerializeField] Transform defaultPosition;
    [SerializeField] Rigidbody RB;
    [SerializeField] Transform carCamera;
    [SerializeField] Transform centerOfMass;
    [SerializeField] Transform capsizedChecker;
    [SerializeField] Transform unCapsizePoint;
    [SerializeField] Material[] carMaterials;



    float VerticalInput;
    float HorizontalInput;

    //SPECIFICATIONS
    [Header("Specifications")]

    [SerializeField] float acceleration = 5f;
    [SerializeField] float backAcceleration = 2.5f;
    [SerializeField] float brakingForce = 10f;
    [SerializeField] float turning = 50f;
    float mass = 0f;
    float gravity = 0f;


    //FORCES

    float motorForce = 0f;
    float frontalDragForce = 0f;
    float sideDragForce = 0f;
    float frontalFrictionForce = 0f;
    float sideFrictionForce = 0f;
    float frontalSpeed = 0f;
    float sideSpeed = 0f;
    int direction = 0;

    [Header("Forces")]
    [SerializeField] float dragCoefficient = 2f;
    [SerializeField] float frictionCoefficient = 0.1f;
    [SerializeField] float sideFrictionCoefficient = 1f;
    [SerializeField] float angularDrag = 0.5f;

    [SerializeField] float flippingForce;
    [SerializeField] float flippingTorque;

    Vector3 resistanceDirection;
    Vector3 sideFrictionDirection;
    bool braking = false;

    float flippingTimer = 0f;

    public bool grounded = false;



    //EFFECTIS, PARTICLES
    [Header("Particles")]

    [SerializeField] GameObject trail;
    [SerializeField] Transform trailStarts;
    [SerializeField] Transform trailParent;
    bool drifting = false;

    //WHEEL ANIMATION
    [Header("Wheels")]

    [SerializeField] Transform wheelBL;
    [SerializeField] Transform wheelBR;
    [SerializeField] Transform wheelFL;
    [SerializeField] Transform wheelFR;
    [SerializeField] Transform FL_controller;
    [SerializeField] Transform FR_controller;
    float wheelRadius = 0.3913f;









    private void Start()
    {
        mass = RB.mass;
        gravity = Physics.gravity.magnitude;

        RB.centerOfMass = centerOfMass.localPosition;


        int tmpMaterialIndex = PlayerPrefs.GetInt("carMaterial");
        if (tmpMaterialIndex >= 0 && tmpMaterialIndex < carMaterials.Length)
        {
            Material[] tmp = GetComponent<MeshRenderer>().materials;
            tmp[0] = carMaterials[tmpMaterialIndex];
            GetComponent<MeshRenderer>().materials = tmp;
        }
    }


    private void Update()
    {

        grounded = false;

        //INPUT

        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("defaultPosition"))
        {
            transform.position = defaultPosition.position;
            transform.rotation = defaultPosition.rotation;
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero;

            foreach (Transform child in trailStarts)
                Unparent_SkidMark(child);
            drifting = false;
        }

        if (Input.GetButton("brake"))
            braking = true;
        else
            braking = false;




        WheelAnimation();

    }


    private void FixedUpdate()
    {

        //IF CAPSIZED
        
        if (flippingTimer >= 1f && Physics.Raycast(capsizedChecker.position, transform.up, out RaycastHit hit, 0.1f))
        {
            RB.AddForce(-transform.up * flippingForce, ForceMode.VelocityChange);
            RB.AddTorque(transform.forward * flippingTorque, ForceMode.VelocityChange);
            flippingTimer = 0f;
        }
        else if (flippingTimer < 1f)
            flippingTimer += Time.fixedDeltaTime;
        
        



        //CALCULATE FORCES
        //SPEED AND DIRECTION

        frontalSpeed = transform.InverseTransformDirection(RB.velocity).z;
        sideSpeed = transform.InverseTransformDirection(RB.velocity).x;

        float localSpeed = transform.InverseTransformDirection(RB.velocity).z;
        if (localSpeed > 0) direction = 1;
        else if (localSpeed < 0) direction = -1;
        else direction = 0;



        //LONGITUDINAL FORCES

        if (Input.GetButton("drift") || (VerticalInput == 0 && Mathf.Abs(frontalSpeed) < 1f) )
        {
            RB.AddForce(-transform.forward * frontalSpeed, ForceMode.VelocityChange);  //STOP IF LOW SPEED
        }

        else
        {

            //FORCE APPLIED BY MOTOR

            if (braking)
                motorForce = -direction * brakingForce * mass;
            else
                motorForce = VerticalInput * ((VerticalInput > 0) ? acceleration : backAcceleration) * mass;

            //DRAG AND FRICTION FORCE

            frontalDragForce = dragCoefficient * frontalSpeed * frontalSpeed;

            frontalFrictionForce = frictionCoefficient * mass * gravity;

            //APPLY FORCES

            if(grounded)
                RB.AddForce(transform.forward * motorForce, ForceMode.Force);   //MOTOR
            RB.AddForce(transform.forward * (-direction) * frontalDragForce, ForceMode.Force);    //DRAG
            if (Mathf.Abs(frontalSpeed) >= gravity * frictionCoefficient * Time.fixedDeltaTime)
                RB.AddForce(transform.forward * (-direction) * frontalFrictionForce, ForceMode.Force);    //FRICTION
        }


        //Debug.Log(frontalSpeed * 3.6f);
        //Debug.Log(sideSpeed * 3.6f);

        //LATERAL FORCES

        
        if (VerticalInput == 0 && Mathf.Abs(sideSpeed) < 1f)
            RB.AddForce(-transform.right * sideSpeed, ForceMode.VelocityChange);    //STOP IF LOW LATERAL SPEED

        
        sideFrictionForce = sideFrictionCoefficient * mass * gravity;
        sideFrictionDirection = transform.right * ((sideSpeed > 0) ? -1 : 1);

        sideDragForce = dragCoefficient * sideSpeed * sideSpeed;

        RB.AddForce(sideFrictionDirection * sideDragForce, ForceMode.Force);    //SIDE DRAG
        if(Mathf.Abs(sideSpeed) >= sideFrictionForce * Time.fixedDeltaTime / mass)
            RB.AddForce(sideFrictionDirection * sideFrictionForce, ForceMode.Force);    //SIDE FRICTION


        //ANGULAR DRAG

        if (RB.angularVelocity.magnitude != 0f)
        {
            RB.angularVelocity -= RB.angularVelocity * angularDrag * Time.fixedDeltaTime * (grounded ? 3 : 1);
        }






        //STEERING

        if (grounded && Mathf.Abs(frontalSpeed ) > 0.1f && HorizontalInput != 0f)
        {
            if (direction > 0)
                transform.Rotate(0, HorizontalInput * Time.fixedDeltaTime * turning, 0, Space.World);
            else if (direction < 0)
                transform.Rotate(0, -HorizontalInput * Time.fixedDeltaTime * turning, 0, Space.World);
        }
        


        //DRIFTING

        if(Mathf.Abs(sideSpeed) > 0.5f || braking == true)
        {

            //SKID MARK

            if(!drifting)
            {
                foreach(Transform child in trailStarts)
                    Create_SkidMark(child);

                drifting = true;

                //CAMERA WHEN DRIFTING

            }

        }

        if( (drifting && Mathf.Abs(sideSpeed) <= 0.5f && braking == false) || !grounded)
        {
            //SKID MARK

            foreach (Transform child in trailStarts)
                Unparent_SkidMark(child);

            drifting = false;
        }

    }






    #region SKID_MARK

    void Create_SkidMark(Transform point)
    {
        Vector3 newPosition = point.position;
        newPosition.y -= 0.9f; //0.37f
        GameObject tmpTrail = Instantiate(trail, newPosition, trail.transform.rotation, point);
        tmpTrail.transform.localScale /= 50f;
        tmpTrail.GetComponent<TrailRenderer>().emitting = true;
    }

    void Unparent_SkidMark(Transform point)
    {
        if (point.childCount != 0)
        {
            Transform tmpTrail = point.GetChild(0);
            tmpTrail.parent = trailParent;
            tmpTrail.GetComponent<TrailRenderer>().emitting = false;
        }
    }

    #endregion




    void WheelAnimation()
    {

        //ROTATION WHEN MOVING

        float circumference = 2 * wheelRadius * 3.14f;
        float tmpRotation = (360f * frontalSpeed / circumference) * Time.deltaTime;

        wheelBL.Rotate(tmpRotation, 0, 0, Space.Self);
        wheelBR.Rotate(tmpRotation, 0, 0, Space.Self);
        wheelFL.Rotate(tmpRotation, 0, 0, Space.Self);
        wheelFR.Rotate(tmpRotation, 0, 0, Space.Self);


        //FRONT WHEELS ROTATION WHEN TURNING

        float sign = (HorizontalInput != 0f)? (HorizontalInput / Mathf.Abs(HorizontalInput)):0f;
        FL_controller.localEulerAngles = new Vector3(0, turning * sign, 0);
        FR_controller.localEulerAngles = new Vector3(0, turning * sign, 0);

    }





    public float GetSpeed()
    {
        return Mathf.Min(180f, Mathf.Abs(frontalSpeed) * 3.6f);
    }

}
