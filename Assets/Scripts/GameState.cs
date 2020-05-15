using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    public static bool playerIsDead = false;
    bool loadStart = false;
    

    // Start is called before the first frame update
    void Start()
    {
        playerIsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( playerIsDead == true && loadStart == false)
        {
            Invoke("LoadGame", 5);
            loadStart = true;
        }
    }

    void LoadGame()
    {
        SceneManager.LoadScene(0);
    }
}
