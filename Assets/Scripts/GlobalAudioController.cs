using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayableSounds
{
    none,
    switchlab,
    switchshop,
    moveItem,  //rearranging item
    buy,
    sell,
    heat,
    pour,
    drop,
    cure,
    produce,
    produceTrace,
    click
}


public class GlobalAudioController : MonoBehaviour
{
    //public AudioSource failure;
    public AudioSource switchLab;
    public AudioSource switchShop;
    public AudioSource moveItem;
    public AudioSource buy;
    public AudioSource sell;
    public AudioSource heat;
    public AudioSource pour;
    public AudioSource drop;
    public AudioSource produce;
    public AudioSource produceTrace;
    public AudioSource cure;
    public AudioSource click;

    //To create an instance that is not destroy on load and play switch sound
    //Source: https://www.youtube.com/watch?v=82Mn8v55nr0
    public static GlobalAudioController Instance { get; private set; } = null;


    private void Awake()
    {

        //Create the instance
        if (Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        
    }

   

    public void PlaySound(PlayableSounds sound)
    {
        switch (sound)
        {
            case PlayableSounds.switchlab:
                //TODO check if switch lab and shop sound are the same to remove one of them
                switchLab.Play();
                
                //if (!switchLab.isPlaying)
                //{
                //    switchLab.Play();
                //    print("This is the time of switchlab" + switchLab.time);
                //}
                //else
                //{
                //    switchLab.Stop();
                //}

                break;
            case PlayableSounds.switchshop:
                switchShop.Play();
                break;
            case PlayableSounds.moveItem:
                moveItem.Play();
                break;
            case PlayableSounds.buy:
                buy.Play();
                break;
            case PlayableSounds.sell:
                sell.Play();
                break;
            case PlayableSounds.heat:
                heat.Play();
                break;
            case PlayableSounds.pour:
                pour.Play();
                break;
            case PlayableSounds.drop:
                drop.Play();
                break;
            case PlayableSounds.produce:
                produce.Play();
                break;

            case PlayableSounds.produceTrace:
                produceTrace.Play();
                break;
            case PlayableSounds.cure:
                cure.Play();
                break;

            case PlayableSounds.click:
                click.Play();
                break;
            case PlayableSounds.none:
                break;
        }
    }


    public void StopSound(PlayableSounds sound)
    {
        switch (sound)
        {
            case PlayableSounds.switchlab:
                //TODO check if switch lab and shop sound are the same to remove one of them
                switchLab.Stop();
                break;
            case PlayableSounds.switchshop:
                switchShop.Stop();
                break;
            case PlayableSounds.moveItem:
                moveItem.Stop();
                break;
            case PlayableSounds.buy:
                buy.Stop();
                break;
            case PlayableSounds.sell:
                sell.Stop();
                break;
            case PlayableSounds.heat:
                heat.Stop();
                break;
            case PlayableSounds.pour:
                pour.Stop();
                break;
            case PlayableSounds.drop:
                drop.Stop();
                break;
            case PlayableSounds.produce:
                produce.Stop();
                break;
            case PlayableSounds.produceTrace:
                produceTrace.Stop();
                break;

            case PlayableSounds.cure:
                cure.Stop();
                break;
            case PlayableSounds.click:
                click.Stop();
                break;
            case PlayableSounds.none:
                break;
        }
    }

}
