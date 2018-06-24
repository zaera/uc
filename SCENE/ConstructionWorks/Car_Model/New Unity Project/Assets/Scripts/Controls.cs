using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public AxleInfo[] carAxis = new AxleInfo[2];//Для осей
    public WheelCollider [] wheelColliders;//Для колёс
    public Transform Helm;
    public float carSpeed;
    public float steerAngle;
    public Transform centerOfMass;

  

    public float minSpeedSmoke;
    public float minAngleSmoke;

    public GameObject nitroEffects; //Основные игровые объекты

    public GameObject BackLights;

    public GameObject Spots;
   
    [Range(0,1)]

    public float driftHelper = 0;
    public float Nitro;
 

    float horInput;
    float verInput;

    float Ysecrotation;
    bool IOnGround;

    Rigidbody rb;
    Quaternion startHelmRotation; //Стартовая позиция руля

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        startHelmRotation = Helm.localRotation;
     
    }

    void SwitchLight ()
    {
        BackLights.SetActive(false);

        if (Input.GetKey(KeyCode.Space)) //Включение задних фонарей
        {
            BackLights.SetActive(true);
        }
    }

    void SwitchForwardLights()
    {
        Spots.SetActive(false);
        if (Input.GetKey(KeyCode.Q)) //Включение передних фар
        {
            Spots.SetActive(true);
        }
    }

  


    void FixedUpdate()
    {
        OnGround();
        horInput = Input.GetAxis("Horizontal"); //-1-a   +1-d
        verInput = Input.GetAxis("Vertical");//-1-w   +1-d


        //Вызывать методы ТУТ

        Speed();
        ApplyNitro();
        ApplyBrake();
        DriftControl();
        SwitchLight();
        SwitchForwardLights();
    }

    void ApplyNitro()
    {
        if (Input.GetKey(KeyCode.LeftShift) && verInput>0.01f)
        {
            rb.AddForce(transform.forward * Nitro); //Проверка на Нитро
            nitroEffects.SetActive(true); 
        }

        else {
            if(nitroEffects.activeSelf)
            nitroEffects.SetActive(false);
        }
    }

    

    void DriftControl()
    {
        if (IOnGround == false)
        {
            return;
        }
        if (Mathf.Abs(transform.rotation.eulerAngles.y - Ysecrotation) < 10f)
        {
            float Floatresult = transform.rotation.eulerAngles.y - Ysecrotation * driftHelper;
            Quaternion rotHelp = Quaternion.AngleAxis(Floatresult, Vector3.up);
            rb.velocity = rotHelp * rb.velocity;
        }
        Ysecrotation = transform.rotation.eulerAngles.y;

    }

    void OnGround() //Проверка на приземление + контроль дрифта
    {
        IOnGround = true;
        foreach (WheelCollider wheelCol in wheelColliders)
        {
            if (!wheelCol.isGrounded)
            {
                IOnGround = false;
            }
        }
    }

    void Speed()
    {

        foreach (AxleInfo axle in carAxis)
        {
           

            if (axle.steering)
            {
                axle.rightWheel.steerAngle = steerAngle *horInput; //Поворот
                axle.leftWheel.steerAngle = steerAngle *horInput;
            }

          

            if (axle.motor)
            {
                axle.rightWheel.motorTorque = carSpeed * verInput;//Скорость
                axle.leftWheel.motorTorque = carSpeed * verInput ;
            }

            VisualRotation(axle.rightWheel, axle.visRightWheel); //Сопоставление мэша и коллайдера
            VisualRotation(axle.leftWheel, axle.visLeftWheel);
        }

        Helm.localRotation = startHelmRotation * Quaternion.Euler(Vector3.left * 90 * horInput); //Вращение руля

    }

    void VisualRotation(WheelCollider col, Transform visWheel)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        visWheel.position = position;
        visWheel.rotation = rotation;
    }

    void ApplyBrake()
    {
        foreach (AxleInfo axle in carAxis)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                axle.rightWheel.brakeTorque = 750000;
                axle.leftWheel.brakeTorque = 750000; //Ручной тормоз
            }

            else
            {
                axle.rightWheel.brakeTorque = 0;
                axle.leftWheel.brakeTorque = 0; //Ручной тормоз
            }
        }

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
