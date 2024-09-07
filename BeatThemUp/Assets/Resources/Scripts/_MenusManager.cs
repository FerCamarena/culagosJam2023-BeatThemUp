using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class _MenusManager : MonoBehaviour {
    [SerializeField] private AudioMixer masterMixer;
    private void Awake() {
        masterMixer.SetFloat("mixerMaster", PlayerPrefs.GetFloat("masterVolume") - 100);
    }
    public void MenuStartButton() {
        SceneManager.LoadScene("Observatory");
    }
    public void MenuSettingstButton() {
        SceneManager.LoadSceneAsync("Settings", LoadSceneMode.Additive);
        Time.timeScale = 0.0f;
    }
    public void SettingsReturnButton()
    {
        Time.timeScale = 1.0f;
        PlayerPrefs.Save();
        SceneManager.UnloadSceneAsync("Settings");
    }
    public void SettingsQuitButton() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    private void Update() {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Time.timeScale == 1.0f)
            {
                MenuSettingstButton();

            }
            else
            {
                SettingsReturnButton();
            }
        }
    }
}