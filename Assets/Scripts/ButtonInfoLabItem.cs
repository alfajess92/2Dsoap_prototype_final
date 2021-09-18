using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]//helps to follow in editor mode what the script does and maintain the changes

//Derived class as some lab items (substances) also have quantity and units
public class ButtonInfoLabItem : ButtonInfoLab
{
    public Text quantity_lab;
    public Text quantity_unit;

    
    public override void Awake()
    {
        base.Awake();
        
    }

    
    public override void UpdateComponents()//called from labmanager
    {
        if (item != null)
        {

            name_lab_item.text = item.name_item;
            artworkImage.sprite = item.artwork;
            //Debug.Log("the item is" + item.name_item);
            //quantity_lab.text = item.quantity_lab.ToString();
            quantity_lab.text = labManagerScript.labItemsQuantity[ItemID].ToString();
            quantity_unit.text = item.quantity_unit;
        }
    }//assign variables quantity and units





    //The quantity labels are updated per object, in the labmanager it did not work
    //public void Start()
    //{
    //    //UpdateQuantityLabel();
    //}

    //public void UpdateQuantityLabel()//called from labmanager
    //{
    //    if (item != null)
    //    {
    //        //quantity_lab.text = item.quantity_lab.ToString();
    //        quantity_lab.text = labManagerScript.labItemsQuantity[ItemID].ToString();
    //        quantity_unit.text = item.quantity_unit;
    //    }
    //}//assign variables quantity and units




    //remove
    //public void Update()
    //{
    //    string itemLabel = "Item_lab" + ItemID.ToString();
    //    gameObject.name = itemLabel;

    //}


}
