using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject followTarget;

     void LateUpdate()
    {
        transform.position = followTarget.transform.position + new Vector3(0, 0, -10);
    }
}
