using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TigerForge;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class LabManagerScript : MonoBehaviour
{

    //class to use EasyFileSave utilities
    EasyFileSave myFile;

    //Serialize data that is saved by EasyFileSave
    //Serialization: give a name and number to find data with a consistent path
    //Serialization/deserialization of input/output data
    [Serializable]
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

        //public int progress = 0;
        public int progress;
    }

    //1D array
    public int[] labItemsQuantity = new int[19];//amount of items multiply by batch
    public int[] numberBatches = new int[19];//Save the amount bought in the shop
    public GameObject[] labItemsList = new GameObject[19];//array to save variables of each gameobject
    public ButtonInfoLab[] itemsInfoLab = new ButtonInfoLab[19];//array to save each script of buttoninfolab and derived classes
    public string[] itemsTypeLab = new string[19];//items type list

    //Save coins
    public Text CoinTxtlab;

    //Reference to change scriptable objects at runtime
    public Equipment HeatingPot200, HeatingPot400, HeatingPot600, CuringForm;
    public Substance Fragrance, LyeSolution, oil, Colorant;
    public Intermediate trace;
    public Product soap;
    public Default empty;
    public RandomItem  crayon, coin, clock, match, pond, star, sstar, bubblebottle;
 

    //Counters
    private int totalCuringForms, totalHeatingPot200, totalHeatingPot400, totalHeatingPot600;
    private int totalColorant, totalFragrance, totalOil, totalLye;
    private int totalSoap, totalTrace;
    private int totalEmpty;

    private int totalMatch, totalCrayon, totalCoin, totalClock, totalPond, totalStar, totalSStar, totalBubbleBottle;

    //LabDictionary: save values and types 
    public Dictionary<string, int> LabDictionary = new Dictionary<string, int>();

    //prefabs for instantiating objects
    public GameObject prefabcolorant, prefabfragrance, prefaboil, prefablye, prefabsoap, prefabtrace, prefabempty;//substances
    public GameObject prefabheater200, prefabheater400, prefabheater600, prefabcuring;//equipment
    public GameObject prefabmatch, prefabcrayon, prefabcoin, prefabclock, prefabpond, prefabsstar, prefabstar, prefabbubblebottle;//Random

    public GameObject[] labHoldersList = new GameObject[19];
    //public Transform[] transformItems = new Transform[19];


    //Read the containers of each equipment
    public InventoryDictionary[] inventoryDictionaries = new InventoryDictionary[19];
    public ItemsInventory[] labItemsInventory = new ItemsInventory[19];
    public List<Containers> inventories = new List<Containers>();//Save the data from itemsinventory dictionary
    public ProgressBar[] progressBars = new ProgressBar[19];


    //Chat System
    public GameObject chatManagerLab, questionsLab;
    public ChatManager chatManagerScriptLab;
    public ChatTrigger chatTriggerScriptLab;

    //TEST to debug saving inventories
    //public static int counterlab = 0;
    public static bool isRead = false;

    //Reference color changer
    public GameObject colorChanger;

    //First to be called
    //Reference: https://docs.unity3d.com/Manual/ExecutionOrder.html
    void Awake()
    {
        //NoDestroyOnLoad anymore. Data is read from the serialized data file.
        FindHolders();//Find labholders LabHoldersList

        //TO DO-SS: Running this function distrupts the data saving.//JD changed to Start() and now it seems to work
        //CalculateQuantity();


    }

    //Second to be called
    void OnEnable()
    {
        Load();//Load the dictionary and labtype
        InstantiateAllItems();//Create instances
        FindItemsinLabScene();//Find Game objects
        FindButtonInfoLabScript();//Find Buttoninfolab scripts
        FindItemsInventory();//Find the items inventory and inventory dictionary scripts
        Debug.Log(">> Application is saved");
    }

    //Third to be called
    void Start()
    {
        Debug.Log(">> Lab is started." + "\n");
        myFile = new EasyFileSave();
        myFile.suppressWarning = false;
        AssignItemLab();//Assign the item types accordingly, important when changing from lab to shop, uses ItemTypesLab array and LabDictionary
        FindProgresssBars();//Necessary to update the progress bars when a process was running and check in the UPDATE method
        CalculateQuantity();//Update the quantities using the batch and the amount written in the scriptable object
        UpdateLabLabels();//update the labels from buttoninfolab scripts

        for (int i = 1; i < 19; i++)
        {
            if (labItemsQuantity[i] == 0)
            {
                labItemsList[i].SetActive(false);
            }
        }//Set unactive gameobjects with no batch

        //Chat System
        chatManagerLab = GameObject.Find("ChatManager");
        chatManagerScriptLab = chatManagerLab.GetComponent<ChatManager>();

        //To trigger chat
        questionsLab = GameObject.Find("Questions");
        //chatTriggerScriptLab = questionsLab.GetComponent<ChatTrigger>();
        //chatTriggerScriptLab.TriggerChat();
        //Debug.Log("triggering chat");

        //Random items 
        CheckCrayon();

    }


    //Check the equipments with a progress ongoing, it is needed to record if an equipment is working
    private void Update()
    {
        
        for (int i = 1; i < 19; i++)
        {
            if (progressBars[i].isUpdating)

            {
                //CheckClock();//Check if the clock is bought and modify the value of the process
                inventoryDictionaries[i].inventory["Process"] = progressBars[i].progressTick;

            }
        }


    }

    //This function stop the timetick system when changing scenes, it is called from "GO TO LAB" button
    public void StopClocks()
    {
        for (int i = 1; i < 19; i++)
        {
            if (progressBars[i].isUpdating)
            {
                progressBars[i].StopClock();
                //StopAllCoroutines();
                //StopCoroutine(nameof(CreateSoap));
                //StopCoroutine(nameof(CreateTrace));
            }
        }

    }

    //Methods
    private void FindProgresssBars()
    {
        for (int i = 1; i < 19; i++)
        {
            progressBars[i] = labItemsList[i].GetComponentInChildren<ProgressBar>();
        }

    }

    public void FindHolders()
    {
        for (int i = 1; i < 19; i++)
        {
            labHoldersList[i] = GameObject.Find("Item_" + i);
        }
    } //Find Lab holders in scene: LabHoldersList

    public void FindItemsinLabScene() //Find Gameobjects in scene: labItemsList
    {
        for (int i = 1; i < 19; i++)
        {
            labItemsList[i] = GameObject.Find("Item_lab" + i);
        }
    }

    public void FindItemsInventory() //Find itemsinventory and inventory dictionary from all objects
    {
        //inventories = new List<Containers>();
        for (int i = 1; i < 19; i++)
        {
            labItemsInventory[i] = labItemsList[i].GetComponent<ItemsInventory>();
            inventoryDictionaries[i] = labItemsList[i].GetComponent<InventoryDictionary>();
        }
    }

    public void UpdateLabLabels()
    {

        for (int i = 1; i < 19; i++)
        {
            itemsInfoLab[i].UpdateComponents();//updates name, artwork
            //The quantity is updated in each gameobject as none all of them has a buttoninfo labitem
            //itemsInfoLab[i].GetComponent<ButtonInfoLabItem>().UpdateQuantityLabel();//updates quantity and name

        }
    }//Call function from buttoninfolab to update name and artwork

    public void FindButtonInfoLabScript()
    {
        for (int i = 1; i < 19; i++)
        {
            itemsInfoLab[i] = labItemsList[i].GetComponent<ButtonInfoLab>();
            itemsInfoLab[i].ItemID = i;//Change the itemID according to the place in the "Lab"
        }
        //ReadLabTypes();
    }//Find the buttoninfolab scripts from each gameobject

    public void ReadLabTypes()
    {
        //itemsTypeLab = new string[19];
        for (int i = 1; i < 19; i++)
        {
            itemsTypeLab[i] = itemsInfoLab[i].item.name_item;
        }
    }//Read the item types from start, it is called from equipment to update types

    public void CalculateQuantity()
    {
        for (int i = 1; i < 19; i++)
        {
            //2D array
            //labItems[3,i] = PlayerPrefs.GetInt("quantityof"+i)*itemsInfoList[i].item.quantity_lab;
            //labItemsQuantity[i] = numberBatches[i] * itemsInfoLab[i].item.quantity_lab;

            if (itemsInfoLab[i].item != null)
            {
                labItemsQuantity[i] = numberBatches[i] * itemsInfoLab[i].item.quantity_lab;
            }
        }

    }//calculate the amount in the lab based on the batches and quantitites of scriptable objects

    public void InstantiateAllItems()
    {
        for (int i = 1; i < 19; i++)
        {
            switch (itemsTypeLab[i])
            {
                //Equipment
                case "Curing Form":
                    InstantiateItems(prefabcuring, labHoldersList[i].transform, i);

                    break;
                case "Heating Pot 200W":
                    InstantiateItems(prefabheater200, labHoldersList[i].transform, i);

                    break;
                case "Heating Pot 400W":
                    InstantiateItems(prefabheater400, labHoldersList[i].transform, i);
                    break;
                case "Heating Pot 600W":
                    InstantiateItems(prefabheater600, labHoldersList[i].transform, i);
                    break;

                //Product
                case "Soap":
                    InstantiateItems(prefabsoap, labHoldersList[i].transform, i);
                    break;

                //Intermediate
                case "Trace":
                    InstantiateItems(prefabtrace, labHoldersList[i].transform, i);
                    break;

                ////Substances
                case "Sunflower oil":
                    InstantiateItems(prefaboil, labHoldersList[i].transform, i);
                    break;
                case "Colorant":
                    InstantiateItems(prefabcolorant, labHoldersList[i].transform, i);
                    break;
                case "Lye Solution":
                    InstantiateItems(prefablye, labHoldersList[i].transform, i);
                    break;
                case "Fragrance":
                    InstantiateItems(prefabfragrance, labHoldersList[i].transform, i);
                    break;

                case "Empty":
                    InstantiateItems(prefabempty, labHoldersList[i].transform, i);
                    break;
                //Random
                case "BubbleBottle":
                    InstantiateItems(prefabbubblebottle, labHoldersList[i].transform, i);
                    break;
                case "Clock":
                    InstantiateItems(prefabclock, labHoldersList[i].transform, i);
                    break;
                case "Coin":
                    InstantiateItems(prefabcoin, labHoldersList[i].transform, i);
                    break;
                case "Crayon":
                    InstantiateItems(prefabcrayon, labHoldersList[i].transform, i);
                    break;
                case "Match":
                    InstantiateItems(prefabmatch, labHoldersList[i].transform, i);
                    break;
                case "Pond":
                    InstantiateItems(prefabpond, labHoldersList[i].transform, i);
                    break;
                case "ShootingStar":
                    InstantiateItems(prefabsstar, labHoldersList[i].transform, i);
                    break;
                case "Star":
                    InstantiateItems(prefabstar, labHoldersList[i].transform, i);
                    break;
            }


        }
    } //Create instance of objects bought in the shop using labHoldersList to place them

    public void InstantiateItems(GameObject prefab, Transform parentTransform, int index)
    {
        var item = Instantiate(prefab, parentTransform);
        item.transform.SetParent(parentTransform);
        item.transform.localPosition = new Vector3(0, 0, 0);
        //Debug.Log("Creating an item in"+ parentTransform);

        //Name the item using the place
        string itemLabel = "Item_lab" + index;
        item.name = itemLabel;
    } //Method to made instances

    public void AssignItemLab()
    {
        int colorants = LabDictionary["Colorant"];
        int fragrances = LabDictionary["Fragrance"];
        int oils = LabDictionary["Sunflower oil"];
        int lyes = LabDictionary["Lye Solution"];

        //Products
        int traces = LabDictionary["Trace"];
        int soaps = LabDictionary["Soap"];

        //Equipment
        int curingforms = LabDictionary["Curing Form"];
        int pots200 = LabDictionary["Heating Pot 200W"];
        int pots400 = LabDictionary["Heating Pot 400W"];
        int pots600 = LabDictionary["Heating Pot 600W"];

        //Empty
        int emptyitems = LabDictionary["Empty"];

        //Random-TODO check if necessary
        int matches = LabDictionary["Match"];

        for (int i = 1; i < 19; i++)
        {
           
            //if (numberBatches[i] > 0)
            //{
            switch (itemsTypeLab[i])

            {
                //Equipment
                case "Curing Form":
                    itemsInfoLab[i].item = CuringForm;
                    numberBatches[i] = 1;
                    RefillCuringForm(i);

                    break;
                case "Heating Pot 200W":
                    itemsInfoLab[i].item = HeatingPot200;
                    numberBatches[i] = 1;
                    RefillHeater(i);

                    break;
                case "Heating Pot 400W":
                    itemsInfoLab[i].item = HeatingPot400;
                    numberBatches[i] = 1;
                    RefillHeater(i);
                    break;
                case "Heating Pot 600W":
                    itemsInfoLab[i].item = HeatingPot600;
                    numberBatches[i] = 1;
                    RefillHeater(i);
                    break;

                //Product
                case "Soap":
                    itemsInfoLab[i].item = soap;
                    numberBatches[i] = 1;
                    break;

                //Intermediate
                case "Trace":
                    itemsInfoLab[i].item = trace;
                    numberBatches[i] = 1;
                    break;

                ////Substances
                case "Sunflower oil":
                    itemsInfoLab[i].item = oil;
                    numberBatches[i] = oils;
                    break;
                case "Colorant":
                    itemsInfoLab[i].item = Colorant;
                    numberBatches[i] = colorants;
                    break;
                case "Lye Solution":
                    itemsInfoLab[i].item = LyeSolution;
                    numberBatches[i] = lyes;
                    break;
                case "Fragrance":
                    itemsInfoLab[i].item = Fragrance;
                    numberBatches[i] = fragrances;
                    break;

                case "Empty":
                    itemsInfoLab[i].item = empty;
                    numberBatches[i] = 0;
                    break;

                //Random
                case "BubbleBottle":
                    itemsInfoLab[i].item = bubblebottle;
                    numberBatches[i] = 1;
                    break;

                case "Clock":
                    itemsInfoLab[i].item = clock;
                    numberBatches[i] = 1;
                    break;

                case "Coin":
                    itemsInfoLab[i].item = coin;
                    numberBatches[i] = 1;
                    break;

                case "Crayon":
                    itemsInfoLab[i].item = crayon;
                    numberBatches[i] = 1;
                    break;
                case "Match":
                    itemsInfoLab[i].item = match;
                    //numberBatches[i] = matches;
                    numberBatches[i] = 1;
                    break;
                case "Pond":
                    itemsInfoLab[i].item = pond;
                    numberBatches[i] = 1;
                    break;
                case "ShootingStar":
                    itemsInfoLab[i].item = sstar;
                    numberBatches[i] = 1;
                    break;
                case "Star":
                    itemsInfoLab[i].item = star;
                    numberBatches[i] = 1;
                    break;
            }

        }


        //Local method to refill the curing form when coming from shop and the user dropped items in the equipment but do not create soap
        void RefillCuringForm(int i)
        {
            
            //find in the list of inventories the one that has as "position" the item in the lab and read the amount
            var resultC = inventories.Find(x => x.position == i);
            //Add to the items inventory (used in curing slot script) and its corresponding amount
            labItemsInventory[i].AddItemTo(Colorant, resultC.amount1);
            labItemsInventory[i].AddItemTo(Fragrance, resultC.amount2);
            labItemsInventory[i].AddItemTo(trace, resultC.amount3);
            
            //Update the images
            EquipmentSlot equipmentSlot= labItemsList[i].GetComponent<EquipmentSlot>();
            equipmentSlot.UpdateCuringFormImages();

            // Add the progress time to the inventory of each item, this value is read then  by progressbar on the equipment
            inventoryDictionaries[i].inventory["Process"] = resultC.progress;
            //TEST BUG switching scenes
            inventoryDictionaries[i].inventory["Colorant"] = resultC.amount1;
            inventoryDictionaries[i].inventory["Fragrance"] = resultC.amount2;
            inventoryDictionaries[i].inventory["Trace"] = resultC.amount3;

            //if there was a progress ongoing, activate the progressbar and update the slider accordingly
            if (resultC.progress > 0)
            {
                //Debug.Log("Find an ongoing process in curing form");
                ProgressBar progressBar = labItemsList[i].GetComponentInChildren<ProgressBar>();
                progressBar.progressTick = resultC.progress;

                //Debug.Log("progress bar tick" + progressBar.progressTick + "curing");


                equipmentSlot.ActivateProgressBar();
                progressBar.SetProgress(progressBar.progressTick * (100 / progressBar.progressTickMax));
                StartCoroutine(equipmentSlot.CreateSoapAfterTime(1));

            }

           

        }

        //Local method to refill the curing form when coming from shop and the user dropped items in the equipment but do not create trace
        void RefillHeater(int i)
        {

            //find in the list of inventories the one that has as "position" the item in the lab and read the amount
            var resultH = inventories.Find(x => x.position == i);

            ////TODO-remove for debug
            //Debug.Log("this is the progress found "+resultH.progress);

            //Add to the items inventory (used in heater slot script) and its corresponding amount
            labItemsInventory[i].AddItemTo(oil, resultH.amount1);
            labItemsInventory[i].AddItemTo(LyeSolution, resultH.amount2);

            //Update the images
            EquipmentSlot equipmentSlot = labItemsList[i].GetComponent<EquipmentSlot>();
            equipmentSlot.UpdateLyeImages();
            equipmentSlot.UpdateOilimages();

            //Add the progress time to the inventory of each item, this value is read then  by progressbar on the equipment
            inventoryDictionaries[i].inventory["Process"] = resultH.progress;
            //TEST bug switching scenes
            inventoryDictionaries[i].inventory["Sunflower oil"] = resultH.amount1;
            inventoryDictionaries[i].inventory["Lye Solution"] = resultH.amount2;


            //if there was a progress ongoing, activate the progressbar and update the slider accordingly
            if (resultH.progress > 0)
            {
                //Debug.Log("Find an ongoing process in heater");
                ProgressBar progressBar = labItemsList[i].GetComponentInChildren<ProgressBar>();
                progressBar.progressTick = resultH.progress;

                //Debug.Log("progress bar tick" + progressBar.progressTick+"heater");

                //equipmentSlot.TurnOn();
                equipmentSlot.ActivateProgressBar();
                progressBar.SetProgress(progressBar.progressTick * (100 / progressBar.progressTickMax));

                //Create correct number of traces depending on the heater (when changing scenes)
                switch (itemsTypeLab[i])
                {
                    case "Heating Pot 200W":
                        StartCoroutine(equipmentSlot.CreateTraceAfterTime());
                        break;
                    case "Heating Pot 400W":
                        for (int y = 0; y < 2; y++)
                        {
                            //var delay = progressBar.progressTickMax * progressBar.processTime;
                            StartCoroutine(equipmentSlot.CreateTraceAfterTime());//Reference in EquipmentSlot
                        }

                        break;
                    case "Heating Pot 600W":
                        for (int y = 0; y < 3; y++)
                        {
                            //var delay = progressBar.progressTickMax * progressBar.processTime;
                            StartCoroutine(equipmentSlot.CreateTraceAfterTime()); //Reference in EquipmentSlot
                        }
                        break;
                }

            }

        }

    }//Assign the scriptable objects according to their "type" using the Labdictionary


    //Main Actions Lab
    public IEnumerator CreateTrace()//Called in HeaterSlot
    //public void CreateTrace()//Called in HeaterSlot
    {
        Debug.Log("start trace");
        for (int i = 1; i < 19; i++)
        {
            //Find item that have "empty" Scriptable Objects 
            if (itemsInfoLab[i].item == empty)//To find an empty spot
            {
                //Change to create an instance instead of changing parameters of the actual gameobject
                Destroy(labItemsList[i]);
                InstantiateItems(prefabtrace, labHoldersList[i].transform, i);

                //Need to find again the gameobject, buttoninfolab and rename it that is created
                labItemsList[i] = GameObject.Find("Item_lab" + i);
                itemsInfoLab[i] = labItemsList[i].GetComponent<ButtonInfoLab>();
                itemsInfoLab[i].ItemID = i;

                //Before only the item was reassign but this created problems if a place of equipment was taken by a substance or viceversa due to the different scripts needed for each item
                //itemsInfoLab[i].item = trace;//Set the item found to be trace
                //Add Curing Script to the prefab, this is necessary otherwise it cannot be dropped in the curing form later
                //GameObject buttonForCondition = labItemsList[i].transform.Find("buttonbuy").gameObject;
                //Curing curingScript = GetComponentInChildren<Curing>() ?? buttonForCondition.AddComponent<Curing>();
                //labItemsList[i].SetActive(true);//Activate the gameobject

                numberBatches[i]++;//increase the amount of soap
                ReadLabTypes();//Update the list of types
                CalculateQuantity();//Recalculate quantity
                itemsInfoLab[i].UpdateComponents();//update image, name, quantity

                CheckCrayon();//Check and change the color of the new created items
                //prefabtrace.GetComponent<FXController_Lab>().PlayAppear();

                Save();//Save data to appear in shop
                yield break;
            }
            //yield return new WaitForSeconds(0.1f);
            //yield return null;
        }

    }


    public IEnumerator CreateSoap(int value)//Called in CuringSlot
    //public void CreateSoap(int value)//Called in CuringSlot
    {
        Debug.Log("start soap");
        for (int i = 1; i < 19; i++)
        {   //Find item that have "empty" Scriptable Objects 
            if (itemsInfoLab[i].item == empty)//To find an empty spot
            {
                //Change to create an instance instead of changing parameters of the actual gameobject
                Destroy(labItemsList[i]);
                InstantiateItems(prefabsoap, labHoldersList[i].transform, i);

                //Need to find again the gameobject, buttoninfolab and rename it that is created
                labItemsList[i] = GameObject.Find("Item_lab" + i);
                itemsInfoLab[i] = labItemsList[i].GetComponent<ButtonInfoLab>();
                itemsInfoLab[i].ItemID = i;

                //itemsInfoLab[i].item = soap;//Set the item found to be soap
                //labItemsList[i].SetActive(true);//Activate the gameobject

                numberBatches[i] += value;//increase the amount of soap
                
                ReadLabTypes();//Update the list of types
                CalculateQuantity();//Recalculate quantity
                itemsInfoLab[i].UpdateComponents();//update image, name, quantity

                labItemsList[i].GetComponent<FXController_Lab>().PlayAppear();//Animation

                //Checks if a bubble bottle is bought and plays an extra VFX
                if (LabDictionary["BubbleBottle"] >= 1)
                {

                    labItemsList[i].GetComponent<FXController_Lab>().PlayOn();//Animation
                }

                CheckCrayon();//Check and change the color of the new created items

                Save();//Save data to appear in shop
                yield break;

            }
        }
        //yield return new WaitForSeconds(0.1f);
        //yield break;
    }


    /// <summary>
    /// Counting Method, count the items from the itemsTypeLab  and save the data in the LabDictionary.
    /// Necessary to ensure that Shop is static but Lab is dynamic and equipment is unstack
    /// </summary>
    public void CountTotalItemsLab()
    {
        //Count Substances
        int indexColorant = Array.IndexOf(itemsTypeLab, "Colorant");
        //check if exist in the array
        if (indexColorant < 0) { totalColorant = 0; }
        //check if it the batch is zero then the total will be zero. This is necessary otherwise the batch will be 1 even if the item was used in the lab
        else if (numberBatches[indexColorant] > 0) { totalColorant = numberBatches[indexColorant]; }
        //catch error
        else { totalColorant = 0; }
        

        int indexFragrance = Array.IndexOf(itemsTypeLab, "Fragrance");
        if (indexFragrance < 0) { totalFragrance = 0; }
        else if (numberBatches[indexFragrance] > 0) { totalFragrance = numberBatches[indexFragrance]; }
        else { totalFragrance = 0; }

        int indexOil = Array.IndexOf(itemsTypeLab, "Sunflower oil");
        if (indexOil < 0) { totalOil = 0; }
        else if (numberBatches[indexOil] > 0) { totalOil = numberBatches[indexOil]; }
        else { totalOil = 0; }


        int indexLye = Array.IndexOf(itemsTypeLab, "Lye Solution");
        if (indexLye < 0) { totalLye = 0; }
        else if (numberBatches[indexLye] > 0) { totalLye = numberBatches[indexLye]; }
        else { totalLye = 0; }

        //Count Equipment, count the number of times in the array
        totalCuringForms = itemsTypeLab.Count(s => s == "Curing Form");
        totalHeatingPot200 = itemsTypeLab.Count(s => s == "Heating Pot 200W");
        totalHeatingPot400 = itemsTypeLab.Count(s => s == "Heating Pot 400W");
        totalHeatingPot600 = itemsTypeLab.Count(s => s == "Heating Pot 600W");

        ////Count Products
        totalSoap = itemsTypeLab.Count(s => s == "Soap");
        totalTrace = itemsTypeLab.Count(s => s == "Trace");

        //Empty
        totalEmpty = itemsTypeLab.Count(s => s == "Empty");

        //CountRandom
        totalBubbleBottle = itemsTypeLab.Count(s => s == "BubbleBottle");
        totalClock = itemsTypeLab.Count(s => s == "Clock");
        totalCoin = itemsTypeLab.Count(s => s == "Coin");
        totalCrayon = itemsTypeLab.Count(s => s == "Crayon");
        totalMatch = itemsTypeLab.Count(s => s == "Match");
        totalPond = itemsTypeLab.Count(s => s == "Pond");
        totalSStar = itemsTypeLab.Count(s => s == "ShootingStar");
        totalStar = itemsTypeLab.Count(s => s == "Star");


        UpdateLabDictionary();
    }
    public void UpdateLabDictionary()
    {
        //print("the lab dictionary is updated");
        //Substances
        LabDictionary["Colorant"] = totalColorant;
        LabDictionary["Fragrance"] = totalFragrance;
        LabDictionary["Sunflower oil"] = totalOil;
        LabDictionary["Lye Solution"] = totalLye;

        //Equipment
        LabDictionary["Curing Form"] = totalCuringForms;
        LabDictionary["Heating Pot 200W"] = totalHeatingPot200;
        LabDictionary["Heating Pot 400W"] = totalHeatingPot400;
        LabDictionary["Heating Pot 600W"] = totalHeatingPot600;

        //Products
        LabDictionary["Trace"] = totalTrace;
        LabDictionary["Soap"] = totalSoap;

        //Empty
        LabDictionary["Empty"] = totalEmpty;

        //Random
        LabDictionary["BubbleBottle"] = totalBubbleBottle;
        LabDictionary["Clock"] = totalClock;
        LabDictionary["Coin"] = totalCoin;
        LabDictionary["Crayon"] = totalCrayon;
        LabDictionary["Match"] = totalMatch;
        //Debug.Log("total of matches" + totalMatch);
        LabDictionary["Pond"] = totalPond;
        LabDictionary["ShootingStar"] = totalSStar;
        LabDictionary["Star"] = totalStar;

    }//called from CountTotalItemsLab


    //SAVING OPTIONS
    void OnApplicationQuit()
    {
        Save(); //save data if the user quits
        Debug.Log(">> Application is saved");
    }

    
    //Called on save, create a list of type Container and save the information from inventory dictionary scripts
    public List<Containers> RecordItemsInventory()
    {
        //To find again the inventoryDictionaries, TODO check if necessary
        //FindItemsInventory();

        //call the list
        inventories = new List<Containers>();

        for (int i = 1; i < 19; i++)
        {
            //check if the inventory dictionary exists (non negative), if not add to the list an empty container. This case works mostly from substances or empty equipments
            //if (inventoryDictionaries[i].inventory.Count <= 0)
            //if(labItemsInventory[i]!=null)

            if (labItemsInventory[i].Container.Count <= 0)
            {
                //Debug.Log("this is the perro" + inventoryDictionaries[9].inventory["Process"]);
                inventories.Add(new Containers
                {
                    position = i,
                    type = itemsInfoLab[i].item.name_item,
                    itemdropped1 = "None",
                    itemdropped2 = "None",
                    itemdropped3 = "None",
                    amount1 = 0,
                    amount2 = 0,
                    amount3 = 0,
                    //if the process starts, the amounts are zero but the process is recorded
                    progress = inventoryDictionaries[i].inventory["Process"]

                });
            }

            //if the item has an inventory dictionary, record the items in it 
            else
            {
                //Case Curing Form
                if (itemsTypeLab[i] == "Curing Form")
                {
                    
                    inventories.Add(new Containers
                    {
                        position = i,
                        type = itemsInfoLab[i].item.name_item,
                        itemdropped1 = "Colorant",
                        itemdropped2 = "Fragrance",
                        itemdropped3 = "Trace",
                        amount1 = inventoryDictionaries[i].inventory["Colorant"],
                        amount2 = inventoryDictionaries[i].inventory["Fragrance"],
                        amount3 = inventoryDictionaries[i].inventory["Trace"],

                        progress = inventoryDictionaries[i].inventory["Process"]

                    }) ;
                }
            
                //Case Heater 200/400/600
                else if (itemsTypeLab[i] == "Heating Pot 200W" || itemsTypeLab[i] == "Heating Pot 400W" || itemsTypeLab[i] == "Heating Pot 600W")
                {

                    inventories.Add(new Containers
                    {
                        position = i,
                        type = itemsInfoLab[i].item.name_item,
                        itemdropped1 = "Sunflower oil",
                        itemdropped2 = "Lye Solution",

                        amount1 = inventoryDictionaries[i].inventory["Sunflower oil"],
                        amount2 = inventoryDictionaries[i].inventory["Lye Solution"],

                        progress = inventoryDictionaries[i].inventory["Process"]

                    }); ;
                }
            }

           
        }
        return inventories;
    }


    //saving data with EasyFileSave 
    // This is the main SAVE data funciton that "translates" the lab data to shop 
    public void Save()
    {
        CountTotalItemsLab();
        inventories = RecordItemsInventory();

        EasyFileSave myFile = new EasyFileSave();
        for (int i = 1; i < 19; i++)
        {
            //myFile.Add("shopItemsQuantity", numberBatches);
            myFile.Add("itemsTypeLab", itemsTypeLab);
            //change itemsTypeLab from shop to typeLab
            myFile.Add("ShopDictionary", LabDictionary);
            //myFile.AddBinary("inventories", inventories);
        }
        myFile.AddSerialized("inventories", inventories);

        //Debug.Log(">> Lab is saved.");
        myFile.Append();

        //TEST
        //counterlab++;
        //Debug.Log("counterlab:"+counterlab);
        //isRead = false;
    }

    //loading data with EasyFileSave
    public void Load()
    {
        Debug.Log("LabContent is not loaded");
        EasyFileSave myFile = new EasyFileSave();
        if (myFile.Load())
        {
            Debug.Log("LabContent is loaded with " + myFile.GetInt("coins"));
            CoinTxtlab.text = "MONEY:" + myFile.GetInt("coins");
            for (int i = 1; i < 19; i++)
            {
                //numberBatches = myFile.GetArray<int>("shopItemsQuantity");
                //itemsTypeLab = myFile.GetArray<string>("itemsTypeShop");

                //TODO check if a for loop is necessary?
                itemsTypeLab = myFile.GetArray<string>("itemsTypeLab");
            }

            //Save the lab dictionary as shop dictionary
            LabDictionary = myFile.GetDictionary<string, int>("ShopDictionary");

            //Save the List of containers with the information from inventorydictionary of heaters and curing form
            inventories = (List<Containers>)myFile.GetDeserialized("inventories", typeof(List<Containers>));

            //Static bool to check if Labtypes where read once
            isRead = myFile.GetBool("isRead");
            //Debug.Log("it is read" + isRead);

           
        }

        //Debug.Log("inventories loaded" + inventories);
        //try
        //{
        //    foreach (var items in inventories)
        //    {
        //        Debug.Log("amount1: " + items.amount1+"  "+"of item"+ items.type +"in position"+items.position);
        //        Debug.Log("amount2: " + items.amount2+ " "+"of item" + items.type+"in position"+items.position);
        //        Debug.Log("process: " + items.progress);
        //    }

        //    Debug.Log("The inventory 4th has: " + inventories[4].amount1 + inventories[4].type);
        //    Debug.Log("The inventory 4th has: " + inventories[4].amount2 + inventories[4].type);
        //}
        //catch
        //{
        //    Debug.Log("couldn't load amigo");
        //}

        //TEST, recommended but not sure if it works, added 23/06/2021
        myFile.Dispose();
    }

    //if save button is added to scene
    public void Savelab()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        Save();
        Debug.Log("Lab was saved");
    }


    //TODO check if necessary from curing/heater, currently not called. It is saving temporal data generated in the lab actions, called from curing slot and heater slot scripts
    public void TemporalSave()
    {
        //ReadLabTypes();
        EasyFileSave myFile = new EasyFileSave();

        for (int i = 1; i < 19; i++)
        {
            myFile.Add("labItemsQuantity", numberBatches);
            myFile.Add("itemsTypeLab", itemsTypeLab);
        }
        Debug.Log(">> Temporal Lab data is saved.");
        myFile.Append();
    }

    //Load data during runtime, not used so far, just TEST
    public void TemporalLoad()
    {
        Debug.Log("LabContent is not loaded");
        EasyFileSave myFile = new EasyFileSave();

        if (myFile.Load())
        {
            CoinTxtlab.text = "MONEY:" + myFile.GetInt("coins");
            for (int i = 1; i < 19; i++)
            {
                numberBatches = myFile.GetArray<int>("labItemsQuantity");
                itemsTypeLab = myFile.GetArray<string>("itemsInfoLab");
            }
        }
    }


    //METHODS for Random items
    private void CheckCrayon()
    {
        if (LabDictionary["Crayon"] >= 1)
        {
            //Debug.Log("A crayon in the lab");
            colorChanger.GetComponent<ColorChangeLab>().ChangeColor();
        }


    }

    //Clock make all process 10s
    //private void CheckClock()
    //{
    //    if (LabDictionary["Clock"] >= 1)
    //    {
    //        for (int i = 1; i < 19; i++)
    //        {

    //            foreach (ProgressBar progressTickMax in progressBars)
    //            {
    //                progressBars[i].ChangeProgressTickMax(progressBars[i]);
    //                //Debug.Log("The progressmax is" + progressBars[i].progressTickMax);
    //            }


    //        }    
    //    }

    //}


    //public void CheckMatch()
    //{
    //    //Added to the progressBar
    //}


    //**************************************************************TESTS*************************************************


    //private void ChangeProgressTickMax(ProgressBar progressBar)
    //{
    //    var value = progressBar.progressTickMax;
    //    Debug.Log("this is value" + value);
    //    var clockvalue = value - (value * 0.1f);
    //    Debug.Log("This is clock value" + clockvalue);
    //    progressBar.progressTickMax = (int)Math.Round(clockvalue);

    //    //progressBar.progressTickMax = 3;
    //}

    //public void Test()
    //{
    //    ItemsInventoryDictionary.AddItemToDic(name, 1, ContainersInventory);
    //}

    //public void AddItemToDic(String _name, int _amount, Dictionary<string, int> _dic)
    //{
    //    bool hasItem = false;
    //    for (int i = 0; i < ItemsInventoryDictionary.Count; i++)
    //    {
    //        if (ItemsInventoryDictionary[i].Dictionary_ == _dic)
    //        {
    //            ItemsInventoryDictionary[i].AddToDic(_name, _amount);
    //            hasItem = true;
    //            break;
    //        }
    //    }

    //        if(!hasItem)
    //            {
    //                for(int i=0; i<19; i++)
    //                    {
    //                        ItemsInventoryDictionary.Add(i, new ItemsInventoryDic(_dic));
    //                    }


    //            }
    //       }


    //public class ItemsInventoryDic
    //{
    //    //public int position;
    //    public Dictionary<string, int> Dictionary_ = new Dictionary<string, int>();

    //    public ItemsInventoryDic (Dictionary<string,int> _dic)
    //    {
    //        //position = _position;
    //        Dictionary_ = _dic;

    //    }

    //    public void AddToDic(String name, int amount)
    //    {
    //        Dictionary_.Add(name, amount);
    //    }


    //}

    /// <summary>
    /// TESTS not used or code deleted
    /// </summary>
    ///


    //instances
    //public Transform transformtest;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {

    //      var   object1= Instantiate(prefabsubstance, transformtest);
    //        object1.transform.SetParent(transformtest);
    //        object1.transform.localPosition = new Vector3(0, 0, 0);
    //        //object1.transform.localposition = new Vector3(0, 0, 0);

    //        string itemLabel = "Item_lab" + 1;
    //        object1.name = itemLabel;
    //    }

    //GameObject prefabequipment = Resources.Load("Prefabs/item_lab_HP200") as GameObject;
    //GameObject equipment = Instantiate(prefabequipment, transform.position, transform.rotation);


    //}

    //To assign all items to empty
    //public void RemoveItem()
    //{
    //    for (int i = 1; i < 19; i++)
    //    {
    //        if (itemsInfoLab[i].item != null)
    //        {
    //            itemsInfoLab[i].item = empty;
    //            labItemsList[i].SetActive(false);
    //            break;
    //        }
    //    }
    //}

    //TEST not used
    //public void UpdateBatchesSubstances()
    //{

    //Debug.Log("this is the index" + indexColorant);
    //Debug.Log("numberbatches" + numberBatches[indexColorant]);
    //Debug.Log("The total now is" + totalColorant);



    //for (int i = 1; i < 19; i++)
    //{
    //    int indexColorant = Array.IndexOf(itemsTypeLab, "Colorant");
    //    totalColorant = numberBatches[indexColorant];
    //    Debug.Log("this is the index" + indexColorant);

    //    //int indexFragrance = Array.IndexOf(itemsTypeLab, "Fragrance");

    //    for (int j=1; j < 19; j++)
    //    {
    //        totalColorant = numberBatches[indexColorant];
    //        Debug.Log("numberbatches" + numberBatches[indexColorant]);
    //        Debug.Log("The total now is" + totalColorant);
    //    }

    //Debug.Log("this is the index" + index);

    //var index = Array.FindIndex(itemsTypeLab, x => x.Contains("Colorant"));
    //int [] index = Array.IndexOf(itemsTypeLab, "Colorant");
    //Debug.Log("this is the index" + index);

    //switch (itemsTypeLab[i])

    //{
    //    ////Equipment
    //    //case "Curing Form":
    //    //            numberBatches[i] = totalCuringForms;
    //    //            Debug.Log("This is the amount of " + numberBatches[i]);
    //    //    //update the number of batches 
    //    //    break; 
    //    //        case "Heating Pot 200W":
    //    //        break;

    //    //        case "Heating Pot 400W":
    //    //        break;
    //    //        case "Heating Pot 600W":
    //    //        break;

    //    ////Product
    //    //case "Soap":
    //    //    numberBatches[i] = totalSoap;
    //    //    break;

    //    ////Intermediate
    //    //case "Trace":
    //    //    numberBatches[i] = totalTrace;
    //    //    break;

    //    ////Substances
    //    //case "Sunflower oil":
    //    //    numberBatches[i] = totalOil;
    //    //    break;

    //    case "Colorant":
    //        numberBatches[i] = totalColorant;
    //        Debug.Log("This is the batch number of colorant" + totalColorant + numberBatches[i]);
    //        break;


    //        //case "Lye Solution":
    //        //    numberBatches[i] = totalLye;
    //        //    break;
    //        //case "Fragrance":
    //        //    numberBatches[i] = totalFragrance;
    //        //    break;


    //}

    //}
    //}

    //public void AssignItemsLabo()
    //{
    //    Debug.Log("testing, testing");
    //    //TODO modify it using the LabDictionary
    //    int colorants = LabDictionary["Colorant"];
    //    int fragrances = LabDictionary["Fragrance"];
    //    int oils = LabDictionary["Sunflower oil"];
    //    int lyes = LabDictionary["Lye Solution"];

    //    //Products
    //    int traces = LabDictionary["Trace"];
    //    int soaps = LabDictionary["Soap"];

    //    //Equipment
    //    int curingforms = LabDictionary["Curing Form"];
    //    int pots200 = LabDictionary["Heating Pot 200W"];
    //    int pots400 = LabDictionary["Heating Pot 400W"];
    //    int pots600 = LabDictionary["Heating Pot 600W"];


    //    //Empty
    //    int emptyitems = LabDictionary["Empty"];

    //    for (int i = 1; i < 19; i++)
    //    {
    //        if (colorants > 0)
    //        { itemsInfoLab[i].item = Colorant; numberBatches[i] = colorants; print("this is the batches of experiment" + numberBatches[i]); break; }
    //        //ReadLabTypes();
    //    }
    //    // this is not working

    //    //print("this is the batches of experiment" + numberBatches[i]);

    //    for (int i = 1; i < 19; i++)
    //    {
    //        if (fragrances > 0) { itemsInfoLab[i].item = Fragrance; numberBatches[i] = fragrances; break; }

    //        //if (colorants > 0) { itemsInfoLab[i].item = Colorant; numberBatches[i] = colorants;  print("this is the batches of experiment" + numberBatches[i]);break;}
    //        if (oils > 0) { itemsInfoLab[i].item = oil; numberBatches[i] = oils; break; }
    //        if (lyes > 0) { itemsInfoLab[i].item = LyeSolution; numberBatches[i] = lyes; break; }

    //        //products
    //        if (soaps > 0) { itemsInfoLab[i].item = soap; numberBatches[i] = soaps; break; }
    //        if (traces > 0) { itemsInfoLab[i].item = trace; numberBatches[i] = traces; break; }

    //        //Equipment
    //        if (curingforms > 0)
    //        {

    //            itemsInfoLab[i].item = CuringForm;
    //            numberBatches[i] = curingforms;
    //            if (numberBatches[i] > 1)
    //            {
    //                Debug.Log("Unstacking element " + i + "with number of batches" + numberBatches[i]);
    //                for (int j = 1; j < numberBatches[i]; j++)
    //                {
    //                    Debug.Log("Unstacking item number " + j);
    //                    UnStackEquipment();
    //                    //numberBatches[i];

    //                }
    //                numberBatches[i] = 1;
    //            }
    //        }

    //    }


    //    //public void UnStackEquipment()
    //    //{
    //    //    for (int i = 1; i < 19; i++)
    //    //    {
    //    //        if (itemsInfoLab[i].item == empty)
    //    //        {
    //    //            itemsInfoLab[i].item = CuringForm;
    //    //            labItemsList[i].SetActive(true);//Activate the gameobject
    //    //            numberBatches[i]++;//increase the batch
    //    //            ReadLabTypes();
    //    //            //CalculateQuantity();//Recalculate quantity
    //    //            itemsInfoLab[i].UpdateComponents();//update image, name, quantity                    
    //    //            Save();//Save data to appear in shop
    //    //            break;
    //    //        }
    //    //    }

    //    //}


    //TEST instantiating
    //int[] index= itemsTypeLab.Select((s, k) => s == "Curing Form" ? k : -1).Where(k => k > 0).ToArray();

    //Debug.Log(index.Length);

    //foreach(int k in index)
    //{
    //    InstantiateItems(prefabcuring, labHoldersList[k].transform);
    //    //if (labHoldersList[k].transform.childCount > 0)
    //    //{
    //    //    break;
    //    //}
    //}

    //for (int j=1; j<index.Length; j++)
    //{
    //    //if the holder has already a child
    //    if (labHoldersList[j].transform.childCount > 0)
    //    {
    //        break;
    //    }
    //    //(if(labHoldersList[j].childCount>1))
    //    Debug.Log("this is the itemsTypeLab index" + index[j]);
    //    InstantiateItems(prefabcuring, labHoldersList[j].transform);
    //}



    //}

    //TEST create new equipment when bought in the shop

    //public void UnStackEquipment()
    //{
    //    for (int i = 1; i < 19; i++)
    //    {
    //        if (itemsInfoLab[i].item == empty)
    //        {
    //            itemsInfoLab[i].item = CuringForm;
    //            labItemsList[i].SetActive(true);//Activate the gameobject
    //            numberBatches[i]++;//increase the batch
    //            ReadLabTypes();
    //            //CalculateQuantity();//Recalculate quantity
    //            itemsInfoLab[i].UpdateComponents();//update image, name, quantity                    
    //            Save();//Save data to appear in shop
    //            break;
    //        }
    //    }

    //}


    //*****TEST Dic/inventory


    //labItemsInventory[i] = labItemsList[i].GetComponent<ItemsInventory>();
    //inventoryDictionaries[i] = labItemsList[i].GetComponent<InventoryDictionary>();
    //Debug.Log("Found inventory in" + i);

    //Check if the containers exists
    //if (labItemsInventory[i] != null && labItemsInventory[i].Container.Count > 0)
    //if(inventoryDictionaries[i]!=null)
    //{

    //if (labItemsInventory[i] != null && labItemsInventory[i].Container.Count > 0)
    //{
    //    //Check if the position (key) has a dictionary
    //    if (!ItemsInventoryDictionary.ContainsKey(i))
    //    {
    //        //Check each item of the container in this position
    //        for (int j = 1; j < labItemsInventory[i].Container.Count; j++)
    //        {
    //            //Update the dictionary of the position per item in the container
    //            ContainersInventory[labItemsInventory[i].Container[j].item.name_item] = labItemsInventory[i].Container[j].amount;
    //            //Debug.Log("chuy chuiiiin" + ContainersInventory[labItemsInventory[i].Container[j].item.name_item]);
    //        }
    //        //if the key-dic pair doesn't exist, add new
    //        ItemsInventoryDictionary.Add(i, new ItemsInventoryDic(ContainersInventory));

    //        //Debug.Log("chuy" + ItemsInventoryDictionary[i]);
    //        //Debug.Log("container exists" + ItemsInventoryDictionary[i].Dictionary_["Colorant"]);
    //        //Debug.Log("item chuy chuin");
    //    }

    //    // if the item of the container was already in the dictionary
    //    else
    //    {
    //        for (int j = 1; j < labItemsInventory[i].Container.Count; j++)
    //        {
    //            ContainersInventory[labItemsInventory[i].Container[j].item.name_item] = labItemsInventory[i].Container[j].amount;
    //            //Debug.Log("chuy chuiiiin" + ContainersInventory[labItemsInventory[i].Container[j].item.name_item]);
    //        }
    //        //update the key value
    //        ItemsInventoryDictionary[i] = new ItemsInventoryDic(ContainersInventory);

    //        //Debug.Log("chuy" + ItemsInventoryDictionary[i]);
    //        //Debug.Log("container exists" + ItemsInventoryDictionary[i].Dictionary_["Colorant"]);
    //        //Debug.Log("item chuy chuin");
    //    }





    //InventoryDictionary[i].Add(labItemsInventory[i].Container[0].item.name_item,
    //                        labItemsInventory[i].Container[0].amount);

    //Debug.Log(InventoryDictionary[i]);
    //for (int j = 1; j < labItemsInventory[i].Container.Count; j++)
    //{
    //    for (int k = 1; k < labItemsInventory[i].Container[j].amount; k++)
    //    {
    //        //labItemsInventory[]
    //    }
    //}



    //if (itemsInfoLab[i].item.type == ItemType.Equipment)
    //{
    //    labItemsInventory[i] = labItemsList[i].GetComponent<ItemsInventory>();
    //    Debug.Log("Found inventory in" + i);
    //    if (labItemsInventory[i].Container.Count > 0)
    //    {
    //        for (int j=1; j<labItemsInventory[i].Container.Count; j++)
    //        {
    //            for(int k=1; k<labItemsInventory[i].Container[j].amount; k++)
    //            {
    //                //labItemsInventory[]
    //            }
    //        }
    //    }
    //}


    //Debug.Log("count"+labItemsInventory[i].Container.Count);
    //Debug.Log("amount"+labItemsInventory[i].Container[0].amount);
    //Debug.Log("item"+labItemsInventory[i].Container[0].item);


}
