using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockGroup : MonoBehaviour {
    public delegate void Listener(FlockUnit unit);

    public int id;

    public event Listener addCallback;
    public event Listener removeCallback;

    private static FlockGroup[] mGroups = new FlockGroup[32];

    private HashSet<FlockUnit> mUnits = new HashSet<FlockUnit>();

    public HashSet<FlockUnit> units {
        get { return mUnits; }
    }

    public int count {
        get { return mUnits.Count; }
    }

    //only call this after awake
    public static FlockGroup GetGroup(int id) {
        return mGroups[id];
    }

    public void AddUnit(FlockUnit unit) {
        //put in this group
        unit.transform.parent = transform;

        if(mUnits.Add(unit)) {
            unit.id = id;

            OnAddUnit(unit);

            if(addCallback != null) {
                addCallback(unit);
            }
        }
    }

    public void RemoveUnit(FlockUnit unit, Transform toParent) {
        if(mUnits.Remove(unit)) {
            if(toParent != null) {
                unit.transform.parent = toParent;
            }

            OnRemoveUnit(unit);

            if(removeCallback != null) {
                removeCallback(unit);
            }
        }
    }

    protected virtual void OnAddUnit(FlockUnit unit) {
    }

    protected virtual void OnRemoveUnit(FlockUnit unit) {
    }

    protected virtual void OnDestroy() {
        mGroups[id] = null;

        addCallback = null;
        removeCallback = null;
    }

    protected virtual void Awake() {
        mGroups[id] = this;
    }

    protected virtual void Start() {
        foreach(Transform t in transform) {
            FlockUnit unit = t.GetComponentInChildren<FlockUnit>();
            if(unit != null) {
                unit.id = id;
                AddUnit(unit);
            }
        }
    }

}
