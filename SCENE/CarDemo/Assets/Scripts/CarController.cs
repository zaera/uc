using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    public AxleInfo[] carAxis = new AxleInfo[2];
    public WheelCollider[] wheelColliders;
    public float carSpeed;
    public float brake;
    public float steerAngle;
    public Transform centerOfMass;

    [Range(0,1)]
    public float steerHelpValue = 0;

    float horInput;
    float vertInput;
    int reverseGearCountPress = 0;
    bool reverseGear = false;

    Rigidbody rb;
    bool onGround;
    float lastYRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("ReverseGear"))
        {
            if (rb.velocity == Vector3.zero)
            {
                reverseGearCountPress++;
                if (reverseGearCountPress % 2 == 1) reverseGear = true;
                else reverseGear = false;
            }
        }

        horInput = Input.GetAxis("HelmRotation");
        vertInput = Input.GetAxis("GasPedal");

        CheckOnGround();

        Accelerate();
        SteerHelpAssist();
    }

    void Accelerate()
    {
        foreach(AxleInfo axle in carAxis)
        {
            if(axle.steering)
            {
                axle.rightWheel.steerAngle = steerAngle * horInput;
                axle.leftWheel.steerAngle = steerAngle * horInput;
            }
            if(axle.motor && vertInput > 0 && reverseGear == false)
            {
                axle.rightWheel.motorTorque = carSpeed * vertInput;
                axle.leftWheel.motorTorque = carSpeed * vertInput;
            }
            VisualWheelsToColliders(axle.rightWheel, axle.visRightWheel);
            VisualWheelsToColliders(axle.leftWheel, axle.visLeftWheel);

            if (vertInput < 0)
            {
                axle.rightWheel.brakeTorque = brake;
                axle.leftWheel.brakeTorque = brake;
            }
            else
            {
                axle.rightWheel.brakeTorque = 0;
                axle.leftWheel.brakeTorque = 0;
            }

            if (axle.motor && vertInput > 0 && reverseGear == true)
            {
                axle.rightWheel.motorTorque = -carSpeed * vertInput;
                axle.leftWheel.motorTorque = -carSpeed * vertInput;
            }

        }


    }

    void VisualWheelsToColliders(WheelCollider col, Transform visWheel)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        visWheel.position = position;
        visWheel.rotation = rotation;
    }

    void SteerHelpAssist()
    {
        if (!onGround)
            return;

        if(Mathf.Abs(transform.rotation.eulerAngles.y - lastYRotation) < 10f)
        {
            float turnAjust = (transform.rotation.eulerAngles.y - lastYRotation) * steerHelpValue;
            Quaternion rotateHelp = Quaternion.AngleAxis(turnAjust, Vector3.up);
            rb.velocity = rotateHelp * rb.velocity;
        }
        lastYRotation = transform.rotation.eulerAngles.y;
    }

    void CheckOnGround()
    {
        onGround = true;
        foreach(WheelCollider wheelCol in wheelColliders)
        {
            if (!wheelCol.isGrounded)
                onGround = false;
        }

    }


}


[System.Serializable]
public class AxleInfo
{

    public WheelCollider rightWheel;
    public WheelCollider leftWheel;

    public Transform visRightWheel; 
    public Transform visLeftWheel; 

    public bool steering; 
    public bool motor; 

}