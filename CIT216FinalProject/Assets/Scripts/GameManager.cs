//4/29/26
//Herman Pagan Alvarez
//Meant to handle overarching gameplay elements (ex. scenes)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>(); //create game manager object if required
            return instance;
        }
    }
    private static GameManager instance = null;

    // Ensures only one GameManager exists
    void Awake()
    {
        //Check if there is an existing instance of this object
        if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
            DestroyImmediate(gameObject); //Delete duplicate
        else
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
    }

    void Start()
    {

    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RestartGame()
    {
        //Load first level
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        //Load first level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    //Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
