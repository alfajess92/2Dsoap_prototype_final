using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TigerForge;


public class SceneSwitcher : MonoBehaviour
{
    //EasyFileSave easyFileSave;

    //IEnumerator SwitchLab()
    //{
    //    SceneManager.LoadScene("scene_lab");
    //    yield return new WaitForSeconds(20f);
    //}

    public void PlayLab()
    {
        SceneManager.LoadScene("scene_lab");
        //StartCoroutine(SwitchLab());
    }

    public void PlayShop()
    {
        SceneManager.LoadScene("scene_shop");
    }

    public void MeetAaron()
    {
        SceneManager.LoadScene("scene_bunno");//change from aaron
    }

    public void PlayMenu()
    {
        SceneManager.LoadScene("scene_intro");
    }

    public void PlayAbout()
    {
        SceneManager.LoadScene("scene_about");
    }


}
