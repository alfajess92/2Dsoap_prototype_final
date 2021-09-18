using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//source: https://www.youtube.com/watch?v=NFvmfoRnarY&pbjreload=101

public class TimeTickSystem : MonoBehaviour
{
    //The tick will be sent as the event arguments
    public class OnTickEventArgs: EventArgs
    {
        public int tick;

    }

    //To send the correct Tick as argument of the event
    public static event EventHandler<OnTickEventArgs> OnTick;
    private const float TICK_TIMER_MAX = 1f;//1f;//0.2f = 5 ticks per second (200 miliseconds), 1f is scale in normal seconds
    private int tick;
    private float tickTimer;
    public const float tickinseconds = TICK_TIMER_MAX;

    //Source:https://answers.unity.com/questions/1108634/dontdestroyonload-many-instances-of-one-object.html
    //public static TimeTickSystem instance;

    private void Awake()
    {
        tick = 0;

        //Source:https://answers.unity.com/questions/1108634/dontdestroyonload-many-instances-of-one-object.html
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else if (instance != this)
        //{
        //    Destroy(gameObject);
        //}

    }


    private void Update()
    {
        tickTimer += Time.deltaTime;//amount of time passed in the current frame
        if (tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;//reset the timer
            tick++; //increase the tickTimer

            //Debug.Log("This is the current tick" + tick);

            //Fire the event, OnTick event will listen 
            OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });
        }

    }

}
