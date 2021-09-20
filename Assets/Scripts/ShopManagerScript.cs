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


public class ShopManagerScript : MonoBehaviour
{
    //class to use EasyFileSave utilities
    EasyFileSave myFile;

    //Serialize data that is saved by EasyFileSave
    //Serialization: give a name and number to find data with a consistent path
    [System.Serializable]
    public class Item
    {
        public string name = "";
        public int quantity = 0;
    }

    [Serializable]
    public class Containers
    {
        public int position = 0;
        public string type = "";

        public string itemdropped1;
        public string itemdropped2;
        public string itemdropped3;

        public int amount1 = 0;
        public int amount2 = 0;
        public int amount3 = 0;

        public int progress = 0;
    }

   
    //1D array, similar LabManagerScript
    public int[] shopItemsQuantity = new int[19]; //ShopItemsQuantity=same as number of batches
    public GameObject[] shopItemsList = new GameObject[19];//ShopItemsList (gameobjects)
    public ButtonInfo[] itemsInfoShop = new ButtonInfo[19];//itemsInfoShop (button info)
    public string[] itemsTypeShop = new string[19];//items type list

    //Array for Lab arrangement
    public string[] itemsTypeLab = new string[19];//items type list

    //Save coins
    public int coins;
    public Text CoinsTXT;

    //Reference to change scriptable objects at runtime
    public Equipment HeatingPot200, HeatingPot400, HeatingPot600, CuringForm;
    public Substance Fragrance, LyeSolution, oil, Colorant;
    public Intermediate trace;
    public Product soap;
    public Default empty;

    //TEST new items
    public RandomItem match, crayon, coin, clock, pond, star, sstar, bubblebottle;
       
    //Counters
    private int totalCuringForms, totalHeatingPot200, totalHeatingPot400, totalHeatingPot600;
    private int totalColorant, totalFragrance, totalOil, totalLye;
    public int totalSoap, totalTrace;
    private int totalEmpty;
    //Random items
    private int totalMatch, totalCrayon, totalCoin, totalClock, totalPond, totalStar, totalSStar, totalBubbleBottle;


    //ShopDictionary: save values and types 
    public Dictionary<string, int> ShopDictionary = new Dictionary<string, int>
        {
            //Substances
            {"Colorant", 0},{"Fragrance", 0},{"Sunflower oil", 0},{"Lye Solution", 0},
            //Equipment
            {"Curing Form", 0},{"Heating Pot 200W", 0},{"Heating Pot 400W", 0},{"Heating Pot 600W", 0},
            //Products
            {"Trace", 0},{"Soap", 0},{"Empty", 0},

            //Random
             {"BubbleBottle",0},{"Clock",0},{"Coin",0},{"Crayon",0},{ "Match",0},{"Pond",0},{"ShootingStar",0},{"Star",0}
        };

    //TODO check if necessary to declare in shop, it is only used in the lab
    //public List<Containers> inventories = new List<Containers>();
    public List<Containers> inventories;


    //Chat System
    public GameObject chatManager, questions;
    public ChatManager chatManagerScript;
    public ChatTrigger chatTriggerScript;


    //TEST to debug saving inventories
    //public static int counter = 0;
    public static bool isRead = false;


    //TEST animations TODO
    //public CFX_AutoDestructShuriken fx;

    //Reference color changer
    public GameObject colorChanger;
    public GlobalAudioController globalAudioController;


    //First to be called
    //Reference: https://docs.unity3d.com/Manual/ExecutionOrder.html
    void Awake()
    {
        //DontDestroyOnLoad(this); NOT NEEDED ANYMORE
        FindItemsinShopScene();//shopItemsList - NOPREF
        FindButtonInfoScript();//itemsInfoShop - NOPREF
    }

    //Second to be called
    void OnEnable()
    {
        Load(); //load data from previous save
        Debug.Log(">> Shop is saved on enable.");
    }

