using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//source: https://www.youtube.com/watch?v=BLfNP4Sc_iA


public class ProgressBar : MonoBehaviour
{

    //This script is attached to the prefab "progressbar"
    public Slider slider;

    //For the gradient function
    public Gradient gradient;
    public Image fill;

    //Amount of ticks that takes to process a sample
    public bool isUpdating;
    public bool isOff;
    public int progressTick;
    public int progressTickMax;

    //TEST
    public int heatertime, curingtime;

    public float processTime;

    public ButtonInfoLab buttonInfoLab;
    public ButtonInfoLabItem buttonInfoLabItem;
    public ProcessType process;
    public int power;

    //inventory of each item in the game
    private InventoryDictionary inventoryDictionary;

    //audio
    public  Audiocontroller audiocontroller;
    public  GlobalAudioController globalAudioController;
    //public AudioSource heat;

    //FX
    private FXController_Lab fXController_Lab;


    //TODO-random items
    public GameObject labManager;
    public LabManagerScript labManagerScript;

    //Function to set progressbar to initial value
    public void SetInitialProgress(int progress)
    {
        //slider start at min of progress
        slider.minValue = progress;
        slider.value = progress;

        //To use a gradient color instead of one color
        fill.color = gradient.Evaluate(1f);
    }

    //Function to set the value to the current progress
    public void SetProgress(int progress)
    {
        slider.value = progress;
        fill.color = gradient.Evaluate(slider.normalizedValue);  //Added for the gradient

    }

    public void Awake()
    {
        //Find LabManager in the scene and define it once
        labManager = GameObject.Find("LabManager");
        labManagerScript = labManager.GetComponent<LabManagerScript>();
        //audiocontroller = GameObject.Find("audio").GetComponent<Audiocontroller>();
        //globalAudioController = GameObject.Find("Sounds").GetComponent<GlobalAudioController>();
        //Debug.Log("awake audio");

    }

    //change to Start 
    //public void Awake()
    //{
    //    isUpdating = false;
    //    //progressTick = 0;
    //    //progressTickMax = 100;//change depending on the process (100=10s)

    //    //call the event systems
    //    TimeTickSystem.OnTick += TimeTickSystem_OnTick;
    //}

    public void Start()
    {
        //audio
        audiocontroller = GetComponentInParent<Audiocontroller>();
        audiocontroller = GameObject.Find("audio").GetComponent<Audiocontroller>();
        globalAudioController = GameObject.Find("Sounds").GetComponent<GlobalAudioController>();
        //globalAudioController.Init


        fXController_Lab = GetComponentInParent<FXController_Lab>();

        inventoryDictionary = GetComponentInParent<InventoryDictionary>();

        processTime = TimeTickSystem.tickinseconds;//set the process to the TICK MAX 
        buttonInfoLab = GetComponentInParent<ButtonInfoLab>();

        //buttonInfoLabItem = GetComponentInParent<ButtonInfoLabItem>();

        heatertime = 60;//30
        curingtime = 90;//45

       

        //if (buttonInfoLab.item.name_item == "Empty")
        //if (buttonInfoLab.item.type == ItemType.Default)
        //{
        //    //if (audiocontroller.cure)
        //    //{
        //    //audiocontroller.StopCureSound();
        //        Debug.Log("this is an empty equipment so no sound");
        //    //globalAudioController.StopSound(PlayableSounds.cure);
        //    //}

        //    //if (audiocontroller.heat)
        //    //{
        //    //audiocontroller.StopHeat();
        //        Debug.Log("this is an empty equipment so no sound");
        //        //globalAudioController.StopSound(PlayableSounds.heat);
        //    //}

        //    //Debug.Log("this is an empty equipment so no sound");

        //}


        if (buttonInfoLab.item.type == ItemType.Equipment)
        {
            //TODO modify with realistic numbers
            switch (buttonInfoLab.item.name_item)
            {
                //Check if a process was running previously
                case "Curing Form":
                    if (inventoryDictionary.inventory["Process"] > 0)
                    {
                        progressTick = inventoryDictionary.inventory["Process"];
                        //progressTickMax = 30;//30 seconds TODO change to more time
                        progressTickMax = curingtime;
                    }
                    else
                    {
                        progressTick = 0;
                        //progressTickMax = 30;//30 seconds TODO change to more time
                        progressTickMax = curingtime;
                    }

                    break;
                case "Heating Pot 200W":
                    if (inventoryDictionary.inventory["Process"] > 0)
                    {
                        progressTick = inventoryDictionary.inventory["Process"];
                        //progressTickMax = 10;
                        progressTickMax = heatertime;
                        //CheckMatch();//Check if a match is bought

                    }
                    else
                    {
                        progressTick = 0;
                        //progressTickMax = 10;
                        progressTickMax = heatertime;
                        //CheckMatch();//Check if a match is bought
                    }

                    break;
                case "Heating Pot 400W":
                    if (inventoryDictionary.inventory["Process"] > 0)
                    {
                        progressTick = inventoryDictionary.inventory["Process"];
                        //progressTickMax = 5;
                        progressTickMax = heatertime;
                        //CheckMatch();//Check if a match is bought

                    }
                    else
                    {
                        progressTick = 0;
                        //progressTickMax = 5;
                        progressTickMax = heatertime;
                        //CheckMatch();//Check if a match is bought

                    }
                    //progressTick = 0;
                    //progressTickMax = 5;
                    break;
                case "Heating Pot 600W":
                    if (inventoryDictionary.inventory["Process"] > 0)
                    {
                        progressTick = inventoryDictionary.inventory["Process"];
                        //progressTickMax = 3;
                        progressTickMax = heatertime;
                        //CheckMatch();//Check if a match is bought

                    }
                    else
                    {
                        progressTick = 0;
                        //progressTickMax = 3;
                        progressTickMax = heatertime;
                        //CheckMatch();//Check if a match is bought

                    }
                    //progressTick = 0;
                    //progressTickMax = 3;
                    break;
                //case "Empty":
                //    //if (buttonInfoLab.item.name_item == "Empty")
                //    //{
                //        audiocontroller.StopCureSound();
                //        audiocontroller.StopHeat();
                //        Debug.Log("this is an empty equipment so no sound");
                //    //}
                //    break;
            }
        }

    }

