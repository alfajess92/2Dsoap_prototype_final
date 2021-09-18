using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LabManagerScriptOrj : MonoBehaviour
{
    //items in the shop and money
    // why 2D array?
    //public int[,] labItems = new int[19, 19];

    //1D array
    public int[] labItemsQuantity = new int[19];//amount of items multiply by batch
    public int[] numberBatches = new int[19];//Save the amount bought in the shop
    public GameObject[] labItemsList = new GameObject[19];//array to save variables of each gameobject
    public ButtonInfoLab[] itemsInfoLab = new ButtonInfoLab[19];//array to save each script of buttoninfolab and derived classes

    //TEST
    public string[] itemsTypeLab = new string[19];

    //Save coins
    public float coins;
    public Text CoinsTXT;

    //Reference to change scriptable objects at runtime
    public Equipment HeatingPot200, HeatingPot400, HeatingPot600, CuringForm;
    public Substance Fragrance, LyeSolution, oil, Colorant;
    public Intermediate trace;
    public Product soap;


    private void Awake()
    {
        //DontDestroyOnLoad(this);//check this function if needed, it creates problem when go back and forth
        //FindItemsinLabScene();//labItemsList
        //FindButtonInfoLabScript(); //itemsInfoList

        //MAIN PROBLEM OF DATA BETWEEN SCENES, work around by getting and setting types?
        GetItemTypesPlayerPrefs();
        AssignNewItems();
        SetItemTypesPlayerPrefs();//Assign scriptable objects "soap" if it was created in lab

        //GetBatchesBought();//numberBatches
        CalculateQuantity(); //calculate quantity * #batches: labItems

    }


    private void Start()
    {
        //load money from previous save
        coins = PlayerPrefs.GetFloat("moneyused");
        CoinsTXT.text = "Money:" + PlayerPrefs.GetFloat("moneyused");
        //GetItemTypesPlayerPrefs();//TEST
        UpdateLabLabels();
        //PlayerPrefs.Save();

        //Activate items that are bought in the shop
        for (int i = 1; i < 19; i++)
        {
            //if (labItems[3, i] == 0)
            if (labItemsQuantity[i] == 0)
            {
                //GameObject.Find("Item_lab" + i).SetActive(false);
                labItemsList[i].SetActive(false);
            }
        }
    }

    
    private void Update()
    {
        //Check the amount used
        //for (int i = 1; i < 19; i++)
        //{
        //    //labItems[3, i] = PlayerPrefs.GetInt("quantityof" + i) * itemsInfoList[i].item.quantity_lab;
        //    //labItems[i] = PlayerPrefs.GetInt("quantityof" + i) * itemsInfoList[i].item.quantity_lab;

        //}
        //Save();
    }

    //Methods for creating arrays
    private void FindItemsinLabScene()
    {
        for (int i = 1; i < 19; i++)
        {
            labItemsList[i] = GameObject.Find("Item_lab" + i);
        }
    }//Find Gameobjects in scene: labItemsList
    private void UpdateLabLabels()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoLab[i].UpdateComponents();//updates name, artwork
            //The quantity is updated in each gameobject as none all of them has a buttoninfo labitem
            //itemsInfoLab[i].GetComponent<ButtonInfoLabItem>().UpdateQuantityLabel();//updates quantity and name
        }
    }//update labels after changing scene


    private void FindButtonInfoLabScript()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoLab[i] = labItemsList[i].GetComponent<ButtonInfoLab>();
        }
    }//Find component ButtonInfoLab and derived classes: itemsInfoLab

    //Saved in playerprefs
    private void GetBatchesBought()
    {
        for (int i = 1; i < 19; i++)
        {
            numberBatches[i] = PlayerPrefs.GetInt("quantityof" + i);
        }
    }// Batches bought: numberBatches


    //Method to calculate items 
    public void CalculateQuantity()
    {
        for (int i = 1; i < 19; i++)
        {
            //2D array
            //labItems[3,i] = PlayerPrefs.GetInt("quantityof"+i)*itemsInfoList[i].item.quantity_lab;
            if (itemsInfoLab[i].item != null)
            {
                //1D array
                labItemsQuantity[i] = numberBatches[i] * itemsInfoLab[i].item.quantity_lab;

            }
        }

    } //quantity= number items bought * number of batches:labItemsQuantity

    //Main Actions done in lab by Equipments (Curing and Heater)
    public void CreateTrace()//Called in HeaterSlot
    {
        for (int i = 1; i < 19; i++)
        {
            //Find item that have "empty" Scriptable Objects 
            if (itemsInfoLab[i].item == null)
            {
                itemsInfoLab[i].item = trace;//Set the item found to be trace
                labItemsList[i].SetActive(true);//Activate the gameobject
                numberBatches[i]++;//increase the amount of soap
                CalculateQuantity();//Recalculate quantity
                itemsInfoLab[i].UpdateComponents();//update image, name, quantity
                SetItemTypesPlayerPrefs();
                Save();//Save data to appear in shop
                break;
                /// Remove object after dragged? Done in the HeaterSlot?
            }
        }

    }

    public void CreateSoap()//Called in CuringSlot
    {
        for (int i = 1; i < 19; i++)
        {   //Find item that have "empty" Scriptable Objects 
            if (itemsInfoLab[i].item == null)
            {
                itemsInfoLab[i].item = soap;//Set the item found to be soap
                labItemsList[i].SetActive(true);//Activate the gameobject
                numberBatches[i]++;//increase the amount of soap
                CalculateQuantity();//Recalculate quantity
                itemsInfoLab[i].UpdateComponents();//update image, name, quantity
                SetItemTypesPlayerPrefs();
                //Debug.Log("Soap Saved");
                Save();//Save data to appear in shop
                break;
            }
        }
    }

    // maybe it can be used for unstacking the equipment????
    private void AssignNewItems()//Assign the scriptable objects
    {
        for (int i = 1; i < 19; i++)
        {
            if (itemsTypeLab[i] == "Soap" && numberBatches[i] > 0)
            {
                itemsInfoLab[i].item = soap;
                Debug.Log("found a soap from the shop");


            }
            else if (itemsTypeLab[i] == "Trace" && numberBatches[i] > 0)
            {
                itemsInfoLab[i].item = trace;
                Debug.Log("found a trace from shop");
            }

        }

    }


    private void GetItemTypesPlayerPrefs()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsTypeLab[i] = PlayerPrefs.GetString("typeof" + i);
            //Set the types in the Shop, important for Soap
        }



    }
    //save in player prefs???? not able to read the type back as it is an asset


    private void SetItemTypesPlayerPrefs()
    {
        for (int i = 1; i < 19; i++)
        {
            if (itemsInfoLab[i].item == null)
            {
                //itemsTypeLab[i] = itemsInfoLab[i].item.name_item;
                PlayerPrefs.SetString("typeof" + i, null);
            }
            //itemsTypeLab[i] = itemsInfoLab[i].item;
            else
            {
                itemsTypeLab[i] = itemsInfoLab[i].item.name_item;
                PlayerPrefs.SetString("typeof" + i, itemsTypeLab[i]);
            }
        }
    }


    public void RemoveItem()
    {
        for (int i = 1; i < 19; i++)
        {
            if (itemsInfoLab[i].item != null)
            {
                itemsInfoLab[i].item = null;
                labItemsList[i].SetActive(false);
                break;
            }
        }
    }//Remove Scriptable Objects from objects


    //Saving options
    public void Save()
    {
        for (int i = 1; i < 19; i++)
        {
            PlayerPrefs.SetInt("quantityof" + i, numberBatches[i]);
            //Debug.Log("lab quantitites were saved");
        }

        SetItemTypesPlayerPrefs();

    }//similar to ShopManager


    public void Savelab()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        Save();
        Debug.Log("Application was saved");
    }//if save button is added to scene



}