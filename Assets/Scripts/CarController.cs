using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider BLWheelCollider;
    public WheelCollider BRWheelCollider;
    public WheelCollider FLWheelCollider;
    public WheelCollider FRWheelCollider;

    [Header("Wheel Transforms")]
    public Transform BLWheelTransform;
    public Transform BRWheelTransform;
    public Transform FLWheelTransform;
    public Transform FRWheelTransform;

    [Header("Car Settings")]
    public Transform carCenterOfMassTransform;
    public float motorForce = 100f;
    public float maxSteeringAngle = 20f;
    public float brakeForce = 1000f;

    public TrailRenderer BLSkidMark;
    public TrailRenderer BRSkidMark;

    public UIManager uiManager;

    private Rigidbody carRigidbody;
    private float verticalInput;
    private float horizontalInput;

    [Header("Audio Settings")]
    public AudioSource engineSound;
    public AudioSource brakeSound;
    public AudioClip crashSound;

    private bool isGasPressed;
    public bool isBrakePressed;
    private bool isMoveRightPressed;
    private bool isMoveLeftPressed;
    private bool wasBrakePressed = false;

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        engineSound.mute = AudioManager.Instance.IsSFXMuted;

        brakeSound.mute = AudioManager.Instance.IsSFXMuted;

        carRigidbody.centerOfMass = carCenterOfMassTransform.localPosition;
    }

    void Update()
    {

        float speed = CarSpeed(); 
        float pitch = Mathf.Lerp(0.5f, 2.0f, speed / 120f); 
        engineSound.pitch = pitch;
    }

    public void SetUIManager(UIManager manager)
    {
        uiManager = manager;
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        MotorForce();
        UpdateWheels();
        GetInput();
        Steering();
        ApplyBrakes();
        PowerStreeing();
    }

    void GetInput()
    {
        verticalInput = isGasPressed ? 1f : 0f;
        horizontalInput = isMoveRightPressed ? 1f : isMoveLeftPressed ? -1f : 0f;
    }

    public void OnGasPress()
    {
        isGasPressed = true;
    }

    public void OnGasRelease()
    {
        isGasPressed = false;
    }

    public void OnBrakePress()
    {
        isBrakePressed = true;
    }

    public void OnBrakeRelease()
    {
        isBrakePressed = false;
    }

    public void OnMoveRightPress()
    {
        isMoveRightPressed = true;
    }

    public void OnMoveRightRelease()
    {
        isMoveRightPressed = false;
    }

    public void OnMoveLeftPress()
    {
        isMoveLeftPressed = true;
    }

    public void OnMoveLeftRelease()
    {
        isMoveLeftPressed = false;
    }


    void ApplyBrakes()
    {
        bool shouldBrake = isBrakePressed && CarSpeed() > 5f;

        if (shouldBrake)
        {
            BLSkidMark.emitting = true;
            BRSkidMark.emitting = true;

            if (!brakeSound.isPlaying)
            {
                brakeSound.loop = true;
                brakeSound.Play();
            }

            BLWheelCollider.brakeTorque = brakeForce;
            BRWheelCollider.brakeTorque = brakeForce;
            FLWheelCollider.brakeTorque = brakeForce;
            FRWheelCollider.brakeTorque = brakeForce;
            carRigidbody.linearDamping = 1f;
        }
        else
        {
            BLSkidMark.emitting = false;
            BRSkidMark.emitting = false;

            if (brakeSound.isPlaying)
            {
                brakeSound.Stop();
            }

            BLWheelCollider.brakeTorque = 0f;
            BRWheelCollider.brakeTorque = 0f;
            FLWheelCollider.brakeTorque = 0f;
            FRWheelCollider.brakeTorque = 0f;
            carRigidbody.linearDamping = 0f;
        }

        wasBrakePressed = shouldBrake;
    }


    void MotorForce()
    {
        FLWheelCollider.motorTorque = motorForce * verticalInput;
        FRWheelCollider.motorTorque = motorForce * verticalInput;
    }

    void Steering()
    {
        FLWheelCollider.steerAngle = maxSteeringAngle * horizontalInput;
        FRWheelCollider.steerAngle = maxSteeringAngle * horizontalInput;
    }

    void PowerStreeing()
    {
        if (horizontalInput == 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
        }
    }

    void UpdateWheels()
    {
        RotateWheel(BLWheelCollider, BLWheelTransform);
        RotateWheel(BRWheelCollider, BRWheelTransform);
        RotateWheel(FLWheelCollider, FLWheelTransform);
        RotateWheel(FRWheelCollider, FRWheelTransform);
    }

    void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    public float CarSpeed()
    {
        return carRigidbody.linearVelocity.magnitude * 3.6f; // Convert from m/s to km/h
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TrafficVehicle"))
        {
            AudioManager.Instance.PlaySFX(crashSound);

            engineSound.Stop();
            brakeSound.Stop();

            uiManager.GameOver();
        }
    }
}
