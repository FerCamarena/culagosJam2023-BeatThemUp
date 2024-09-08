using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class _SettingsManager : MonoBehaviour {
    public GameObject combos;

    [SerializeField] private Slider masterVolumeSlider;

    [SerializeField] private Button masterVolumeButton;

    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;

    private void Awake() {
        LoadPreferences();
        InitialVisualsUpdate();
    }

    public void LoadPreferences() {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 50.0f);
    }
    public void InitialVisualsUpdate() {
        UpdateMasterVisuals();
    }
    public void UpdateMasterVolume()
    {
        //Updating the PlayerPref for master value
        PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
        //Sending the values to the Master mix channel
        masterMixer.SetFloat("mixerMaster", masterVolumeSlider.value - 100);
        //Updating master mute button visuals from input
        UpdateMasterVisuals();
    }
    public void ToggleMasterVolume() {
        //Toggling between mute and unmute Master channel based on current value
        if (masterVolumeSlider.value > 0f) {
            //Saving the previous Master volume into preferences
            PlayerPrefs.SetFloat("previousMasterVolume", masterVolumeSlider.value);
            //Setting the Master channel as muted
            masterVolumeSlider.value = 0f;
        } else {
            //Loading the previous Master volume from preferences
            masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 50.0f);
        }
        //Sending the values to the Master mix channel
        masterMixer.SetFloat("mixerMaster", masterVolumeSlider.value - 100);
        //Updating Master mute button visuals from current values
        UpdateMasterVisuals();
    }
    public void UpdateMasterVisuals(){
        //Checks if the Master volume value is more than 0
        if (masterVolumeSlider.value > 0f) {
            //Changing the button sprite for the unmuted one
            masterVolumeButton.image.sprite = unmutedSprite;
        } else {
            //Changing the button sprite for the muted one
            masterVolumeButton.image.sprite = mutedSprite;
        }
    }
    public void CloseSettings() {
        PlayerPrefs.Save();
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("SettingsInGame");
    }
    public void SettingsGoMenu() {
        SceneManager.LoadScene(0);
        Destroy(Camera.main.gameObject);
        CloseSettings();
    }
    public void DisplayCombos() {
        combos.SetActive(true);
    }
    public void HideCombos() {
        combos.SetActive(false);
    }
}