    //Third to be called
    void Start()
    {
        ReadShopTypes();//itemsTypeShop
        //TODO when the list is empty or new this function is necessary, check if need to save isRead for next session
        ReadLabTypes();
        UpdateShopBatches();//using dictionary values loaded to update the number of batches

        //start with EasyFileSave to load and save data in the game during runtime
        myFile = new EasyFileSave();
        myFile.suppressWarning = false;

        //delete is only activated if the game starts from the beginning each time.
        //myFile.Delete();
        UpdateShopLabels();
        Debug.Log(">> Shop is started." + "\n");

        //Chat System
        chatManager = GameObject.Find("ChatManager");
        chatManagerScript = chatManager.GetComponent<ChatManager>();

        //To trigger chat
        questions = GameObject.Find("Questions");
        //chatTriggerScript = questions.GetComponent<ChatTrigger>();
        //chatTriggerScript.TriggerChat();
        //Debug.Log("triggering chat");

        Analytics.CustomEvent("data_amount");
        //money_soap works in analytics
        Analytics.CustomEvent("money_soap", new Dictionary<string, object>
        {
            {"player_money", coins },
            {"player_soap", totalSoap },

        });

        //TEST Color Changer
        colorChanger = GameObject.Find("ColorChanger");

        Checkbuysell();
        globalAudioController = GameObject.Find("Sounds").GetComponent<GlobalAudioController>();
    }


