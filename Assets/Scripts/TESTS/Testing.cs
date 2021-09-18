using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GameObject Progress_panel;

    public void OpenPanel()
    {
        if(Progress_panel != null)
        {
            bool isActive = Progress_panel.activeSelf;
            Progress_panel.SetActive(!isActive);
            Debug.Log("panel is open");
        }

    }


    /*
    private const float DOUBLE_CLICK_TIME = .2f;

    //[SerializeField] private LabManagerScript labManager;
    private float lastClickTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= DOUBLE_CLICK_TIME)
            {
                Debug.Log("DoubleClick");
                GameObject.Find("Progress_panel").SetActive(true);
            }
            else 
            {
                Debug.Log("NormalClick");
            }

            lastClickTime = Time.time;
        }
    }
    */

}
