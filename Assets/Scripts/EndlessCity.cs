using UnityEngine;

public class EndlessCity : MonoBehaviour
{
    public Transform playerCarTransform;
    public Transform otherCityTransform;
    public float halfLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SetPlayerCarTransform(Transform carTransform)
    {
        playerCarTransform = carTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCarTransform.position.z > transform.position.z + (halfLength * 2))
        {
            transform.position = new Vector3(0, 0, otherCityTransform.position.z + (halfLength * 2)
            );
        }
    }
}
