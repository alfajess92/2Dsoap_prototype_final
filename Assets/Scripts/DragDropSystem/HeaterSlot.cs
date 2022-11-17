using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
public class HeaterSlot : EquipmentSlot
//Scriptable Objects https://www.youtube.com/watch?v=E91NYvDqsy8

{
    //To assign back to an empty element when trace is used
   public  Default empty;

   int limit, limita;//total limit and limit per item
   int total=0;
   int itemX, itemY;

    //override is used to redefine how a function or property works when extending a class
    protected override void Awake()
    {
        base.Awake();//from EquipmentSlot
        DropArea.DropConditions.Add(new IsHeaterCondition());
        
    }

    //Read the power of the equipment
    private void Start()
    {
        power = buttonInfoLabScript.power;
        //print("power of equipment" + power);

        //declare the limit of items per type of heater
        switch (power)
        {
            case 200:
                limit = 2;
                limita = 1;
                break;
            case 400:
                limit = 4;
                limita = 2;
                break;
            case 600:
                limit = 6;
                limita = 3;;
                break;
        }

        //TEST
        //labManagerScript.Save();
        //Debug.Log("Saving the lab on start");

    }

    //TEST
    //private void TotalAmount()
    //{
    //    total = inventoryDictionary.inventory["Lye Solution"] + inventoryDictionary.inventory["Sunflower oil"];
    //    Debug.Log("this is the total" + total);
    //    int totalLye = inventoryDictionary.inventory["Lye Solution"];
    //    int totalOil = inventoryDictionary.inventory["Sunflower oil"];
    //}

    private void Update()
    {
        //Check the drop items
        if (DropArea.item != null)
            {
            if (DropArea.item.name_item == "Lye Solution" || DropArea.item.name_item == "Sunflower oil")
                {

                itemX = inventoryDictionary.inventory["Sunflower oil"];
                itemY = inventoryDictionary.inventory["Lye Solution"];
                CalculateItemsDropped();

                //TEST
                //labManagerScript.RecordItemsInventory();
                //check total of items if lower than the limit per heater-continue, else break
                if (total <= limit)
                {
                    //add item to a list
                    itemsInventory.AddItemTo(DropArea.item, 1);

                    //This dictionary is to save content when changing scenes and keep it on the equipment 
                    if (inventoryDictionary.inventory.ContainsKey(DropArea.item.name_item))//if the item was already dropped, only increase 
                    {
                        //check the inventory and the limit per item (limita)
                        if (itemX < limita && inventoryDictionary.inventory["Sunflower oil"] == inventoryDictionary.inventory[DropArea.item.name_item])
                        {
                            //Audio and FX
                            //audiocontroller.PlayDrop();//TODO removed on 17/11/22 bug on sound in Android
                            fXController.PlayDrop();//TODO test check if this works

                            inventoryDictionary.inventory[DropArea.item.name_item]++;

                            labManagerScript.numberBatches[DropArea.itemID]--;//Reduce the amount of batches
                            labManagerScript.CalculateQuantity();//Calculate the new quantity
                            //Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                            labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();
                        }

                        else if (itemY < limita && inventoryDictionary.inventory["Lye Solution"] == inventoryDictionary.inventory[DropArea.item.name_item])
                        {
                            //Audio
                            //audiocontroller.PlayDrop();//TODO removed on 17/11/22 bug on sound in Android
                            fXController.PlayDrop();//TODO test check if this works

                            inventoryDictionary.inventory[DropArea.item.name_item]++;
                            labManagerScript.numberBatches[DropArea.itemID]--;//Reduce the amount of batches
                            labManagerScript.CalculateQuantity();//Calculate the new quantity

                            //Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                            labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();
                        }
                    }

                    else
                    {
                        inventoryDictionary.inventory.Add(DropArea.item.name_item, 1);//if it is new, add one element
                        labManagerScript.numberBatches[DropArea.itemID]--;//Reduce the amount of batches
                        labManagerScript.CalculateQuantity();//Calculate the new quantity

                        //Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                        labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();
                    }

                    //-----FIXED 18/06/2021
                    //labManagerScript.numberBatches[DropArea.itemID]--;//Reduce the amount of batches
                    //labManagerScript.CalculateQuantity();//Calculate the new quantity

                    ////Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                    //labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();
                    //----

                    //Modify the transparency of the objects been dropped
                    switch (DropArea.item.name_item)
                    {
                        case "Sunflower oil":
                            UpdateOilimages();
                            break;
                        case "Lye Solution":
                            UpdateLyeImages();
                            break;
                        default:
                            break;
                    }

                    ////check if the batch number of item is zero and unactivate the gameobject
                    if (labManagerScript.numberBatches[DropArea.itemID] == 0)
                    {
                        labManagerScript.itemsInfoLab[DropArea.itemID].item = empty;
                        labManagerScript.ReadLabTypes();//To update the type after using the trace
                        labManagerScript.labItemsList[DropArea.itemID].SetActive(false);
                    }

                    DropArea.item = null;//Remove item from the dropArea
                }
                else
                {
                    //print("the limit was reached");
                }



            }//If something is dragged and only if name is conciding
             //TODO check if needed
            labManagerScript.Save();//Save data and update batches
        }

        //check the number of items in the inventory
        if (itemsInventory.Container.Count >= 2)
            {
            
            //Read power of heater
            switch (power)
            {
                //Heater xWatts with X batch of oil and X of lye at least
                case 200 when itemsInventory.Container[0].amount >= 1 && itemsInventory.Container[1].amount >= 1:
                    {
                        //labManagerScript.Save();//Save the content
                        //button is interactable
                        ActivateOnButton();
                        //check if the button is on and off and activate the bar and after a while return transparency
                        if (onOffButton.isOnPressed == true)
                        {
                            ActivateProgressBar();
                            //ActivateParticleSystem();//TODO does not appear in Gamescene
                            ChangeTransparencyItems();
                            //fXController.PlayOn();
                            //Debug.Log("The on is playing fX");

                            //Creates a trace of one batch
                            //StartCoroutine(CreateTrace(1, progressBar.progressTickMax * progressBar.processTime));
                            //var delay = progressBar.progressTickMax * progressBar.processTime;
                            //print("this is the delay" + delay);

                            StartCoroutine(CreateTraceAfterTime());//Reference in EquipmentSlot
                            //Clear the inventory of the equipment
                            itemsInventory.Container.Clear();//Remove items from inventory

                        }
                        break;
                    }

                case 400 when itemsInventory.Container[0].amount >= 2 && itemsInventory.Container[1].amount >= 2:
                    {
                        //labManagerScript.Save();//Save the content
                        //button is interactable
                        ActivateOnButton();
                        //check if the button is on and off and activate the bar and after a while return transparency
                        if (onOffButton.isOnPressed == true)
                        {
                            ActivateProgressBar();
                            //ActivateParticleSystem();//TODO does not appear in Gamescene
                            ChangeTransparencyItems();
                            //fXController.PlayOn();
                            //Debug.Log("The on is playing fX");

                            //Creates 2 trace of 1 batch
                            //StartCoroutine(CreateTrace(2, progressBar.progressTickMax * progressBar.processTime));
                            for (int i = 0; i < 2; i++)
                            {
                                //var delay = progressBar.progressTickMax * progressBar.processTime;
                                StartCoroutine(CreateTraceAfterTime());//Reference in EquipmentSlot
                            }

                            //Clear the inventory of the equipment 
                            itemsInventory.Container.Clear();//Remove items from inventory


                        }

                        break;
                    }

                case 600 when itemsInventory.Container[0].amount >= 3 && itemsInventory.Container[1].amount >= 3:
                    {
                        //labManagerScript.Save();//Save the content
                        //button is interactable
                        ActivateOnButton();
                        //check if the button is on and off and activate the bar and after a while return transparency
                        if (onOffButton.isOnPressed == true)
                        {
                            ActivateProgressBar();
                            //ActivateParticleSystem();//TODO does not appear in Gamescene
                            ChangeTransparencyItems();
                            //fXController.PlayOn();
                            //Debug.Log("The on is playing fX");

                            //Creates 3 trace of 1 batch                                
                            //StartCoroutine(CreateTrace(3, progressBar.progressTickMax * progressBar.processTime));
                            for (int i = 0; i < 3; i++)
                            {
                                //var delay = progressBar.progressTickMax * progressBar.processTime;
                                StartCoroutine(CreateTraceAfterTime()); //Reference in EquipmentSlot

                            }
                            //StartCoroutine(CreateTrace(3, progressBar.progressTickMax));
                            //print("this is the tickmax" + progressBar.progressTickMax);

                            //Clear the inventory of the equipment 
                            itemsInventory.Container.Clear();//Remove items from inventory

                        }
                        break;
                    }
            }

        }

        else
            {
                DeActivateOnButton();
                TurnOff();//turn button off
                //labManagerScript.RecordItemsInventory();
        }
    }

    //Make a sum of the items in the inventory of each heater 
    private int CalculateItemsDropped()
    {
        //if the inventory is created
        if (itemsInventory.Container.Count >= 0)
        {
            total = itemX + itemY;
        }
        
        return total;
    }


    //To create instance inside Update
    //private void CreateTrace()
    //{
    //    // to something better the amount of TRACE created!
    //    labManagerScript.CreateTrace(1);

    //}
}







