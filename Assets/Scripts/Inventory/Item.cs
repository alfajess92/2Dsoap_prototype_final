using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To create item from the template
//source:  https://www.youtube.com/watch?v=aPXvoWVabPY

public enum ItemType
{
    Equipment,//heater or curing form
    Substance, //oil, fragrance, 
    Intermediate, //trace with/without fragrance
    Product, //Soap
    Default,//Empty
    Random,//Random items
}

//TODO not used now, check if useful for progress bar
public enum ProcessType
{
    saponification,//transform oil and lye to trace (~ depending heater power 200-30min, 400-15 min, 600-9min)
    cooling,//TODO allow trace to cool down (~1 hours in real life depending in the amount)
    curing,//transform trace into soap after adding colorant and fragrance (~12 hours in real life)
    none,
}


//[CreateAssetMenu(fileName = "New item", menuName = "item")]
//    public class Item : ScriptableObject


//By making it an abstract class is now the only a base for creating items but it can not instantiated its own

public abstract class Item : ScriptableObject

{
    //public int itemID;
    public string name_item;

    public ItemType type;

    public ProcessType process;

    public GameObject prefab;

    public int power;

    //For the shop
    public int sellprice;
    public int buyprice;

    //Added 
    //public int referencesellprice;
    //public int referencebuyprice;

    //For the lab
    public int quantity_lab; //quantity per batch
    public string quantity_unit;
    public Sprite artwork;

  
    public void Print()
    {
        Debug.Log(name_item + ":" + type);
    }


}
