using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{


    //ParticleSystem particleSystem;
    //Renderer renderer;


    //void Start()
    //{
    //    particleSystem.render
    //    // Set the sorting layer of the particle system.
    //    particleSystem.renderer.sortingLayerName = "Default";
    //    particleSystem.renderer.sortingOrder = 2;
    //}

    public int sortingOrder=2;
    public string sortingLayerName="TEST";

    // Use this for initialization
    void Start()
    {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder;

    }


}