//Code to use the items as they are drop into the scene

// check the number of items inside of inventory
//int numberItemsInventory = itemsInventory.Container.Count;
//switch (numberItemsInventory)
//{
//    //first item "dropped" into item1
//    case 1:
//        //item_image1.sprite = DropArea.item.artwork;
//        Debug.Log("1 item in");
//        break;

//    //second item "dropped" into item2
//    case 2:
//        //item_image2.sprite = DropArea.item.artwork;
//        color2.a = final_alpha_color;

//        Debug.Log("2 item in");
//        break;
//    //third item "dropped" into item3
//    case 3:
//        item_image3.sprite = DropArea.item.artwork;
//        Debug.Log("3 item in");
//        break;
//    default:
//        Debug.Log("no items in inventory");
//        break;
//}

//For loop removed after adding item id to dropArea
//        

//for (int i = 1; i < 19; i++)
//{
//    //calculate 
//    if (DropArea.item == labManagerScript.itemsInfoLab[i].item)
//    {
//        //read the item id been dropped
//        Debug.Log("These are the current batches" + labManagerScript.numberBatches[i]);
//        labManagerScript.numberBatches[i]--;//Reduce the amount of batches 
//        //Not sure if calculating this here is ok
//        labManagerScript.CalculateQuantity();
//        //Save new amount in playerprefs 
//        labManagerScript.Save();
//        Debug.Log("This is the remaining amount" + labManagerScript.labItemsQuantity[i]);
//        break;
//    }

//}