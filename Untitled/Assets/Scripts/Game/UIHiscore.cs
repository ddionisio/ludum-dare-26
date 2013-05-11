using UnityEngine;
using System.Collections;

public class UIHiscore : MonoBehaviour {
    public const string scoreKey = "hi";

    public const string maxLevelSceneKey = "maxLevels";

    public const string timeParKeyFormat = "pex_par_{0}";

    public tk2dTextMesh hi;
    public tk2dTextMesh prev;

    public float hpCriteria = 50.0f; //if greater, bonus
    
    public float hpBonus = 100.0f; //per hp
    public float timeBonus = 50.0f; //per second
    public int flowerBonus = 500; //per flower
    public int levelCompleteBonus = 1000;

    // Use this for initialization
    void Start() {
        StartCoroutine(SetScore());
    }

    IEnumerator SetScore() {
        //ensure stuff are initialized...
        yield return new WaitForFixedUpdate();

        UserData ud = UserData.instance;

        int score = ComputeHiScore();

        int curHi = ud.GetInt(scoreKey);

        int hiTop, hiBottom;

        bool newHi = score > curHi;

        if(newHi) {
            ud.SetInt(scoreKey, score);

            hiTop = score;
            hiBottom = curHi;
        }
        else {
            hiTop = curHi;
            hiBottom = score;
        }

        if(newHi) {
            hi.text = string.Format("HI {0} *", hiTop.ToString("D7"));
            prev.text = hiBottom.ToString("D7");
        }
        else {
            hi.text = string.Format("HI {0}", hiTop.ToString("D7"));
            prev.text = string.Format("{0} *", hiBottom.ToString("D7"));
        }

        hi.Commit();
        prev.Commit();

        yield break;
    }
    
    private int ComputeHiScore() {
        SceneState ss = SceneState.instance;

        int score = 0;
        int maxLevels = ss.GetGlobalValue(maxLevelSceneKey);
                
        //go through levels
        for(int i = 0; i < maxLevels; i++) {
            int level = i + 1;

            bool levelComplete = ss.CheckGlobalFlag(PlayerActChangeScene.playerGameExState, level);
            int numFlowers = PlayerActChangeScene.GetFlowerValue(level);
            float lowestHP = PlayerActChangeScene.GetLowestHealthValue(level);
            
            float secs = PlayerActChangeScene.GetTimeValue(level);
            float secsPar = ss.GetGlobalValueFloat(string.Format(timeParKeyFormat, level));

            if(levelComplete)
                score += levelCompleteBonus;
                        
            score += flowerBonus * numFlowers;

            //bonus for time and hits is only if all flowers are collected for that level
            if(ss.CheckGlobalFlag(PlayerActChangeScene.playerLevelMaxedFlowers, level)) {
                if(secs < secsPar)
                    score += Mathf.RoundToInt((secsPar - secs) * timeBonus);

                if(lowestHP > hpCriteria)
                    score += Mathf.RoundToInt((lowestHP - hpCriteria) * hpBonus);
            }
        }

        return score;
    }
}
