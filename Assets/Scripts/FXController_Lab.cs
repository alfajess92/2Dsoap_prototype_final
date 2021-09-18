using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController_Lab : MonoBehaviour
{

    public GameObject FX_drop, FX_ON, FX_appear;

    public void PlayDrop()
    {
        FX_drop.SetActive(true);
        FX_drop.GetComponent<ParticleSystem>().Play();
    }

    public void PlayOn()
    {
     FX_ON.SetActive(true);
     FX_ON.GetComponent<ParticleSystem>().Play();
    }

    public void PlayAppear()
    {
        FX_appear.SetActive(true);
        FX_appear.GetComponent<ParticleSystem>().Play();
    }
}
