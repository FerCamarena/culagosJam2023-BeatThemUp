using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal.Internal;

public class _GameManager : MonoBehaviour {
    public float respawnPlayer1Time;
    public float respawnPlayer2Time;

    public int jugador1ciclo;
    public int jugador2ciclo;
    public int jugadores;
    public int nivel = 0;

    public bool lobby = true;

    public Sprite[] splashes;
    public Camera camara;
    public GameObject splash;
    public GameObject hold1;
    public GameObject hold2;
    public GameObject hold3;
    public GameObject hold4;
    public GameObject portal;
    public GameObject jugador1;
    public GameObject jugador2;
    public GameObject player1UI;
    public GameObject player2UI;
    public GameObject vida1;
    public GameObject vida2;
    public GameObject[] personajes1;
    public GameObject[] personajes2;
    void Start()
    {
        splash.SetActive(true);
        splash.GetComponent<Image>().sprite = this.splashes[nivel];
        nivel++;
        DontDestroyOnLoad(Camera.main);
        DontDestroyOnLoad(portal);
    }
    void Update()
    {
        //off menu
        if (Input.GetKeyDown(KeyCode.Escape) && lobby) {
            
            SceneManager.LoadSceneAsync("SettingsInGame", LoadSceneMode.Additive);
            Time.timeScale = 0.0f;
            lobby = false;
        } else if (Input.GetKeyDown(KeyCode.Escape) && !lobby) {
            lobby = true;
            Time.timeScale = 1.0f;
            SceneManager.UnloadSceneAsync("SettingsInGame");
        }

        //in lobby
        if (lobby) {
            //Checking for portal collisions
            if (jugador1)
            {
                vida1.GetComponent<Slider>().value = jugador1.GetComponent<Jugador1>().currentHealth;
                if (Vector2.Distance(portal.transform.position, jugador1.transform.position) < 4) {
                    if (jugador2)
                    {
                        vida2.GetComponent<Slider>().value = jugador2.GetComponent<Jugador2>().currentHealth;
                        if (Vector2.Distance(portal.transform.position, jugador2.transform.position) < 4) {
                            portal.transform.position = new Vector3(138.0f, 0f, 7.5f);
                            lobby = false;
                            SceneManager.LoadScene(4);
                            splash.SetActive(true);
                            splash.GetComponent<Image>().sprite = this.splashes[0];
                            jugador1.transform.position = new Vector3(-3f, 1f, 0f);
                            jugador2.transform.position = new Vector3(-3f, -1f, 0f);
                            PlayerPrefs.SetInt("LastLevel", PlayerPrefs.GetInt("LastLevel", 0) + 1);
                            PlayerPrefs.Save();
                        }
                    } else {
                        portal.transform.position = new Vector3(138.0f, 0f, 7.5f);
                        lobby = false;
                        SceneManager.LoadScene(4);
                        splash.SetActive(true);
                        jugador1.transform.position = new Vector3(-3f, 1f, 0f);
                        splash.GetComponent<Image>().sprite = this.splashes[0];
                        PlayerPrefs.SetInt("LastLevel", PlayerPrefs.GetInt("LastLevel", 0) + 1);
                        PlayerPrefs.Save();
                    }
                }
            }

            //Updating UI
            if (!jugador1 ) {
                hold1.SetActive(true);
                hold3.SetActive(false);
                player1UI.SetActive(false);
                jugadores = 0;
            } else {
                jugadores = 1;
                hold1.SetActive(false);
                if (lobby)
                {
                    hold3.SetActive(true);
                }
                player1UI.SetActive(true);
            }
            if (jugador1) {
                if(!jugador2)
                {
                    hold2.SetActive(true);
                    hold4.SetActive(false);
                    player2UI.SetActive(false);
                } else { 
                    player2UI.SetActive(true);
                    hold2.SetActive(false);
                    if (lobby)
                    {
                        hold4.SetActive(true);
                    }
                    jugadores = 2;
                }
            } else {
                hold2.SetActive(false);
            }

            //Respawning player 1
            if (Input.GetButton("Simple1") || Input.GetKey(KeyCode.G)) {
                respawnPlayer1Time++;
                if (respawnPlayer1Time > 200) {
                    respawnPlayer1Time = 0;
                    if (jugador1){
                        Destroy(jugador1);
                        jugador1 = null;
                    }
                    jugador1ciclo++;
                    if (jugador1ciclo > personajes1.Length - 1) {
                        jugador1ciclo = 0;
                    }
                    GameObject personajeSeleccionado = personajes1[jugador1ciclo];
                    jugador1 = Instantiate(personajeSeleccionado);
                    DontDestroyOnLoad(jugador1);
                }
            } else {
                respawnPlayer1Time = 0;
            }

            //Respawning player 2
            if (Input.GetButton("Simple2") || Input.GetKey(KeyCode.Keypad0)) {
                if (jugador1){
                    respawnPlayer2Time++;
                }
                if (respawnPlayer2Time > 200) {
                    respawnPlayer2Time = 0;
                    if (jugador2){
                        Destroy(jugador2);
                        jugador2 = null;
                    }
                    jugador2ciclo++;
                    if (jugador2ciclo > personajes2.Length - 1) {
                        jugador2ciclo = -1;
                    }
                    if(jugador2ciclo >= 0) { 
                        GameObject personajeSeleccionado = personajes2[jugador2ciclo];
                        jugador2 = Instantiate(personajeSeleccionado);
                        DontDestroyOnLoad(jugador2);
                    }
                }
            } else {
                respawnPlayer2Time = 0;
            }
            //Fuera lobby
        } else {
            if (!jugador2)
            {
                if (jugador1)
                {
                    vida1.GetComponent<Slider>().value = jugador1.GetComponent<Jugador1>().currentHealth;
                    Camera.main.transform.position = new Vector3(jugador1.transform.position.x, 0, 0f);
                }
            } else
            {
                vida2.GetComponent<Slider>().value = jugador2.GetComponent<Jugador2>().currentHealth;
                if (jugador1)
                {
                    Camera.main.transform.position = new Vector3((jugador1.transform.position.x + jugador2.transform.position.x) / 2f, 0f, 0f);
                }
            }

            if(Camera.main .transform.position.x < 0.001f)
            {
                Camera.main.transform.position = new Vector3(0.001f, 0f, -0.1f);
            }
            hold1.SetActive(false);
            hold2.SetActive(false);
            hold3.SetActive(false);
            hold4.SetActive(false);
            
            //Checking for portal collisions
            if (jugador1) {
                if (Vector2.Distance(portal.transform.position, jugador1.transform.position) < 4) {
                    if (jugador2) {
                        if (Vector2.Distance(portal.transform.position, jugador2.transform.position) < 4) {
                            portal.transform.position = new Vector3(12.0f, 0f, 0.0f);
                            lobby = true;
                            SceneManager.LoadScene(3);
                            splash.SetActive(true);
                            splash.GetComponent<Image>().sprite = this.splashes[0];
                            jugador1.transform.position = new Vector3(-3f, 1f, 0f);
                            jugador2.transform.position = new Vector3(-3f, -1f, 0f);
                            PlayerPrefs.SetInt("LastLevel", PlayerPrefs.GetInt("LastLevel", 0) + 1);
                            PlayerPrefs.Save();
                            nivel++;
                            Camera.main.transform.position = new Vector3(0f, 0f, -.1f); ;
                        }
                    } else {
                        portal.transform.position = new Vector3(12.0f, 0f, 7.5f);
                        lobby = true;
                        SceneManager.LoadScene(3);
                        splash.SetActive(true);
                        splash.GetComponent<Image>().sprite = this.splashes[0];
                        jugador1.transform.position = new Vector3(-3f, 1f, 0f);
                        PlayerPrefs.SetInt("LastLevel", PlayerPrefs.GetInt("LastLevel", 0) + 1);
                        PlayerPrefs.Save();
                        nivel++;
                        Camera.main.transform.position = new Vector3(0f,0f, -.1f); ;
                    }
                }
            }

        }
        if(PlayerPrefs.GetInt("LastLevel", 0) > 1)
        {

            PlayerPrefs.SetInt("LastLevel", 0);
            PlayerPrefs.Save();
        }
    }

    public void StartGame() {
        lobby ^= true;
    }
}