    public void CallClock()
    {
        isOff = false;
        isUpdating = false;
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
    }

    //Called from LabManager Script in update
    public void StopClock()
    {
        isUpdating = false;
        isOff = true;

    }

    //Start TickSystem and progressbar
    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        

        if (isUpdating&&isOff)
        {
            //Check if random items that modify processtimes are bought
            CheckClock();
            CheckMatch();
            CheckSStar();

            progressTick += 1;


            if (progressTick > progressTickMax)
            {
                Debug.Log("enter final progress step");
                //Process is completed
                isUpdating = false;
                isOff = false;


                audiocontroller.PlayProduce();

                //fXController_Lab.PlayOn();
                fXController_Lab.PlayAppear();

                if (buttonInfoLab.item.name_item== "Curing Form")
                {

                    //**
                    audiocontroller.StopCureSound();
                    Debug.Log("the sound of curing form is off");

                }

                if (buttonInfoLab.item.name_item == "Heating Pot 200W" || buttonInfoLab.item.name_item == "Heating Pot 400W" || buttonInfoLab.item.name_item == "Heating Pot 600W")
                {

                    //**
                    audiocontroller.StopHeat();
                    Debug.Log("the sound of heat is off");
                }

                Debug.Log("Process is completed");
                //this.Hide();//check
                this.SetProgress(0);
                progressTick = 0;

                //Reset the value when is done 
                inventoryDictionary.inventory["Process"] = progressTick;

                //Reset the progressTickMax-TEST
                ResetProgressTickMax();//reset if there was a clock

            }

            else
            {
                //Process is still running
                //Debug.Log("Process is still running");
                this.SetProgress(progressTick * (100 / progressTickMax));



                //Set the progress bar to current normalized value 
                //this.SetProgress(progressTick);


                //*****
                if (buttonInfoLab.item.name_item == "Curing Form")
                {

                    //audiocontroller.cure = true;
                    if (!globalAudioController.cure.isPlaying)
                    {
                        globalAudioController.PlaySound(PlayableSounds.cure);
                    }

                }

                if (buttonInfoLab.item.name_item == "Heating Pot 200W" || buttonInfoLab.item.name_item == "Heating Pot 400W" || buttonInfoLab.item.name_item == "Heating Pot 600W")
                {
                    //audiocontroller.heat = true;
                    if (!globalAudioController.heat.isPlaying)
                    {
                        globalAudioController.PlaySound(PlayableSounds.heat);
                    }

                }
               // *****

            }
        }

    }

    //Functions to show/hide the progressbar
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }


    public void CheckMatch()
    {
        if (labManagerScript.LabDictionary["Match"] >= 1)
        {
            if (buttonInfoLab.item.name_item != "Curing Form")
            {
                progressTickMax = 10;
                Debug.Log("Find a match" + progressTickMax);
            }
        }
            
    }//Check if a match bought and change the processtime of all heaters to 10s

    public void CheckClock()
    {
        if (labManagerScript.LabDictionary["Clock"]>= 1)
        {
            progressTickMax -= 5;
            Debug.Log("Find clock" + progressTickMax);
        }

        else
        {
            //Keep same value of progressTickMax
        }
    }//Check if the clock is bought and reduce the processtime of all equipments by 5s


    public void CheckSStar()
    {
        if (labManagerScript.LabDictionary["Diadem"] >= 1)
        {
            if (buttonInfoLab.item.name_item == "Curing Form")
            {
                progressTickMax = 5;
                Debug.Log("Find a sstar" + progressTickMax);
            }
        }

    }//Check if a match bought and change the processtime of all curing forms to 10s

    //Resets the value in case the clock is present
    public void ResetProgressTickMax()
    {

        if (buttonInfoLab.item.name_item == "Curing Form")
        {
            progressTickMax = curingtime;
            print("back to curingtime");
        }

        if (buttonInfoLab.item.name_item == "Heating Pot 200W" || buttonInfoLab.item.name_item == "Heating Pot 400W" || buttonInfoLab.item.name_item == "Heating Pot 600W")
        {
           progressTickMax=heatertime;
           print("back to heatertime");
        }



    }


    public void ChangeProgressTickMax(ProgressBar progressBar)
    {
        if (progressBar is null)
        {
            throw new ArgumentNullException(nameof(progressBar));
        }
        progressTickMax -= 5;
        print("this is new max" + progressTickMax);
    }


}