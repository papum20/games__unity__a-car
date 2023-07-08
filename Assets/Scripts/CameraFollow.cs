using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform carModel;
    [SerializeField] Transform cameraDefaultPosition;


    float mouseX = 0f;
    float mouseY = 0f;
    [SerializeField] float rotationSpeed;




    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void LateUpdate()
    {

        if (Input.GetButton("FreeCamera"))
        {
            mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            mouseY = Mathf.Clamp(Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime, -35f, 60f);

            transform.RotateAround(carModel.position, Vector3.up, mouseX);
            transform.RotateAround(carModel.position, transform.right, -mouseY);
        }
        else if (Input.GetButtonUp("FreeCamera"))
        {
            transform.position = cameraDefaultPosition.position;
            transform.rotation = cameraDefaultPosition.rotation;
        }

    }




}
