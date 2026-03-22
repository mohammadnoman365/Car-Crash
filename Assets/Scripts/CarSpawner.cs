using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    public GameObject[] carsPrefab;
    public CameraMovement cameraMovement;
    public UIManager uiManager;
    public EndlessCity[] cityArray;
    public TrafficManager trafficManager;
    public LaneMovement laneMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCar();
    }

    void SpawnCar()
    {
        int currentCarIndex = PlayerPrefs.GetInt("CarIndexValue", 0);
        GameObject newCar  = Instantiate(carsPrefab[currentCarIndex], transform.position, transform.rotation);

        CarController carController = newCar.GetComponent<CarController>();
        cameraMovement.SetPlayerCarTransform(carController.transform);

        carController.SetUIManager(uiManager);
        uiManager.SetCarController(carController);

        foreach (EndlessCity city in cityArray)
        {
            city.SetPlayerCarTransform(carController.transform);
        }

        trafficManager.SetCarController(carController);
        laneMovement.SetPlayerCarTransform(carController.transform);
    }
}
