using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerActOfferFlowers : PlayerActSensor {
    public GameObject activateEnter;

    public GameObject activateOffer;

    public Transform offerHolder;

    protected override void UnitEnter(PlayerController unit) {
        base.UnitEnter(unit);

        activateEnter.SetActive(true);
    }

    protected override void UnitExit(PlayerController unit) {
        base.UnitExit(unit);

        activateEnter.SetActive(false);
    }

    //called by player controller when they hit action
    public override void Action(PlayerController ctrl) {
        base.Action(ctrl);

        List<Transform> flowers = ctrl.player.flowers;

        if(flowers.Count > 0) {
            UserData ud = UserData.instance;

            int numAngelFlowers = ud.GetInt(EndSpawnFlowers.angelFlowersKey);

            foreach(Transform flower in flowers) {
                FlockUnit fu = flower.GetComponentInChildren<FlockUnit>();
                fu.moveTarget = offerHolder;

                FollowPlayerSensor fps = flower.GetComponentInChildren<FollowPlayerSensor>();
                fps.gameObject.SetActive(false);

                numAngelFlowers++;
            }

            ud.SetInt(EndSpawnFlowers.angelFlowersKey, numAngelFlowers);

            flowers.Clear();

            activateOffer.SetActive(true);
        }
    }

    void Start() {
        activateEnter.SetActive(false);
        activateOffer.SetActive(false);
    }
}
