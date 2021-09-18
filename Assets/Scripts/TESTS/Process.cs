using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source: https://www.youtube.com/watch?v=BLfNP4Sc_iA

public class Process : MonoBehaviour


{
    //public int startProgress = 0;
    //public int currentProgress;

    public ProgressBar progressBar;

    //public int progress;


    // Start is called before the first frame update
    private void Start()
    {

        //Subscribe to the event
        //TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        //{
        //    Debug.Log("tick"+e.tick);
        //};

        //currentProgress = startProgress;
      
        //progressBar.SetInitialProgress(startProgress);
        
        }


    // Update is called once per frame
    private void Update()
    {
        //change to button ON interaction
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Starts "saponification process" creates this class
            //new Saponifying(processTime);

            //AdvanceProgress(currentProgress);

            progressBar.isUpdating = true;
        }
    }

    //public void AdvanceProgress(int progress)
    //{
    //    currentProgress += progress;

    //    progressBar.SetProgress(currentProgress);
    //}


    //Update Function
    //Find the on button click of heater
    //Start/Stop the saponification





}
