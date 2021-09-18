using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TigerForge;
using System;
using System.Linq;
using UnityEngine.Analytics;

public class NewGame : MonoBehaviour
{
    EasyFileSave myFile;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newGame()
    {
        //EasyFileSave myFile = new EasyFileSave();
        EasyFileSave myFile = new EasyFileSave();
        myFile.Delete();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene("scene_shop");

    }
}
