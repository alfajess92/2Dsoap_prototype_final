using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Audiocontroller : MonoBehaviour
{
    //TODO check if switchlab and shop will be different
    public PlayableSounds SwitchLab;
    public PlayableSounds SwitchShop;
    public PlayableSounds MoveItem;
    public PlayableSounds Buy;
    public PlayableSounds Sell;
    public PlayableSounds Heat;
    public PlayableSounds Pour;
    public PlayableSounds Drop;
    public PlayableSounds Produce;
    public PlayableSounds ProduceTrace;
    public PlayableSounds Cure;
    public PlayableSounds Click;

    private GlobalAudioController globalAudioController;


    //private void Awake()
    //{
    //    DontDestroyOnLoad(transform.gameObject);

    //}

    // Start is called before the first frame update
    void Start()
    {
        globalAudioController = GameObject.Find("Sounds").GetComponent<GlobalAudioController>();
        //Debug.Log("found button in every button");
        //GetComponentInChildren<Button>().onClick.AddListener(playHeat);

    }

    public void PlaySwitchLab()
    {
        
        globalAudioController.PlaySound(SwitchLab);
    }

    public void StopSwitchLab()
    {

        globalAudioController.StopSound(SwitchLab);
    }


    public void PlaySwitchShop()
    {
        globalAudioController.PlaySound(SwitchShop);
    }

    public void StopSwitchShop()
    {
        globalAudioController.StopSound(SwitchShop);
    }


    public void PlayMoveItem()
    {
        globalAudioController.PlaySound(MoveItem);
    }

    public void StopMoveItem()
    {
        globalAudioController.StopSound(MoveItem);
    }

    public void PlayBuy()
    {
        globalAudioController.PlaySound(Buy);
    }

    public void StopBuy()
    {
        globalAudioController.StopSound(Buy);
    }

    public void PlaySell()
    {
        globalAudioController.PlaySound(Sell);
    }

    public void StopSell()
    {
        globalAudioController.StopSound(Sell);
    }

    public void PlayHeat()
    {
        globalAudioController.PlaySound(Heat);
        print("heating sound running");


    }

    public void StopHeat()
    {
        globalAudioController.StopSound(Heat);

    }

    public void PlayPour()
    {
        globalAudioController.PlaySound(Pour);
    }

    public void StopPour()
    {
        globalAudioController.StopSound(Pour);
    }

    public void PlayDrop()
    {
        globalAudioController.PlaySound(Drop);
    }

    public void StopDrop()
    {
        globalAudioController.StopSound(Drop);
    }

    public void PlayProduce()
    {
        globalAudioController.PlaySound(Produce);
    }

    public void StopProduceSoap()
    {
        globalAudioController.StopSound(Produce);
    }


    public void PlayProduceTrace()
    {
        globalAudioController.PlaySound(ProduceTrace);
    }

    public void StopProduceTrace()
    {
        globalAudioController.StopSound(ProduceTrace);
    }


    public void PlayOtherSound(PlayableSounds sound)
    {
        globalAudioController.PlaySound(sound);
    }

    public void StopOtherSound(PlayableSounds sound)
    {
        globalAudioController.StopSound(sound);
    }


    public void PlayCureSound()
    {
        globalAudioController.PlaySound(Cure);
        print("curing sound running");

    }

    public void StopCureSound()
    {
        globalAudioController.StopSound(Cure);
    }


    public void PlayClickSound()
    {
        globalAudioController.PlaySound(Click);
    }

    public void StopClickSound()
    {
        globalAudioController.StopSound(Click);
    }
}
