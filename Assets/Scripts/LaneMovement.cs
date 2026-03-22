using UnityEngine;

public class LaneMovement : MonoBehaviour
{
    public Transform playerCarTransform;
    public float Offset = -6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SetPlayerCarTransform(Transform carTransform)
    {
        playerCarTransform = carTransform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerCarTransform == null)
        {
            return;
        }

        Vector3 cameraPosition = transform.position;
        cameraPosition.z = playerCarTransform.position.z + Offset;
        transform.position = cameraPosition;
    }
}
