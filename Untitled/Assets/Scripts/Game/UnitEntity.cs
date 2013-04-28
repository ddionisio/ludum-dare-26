using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitEntity : UnitBaseEntity {
    public string waypointFollow;
    public bool isLeader = false;
        
    private FlockUnit mFlockUnit;
    private ActionListener mListener;

    public FlockUnit flockUnit { get { return mFlockUnit; } }
    public ActionListener listener { get { return mListener; } }

    public override int flockId { get { return mFlockUnit.id; } }
    
    /*public Player owner {
        get {
            if(mFlockUnit != null) {
                int index = Player.GroupToIndex(mFlockUnit.type);
                if(index >= 0 && index < Player.playerCount) {
                    return Player.GetPlayer(index);
                }
            }

            return null;
        }
    }*/

    protected override void Awake() {
        base.Awake();

        mFlockUnit = GetComponentInChildren<FlockUnit>();
        mListener = GetComponentInChildren<ActionListener>();

        if(mFlockUnit != null) {
            mFlockUnit.groupMoveEnabled = false;
        }

        if(mListener != null) {
            mListener.enterCallback += OnActionEnter;
            mListener.hitEnterCallback += OnActionHitEnter;
            mListener.hitExitCallback += OnActionHitExit;
            mListener.finishCallback += OnActionFinish;
        }
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();

        if(isLeader) {
            actionTarget.type = ActionType.Follow;

            FlockGroup grp = FlockGroup.GetGroup(flockId);
            if(grp != null) {
                grp.addCallback += OnGroupAdd;
                grp.removeCallback += OnGroupRemove;

                //add current entities from grp
                foreach(FlockUnit unit in grp.units) {
                    OnGroupAdd(unit);
                }
            }
        }
    }

    protected override void OnDespawned() {
        ClearData();

        base.OnDespawned();
    }

    public override void SpawnFinish() {
        FlockUnitInit();

        if(mListener != null) {
            mListener.SetActive(true);
        }
    }

    protected override void ActivatorWakeUp() {
        if(!doSpawnOnWake) {
            SpawnFinish();
        }

        base.ActivatorWakeUp();
    }

    protected override void ActivatorSleep() {
        base.ActivatorSleep();

        ClearData();
    }

    protected override void SetBlink(bool blink) {
    }

    protected override void OnDestroy() {
        if(isLeader) {
            FlockGroup grp = FlockGroup.GetGroup(flockId);
            if(grp != null) {
                grp.addCallback -= OnGroupAdd;
                grp.removeCallback -= OnGroupRemove;
            }
        }

        ClearData();

        if(mListener != null) {
            mListener.enterCallback -= OnActionEnter;
            mListener.hitEnterCallback -= OnActionHitEnter;
            mListener.hitExitCallback -= OnActionHitExit;
            mListener.finishCallback -= OnActionFinish;
        }

        base.OnDestroy();
    }

    //optional implements of callbacks

    protected virtual void OnActionEnter(ActionListener listen) {
        if(FSM != null) {
            FSM.SendEvent(EntityEvent.ActionEnter);
        }
    }

    protected virtual void OnActionFinish(ActionListener listen) {
        if(FSM != null) {
            FSM.SendEvent(EntityEvent.ActionFinish);
        }
    }

    protected virtual void OnActionHitEnter(ActionListener listen, ContactPoint info) {
        if(FSM != null) {
            FSM.SendEvent(EntityEvent.ActionHitEnter);
        }
    }

    protected virtual void OnActionHitExit(ActionListener listen) {
        if(FSM != null) {
            FSM.SendEvent(EntityEvent.ActionHitExit);
        }
    }

    private void ClearData() {
        if(mListener != null) {
            mListener.SetActive(false);
        }

        FlockUnitRelease();
    }

    private void FlockUnitInit() {
        //add to group
        //remove from group if it still exists
        if(mFlockUnit != null) {
            FlockGroup grp = FlockGroup.GetGroup(mFlockUnit.id);
            if(grp != null) {
                grp.AddUnit(mFlockUnit);
            }
        }
    }

    private void FlockUnitRelease() {
        //remove from group if it still exists
        if(mFlockUnit != null) {
            FlockGroup grp = FlockGroup.GetGroup(mFlockUnit.id);
            if(grp != null) {
                grp.RemoveUnit(mFlockUnit, null);
            }

            mFlockUnit.ResetData();
        }
    }

    void OnGroupAdd(FlockUnit flockUnit) {
        UnitEntity unit = flockUnit.GetComponent<UnitEntity>();
        if(unit != null && unit != this) {
            FlockActionController actionListen = unit.listener as FlockActionController;
            if(actionListen != null && actionListen.leader == null) {
                actionListen.defaultTarget = actionTarget;
                actionListen.leader = transform;
            }
        }
    }

    void OnGroupRemove(FlockUnit flockUnit) {
        UnitEntity unit = flockUnit.GetComponent<UnitEntity>();
        if(unit != null) {
            FlockActionController actionListen = unit.listener as FlockActionController;
            if(actionListen != null && actionListen.leader == transform) {
                actionListen.defaultTarget = null;
                actionListen.leader = null;
            }
        }
    }
}
