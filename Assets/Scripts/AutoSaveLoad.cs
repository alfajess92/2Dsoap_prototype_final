using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSaveLoad : MonoBehaviour
{

    public GameObject content;

    private void Start()
    {
    }

    void OnEnable()
    {
        print("Application was loaded");
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
        Debug.Log("Application was saved");
    }

    //Saving
    public void Save()
    {
        PlayerPrefs.SetFloat("ContentX", content.transform.position.x);
        PlayerPrefs.SetFloat("ContentY", content.transform.position.y);
        Debug.Log("Content position is saved in PlayerPrefs");
    }

    //Loading 
    public void Load()
    {
        transform.position = new Vector2(PlayerPrefs.GetFloat("ContentX"), PlayerPrefs.GetFloat("ContentY"));
        Debug.Log("Content position is loaded from PlayerPrefs");
    }
}