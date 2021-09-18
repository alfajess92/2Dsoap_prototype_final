using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]//helps to follow in editor mode what the script does and maintain the changes

public class ButtonInfoLab : MonoBehaviour
{
    public Item item;//From Scriptable Objects
    public int ItemID;
    public Text name_lab_item;
    public Image artworkImage;
    public GameObject labManager;
    public LabManagerScript labManagerScript;

    public int power;


    public virtual void Awake()
    {
        //Find LabManager in the scene and define it once
        labManager = GameObject.Find("LabManager");
        labManagerScript = labManager.GetComponent<LabManagerScript>();

    }

    //virtual help to update use the function in the inherited classes
    public virtual void UpdateComponents()//called from labmanager
    {
        if (item != null)
        {
            //prefab = item.prefab;

            name_lab_item.text = item.name_item;
            artworkImage.sprite = item.artwork;
            //Debug.Log("the item is"+ item.name_item);

            if (item.type == ItemType.Equipment)
            {
                power = item.power;
                //Debug.Log("This is the power of the item" + power);

            }

        }
        
    }//public modifier, called from LabManagerScript

    //remove
    public void Update()
    {
        ////label in the inspector with the item ID
        //string itemLabel = "Item_lab" + ItemID.ToString();
        //gameObject.name = itemLabel;

        //NOTE the components are updated after actions handle by heater and curing forms not in each frame

        //quantity_lab.text = LabManager.GetComponent<LabManagerScript>().labItems[3, ItemID].ToString();
        //quantity_lab is equal to quantity_lab from scriptable object times the quantity purchased
        //name_lab_item.text = "N: " + LabManager.GetComponent<ShopManagerScript>().shopItems[4, ItemID].ToString();

    }



    //TEST to read the Item ID
    //source: https://www.youtube.com/watch?v=isAmoM3RPEI
    //function to read the item being drop , it is called in the DropArea Script
    public Item GetItem()//called from DropArea
    {
        return item;
    }
    public int GetID()
    {
        return ItemID;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(Item item)
    {
        gameObject.SetActive(true);
        SetItem(item);
    }
    public void SetItem(Item item)
    {
        this.item = item;
    }


}
