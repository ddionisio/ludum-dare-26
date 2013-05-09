using UnityEngine;
using System.Collections;

public class UIOptions : UIController {
    public string soundFormat = "SOUND: {0}";
    public string musicFormat = "MUSIC: {0}";

    public UILabel soundLabel;
    public UILabel musicLabel;

    public UIEventListener sound;
    public UIEventListener music;
    public UIEventListener exit;

    public string exitScene;

    private Player mPlayer;

    protected override void OnActive(bool active) {
    }

    protected override void OnOpen() {
        StartCoroutine(OnPause());
    }

    protected override void OnClose() {
        StopAllCoroutines();

        Main.instance.sceneManager.Resume();
    }

    // Use this for initialization
    void Start() {
        sound.onClick += SoundToggle;
        music.onClick += MusicToggle;
        exit.onClick += Exit;

        soundLabel.text = string.Format(soundFormat, Main.instance.userSettings.isSoundEnable ? "YES" : "NO");
        musicLabel.text = string.Format(musicFormat, Main.instance.userSettings.isMusicEnable ? "YES" : "NO");

        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        if(playerGo != null) {
            mPlayer = playerGo.GetComponentInChildren<Player>();
        }
    }

    IEnumerator OnPause() {
        yield return new WaitForFixedUpdate();

        Main.instance.sceneManager.Pause();

        yield break;
    }

    void SoundToggle(GameObject go) {
        Main.instance.userSettings.isSoundEnable = !Main.instance.userSettings.isSoundEnable;

        if(Main.instance.userSettings.isSoundEnable) {
            Main.instance.userSettings.volume = 1.0f;

            if(SoundPlayerGlobal.instance != null)
                SoundPlayerGlobal.instance.Play("tele");
        }

        soundLabel.text = string.Format(soundFormat, Main.instance.userSettings.isSoundEnable ? "YES" : "NO");
    }

    void MusicToggle(GameObject go) {
        Main.instance.userSettings.isMusicEnable = !Main.instance.userSettings.isMusicEnable;

        if(Main.instance.userSettings.isMusicEnable) {
            Main.instance.userSettings.volume = 1.0f;
        }

        musicLabel.text = string.Format(musicFormat, Main.instance.userSettings.isMusicEnable ? "YES" : "NO");
    }

    void Exit(GameObject go) {
        UIModalManager.instance.ModalCloseTop();

        if(mPlayer != null) {
            mPlayer.Exit();
        }
        else if(Application.loadedLevelName != exitScene) {
            Main.instance.sceneManager.LoadScene(exitScene);
        }
    }
}
