using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private Singleton 

    [Header("Set in Inspector")]
    public Text uitLevel; // UIText_Level
    public Text uitShots; // UIText_Shots
    public Text uitButton; // Text on UITButton_view
    public Vector3 castlePos; // the place to put castles
    public GameObject[] castles; // array of castles

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode

    void Start()
    {
        S = this; // define the Singleton
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        // get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        // Destroy old projectiles if they exits 
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // instantiate new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI(); 

        // check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // chaneg mdoe to stop checking for level end
            mode = GameMode.levelEnd;
            // zoom out
            SwitchView("Show Both");
            // Start the next level in 2 second
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }

        StartLevel();
    }

    public void SwitchView (string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired ()
    {
        S.shotsTaken++;
    }
}
