using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{

    //protected
    protected DropArea DropArea;
    protected ItemsInventory itemsInventory;
    protected ProgressBar progressBar;     
    protected OnOffButton onOffButton;
    //protected ParticleSystem particleSystem;//TODO add bubbles 

    protected GameObject item1, item2, item3, item4, item5, item6;
    //protected GameObject buttonCondition;

    //add canvasgroup to the draggable item in the inspector
    protected CanvasGroup item_image1, item_image2, item_image3, item_image4, item_image5, item_image6;
    //Added to change the transparency of the objects added
    //protected Color color1, color2, color3;
    protected float initial_alpha_color = 0.4f;
    protected float final_alpha_color = 1f;

    //Calling the Labmanager to transfer the amount used
    protected GameObject labManager;
    protected LabManagerScript labManagerScript;
    protected ButtonInfoLab buttonInfoLabScript;

    //power
    protected int power;

    //To Play sounds
    public Audiocontroller audiocontroller;

    //TEST TODO animation
    public FXController_Lab fXController;

    //TEST to track what is dropped in the equipment and save the data
    public InventoryDictionary inventoryDictionary;


    //virtual means, use the extended class's version if it has one otherwise use this base class's version
    protected virtual void Awake()
    {
        //power
        buttonInfoLabScript = GetComponent<ButtonInfoLab>();

        //Inventory
        inventoryDictionary = GetComponent<InventoryDictionary>();

        //Audio
        audiocontroller = GetComponent<Audiocontroller>();
        //audiocontroller = GameObject.Find("audio").GetComponent<Audiocontroller>();

        //FX
        fXController = GetComponent<FXController_Lab>();

        DropArea = GetComponent<DropArea>() ?? gameObject.AddComponent<DropArea>();//get the component or add one if the component is not attached
        DropArea.OnDropHandler += OnItemDropped; //add a listener to the ondrophandler event from the drop area

        //Define the gameobjects and call the components attached to the prefab
        itemsInventory = GetComponent<ItemsInventory>();
        progressBar = GetComponentInChildren<ProgressBar>();
        onOffButton = GetComponentInChildren<OnOffButton>();
        onOffButton.GetComponent<Button>().interactable = false;

        
        //particleSystem = GetComponentInChildren<ParticleSystem>();
        //particleSystem.enableEmission = false;

        //Find Labmanager in the scene
        labManager = GameObject.Find("LabManager");
        labManagerScript = labManager.GetComponent<LabManagerScript>();

        //To call the items to be dropped 
        item1 = transform.Find("item1").gameObject;
        if (item1 != null)
        {
            //item_image1 = item1.GetComponent<Image>();
            //color1 = item_image1.color;
            //color1.a = initial_alpha_color;
            item_image1 = item1.GetComponent<CanvasGroup>();
            item_image1.alpha = initial_alpha_color;
            item_image1.blocksRaycasts = false;

        }

        item2 = transform.Find("item2").gameObject;
        if (item2 != null)
        {
            //item_image2 = item2.GetComponent<Image>();
            //color2 = item_image2.color;
            //color2.a = initial_alpha_color;
            item_image2 = item2.GetComponent<CanvasGroup>();
            item_image2.alpha = initial_alpha_color;
            item_image2.blocksRaycasts = false;
        }

        item3 = transform.Find("item3").gameObject;
        if (item3 != null)
        {
            //item_image3 = item3.GetComponent<Image>();
            //color3 = item_image3.color;
            //color3.a = initial_alpha_color;
            item_image3 = item3.GetComponent<CanvasGroup>();
            item_image3.alpha = initial_alpha_color;
            item_image3.blocksRaycasts = false;
        }

        item4 = transform.Find("item4").gameObject;
        if (item4 != null)
        {
            //item_image3 = item3.GetComponent<Image>();
            //color3 = item_image3.color;
            //color3.a = initial_alpha_color;
            item_image4 = item4.GetComponent<CanvasGroup>();
            item_image4.alpha = initial_alpha_color;
            item_image4.blocksRaycasts = false;
        }

        item5 = transform.Find("item5").gameObject;
        if (item5 != null)
        {
            //item_image3 = item3.GetComponent<Image>();
            //color3 = item_image3.color;
            //color3.a = initial_alpha_color;
            item_image5 = item5.GetComponent<CanvasGroup>();
            item_image5.alpha = initial_alpha_color;
            item_image5.blocksRaycasts = false;
        }

        item6 = transform.Find("item6").gameObject;
        if (item6 != null)
        {
            //item_image3 = item3.GetComponent<Image>();
            //color3 = item_image3.color;
            //color3.a = initial_alpha_color;
            item_image6 = item6.GetComponent<CanvasGroup>();
            item_image6.alpha = initial_alpha_color;
            item_image6.blocksRaycasts = false;
        }


    }

    private void OnItemDropped(DraggableComponent draggable)
    {
        //set draggable position same to equipment slot position to send the draggable in the drop area
        draggable.transform.position = transform.position;
        
    }

   //Button On
    public void ActivateOnButton()

    {
        onOffButton.GetComponent<Button>().interactable = true;
        //onOffButton.SetTextOFF();
        //onOffButton.isOnPressed = true;
    }
    public void DeActivateOnButton()
    {
        onOffButton.GetComponent<Button>().interactable = false;
        //onOffButton.SetTextON();
    }
    public void TurnOn()
    {
        onOffButton.isOnPressed = true;
    }
    public void TurnOff()
    {
        onOffButton.isOnPressed = false;

    }

    //Progress bar
    public void ActivateProgressBar()
    {
        progressBar.CallClock();
        progressBar.isUpdating = true;
        progressBar.isOff = true;
        //Debug.Log("the bar is working");
        
    }
    public void StopProgressBar()
    {
        progressBar.isUpdating = false;
        //Debug.Log("the bar is stopped");
    }

    //To activate items dropped all at once
    public  void ActivateItems()
    {
        item1.SetActive(true);
        item2.SetActive(true);
        item3.SetActive(true);
        item4.SetActive(true);
        item5.SetActive(true);
        item6.SetActive(true);
    }
    //To deactive items dropped all at once
    public void DeactivateItems()
    {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);
        item4.SetActive(false);
        item5.SetActive(false);
        item6.SetActive(false);
    }
    //Reset the transparency of the items inside equipment using canvasgroup
    public void ChangeTransparencyItems()
    {
        item_image1.alpha = initial_alpha_color;
        //item_image1.blocksRaycasts = true;

        item_image2.alpha = initial_alpha_color;
        //item_image2.blocksRaycasts = true;

        item_image3.alpha = initial_alpha_color;
        //item_image3.blocksRaycasts = true;

        item_image4.alpha = initial_alpha_color;
        item_image5.alpha = initial_alpha_color;
        item_image6.alpha = initial_alpha_color;

    }
    //Update transparency of images as been dropped
    public void UpdateCuringFormImages()
    {
        if (itemsInventory.Container.Count == 1)
        {
            switch (itemsInventory.Container[0].item.name_item)
            {
                case "Trace" when itemsInventory.Container[0].amount == 1:
                    item_image1.alpha = final_alpha_color;
                    break;
                case "Fragrance" when itemsInventory.Container[0].amount == 1:
                     item_image2.alpha = final_alpha_color;
                    break;

                case "Colorant" when itemsInventory.Container[0].amount == 1:
                    item_image3.alpha = final_alpha_color;
                    break;  
            }
        }
        else
        {
            if (itemsInventory.Container.Count <= 1)
            {
                return;
            }
            if ((itemsInventory.Container[0].item.name_item == "Trace" && itemsInventory.Container[0].amount == 1) || (itemsInventory.Container[1].item.name_item == "Trace" && itemsInventory.Container[1].amount == 1) || (itemsInventory.Container[2].item.name_item == "Trace" && itemsInventory.Container[2].amount == 1))
            {
                item_image1.alpha = final_alpha_color;
            }

            else if ((itemsInventory.Container[0].item.name_item == "Fragrance" && itemsInventory.Container[0].amount == 1) || (itemsInventory.Container[1].item.name_item == "Fragrance" && itemsInventory.Container[1].amount == 1) || (itemsInventory.Container[2].item.name_item == "Fragrance" && itemsInventory.Container[2].amount == 1))
            {
                item_image2.alpha = final_alpha_color;
            }

            else if ((itemsInventory.Container[0].item.name_item == "Colorant" && itemsInventory.Container[0].amount == 1) || (itemsInventory.Container[1].item.name_item == "Colorant" && itemsInventory.Container[1].amount == 1) || itemsInventory.Container[2].item.name_item == "Colorant" && itemsInventory.Container[2].amount == 1)
            {
                item_image3.alpha = final_alpha_color;
            }
        }

    }

    //Update transparency of images as been dropped
    public void UpdateOilimages()
    {
        //Checks if the oil was dropped first or second and check the amount to highlight the second or third bottle

        //if user only drops oil first, the list with have a size of 1
        if (itemsInventory.Container.Count == 1)
        {

            switch (itemsInventory.Container[0].item.name_item)
            {
                case "Sunflower oil" when itemsInventory.Container[0].amount == 1:
                    item_image1.alpha = final_alpha_color;
                    break;
                case "Sunflower oil" when itemsInventory.Container[0].amount == 2:
                    item_image3.alpha = final_alpha_color;
                    break;
                case "Sunflower oil" when itemsInventory.Container[0].amount == 3:
                    item_image5.alpha = final_alpha_color;
                    break;
            }
        }
        //if user only drops oil and lye indistinctively, the list will grow 
        else
        {
            if (itemsInventory.Container.Count > 1)
            {
                if ((itemsInventory.Container[0].item.name_item == "Sunflower oil" && itemsInventory.Container[0].amount == 1) || (itemsInventory.Container[1].item.name_item == "Sunflower oil" && itemsInventory.Container[1].amount == 1))
                {
                    item_image1.alpha = final_alpha_color;
                }

                //Highlight Second oil
                else if ((itemsInventory.Container[0].item.name_item == "Sunflower oil" && itemsInventory.Container[0].amount == 2) || (itemsInventory.Container[1].item.name_item == "Sunflower oil" && itemsInventory.Container[1].amount == 2))
                {
                    item_image1.alpha = final_alpha_color;
                    item_image3.alpha = final_alpha_color;
                }
                //Highlight Third oil
                else if (itemsInventory.Container[0].item.name_item == "Sunflower oil" && itemsInventory.Container[0].amount == 3 || itemsInventory.Container[1].item.name_item == "Sunflower oil" && itemsInventory.Container[1].amount == 3)
                {
                    item_image1.alpha = final_alpha_color;
                    item_image5.alpha = final_alpha_color;
                    item_image3.alpha = final_alpha_color;
                }
            }
        }
    }


    //Update transparency of images as been dropped
    public void UpdateLyeImages()
    {
        //Checks if the lye was dropped first or second and check the amount to highlight the second or third beaker

        //if user only drops oil first, the list with have a size of 1
        if (itemsInventory.Container.Count == 1)
        {
            switch (itemsInventory.Container[0].item.name_item)
            {
                case "Lye Solution" when itemsInventory.Container[0].amount == 1:
                    item_image2.alpha = final_alpha_color;
                    break;
                case "Lye Solution" when itemsInventory.Container[0].amount == 2:
                    item_image4.alpha = final_alpha_color;
                    break;
                case "Lye Solution" when itemsInventory.Container[0].amount == 3:
                    item_image6.alpha = final_alpha_color;
                    break;
            }
        }
        //if user only drops oil and lye indistinctively, the list will grow 
        else
        {
            if (itemsInventory.Container.Count > 1)
            {
                // Highlight first lye
                if ((itemsInventory.Container[0].item.name_item == "Lye Solution" && itemsInventory.Container[0].amount == 1) || itemsInventory.Container[1].item.name_item == "Lye Solution" && itemsInventory.Container[1].amount == 1)
                {
                    item_image2.alpha = final_alpha_color;
                }

                //Highlight Second lye
                else if (itemsInventory.Container[0].item.name_item == "Lye Solution" && itemsInventory.Container[0].amount == 2 || itemsInventory.Container[1].item.name_item == "Lye Solution" && itemsInventory.Container[1].amount == 2)
                {
                    item_image2.alpha = final_alpha_color;
                    item_image4.alpha = final_alpha_color;
                }

                //Highlight third lye
                else if (itemsInventory.Container[0].item.name_item == "Lye Solution" && itemsInventory.Container[0].amount == 3 || itemsInventory.Container[1].item.name_item == "Lye Solution" && itemsInventory.Container[1].amount == 3)
                {
                    item_image2.alpha = final_alpha_color;
                    item_image6.alpha = final_alpha_color;
                    item_image4.alpha = final_alpha_color;
                }
            }
        }
    }


    //To create instance inside Update
    public IEnumerator CreateSoapAfterTime(int value)
    {
        yield return WaitForDone(120f);// wait for done or 46/91 seconds, whichever comes first. 

        if (!progressBar.isUpdating&&!progressBar.isOff)
        {
            Debug.Log("start creating soap");
            //labManagerScript.CreateSoap(value);
            StartCoroutine(labManagerScript.CreateSoap(value));/* done code here */
            Debug.Log("soap ready");
        }
        else
        {

            labManagerScript.Save();
            //Debug.Log("not ready to create soap");
        }
        //yield return new WaitForSeconds(delay);
    }

    //To create instance inside Update
   public IEnumerator CreateTraceAfterTime()
    {

        yield return WaitForDone(70f);// wait for done or 31/61 seconds, whichever comes first. //TODO take the time from inventory if switch scenes

        if (!progressBar.isUpdating && !progressBar.isOff)
        {
            StartCoroutine(labManagerScript.CreateTrace());/* done code here */
            Debug.Log("start creating trace");
        }
        else
        {
            Debug.Log("not ready to create trace");
        }

    }


    //Source: https://forum.unity.com/threads/coroutine-question-can-you-combine-waitforseconds-with-a-condition.107041/
    //Check if the progress is running
    IEnumerator WaitForDoneProcess(float timeout)
    {
        //print("entring wait for done process");
        while (progressBar.isUpdating)
        {
            yield return null;
            timeout -= Time.deltaTime;
            //print("this is timeout" + timeout);
            if (timeout <= 0f) break;
        }
    }

    //Call the coroutine after the time is completed
    YieldInstruction WaitForDone(float timeout)
    {

        return StartCoroutine(WaitForDoneProcess(timeout));
    }

   
    //Particle System from progressBar, TODO add bubbles 
    //source: https://docs.unity3d.com/ScriptReference/ParticleSystem.html
    //public void ActivateParticleSystem()
    //{
    //    particleSystem.enableEmission = true;
    //    particleSystem.Play();
    //    Debug.Log("particles are on");
    //    //particleSystem.Play();
    //}




    //TRY to change color by alpha not working therefore change to canvas group
    ////Debug.Log(item_image1.color);
    //color1.a = initial_alpha_color;
    //item_image1.color = color1;
    //Debug.Log("The color is " + item_image1.color);

}
