using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public AxleInfo[] carAxis = new AxleInfo[2];
    public float carSpeed;
    public float steerAngle;
    public Transform centerOfMass;

    float horInput;
    float verInput;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    void FixedUpdate()
    {
        horInput = Input.GetAxis("Horizontal"); //-1-a   +1-d
        verInput = Input.GetAxis("Vertical");//-1-w   +1-d

        Speed();
    }

    void Speed()
    {
        foreach (AxleInfo axle in carAxis)
        {
            if (axle.steering)
            {
                axle.rightWheel.steerAngle = steerAngle * horInput; //Поворот
                axle.leftWheel.steerAngle = steerAngle * horInput;
            }

            if (axle.motor)
            {
                axle.rightWheel.motorTorque = carSpeed * verInput;//Скорость
                axle.leftWheel.motorTorque = carSpeed * verInput;
            }


            VisualRotation(axle.rightWheel, axle.visRightWheel); //Сопоставление мэша и коллайдера
            VisualRotation(axle.leftWheel, axle.visLeftWheel);
        }
    }

    void VisualRotation(WheelCollider col, Transform visWheel)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        visWheel.position = position;
        visWheel.rotation = rotation;
    }
}

[System.Serializable] //Для редакции в инспекторе

public class AxleInfo {

    public WheelCollider rightWheel; //Коллайдеры
    public WheelCollider leftWheel;

    public Transform visRightWheel; //Мэши
    public Transform visLeftWheel;

    public bool steering; //Поворот
    public bool motor;

}
