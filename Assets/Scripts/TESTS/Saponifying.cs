using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Saponifying 
{
   //Amount of ticks that takes to saponify a sample
    private int saponifyTick;
    private int saponifyTickMax;
    private bool isSaponifying;


    // Luis
    private ProgressBar progressBar;
    
    //private float progress; 


    //Constructor
    public Saponifying(int ticksToSaponify)
    {
        saponifyTick = 0;
        saponifyTickMax = ticksToSaponify;

        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
        isSaponifying = true;

        //progressBar = new ProgressBar();

    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        if (isSaponifying)
        {
            saponifyTick += 1;


            //float progress = saponifyTick / saponifyTickMax;//normalized value
            //if (progress >= 1f) Debug.Log("the progress is" +progress);


            if (saponifyTick >= saponifyTickMax)
            {
                //Mix is fully saponified
                isSaponifying = false;

                Debug.Log("Mix is fully saponified");

               
            }
            
            else
            {
                //Mix is still under saponification
                Debug.Log("mix still under saponificationg");
                //Set the progress bar to current normalized value (saponifyTick / saponifyTickMax)
                progressBar.SetProgress(1);
                //Debug.Log(progressBar.slider.value);

            }
        }
    }
}
