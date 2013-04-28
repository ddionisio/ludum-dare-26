using UnityEngine;
using System.Collections;

public class PlayerLevelStartPoint : MonoBehaviour {

    private static PlayerLevelStartPoint mInstance;

    private string mWaypoint;
    private string mLevel;

    public static PlayerLevelStartPoint instance { get { return mInstance; } }

    public void Apply(string level, string waypoint) {
        mLevel = level;
        mWaypoint = waypoint;
    }

    public void Set(Player p) {
        if(WaypointManager.instance != null && Application.loadedLevelName == mLevel && !string.IsNullOrEmpty(mWaypoint)) {
            Transform wp = WaypointManager.instance.GetWaypoint(mWaypoint);
            if(wp != null)
                p.transform.position = wp.transform.position;
        }
    }

    void OnDestroy() {
        if(mInstance == this)
            mInstance = null;
    }

    void Awake() {
        //Object.DontDestroyOnLoad(gameObject);
        if(mInstance == null)
            mInstance = this;
    }
}
