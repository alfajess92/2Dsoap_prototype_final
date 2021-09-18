using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CuringSlot : EquipmentSlot
{
    //Item newSoap;

    //To assign back to an empty element when trace is used
    public Default empty;

    //value to adjust depending on the amount of trace used
    //int amounttrace = 1;
    //int amountcolorant;
    //int amountfragrance;

    //for counting items in the equipment
    int total = 0;
    int limit, limita;
    int itemZ, itemY, itemX;

    //override is used to redefine how a function or property works when extending a class
    protected override void Awake()
    {
        base.Awake();//from EquipmentSlot
        DropArea.DropConditions.Add(new IsCuringCondition());
        
    }

    private void Start()
    {
        limit = 3;//curing form can only have 3 items TODO if different curing forms are added to the game
        limita = 1;

        //TEST
        //labManagerScript.Save();
        //Debug.Log("Saving the lab on start");
    }

    private void Update()
    {
        //Check the drop items
        if (DropArea.item != null)
          {

            if (DropArea.item.name_item == "Colorant" || DropArea.item.name_item == "Fragrance" || DropArea.item.name_item == "Trace")
            {
               
                itemX = inventoryDictionary.inventory["Colorant"];
                itemY= inventoryDictionary.inventory["Fragrance"];
                itemZ = inventoryDictionary.inventory["Trace"];

                //calculate items dropped in the curing form
                CalculateItemsDropped();

                if (total <= limit)
                {
                    itemsInventory.AddItemTo(DropArea.item, 1);
                    
                    //This dictionary is to save content when changing scenes and keep it on the equipment
                    if (inventoryDictionary.inventory.ContainsKey(DropArea.item.name_item))//if the item was already dropped, only increase 
                    {

                        if (itemX < limita && inventoryDictionary.inventory["Colorant"] == inventoryDictionary.inventory[DropArea.item.name_item] || itemY < limita && inventoryDictionary.inventory["Fragrance"] == inventoryDictionary.inventory[DropArea.item.name_item])

                        {
                            //Audio and FX
                            audiocontroller.PlayDrop();
                            fXController.PlayDrop();//animation

                            inventoryDictionary.inventory[DropArea.item.name_item]++;
                            labManagerScript.numberBatches[DropArea.itemID] --;//reduce amount in the list
                            labManagerScript.CalculateQuantity();//Calculate the new quantity

                            //Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                            labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();

                        }

                        else if (itemZ < limita && inventoryDictionary.inventory["Trace"] == inventoryDictionary.inventory[DropArea.item.name_item])
                        {
                            audiocontroller.PlayDrop();
                            fXController.PlayDrop();//animation

                            inventoryDictionary.inventory[DropArea.item.name_item]++;
                            labManagerScript.numberBatches[DropArea.itemID] --;//reduce amount in the list
                            labManagerScript.CalculateQuantity();//Calculate the new quantity

                            //Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                            labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();

                            //Remove TRACE once it is dropped from the dropArea
                            if (DropArea.item.name_item == "Trace")
                            {
                                audiocontroller.PlayDrop();
                                fXController.PlayDrop();//animation
                                labManagerScript.itemsInfoLab[DropArea.itemID].item = empty;//Set the item to empty to make it available once again
                                labManagerScript.ReadLabTypes();//To update the type after using the trace
                                labManagerScript.labItemsList[DropArea.itemID].SetActive(false);

                                
                            }
                        }

                    }

                    else
                    {
                        inventoryDictionary.inventory.Add(DropArea.item.name_item, 1);//if it is new, add one element
                        labManagerScript.numberBatches[DropArea.itemID] --;//reduce amount in the list
                        labManagerScript.CalculateQuantity();//Calculate the new quantity

                        //Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                        labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();

                    }

                    //remove all its content
                    //if(DropArea.item.name_item == "Trace")
                    //{
                    //    amounttrace = labManagerScript.numberBatches[DropArea.itemID];
                    //    if (amounttrace>= 1)
                    //    {
                    //        amountcolorant = amounttrace;
                    //        amountfragrance = amounttrace;
                    //    }
                    //}

                    //----
                    //labManagerScript.numberBatches[DropArea.itemID] -= amounttrace;//reduce amount in the list
                    //labManagerScript.CalculateQuantity();//Calculate the new quantity

                    ////Note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
                    //labManagerScript.labItemsList[DropArea.itemID].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[DropArea.itemID].ToString();

                    //----

                    //Modify the transparency of the objects been dropped
                    switch (DropArea.item.name_item)
                    {
                        case "Trace":
                            item_image1.alpha = final_alpha_color;
                            break;
                        case "Fragrance":
                            item_image2.alpha = final_alpha_color;
                            break;
                        case "Colorant":
                            item_image3.alpha = final_alpha_color;
                            break;
                    }

                    ////check if the batch number of item is zero and unactivate the gameobject
                    if (labManagerScript.numberBatches[DropArea.itemID] == 0)
                    {
                        labManagerScript.itemsInfoLab[DropArea.itemID].item = empty;
                        labManagerScript.ReadLabTypes();//To update the type after using the trace
                        labManagerScript.labItemsList[DropArea.itemID].SetActive(false);
                    }

                    ///Change to the top in trace condition-FIXED 21/06/2021
                    //Remove TRACE once it is dropped from the dropArea
                    //if (DropArea.item.name_item == "Trace")
                    //{
                    //    labManagerScript.itemsInfoLab[DropArea.itemID].item = empty;//Set the item to empty to make it available once again
                    //    labManagerScript.ReadLabTypes();//To update the type after using the trace
                    //    labManagerScript.labItemsList[DropArea.itemID].SetActive(false);
                    //}

                    DropArea.item = null;//Remove item from the dropArea

                }
                else
                {
                    //print("the limit was reached");
                }


            }//If something is dragged and only if name is conciding

            //TODO check if needed
            labManagerScript.Save();//Save data in case the user does not pressed the button
                                    //labManagerScript.TemporalSave();
        }

        //check the number of items in the inventory
        if (itemsInventory.Container.Count >= 3)
       
        {
            if (itemsInventory.Container[0].amount >= 1 && itemsInventory.Container[1].amount >= 1 && itemsInventory.Container[2].amount >= 1)
            {
                //button is interactable
                ActivateOnButton();
                //check if the button is on and off and activate the bar and after a while return transparency
                if (onOffButton.isOnPressed == true)
                {
                    ActivateProgressBar();
                    //inventoryDictionary.inventory["Process"] = progressBar.progressTick;
                    //fXController.PlayOn();
                    //Debug.Log("The on is playing fX");

                    //ActivateParticleSystem();//TODO does not appear in Gamescene
                    ChangeTransparencyItems();
                    itemsInventory.Container.Clear();//Remove items from inventory

                    //var delay = progressBar.progressTickMax * progressBar.processTime;
                    //Debug.Log("This is the delay");
                    
                    StartCoroutine(CreateSoapAfterTime(1));//using variables "delay" was problematic, better direct numbers, see EquipmentSlot
                  
                    //Clear the inventory of the equipment 
                    //inventoryDictionary.inventory.Clear();

                }
            }
                
            }

        else
        {
            
            DeActivateOnButton();
            TurnOff();//turn button off
            
        }
    }

    //Make a sum of the items in the inventory of each heater 
    private int CalculateItemsDropped()
    {
        //if the inventory is created
        if (itemsInventory.Container.Count >= 0)
        {
            total = itemX + itemY + +itemZ;
            //print("this is the total"+total);
            //print("this is itemx" + itemX);
            //print("this is itemy" + itemY);
            //print("this is itemz" + itemZ);
        }

        return total;
    }


}


