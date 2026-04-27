//GameManager
//Singleton and persistent object to manage game state
//For high level control over game
//--------------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//--------------------------------------------------------------
public class GameManager : MonoBehaviour
{
    //--------------------------------------------------------------
    //public properties
    //C# property to retrieve currently active instance of object, if any
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>(); //create game manager object if required
            return instance;
        }
    }
    //--------------------------------------------------------------
    //Private variables
    //--------------------------------------------------------------
    //Internal reference to single active instance of object - for singleton behaviour
    private static GameManager instance = null;


    // Called before Start on object creation -- singleton (just different way)
    void Awake()
    {
        //Check if there is an existing instance of this object
        if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
            DestroyImmediate(gameObject); //Delete duplicate
        else
        {
            instance = this; //Make this object the only instance
            DontDestroyOnLoad(gameObject); //Set as do not destroy
        }
    }
    //--------------------------------------------------------------
    // Use this for initialization
    void Start()
    {

    }

    //--------------------------------------------------------------
    //Restart Game
    public void RestartGame()
    {
        //Load first level
        SceneManager.LoadScene(0);
    }
    //--------------------------------------------------------------
    //Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }
    //--------------------------------------------------------------
}
