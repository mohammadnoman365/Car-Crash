using UnityEngine;
using UnityEngine.SceneManagement;

public class TrafficVehicleLights : MonoBehaviour
{
    [Header("Head & Tail Lights")]
    public GameObject frontRightLight;
    public GameObject frontLeftLight;
    public GameObject backRightLight;
    public GameObject backLeftLight;

    void Start()
    {

        bool isNight = SceneManager.GetActiveScene().name == "Night Scene";

        frontRightLight.SetActive(isNight);
        frontLeftLight.SetActive(isNight);
        backRightLight.SetActive(isNight);
        backLeftLight.SetActive(isNight);

    }
}