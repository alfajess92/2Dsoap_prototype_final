using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Animations
{
    none,
    buy,
    sell,
    dropHeater,
    dropCuring,
    onProcess,
    soapAppear,
    traceAppear,

}

public class GlobalFXController : MonoBehaviour
{

    public GameObject FX_buy;
    public GameObject FX_sell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    ////To create an instance that is not destroy on load and play switch sound
    ////Source: https://www.youtube.com/watch?v=82Mn8v55nr0
    
    public static GlobalFXController Instance { get; private set; } = null;


    private void Awake()
    {

        //Create the instance
        //if (Instance != null && Instance != this)
        //{
        //    //Destroy(gameObject);
        //    return;
        //}

        //else
        //{
        //    Instance = this;
        //}

        //DontDestroyOnLoad(gameObject);

    }



    public void ActivateFX(Animations animations)
    {
        switch (animations)
        {
            case Animations.buy:
                FX_buy.SetActive(true);
                FX_buy.GetComponent<ParticleSystem>().Play();
                //ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
                //em.enabled = true;
                Debug.Log("activating the FX buy");
                break;
        }
    }

    public void DeactivateFX(Animations animations)
    {
        switch (animations)
        {
            case Animations.buy:
                FX_buy.SetActive(false);
                Debug.Log("deactivating the FX buy");
                break;
        }
    }


}
