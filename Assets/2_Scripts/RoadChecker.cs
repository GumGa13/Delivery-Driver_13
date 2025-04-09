using UnityEngine;

public class RoadChecker : MonoBehaviour
{

    public bool isOnRoad;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
        {
            isOnRoad = true;
            Debug.Log("도로 진입");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
        {
            isOnRoad = false;
            Debug.Log("도로 벗어남");
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
