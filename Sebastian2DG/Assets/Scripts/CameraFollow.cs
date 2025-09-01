using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private float delay; // higher = less delay, lower = more delay
    [SerializeField] private Vector3 offset;

    public Transform target;

    private Vector3 vel = Vector3.zero;

    void FixedUpdate()
    {

        Vector3 targetPosition = target.position + offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, delay);
    }
}
