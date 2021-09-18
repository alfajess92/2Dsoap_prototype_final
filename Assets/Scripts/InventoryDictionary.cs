using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDictionary : MonoBehaviour

{
    public Dictionary<string, int> inventory = new Dictionary<string, int>
    {
         //Substances
            {"Colorant", 0},{"Fragrance", 0},{"Sunflower oil", 0},{"Lye Solution", 0},  {"Trace", 0}, {"Process",0}
            //Equipment
            //{"Curing Form", 0},{"Heating Pot 200W", 0},{"Heating Pot 400W", 0},{"Heating Pot 600W", 0},
            //Products
          
    }; //Dictionary of containers

    //string substance;
    //int amount;


    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, int> inventory = new Dictionary<string, int>();

    }

}
