using UnityEngine;
using System.Collections;

public class EndSpawnFlowers : MonoBehaviour {
    public const string angelFlowersKey = "angel";

    //public int 

    public GameObject flowerPrefab;

    public Transform playerFlowerContainer;
    public Transform angelFlowerContainer;

    public float scatterRadius = 2.0f;

    public int flowerLimit = 50;

    private Player mPlayer;

    void Awake() {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        mPlayer = playerGO.GetComponentInChildren<Player>();
    }

	// Use this for initialization
	void Start () {
        //allow others to start
        StartCoroutine(DoIt());
	}

    IEnumerator DoIt() {
        yield return new WaitForFixedUpdate();

        SceneState ss = SceneState.instance;
        UserData ud = UserData.instance;

        int numPlayerFlowers = 0;
        int numAngelFlowers = ud.GetInt(angelFlowersKey);

        int maxLevels = ss.GetGlobalValue(UIHiscore.maxLevelSceneKey);

        //go through levels
        for(int i = 0; i < maxLevels; i++) {
            int level = i + 1;

            int numFlowers = PlayerActChangeScene.GetFlowerValue(level);

            numPlayerFlowers += numFlowers;
        }

        int playerSpawnFlowers = numPlayerFlowers - numAngelFlowers;

        if(playerSpawnFlowers > 0) {
            for(int i = 0; i < playerSpawnFlowers; i++) {
                SpawnFlower(playerFlowerContainer, true);
            }
        }

        if(numAngelFlowers > 0) {
            if(numAngelFlowers > flowerLimit)
                numAngelFlowers = flowerLimit;

            for(int i = 0; i < numAngelFlowers; i++) {
                SpawnFlower(angelFlowerContainer, false);
            }
        }
        
        yield break;
    }

    private void SpawnFlower(Transform holder, bool toPlayer) {
        GameObject go = (GameObject)GameObject.Instantiate(flowerPrefab);

        Transform t = go.transform;

        t.parent = holder;

        Vector2 ofs = Random.insideUnitCircle * scatterRadius;

        if(toPlayer) {
            Vector3 pos = mPlayer.transform.position;
            pos.x += ofs.x;
            pos.y += ofs.y;
            pos.z = holder.position.z;

            t.position = pos;

            FlockUnit fu = go.GetComponentInChildren<FlockUnit>();
            fu.moveTarget = mPlayer.transform;
            mPlayer.flowers.Add(t);
        }
        else {
            Vector3 pos = holder.position;
            pos.x += ofs.x;
            pos.y += ofs.y;

            t.position = pos;

            FollowPlayerSensor fps = go.GetComponentInChildren<FollowPlayerSensor>();
            fps.gameObject.SetActive(false);

            FlockUnit fu = go.GetComponentInChildren<FlockUnit>();
            fu.moveTarget = holder;
        }
    }
}
