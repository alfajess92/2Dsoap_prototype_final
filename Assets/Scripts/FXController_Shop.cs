using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FXController_Shop : MonoBehaviour
{

    //public Animations Buy;
    //public Animations Sell;

    //private GlobalFXController globalFXController;

    public GameObject FX_buy, FX2_sell;

    //public ButtonInfo buttonInfo;

    // Start is called before the first frame update
    void Start()
    {
        //globalFXController = GameObject.Find("FX").GetComponent<GlobalFXController>();
        //FX = buttonInfo.FX_place;
        //FX = GameObject.Find("FX_place");
        //Debug.Log("THis is FX transform" + FX_buy.transform.localPosition.x+ FX_buy.transform.localPosition.y+ FX_buy.transform.localPosition.z);


    }

    public void PlayBuy()
    {
        //InstantiateItems(globalFXController.FX_buy, FX.transform);
        //InstantiateItems(globalFXController.FX_buy, this.transform);
        //InstantiateItems(globalFXController.FX_buy);

        //globalFXController.ActivateFX(Buy);
        FX_buy.SetActive(true);
        FX_buy.GetComponent<ParticleSystem>().Play();

    }

    public void PlaySell()
    {
        FX2_sell.SetActive(true);
        FX2_sell.GetComponent<ParticleSystem>().Play();
    }


    //public void InstantiateItems(GameObject prefab, Transform parentTransform)
    //{
    //    var item = Instantiate(prefab, parentTransform);
    //    item.transform.SetParent(parentTransform);
    //    item.transform.localPosition = new Vector3(1, 1, 1);

    //    Debug.Log("Creating an item in"+ parentTransform.localPosition.x);

    //    //Name the item using the place
    //    //string itemLabel = "Item_lab" + index;
    //    //item.name = itemLabel;
    //} //Method to made instances

    //public void InstantiateItems(GameObject prefab)
    //{
    //    //var item = Instantiate(prefab);
    //    //prefab.transform.SetParent(transform);
    //    //item.transform.SetParent(transform);
      

    //    //Debug.Log("Creating an item in" + transform.localPosition.x);

    //    //Name the item using the place
    //    //string itemLabel = "Item_lab" + index;
    //    //item.name = itemLabel;
    //} //Method to made instances

   
    //public void StopBuy()
    //{
    //    globalFXController.DeactivateFX(Buy);
    //}

}
