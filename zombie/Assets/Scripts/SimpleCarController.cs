using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public GameObject leftWheelVisuals;
    private bool leftGrounded = false;
    private float travelL = 0f;
    private float leftAckermanCorrectionAngle = 0;

    public WheelCollider rightWheel;
    public GameObject rightWheelVisuals;
    private bool rightGrounded = false;
    private float travelR = 0f;
    private float rightAckermanCorrectionAngle = 0;

    public bool motor;
    public bool steering;

    public float Antiroll = 10000;
    private float AntrollForce = 0;

    public float ackermanSteering = 1f;

    
    public void ApplyLocalPositionToVisuals()
    {
        //left wheel
        if (leftWheelVisuals == null)
        {
            return;
        }
        Vector3 position;
        Quaternion rotation;
        leftWheel.GetWorldPose(out position, out rotation);

        leftWheelVisuals.transform.position = position;
        leftWheelVisuals.transform.rotation = rotation;

        //right wheel
        if (rightWheelVisuals == null)
        {
            return;
        }

        rightWheel.GetWorldPose(out position, out rotation);

        rightWheelVisuals.transform.position = position;
        rightWheelVisuals.transform.rotation = rotation;
    }
    public void CalculateAndApplyAntiRollForce(Rigidbody theBody)
    {
        WheelHit hit;

        leftGrounded = leftWheel.GetGroundHit(out hit);
        if (leftGrounded)
        {
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
        }
        else
        {
            travelL = 1f;
        }

        rightGrounded = rightWheel.GetGroundHit(out hit);
        if (rightGrounded)
        {
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
        }
        else
        {
            travelR = 1f;
        }

        AntrollForce = (travelL - travelR) *Antiroll;

        if (leftGrounded)
        {
            theBody.AddForceAtPosition(leftWheel.transform.up * -AntrollForce, leftWheel.transform.position);
        }

        if (rightGrounded)
        {
            theBody.AddForceAtPosition(rightWheel.transform.up * AntrollForce, rightWheel.transform.position);
        }
    }
    public void CalculateAndApplySteering(float input, float maxSteerAngle, List<AxleInfo> allAxles)
    {
        //first find farest axle, we got to apply default values
        AxleInfo farestAxle = allAxles[0];
        //calculate start point for checking
        float farestAxleDistantion = ((allAxles[0].leftWheel.transform.localPosition - allAxles[0].rightWheel.transform.localPosition) / 2f).z;
        for (int a = 0; a < allAxles.Count; a++)
        {
            float theDistance = ((allAxles[a].leftWheel.transform.localPosition - allAxles[a].rightWheel.transform.localPosition)/ 2f).z;
        // if we found axle that farer – save it
        if (theDistance < farestAxleDistantion)
        {
            farestAxleDistantion = theDistance;
            farestAxle = allAxles[a];
        }
    }
    float wheelBaseWidth = (Mathf.Abs(leftWheel.transform.localPosition.x) + Mathf.Abs(rightWheel.transform.localPosition.x)) / 2;
    float wheelBaseLength = Mathf.Abs(((farestAxle.leftWheel.transform.localPosition + farestAxle.rightWheel.transform.localPosition) / 2f).z) +
        Mathf.Abs(((leftWheel.transform.localPosition + rightWheel.transform.localPosition) / 2f).z);

    float angle = maxSteerAngle * input;
    //ackerman implementation
    float turnRadius = Mathf.Abs(wheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(angle))));
        //38.363
        if (input != 0)
        {
            //right wheel
            if (angle > 0)
                {//turn right

                rightAckermanCorrectionAngle = Mathf.Rad2Deg* Mathf.Atan(wheelBaseLength / (turnRadius - wheelBaseWidth / 2f));
                rightAckermanCorrectionAngle = (rightAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                rightAckermanCorrectionAngle = Mathf.Sign(angle) * rightAckermanCorrectionAngle;

}
            else
                {//turn left

                rightAckermanCorrectionAngle = Mathf.Rad2Deg* Mathf.Atan(wheelBaseLength / (turnRadius + wheelBaseWidth / 2f));
                rightAckermanCorrectionAngle = (rightAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                rightAckermanCorrectionAngle = Mathf.Sign(angle) * rightAckermanCorrectionAngle;

            }     
         
            //left wheel
            if (angle > 0)
            {//turn right
                leftAckermanCorrectionAngle = Mathf.Rad2Deg* Mathf.Atan(wheelBaseLength / (turnRadius + wheelBaseWidth / 2f));
                leftAckermanCorrectionAngle = (leftAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                leftAckermanCorrectionAngle = Mathf.Sign(angle) * leftAckermanCorrectionAngle;
            }
            else
            {//turn left
                leftAckermanCorrectionAngle = Mathf.Rad2Deg* Mathf.Atan(wheelBaseLength / (turnRadius - wheelBaseWidth / 2f));
                leftAckermanCorrectionAngle = (leftAckermanCorrectionAngle - Mathf.Abs(angle)) * ackermanSteering + (Mathf.Abs(angle));
                leftAckermanCorrectionAngle = Mathf.Sign(angle) * leftAckermanCorrectionAngle;
            }
      
        }
        else
        {
            rightAckermanCorrectionAngle = 0f;
            leftAckermanCorrectionAngle = 0f;
        }
        leftWheel.steerAngle = leftAckermanCorrectionAngle;
        rightWheel.steerAngle = rightAckermanCorrectionAngle;
               
    }
}

[RequireComponent(typeof(Rigidbody))]
public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float BreakForce;
    public float speed, rotation;

    private Rigidbody body;
    private GameObject player;
    public GameObject playerCanvas, carCanvas, carCamera, sitButton;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        for (int a = 0; a < axleInfos.Count; a++)
        {
            axleInfos[a].leftWheel.ConfigureVehicleSubsteps(5, 12, 15);
            axleInfos[a].rightWheel.ConfigureVehicleSubsteps(5, 12, 15);
        }
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * speed;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.CalculateAndApplySteering(rotation, maxSteeringAngle, axleInfos);
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor; 
            }
            axleInfo.ApplyLocalPositionToVisuals();
            axleInfo.CalculateAndApplyAntiRollForce(body);
        }
    }

    public void rotate(int i)
    {
        rotation = i;
    }

    public void SitInCar()
    {
        carCanvas.SetActive(true);
        playerCanvas.SetActive(false);
        carCamera.SetActive(true);
        player.SetActive(false);
    }

    public void SitOutCar()
    {
        carCanvas.SetActive(false);
        playerCanvas.SetActive(true);
        carCamera.SetActive(false);
        player.SetActive(true);
        player.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 3);
    }

    public void turn(int i)
    {
        speed = i;
        if (i == 0)
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = BreakForce;
                axleInfo.rightWheel.brakeTorque = BreakForce;
            }
        }
        else
        {
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }
        }
    }

    public void StopDown()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = BreakForce;
            axleInfo.rightWheel.brakeTorque = BreakForce;
        }
        speed = 0;
    }

    public void StopUp()
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = 0;
            axleInfo.rightWheel.brakeTorque = 0;
        }
    }

    public void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance < 6f)
        {
            sitButton.SetActive(true);
        }
        else
        {
            sitButton.SetActive(false);
        }
    }
}
