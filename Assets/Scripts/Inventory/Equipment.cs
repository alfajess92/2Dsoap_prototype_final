using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




[CreateAssetMenu(fileName ="New Equipment", menuName ="Inventory System/Equipment")]
public class Equipment : Item
{

    public void Awake()
    {
        type = ItemType.Equipment;
        process = ProcessType.saponification;

      
    }


}


//public class Equipment : MonoBehaviour
//{

//    public GameObject Progress_panel;
//    public GameObject onoff;
//    public GameObject LabManager;

//    public bool isEquipmentOn=false;
//    public bool isEquipmentEmpty=false;


//    //public int[,] labItems = new int[19, 19];

//    //private void Start()
//    //{
//    //    labItems = LabManager.GetComponent<LabManagerScript>().labItems;
//    //}

//    public void ActivateItem()
//        {


//        //click on heater
//        //hightlight oil and lye (they have attached a buttonhandler script that increase or decrease transparency)
//        //if user clicks and item and it is oil or lye, read the labitem prefs
//        //call a function or script call deposit/interactions
//    }



//    public void DepositItem()
//    {
//        //read the amount in lab item
//        //reduce the amount of lab item by one
//        //activates de slider on progress bar with the amount?

//    }


//    public void ClearItem()
//    {
//        //the slider on the progress bar is removed or reduce
//    }


//    public void TurnOn()
//    {
//        //related to on/off button
//        //called the progress bar in the top and measure time?
//        //if (onoff != null)
//        //{
//        //    bool isActive = onoff.activeSelf;
//        //    onoff.SetActive(!isActive);
//        //}

//    }


//    public void OpenPanel()
//    {
//        if (Progress_panel != null)
//        {
//            bool isActive = Progress_panel.activeSelf;
//            Progress_panel.SetActive(!isActive);
//        }

//    }

//}
