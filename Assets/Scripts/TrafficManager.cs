using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class TrafficManager : MonoBehaviour
{
    public Transform[] lane;
    public GameObject[] trafficVehicle;
    public CarController carController;
    public float minSpawnTime = 30f;
    public float maxSpawnTime = 60f;
    private float dynamicTimer = 2f;

    private float[] laneCooldown; // Cooldown timer per lane

    void Start()
    {
        laneCooldown = new float[lane.Length]; // Initialize cooldowns
        StartCoroutine(TrafficSpawner());
    }

    public void SetCarController(CarController controller)
    {
        carController = controller;
    }

    IEnumerator TrafficSpawner()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (carController.CarSpeed() > 20f)
            {
                dynamicTimer = Random.Range(minSpawnTime, maxSpawnTime) / carController.CarSpeed();
                SpawnTrafficVehicle();
            }
            yield return new WaitForSeconds(dynamicTimer);
        }
    }

    void SpawnTrafficVehicle()
    {
        // Get all lanes that are NOT on cooldown
        List<int> availableLanes = new List<int>();

        for (int i = 0; i < lane.Length; i++)
        {
            if (Time.time > laneCooldown[i])
                availableLanes.Add(i);
        }

        // At least 1 lane must be free for player to pass
        if (availableLanes.Count <= 1) return;

        // Pick random lane from available lanes
        int randomLaneIndex = availableLanes[Random.Range(0, availableLanes.Count)];
        int randomVehicleIndex = Random.Range(0, trafficVehicle.Length);

        Instantiate(trafficVehicle[randomVehicleIndex], lane[randomLaneIndex].position, Quaternion.identity);

        laneCooldown[randomLaneIndex] = Time.time + 3f; // 3 sec cooldown for this lane
    }
}