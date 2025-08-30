using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float closingDistance = 0.1f;
    private int currWaypointIndex = 0;
    public float movementSpeed = 2f;

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        if (Vector2.Distance(waypoints[currWaypointIndex].position, transform.position) < closingDistance)
        {
            currWaypointIndex++;
            
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
        }
        
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currWaypointIndex].position, Time.deltaTime * movementSpeed);
    }
}