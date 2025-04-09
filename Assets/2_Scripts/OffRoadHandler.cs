using UnityEngine;

public class OffRoadHander : MonoBehaviour
{
    private bool isOnRoad = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            isOnRoad = true;
            Debug.Log("도로 진입");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            isOnRoad = false;
            Debug.Log("도로 벗어남");
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }
}