//Code to use the items as they are drop into the scene
//// check the number of items inside of inventory
//int numberItemsInventory = itemsInventory.Container.Count;

//switch (numberItemsInventory)
//{
//    //first item "dropped" into item1
//    case 1:
//        item_image1.sprite = DropArea.item.artwork;
//        Debug.Log("1 item in");
//        break;

//    //second item "dropped" into item2
//    case 2:
//        item_image2.sprite = DropArea.item.artwork;
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


//For Loop not needed
//for (int i = 1; i < 19; i++)

//{
//    if (DropArea.item == labManagerScript.itemsInfoLab[i].item)//check if the dropped item is in the item info list
//    {
//        Debug.Log("esto no es basura" + DropArea.itemID);
//        //var itemid = labManagerScript.labItemsList[i].GetComponent<ButtonInfoLabItem>().ItemID;//read the item id of the buttoninfolab


//        labManagerScript.numberBatches[i]--;
//        //Not sure if calculating this here is ok
//        labManagerScript.CalculateQuantity();
//        labManagerScript.Save();
//        //note that items here are substances and buttonInfoLabItem but the parent class is ButtonInfoLab
//        //labManagerScript.labItemsList[i].GetComponent<ButtonInfoLabItem>().quantity_lab.text = labManagerScript.labItemsQuantity[itemid].ToString();
//        //Debug.Log("updating only what is in the drop area"+itemid);
//        break;

//    }
//}