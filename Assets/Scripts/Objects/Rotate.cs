using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private RotationDirection rotationDirection = RotationDirection.clockwise;

    private void Update()
    {
        DoRotate();   
    }

    private void DoRotate()
    {
        switch (rotationDirection)
        {
            case RotationDirection.clockwise:
                transform.Rotate(0, 0, -360 * rotationSpeed * Time.deltaTime);
                break;
            case RotationDirection.anticlockwise:
                transform.Rotate(0, 0, 360 * rotationSpeed * Time.deltaTime);
                break;
            case RotationDirection.none:
                break;
        }
    }
}