    //TEST
    private void Checkbuysell()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoShop[i].CheckSell();
            itemsInfoShop[i].CheckBuy();
        }

    }

    //Methods to create arrays
    public void FindItemsinShopScene()//Find Gameobjects in scene: shopItemsList
    {
        for (int i = 1; i < 19; i++)
        {
            shopItemsList[i] = GameObject.Find("Item_shop" + i);
        }
    }
    public void FindButtonInfoScript()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoShop[i] = shopItemsList[i].GetComponent<ButtonInfo>();
        }
    }//Find the buttoninfo scripts from each gameobject

    public void ReadShopTypes()
    {
        itemsTypeShop = new string[19];

        for (int i = 1; i < 19; i++)
        {
            itemsTypeShop[i] = itemsInfoShop[i].item.name_item;
        }
        //return itemsTypeShop;
    }//Read the item types from start

    //TODO this was necessary for the first creation of the list, check if it works always
    public void ReadLabTypes()
    {
        if (isRead)
        {
            Debug.Log("reading types from last session");
            //for (int i = 1; i < 19; i++)
            //{
            //    itemsTypeLab = new string[19];
            //    if (itemsTypeLab[i] == null)
            //    {
            //        itemsTypeLab[i] = "Empty";
            //        Debug.Log("catching initial error");
            //    }
            //    //else()
            //}
                
            return;

        }

        itemsTypeLab = new string[19];

        for (int i = 1; i < 19; i++)
        {
            //when is called for the first time, start lab as in the shop
            if (itemsTypeLab[i] == null)
            {
                itemsTypeLab[i] = "Empty";
                isRead = true;
                Debug.Log("reading types for lab for the first time");
            }
            else
            {
                //itemsTypeLab[i] = itemsTypeShop[i];
                //itemsTypeLab[i] = itemsTypeLab
                //Debug.Log("reading types from last session");
            }
                //itemsTypeLab[i] = "Empty";// itemsTypeLab[i] = itemsTypeShop[i]; 
        }



    } //Read the types that come from lab

    //UPDATE methods
    public void UpdateShopLabels()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoShop[i].UpdateComponents();
            //CheckCoin(i);//TEST
        }
       

    }//called from items info in buttoninfo script

    public void UpdateShopBatches()
    {
        //use dictionary values to update the number of batches or shopquantity
        for (int i = 1; i < 19; i++)

        {
            switch (itemsTypeShop[i]) {

                //Substances
                case "Colorant":
                    shopItemsQuantity[i] = ShopDictionary["Colorant"];
                    break;
                case "Fragrance":
                    shopItemsQuantity[i] = ShopDictionary["Fragrance"];
                    break;
                case "Sunflower oil":
                    shopItemsQuantity[i] = ShopDictionary["Sunflower oil"];
                    break;
                case "Lye Solution":
                    shopItemsQuantity[i] = ShopDictionary["Lye Solution"];
                    break;
                
                //Equipment
                case "Curing Form":
                    shopItemsQuantity[i]=ShopDictionary["Curing Form"];
                    break;
                case "Heating Pot 200W":
                    shopItemsQuantity[i] = ShopDictionary["Heating Pot 200W"];
                    break;
                case "Heating Pot 400W":
                    shopItemsQuantity[i] = ShopDictionary["Heating Pot 400W"];
                    break;
                case "Heating Pot 600W":
                    shopItemsQuantity[i] = ShopDictionary["Heating Pot 600W"];
                    break;

                    //Product
                    case "Soap":
                    shopItemsQuantity[i] = ShopDictionary["Soap"];
                    break;

                //Intermediate
                case "Trace":
                    shopItemsQuantity[i] = ShopDictionary["Trace"];
                    break;

                case "Empty":
                    shopItemsQuantity[i] = 0;
                    break;

                //Random
                case "BubbleBottle":
                    shopItemsQuantity[i] = ShopDictionary["BubbleBottle"];
                    break;

                case "Clock":
                    shopItemsQuantity[i] = ShopDictionary["Clock"];
                    break;
                case "Coin":
                    shopItemsQuantity[i] = ShopDictionary["Coin"];
                    CheckCoin(i);
                    break;
                case "Crayon":
                    shopItemsQuantity[i] = ShopDictionary["Crayon"];
                    CheckCrayon();

                    break;
                case "Match":
                    shopItemsQuantity[i] = ShopDictionary["Match"];
                    break;
                case "Pond":
                    shopItemsQuantity[i] = ShopDictionary["Pond"];
                    CheckPond(i);
                    break;
                case "ShootingStar":
                    shopItemsQuantity[i] = ShopDictionary["ShootingStar"];
                    break;
                case "Star":
                    shopItemsQuantity[i] = ShopDictionary["Star"];
                    CheckStar(i);
                    break;
            }
        }

    }

    //Main Actions Shop
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (ButtonRef.GetComponentInParent<ButtonInfo>().item != empty && ButtonRef.GetComponentInParent<ButtonInfo>().item != trace)
        {
            //Count the empty spaces in the lab to confirm there is space available
            int indexEmpty = Array.IndexOf(itemsTypeLab, "Empty");

            if (coins >= ButtonRef.GetComponentInParent<ButtonInfo>().item.buyprice && indexEmpty>=0)//J added indexEmpty as condition
            {
                ButtonRef.GetComponentInParent<Audiocontroller>().PlayBuy();//Play Audio
                ButtonRef.GetComponentInParent<FXController_Shop>().PlayBuy();//Play Animation
                

                coins -= ButtonRef.GetComponentInParent<ButtonInfo>().item.buyprice;
                shopItemsQuantity[ButtonRef.GetComponentInParent<ButtonInfo>().ItemID]++;
                CoinsTXT.text = "MONEY:" + coins.ToString();
                ButtonRef.GetComponentInParent<ButtonInfo>().QuantityTxt.text = shopItemsQuantity[ButtonRef.GetComponentInParent<ButtonInfo>().ItemID].ToString();

                //Create new item in lab and if equipment, unstack
                CreateNew(ButtonRef.GetComponentInParent<ButtonInfo>().item);


                //Random items

                //Color change if a crayon is bought
                if(ButtonRef.GetComponentInParent<ButtonInfo>().item == crayon)
                {
                    colorChanger.GetComponent<ColorChange>().ChangeColor();
                }

                //Sell prices of equipments increase when coin is bought
                if (ButtonRef.GetComponentInParent<ButtonInfo>().item == coin)
                {
                    for (int i= 1; i< 19; i++)
                    {
                        itemsInfoShop[i].UpdateEquipmentSellingPrices();
                    }
                    
                }

                //Sell prices of substances increase when pond is bought
                if (ButtonRef.GetComponentInParent<ButtonInfo>().item == pond)
                {
                    for (int i = 1; i < 19; i++)
                    {
                        itemsInfoShop[i].UpdateSubstancesSellingPrice();
                    }

                }

                //Sell price of soap doubles when Star is bought
                if (ButtonRef.GetComponentInParent<ButtonInfo>().item == star)
                {
                    for (int i = 1; i < 19; i++)
                    {
                        itemsInfoShop[i].UpdateSoapSellingPrice();
                    }
                }

                ButtonRef.GetComponentInParent<ButtonInfo>().CheckBuy();//checks if this item can be buy
            }

        }
    }

    private void CreateNew(global::Item item)
    {

        for (int i = 1; i < 19; i++)
        {
            //int indexEmpty = Array.IndexOf(itemsTypeLab, "Empty");
            //int emptyleft = 18 - indexEmpty;
            //Debug.Log("this are the empty left" + emptyleft);

            //if (indexEmpty>=0)
            //{
                //Unstack Equipment
                if (itemsTypeLab[i] != item.name_item) 
                {
                    //Check if there are empty
                    if (itemsTypeLab[i] == "Empty")
                    { itemsTypeLab[i] = item.name_item; break; }
                    
                }
            //else if (item.type == ItemType.Substance || item.type == ItemType.Intermediate || item.type == ItemType.Product)
            //Remove product from the condition-BUG identify by Michi
            //The soap now it is unstackable 
            else if (item.type == ItemType.Substance || item.type == ItemType.Intermediate)

            {
                itemsTypeLab[i] = item.name_item; break;

            }
                //Other posibility to use the process as condition
            //    else if(item.process==ProcessType.curing)
            //    //item.name_item == "Soap")
            //{
            //    print("buying soap");
            //}
            //}
        }
    }//Create a new item in the itemTypeLab array, unstack equipment

    //Hide buy/sell prices when sell/buy button is pressed
    public void UpdateLabelsSell()
    {
        
        for (int i = 1; i < 19; i++)
        {
            itemsInfoShop[i].UpdateComponentsSell();
        }
    }

    public void UpdateLabelsBuy()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoShop[i].UpdateComponentsBuy();
        }
    }

    public void Sell()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        //TODO check trace condition for the final prototype
        if (ButtonRef.GetComponentInParent<ButtonInfo>().item != empty && ButtonRef.GetComponentInParent<ButtonInfo>().item != trace)
        {
            if (ButtonRef.GetComponentInParent<ButtonInfo>().ItemID > 0)
            {
                if (shopItemsQuantity[ButtonRef.GetComponentInParent<ButtonInfo>().ItemID] > 0)
                {

                    ButtonRef.GetComponentInParent<Audiocontroller>().PlaySell();//play audio
                    ButtonRef.GetComponentInParent<FXController_Shop>().PlaySell();//FX

                    coins += ButtonRef.GetComponentInParent<ButtonInfo>().item.sellprice;
                    shopItemsQuantity[ButtonRef.GetComponentInParent<ButtonInfo>().ItemID]--;
                    CoinsTXT.text = "MONEY:" + coins.ToString();
                    ButtonRef.GetComponentInParent<ButtonInfo>().QuantityTxt.text = shopItemsQuantity[ButtonRef.GetComponentInParent<ButtonInfo>().ItemID].ToString();
                    //remove item from itemLabType array 
                    RemoveItem(ButtonRef.GetComponentInParent<ButtonInfo>().item);



                //Random items

                //Reset Color change if a crayon is sold
                if (ButtonRef.GetComponentInParent<ButtonInfo>().item == crayon)
                    {
                        colorChanger.GetComponent<ColorChange>().ResetColor();
                        Debug.Log("crayon is gone and the color is back");
                    }

                    //Reset Selling prices of equipments when coin is sold
                    if (ButtonRef.GetComponentInParent<ButtonInfo>().item == coin)
                    {
                        for (int i = 1; i < 19; i++)
                        {
                            itemsInfoShop[i].ResetEquipmentSellingPrices();
                        }

                    }

                    //Reset Selling prices of substances when pond is sold
                    if (ButtonRef.GetComponentInParent<ButtonInfo>().item == pond)
                    {
                        for (int i = 1; i < 19; i++)
                        {
                            itemsInfoShop[i].ResetSubstancesSellingPrice();
                        }

                    }

                    //Reset price of soap
                    if (ButtonRef.GetComponentInParent<ButtonInfo>().item == star)
                    {
                        for (int i = 1; i < 19; i++)
                        {
                            itemsInfoShop[i].ResetSoapPrice();
                        }

                    }
                }
                ButtonRef.GetComponentInParent<ButtonInfo>().CheckSell();//checks if this item can be buy
                
            }
        }
        //UpdateLabelsSell(); //TEST
    }

    private void RemoveItem(global::Item item)
    {
        for (int i = 1; i < 19; i++)
        {
            //Find item that have "empty" Scriptable Objects 
            if (itemsTypeLab[i] == item.name_item)//To find the first spot with the same name
            {
                if (inventories[i].progress >= 0)
                {
                    Debug.Log("Stop sound");
                    if(item.name_item=="Curing Form")
                    {
                        globalAudioController.StopSound(PlayableSounds.cure);
                        Debug.Log("Stop cure sound");
                    }

                    if(item.name_item== "Heating Pot 200W"|| item.name_item == "Heating Pot 400W" || item.name_item == "Heating Pot 600W")
                    {
                        globalAudioController.StopSound(PlayableSounds.heat);
                        Debug.Log("Stop heating sound");
                    }
                }

                itemsTypeLab[i] = "Empty";
                break;
            }
        }

    }//Remove items in the itemTypeLab array


    //Counting Methods
    //CountTotal Items in the shop (using itemstypeshop array) and updating ShopDictionary, called from SAVE
    public void CountTotalItemsShop()
    {
        //Count Substances
        int indexColorant = Array.IndexOf(itemsTypeShop, "Colorant");
        totalColorant = shopItemsQuantity[indexColorant];

        int indexFragrance = Array.IndexOf(itemsTypeShop, "Fragrance");
        totalFragrance = shopItemsQuantity[indexFragrance];

        int indexOil = Array.IndexOf(itemsTypeShop, "Sunflower oil");
        totalOil = shopItemsQuantity[indexOil];

        int indexLye = Array.IndexOf(itemsTypeShop, "Lye Solution");
        totalLye = shopItemsQuantity[indexLye];

        //Count Equipment
        int indexCuring = Array.IndexOf(itemsTypeShop, "Curing Form");
        totalCuringForms = shopItemsQuantity[indexCuring];

        int indexHeating200 = Array.IndexOf(itemsTypeShop, "Heating Pot 200W");
        totalHeatingPot200 = shopItemsQuantity[indexHeating200];

        int indexHeating400 = Array.IndexOf(itemsTypeShop, "Heating Pot 400W");
        totalHeatingPot400 = shopItemsQuantity[indexHeating400];

        int indexHeating600 = Array.IndexOf(itemsTypeShop, "Heating Pot 600W");
        totalHeatingPot600 = shopItemsQuantity[indexHeating600];

        //Count Products, this may be missing in the array if user does not create them
        int indexSoap = Array.IndexOf(itemsTypeShop, "Soap");
        if (indexSoap < 0) { totalSoap = 0; }
        else { totalSoap = shopItemsQuantity[indexSoap]; }

        int indexTrace = Array.IndexOf(itemsTypeShop, "Trace");
        if (indexTrace < 0) { totalTrace = 0; }
        else { totalTrace = shopItemsQuantity[indexTrace]; }

        //Empty
        int indexEmpty = Array.IndexOf(itemsTypeShop, "Empty");
        if (indexEmpty < 0) { totalEmpty = 0; }
        else { totalEmpty = shopItemsQuantity[indexEmpty]; }

        //Random Items
        int indexBubbleBottle = Array.IndexOf(itemsTypeShop, "BubbleBottle");
        totalBubbleBottle = shopItemsQuantity[indexBubbleBottle];

        int indexClock = Array.IndexOf(itemsTypeShop, "Clock");
        totalClock = shopItemsQuantity[indexClock];

        int indexCoin = Array.IndexOf(itemsTypeShop, "Coin");
        totalCoin = shopItemsQuantity[indexCoin];

        int indexCrayon = Array.IndexOf(itemsTypeShop, "Crayon");
        totalCrayon = shopItemsQuantity[indexCrayon];

        int indexMatch = Array.IndexOf(itemsTypeShop, "Match");
        totalMatch = shopItemsQuantity[indexMatch];

        int indexPond = Array.IndexOf(itemsTypeShop, "Pond");
        totalPond = shopItemsQuantity[indexPond];

        int indexShootingStar = Array.IndexOf(itemsTypeShop, "ShootingStar");
        totalSStar = shopItemsQuantity[indexShootingStar];

        int indexStar = Array.IndexOf(itemsTypeShop, "Star");
        totalStar = shopItemsQuantity[indexStar];

        UpdateShopDictionary();//update the shop dictionary 

    }

    //called from CountTotalItemShop, update the dictionary using the totals of each items, the dictionary is saved for LabDictionary
    public void UpdateShopDictionary()
    {
        //Substances
        ShopDictionary["Colorant"] = totalColorant;
        ShopDictionary["Fragrance"] = totalFragrance;
        ShopDictionary["Sunflower oil"] = totalOil;
        ShopDictionary["Lye Solution"] = totalLye;

        //Equipment
        ShopDictionary["Curing Form"] = totalCuringForms;
        ShopDictionary["Heating Pot 200W"] = totalHeatingPot200;
        ShopDictionary["Heating Pot 400W"] = totalHeatingPot400;
        ShopDictionary["Heating Pot 600W"] = totalHeatingPot600;

        //Products
        ShopDictionary["Trace"] = totalTrace;
        ShopDictionary["Soap"] = totalSoap;
        //Debug.Log("this is the total of soaps" + totalSoap);

        //Empty
        ShopDictionary["Empty"] = totalEmpty;

        //Random

        ShopDictionary["BubbleBottle"] = totalBubbleBottle;
        ShopDictionary["Clock"] = totalClock;
        ShopDictionary["Coin"] = totalCoin;
        ShopDictionary["Crayon"] = totalCrayon;
        ShopDictionary["Match"] = totalMatch;
        ShopDictionary["Pond"] = totalPond;
        ShopDictionary["ShootingStar"] = totalSStar;
        ShopDictionary["Star"] = totalStar;

    }

    //SAVING OPTIONS
    void OnApplicationQuit()
    {
        Save(); //save data if the user quits
        Debug.Log(">> Shop is saved on quit.");
    }

    //saving data with EasyFileSave
    public void Save()
    {
        
        CountTotalItemsShop();//Count and update the ShopDictionary
        //inventories = RecordItemsInventoryShop();
        EasyFileSave myFile = new EasyFileSave();

        //see EasyFileSave manual for more
        myFile.Add("coins", coins);
        Debug.Log(">> Money saved: " + coins);
        CoinsTXT.text = "MONEY:" + coins;

        //isRead = false;
        //Static bool to check if Labtypes where read once
        myFile.Add("isRead", isRead);

        for (int i = 1; i < 19; i++)
        {
            //TODO check if a for loop is necessary?
            //myFile.Add("shopItemsQuantity", shopItemsQuantity);
            myFile.Add("itemsTypeLab", itemsTypeLab);
            myFile.Add("ShopDictionary", ShopDictionary);
            //TEST TODO
            myFile.AddSerialized("inventories", inventories);


            //string[] itemsTypeShop = new string[19];
            //for (int i = 1; i < 19; i++)
            //{
            //    if (itemsInfoShop[i].item == null)
            //    {
            //        itemsTypeShop[i] = "Empty";
            //        myFile.Add("itemsInfoShop", itemsTypeShop);
            //        //PlayerPrefs.SetString("typeof" + i, null);
            //    }

            //    else
            //    {
            //        itemsTypeShop[i] = itemsInfoShop[i].item.name_item;
            //        myFile.Add("itemsInfoShop", itemsTypeShop);
            //        //PlayerPrefs.SetString("typeof" + i, itemsTypeShop[i]);
            //    }
        }
        Debug.Log(">> Shop is saved.");
        myFile.Save();


        //TEST
        //counter++;
        //Debug.Log("counter:"+ counter);
    }

    //loading data with EasyFileSave
    public void Load()
    {
        EasyFileSave myFile = new EasyFileSave();
        if (myFile.Load())
        {
            Debug.Log(">> Content is loaded with money " + myFile.GetInt("coins"));
            CoinsTXT.text = "MONEY:" + myFile.GetInt("coins");

            for (int i = 1; i < 19; i++)
            {
                itemsTypeLab = myFile.GetArray<string>("itemsTypeLab");

                //shopItemsQuantity = myFile.GetArray<int>("shopItemsQuantity");
                //Debug.Log("get items quantity");
                //itemsTypeShop = myFile.GetArray<string>("itemsTypeShop");

            }

            ShopDictionary = myFile.GetDictionary<string, int>("ShopDictionary");
            coins = myFile.GetInt("coins");


            inventories = (List<Containers>)myFile.GetDeserialized("inventories", typeof(List<Containers>));

            //Static bool to check if Labtypes where read once
            isRead = myFile.GetBool("isRead");
            //Debug.Log("it is read"+isRead);
        }

        //Debug.Log("inventories loaded" + inventories);
        //try
        //{
        //    foreach (var items in inventories)
        //    {
        //        Debug.Log("amount1: " + items.amount1 + "of item" + items.type + "in position" + items.position);
        //        Debug.Log("amount2: " + items.amount2 + "of item" + items.type + "in position" + items.position);


        //    }
        //    Debug.Log("The inventory 4th has: " + inventories[4].amount1+ inventories[4].type);
        //    Debug.Log("The inventory 4th has: " + inventories[4].amount2 + inventories[4].type);
        //}
        //catch
        //{
        //    Debug.Log("couldn't load amigo");
        //}

        //TEST, recommended but not sure if it works, added 23/06/2021
        myFile.Dispose();
    }


    //saving Options with button, enable and quit
    public void Saveshop()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        Save();
        Debug.Log(">> Shop is saved by user.");


    }

    //METHODS for Random items when coming back from lab
    private void CheckCrayon()
    {
        if (ShopDictionary["Crayon"] >= 1)
        {
            colorChanger.GetComponent<ColorChange>().ChangeColor();
            //Debug.Log("A crayon was found");
        }
    }

   private void CheckCoin(int i)
    {
        //print("checking coin ");
        if (ButtonInfo.updateEquipment == false)
        {
            if (ShopDictionary["Coin"] >= 1)
            {
                itemsInfoShop[i].UpdateEquipmentSellingPrices();
                Debug.Log("Equipment prices updated");
            }

        }

    }

    private void CheckPond(int i)
    {
        //print("checking pond");
        if (ButtonInfo.updateSubstance == false)
        {
            if (ShopDictionary["Pond"] >= 1)
            {
                itemsInfoShop[i].UpdateSubstancesSellingPrice();
                Debug.Log("Substances prices updated");
            }

        }
    }

    private void CheckStar(int i)
    {
        //print("checking star ");
        if (ButtonInfo.updateSoap == false)
        {
            if (ShopDictionary["Star"] >= 1)
            {
                itemsInfoShop[i].UpdateSoapSellingPrice();
                Debug.Log("Soap prices updated");
            }

        }
    }


    //****************************TESTS*************************


    //public void AssignItemShop()//Assign the scriptable objects according to their "type"
    //{

    //    for (int i = 1; i < 19; i++)
    //    {
    //        if (shopItemsQuantity[i] > 0)
    //        {

    //            switch (itemsTypeShop[i])

    //            {
    //                //////Equipment
    //                //case "Curing Form":
    //                //    itemsInfoShop[i].item = CuringForm;
    //                //    //update the number of batches 
    //                //    break;
    //                //case "Heating Pot 200W":
    //                //    itemsInfoShop[i].item = HeatingPot200;
    //                //    break;
    //                //case "Heating Pot 400W":
    //                //    itemsInfoShop[i].item = HeatingPot400;
    //                //    break;
    //                //case "Heating Pot 600W":
    //                //    itemsInfoShop[i].item = HeatingPot600;
    //                //    break;

    //                //Product
    //                case "Soap":
    //                    itemsInfoShop[i].item = soap;
    //                    break;

    //                //Intermediate
    //                case "Trace":
    //                    itemsInfoShop[i].item = trace;
    //                    break;

    //                //////Substances
    //                //case "Sunflower oil":
    //                //    itemsInfoShop[i].item = oil;
    //                //    break;
    //                //case "Colorant":
    //                //    itemsInfoShop[i].item = Colorant;
    //                //    break;
    //                //case "Lye Solution":
    //                //    itemsInfoShop[i].item = LyeSolution;
    //                //    break;
    //                //case "Fragrance":
    //                //    itemsInfoShop[i].item = Fragrance;
    //                //    break;
    //                //default:
    //                //    itemsInfoShop[i].item = empty;
    //                //    break;
    //            }
    //        }

    //        else if (shopItemsQuantity[i] < 0)
    //        {
    //            itemsInfoShop[i].item = empty;
    //        }

    //    }

    //    //THIS WORKS
    //    //for (int i = 1; i < 19; i++)
    //    //    if (itemsTypeShop[i]=="Trace")
    //    //    {
    //    //        itemsInfoShop[i].item = trace;
    //    //        Debug.Log("working...");
    //    //    }
    //}


    //TEST
    //public void CreateDictionaryShop()
    //{
    //    //Types - total of items 

    //    //for (int i = 1; i < 19; i++)
    //    //{
    //    //    ShopDictionary.Add();
    //    //}

    //    ShopDictionary = new Dictionary<string, int>
    //    {
    //        {"Colorant", 0},{"Fragrance", 0},{"Sunflower oil", 0},
    //        {"Lye Solution", 0},{"Curing Form", 0},{"Heating Pot 200W", 0},
    //        {"Heating Pot 400W", 0},{"Heating Pot 600W", 0},{"Trace", 0},
    //        {"Soap", 0},{"Empty", 0}
    //    };

    //    Debug.Log("This is the colorant in the dictionary"+ShopDictionary["Colorant"]);
    //}


    //////Substances
    //totalColorant = myFile.GetInt("totalColorant");
    //totalFragrance = myFile.GetInt("totalFragrance");
    //totalOil = myFile.GetInt("totalOil");
    //totalLye = myFile.GetInt("totalLye");


    ////Equipment
    //totalCuringForms = myFile.GetInt("totalCuringForms");
    //totalHeatingPot200 = myFile.GetInt("totalHeatingPot200");
    //totalHeatingPot400 = myFile.GetInt("totalHeatingPot400");
    //totalHeatingPot600 = myFile.GetInt("totalHeatingPot600");

    ////Products
    //totalSoap = myFile.GetInt("totalSoap");
    //totalTrace = myFile.GetInt("totalTrace");
    //totalEmpty = myFile.GetInt("totalEmpty");


    //Debug.Log(">> the total empty " + myFile.GetInt("totalEmpty"));
    //Debug.Log(">> the total lye " + myFile.GetInt("totalLye"));

    //Debug.Log(">> Content is loaded from data file.");
    //Debug.Log(">> Money is " + myFile.GetInt("coins"));


    //Equipment
    //myFile.Add("totalCuringForms", totalCuringForms);
    //myFile.Add("totalHeatingPot200", totalHeatingPot200);
    //myFile.Add("totalHeatingPot400", totalHeatingPot400);
    //myFile.Add("totalHeatingPot600", totalHeatingPot600);

    ////Substances
    //myFile.Add("totalColorant", totalColorant);
    //myFile.Add("totalFragrance", totalFragrance);
    //myFile.Add("totalOil", totalOil);
    //myFile.Add("totalLye", totalLye);

    ////Products
    //myFile.Add("totalSoap", totalSoap);
    //myFile.Add("totalTrace", totalTrace);

    ////Empty
    //myFile.Add("totalEmpty", totalEmpty);


    //private List<Containers> RecordItemsInventoryShop()
    //{
    //    //To find again the inventoryDictionaries, check if necessary
    //    FindItemsInventory();

    //    Debug.Log("creating inventories in the shop");
    //    //call the list
    //    inventories = new List<Containers>();

    //    for (int i = 1; i < 19; i++)
    //    {
    //        //check if the inventory dictionary exists (non negative), if not add to the list an empty container. This case works mostly from substances or empty equipments
    //        if (inventoryDictionaries[i].inventory.Count <= 0)
    //        {
    //            inventories.Add(new Containers
    //            {
    //                position = i,
    //                type = itemsTypeLab[i],
    //                itemdropped1 = "None",
    //                itemdropped2 = "None",
    //                itemdropped3 = "None",
    //                amount1 = 0,
    //                amount2 = 0,
    //                amount3 = 0,

    //            });
    //        }

    //        //if the item has an inventory dictionary, record the items in it 
    //        else
    //        {
    //            //Case Curing Form
    //            if (itemsTypeLab[i] == "Curing Form")
    //            {
    //                inventories.Add(new Containers
    //                {
    //                    position = i,
    //                    type = itemsTypeLab[i],
    //                    itemdropped1 = "Colorant",
    //                    itemdropped2 = "Fragrance",
    //                    itemdropped3 = "Trace",
    //                    amount1 = inventoryDictionaries[i].inventory["Colorant"],
    //                    amount2 = inventoryDictionaries[i].inventory["Fragrance"],
    //                    amount3 = inventoryDictionaries[i].inventory["Trace"]

    //                });
    //            }

    //            //Case Heater 200/400/600
    //            else if (itemsTypeLab[i] == "Heating Pot 200W" || itemsTypeLab[i] == "Heating Pot 400W" || itemsTypeLab[i] == "Heating Pot 600W")
    //            {
    //                inventories.Add(new Containers
    //                {
    //                    position = i,
    //                    type = itemsTypeLab[i],
    //                    itemdropped1 = "Sunflower oil",
    //                    itemdropped2 = "Lye Solution",

    //                    amount1 = inventoryDictionaries[i].inventory["Sunflower oil"],
    //                    amount2 = inventoryDictionaries[i].inventory["Lye Solution"]

    //                });
    //            }
    //        }
    //    }
    //    return inventories;
    //}

    //private void FindItemsInventory()
    //{
    //    //inventories = new List<Containers>();
    //    for (int i = 1; i < 19; i++)
    //    {
    //        //labItemsInventory[i] = labItemsList[i].GetComponent<ItemsInventory>();
    //        inventoryDictionaries[i] = shopItemsList[i].GetComponent<InventoryDictionary>();
    //    }
    //}



}


