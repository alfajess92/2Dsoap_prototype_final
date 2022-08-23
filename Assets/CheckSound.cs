using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSound : MonoBehaviour
{

    GameObject globalAudioController;
    // Start is called before the first frame update
    void Start()
    {
        globalAudioController = GameObject.Find("Sounds");

    }


    public void ChangeMute()
    {
        globalAudioController.GetComponent<GlobalAudioController>().ChangeValueMute();
    }

    public GameObject sound, nosound;

    // Update is called once per frame
    void Update()
    {
        if (GlobalAudioController.mute)
        {
            nosound.SetActive(true);
            sound.SetActive(false);
        }

        else
        {
            nosound.SetActive(false);
            sound.SetActive(true);
        }
    }

    }

