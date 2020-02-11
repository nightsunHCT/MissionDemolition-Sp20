using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // the static point of interest

    [Header("Set Dynamically")]
    public float camZ;

    void Awake()
    {
        camZ = this.transform.position.z;

    }

    void FixedUpdate()
    {
        // if there's only one line following an if, it doesn't need braces 
        if (POI == null) return; // return if there is no POI

        // Get the position of the poi
        Vector3 destination = POI.transform.position;
        // Foce destination.z to be camZ to keep teh camera far enough away
        destination.z = camZ;
        // set teh camera to the destination
        transform.position = destination;
    }
}
