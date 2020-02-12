using System.Collections;
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

}
