using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Tooltip ("The target the camera will follow. Defaults to finding the Player by tag.")]
    [SerializeField] private Transform target;

    [Tooltip ("Sets the bounds of camera movement in the X-Axis")]
    public Vector2 minMaxX;
    [Tooltip ("Sets the bounds of camera movement in the Y-Axis")]
    public Vector2 minMaxY;
    [Tooltip ("Sets dead zone in which the camera won't move")]
    public Vector2 axisBuffer = Vector2.zero;
    [Tooltip ("Offsets the origin of focus on the target")]
    [SerializeField] private Vector3 offset;
    [Tooltip ("Sets the ammount of smoothing applied to the camera movement. 0 being no smoothing and 1 being max smoothing")]
    [SerializeField] [Range (0, 1)] private float smoothing = 0.9f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (!target)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        //Get the wanted position
        Vector3 wantedPosition = target.position + offset;
        wantedPosition.z = transform.position.z;

        //Create a pocket in which the player can move freely without the camera moving
        if (target.position.x > transform.position.x - axisBuffer.x && target.position.x < transform.position.x + axisBuffer.x)
        {
            //If the player is within the pocket, set the axis to cameras own position
            wantedPosition.x = transform.position.x;
        }

        //Same as above but for y axis
        if (target.position.y > transform.position.y - axisBuffer.y && target.position.y < transform.position.y + axisBuffer.y)
        {
            wantedPosition.y = transform.position.y;
        }

        //Clamp to level bounds
        wantedPosition.x = Mathf.Clamp(wantedPosition.x, minMaxX.x, minMaxX.y);
        wantedPosition.y = Mathf.Clamp(wantedPosition.y, minMaxY.x, minMaxY.y);

        //Smoothly apply following
        transform.position = Vector3.SmoothDamp(transform.position, wantedPosition, ref velocity, smoothing);
    }
}
