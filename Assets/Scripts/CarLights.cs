using UnityEngine;
using UnityEngine.SceneManagement;

public class CarLights : MonoBehaviour
{
    [Header("Head & Tail Lights")]
    public GameObject frontRightLight;
    public GameObject frontLeftLight;
    public GameObject backRightLight;
    public GameObject backLeftLight;

    [Header("Brake Lights")]
    public GameObject brakeRightLight;
    public GameObject brakeLeftLight;

    private CarController carController;

    void Start()
    {
        carController = GetComponent<CarController>();

        // Turn on lights only if Night Scene
        bool isNight = SceneManager.GetActiveScene().name == "Night Scene";
        frontRightLight.SetActive(isNight);
        frontLeftLight.SetActive(isNight);
        backRightLight.SetActive(isNight);
        backLeftLight.SetActive(isNight);

        // Brake lights always start off
        brakeRightLight.SetActive(false);
        brakeRightLight.SetActive(false);
    }

    void Update()
    {
        bool isBraking = carController.isBrakePressed;
        brakeRightLight.SetActive(isBraking);
        brakeLeftLight.SetActive(isBraking);
    }
}