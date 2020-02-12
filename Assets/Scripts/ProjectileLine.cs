﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // singleton

    [Header("Set in Inspector")]
    public float minDist = .1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this; // set the singleton
        // get a reference to the LineRenderer
        line = GetComponent<LineRenderer>();
        // Disable the LineRenderer until it's needed
        line.enabled = false;
        // Initialize the point list
        points = new List<Vector3>();

    }

    // this is a property (that is, a method maquerading as a field) 
    public GameObject poi
    {
        get
        {
            return (_poi);
        } set
        {
            _poi = value;
            if (_poi != null)
            {
                // when _poi is set to something new, it resets everything 
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // this can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        // this is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // if the point isn't far enough from the last point, it returns
            return;
        }

        if (points.Count == 0) // if this is the launch point
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // tbd
            // .. it adds and extra bit of line to aid aiming later
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // set the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // enables the LineRenderer
            line.enabled = true;
        } else
        {
            // normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    // Returns the location of the most recently added points 
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // if there are no points, return Vector3.zero
                return (Vector3.zero); 
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            // if there is no poi, search for one 
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                } else
                {
                    return; // return if no poi can be found
                }
            } else
            {
                return; // if no poi found
            }
        }

        AddPoint();
        if (FollowCam.POI == null)
        {
            // Once FollowCam.POI is null, make the local poi null too
            poi = null; 
        }

    }

}
