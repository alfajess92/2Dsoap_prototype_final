using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]//helps to follow in editor mode what the script does and maintain the changes

public class ButtonInfo : MonoBehaviour
{
    public Item item;//From scriptable objects
    public int ItemID;
    public Text PriceTxt;
    public Text SellTxt;
    public Text QuantityTxt;
    public Text name_item;
    public Image artworkImageSell;
    public Image artworkImageBuy;
    public GameObject ShopManager;
    public ShopManagerScript shopManagerScript;

    //TEST
    public Button buttonsell, buttonbuy;

    public int initialsellingprice;
    public int initialbuyingprice;

    //public static bool isNewPrice=false;

    public static bool updateSubstance = false;
    public static bool updateEquipment = false;
    public static bool updateSoap = false;


    public void Awake()
    {
        //Find ShopManager in scene
        ShopManager = GameObject.Find("ShopManager");
        shopManagerScript = ShopManager.GetComponent<ShopManagerScript>();

        initialsellingprice = item.sellprice;
        //if (isNewPrice == false)
        //{
        //    initialsellingprice = item.sellprice;
        //    print("This is the initialselling" + initialsellingprice);
        //}

        //else
        //{
        //    initialsellingprice = initialsellingprice;
        //}

        //initialbuyingprice = item.buyprice;

    }

    public void UpdateComponents()//assign variables name, price etc. Called from Shopmanager
    {
        //Debug.Log("updating items");
        if (item != null)
        {
            name_item.text = item.name_item;
            PriceTxt.text = "BUY: " + item.buyprice.ToString();
            SellTxt.text = "SELL: " + item.sellprice.ToString();
            artworkImageSell.sprite = item.artwork;
            artworkImageBuy.sprite = item.artwork;
            //QuantityTxt.text = shopManagerScript.shopItemsQuantity[ItemID].ToString();
            //TEST Quantity should be according to the dictionary
            if (item.name_item == "Empty")
            {
                QuantityTxt.text = "";
            }
         
            else {
                QuantityTxt.text = shopManagerScript.ShopDictionary[item.name_item].ToString();}
            //Debug.Log("this is what was updated" + shopManagerScript.ShopDictionary[item.name_item].ToString());
        }

    }

    public void UpdateComponentsBuy()
    {
        if (item != null)
        {
            name_item.text = item.name_item;
            PriceTxt.text = "BUY: " + item.buyprice.ToString();
            SellTxt.text = "";
            artworkImageSell.sprite = item.artwork;
            artworkImageBuy.sprite = item.artwork;
            //QuantityTxt.text = shopManagerScript.shopItemsQuantity[ItemID].ToString();
            //TEST Quantity should be according to the dictionary
            if (item.name_item == "Empty")
            {
                QuantityTxt.text = "";
            }

            else
            {
                //QuantityTxt.text = shopManagerScript.ShopDictionary[item.name_item].ToString();
                QuantityTxt.text = shopManagerScript.shopItemsQuantity[ItemID].ToString();
            }
            //Debug.Log("this is what was updated" + shopManagerScript.ShopDictionary[item.name_item].ToString());
        }

        CheckBuy();

    }

    //Can Buy if there is Money
    public void CheckBuy()
    {
        if (item.buyprice >= shopManagerScript.coins)
        {
            buttonbuy.interactable = false;//cant buy
        }
        else if (item.buyprice <= shopManagerScript.coins)
        {
            buttonbuy.interactable = true;//buy
        }
    }

    public void UpdateComponentsSell()
    {
        if (item != null)
        {
            name_item.text = item.name_item;
            PriceTxt.text = "";
            SellTxt.text = "SELL: " + item.sellprice.ToString();
            artworkImageSell.sprite = item.artwork;
            artworkImageBuy.sprite = item.artwork;
            //QuantityTxt.text = shopManagerScript.shopItemsQuantity[ItemID].ToString();
            //TEST Quantity should be according to the dictionary
            if (item.name_item == "Empty")
            {
                QuantityTxt.text = "";

            }

            else
            {
                //QuantityTxt.text = shopManagerScript.ShopDictionary[item.name_item].ToString();
                QuantityTxt.text = shopManagerScript.shopItemsQuantity[ItemID].ToString();
            }

            //Debug.Log("this is what was updated" + shopManagerScript.ShopDictionary[item.name_item].ToString());
            CheckSell();

        }

    }

    //Can sell if there is item to sell
    public void CheckSell()
    {
        if (shopManagerScript.shopItemsQuantity[ItemID] == 0)

            //if (shopManagerScript.ShopDictionary[item.name_item]==0)
        {
            buttonsell.interactable = false;
            //Debug.Log("There is no" + item.name_item + "item to sell");
        }
        //else if (shopManagerScript.ShopDictionary[item.name_item]>0)
            else if(shopManagerScript.shopItemsQuantity[ItemID]>0)
        {
            buttonsell.interactable = true;
            //Debug.Log("There is" + item.name_item + "item to sell");
        }
            
        
    }



    //updates prices of equipments-COIN
    public void UpdateEquipmentSellingPrices()
    {
        if (item.type == ItemType.Equipment)
        {
            int sellvalue = item.sellprice += 10;


            if (item != null)
            {
                SellTxt.text = "SELL: " + sellvalue.ToString();
                print("this is the new sell price" + sellvalue);

            }
            updateEquipment = true;
            //isNewPrice = true;
        }
    }

    //resets prices of equipments-COIN
    public void ResetEquipmentSellingPrices()
    {
        if (item.type == ItemType.Equipment)
        {
            int sellvalue = item.sellprice -= 10;

            //item.sellprice = initialsellingprice;
            print("reseting the price to" + initialsellingprice);
            if (item != null)
            {
                SellTxt.text = "Sell: " + sellvalue.ToString();
                //item.sellprice = initialsellingprice;

            }
            updateEquipment = false;
            //isNewPrice = true;
        }         
    }


    //updates prices of substances-POND
    public void UpdateSubstancesSellingPrice()
    {
        //update = false;
        if (item.type == ItemType.Substance)
        {
            int sellvalue = item.sellprice += 5;

            if (item != null)
            {
                SellTxt.text = "SELL: " + sellvalue.ToString();
                print("this is the new sell price" + sellvalue);

            }
            updateSubstance = true;
        }

    }

    //resets prices of substances-POND
    public void ResetSubstancesSellingPrice()
    {
        if (item.type == ItemType.Substance)
        {
            int sellvalue = item.sellprice -= 5;
            if (item != null)
            {
                SellTxt.text = "SELL: " + sellvalue.ToString();
            }
        }
        updateSubstance = false;
    }


    //Star doubles the price of SOAP
    public void UpdateSoapSellingPrice()
    {

        if (item.type==ItemType.Product)
        {
            int sellvalue = item.sellprice * 2;

            if (item != null)
            {
                SellTxt.text = "SELL: " + sellvalue.ToString();
                print("this is the new sell price" + sellvalue);

            }
            updateSoap = true;
        }

    }

    //Reset soap price
    public void ResetSoapPrice()
    {
        if (item.type == ItemType.Product)

        {
            int sellvalue = item.sellprice;
            if (item != null)
            {
                SellTxt.text = "SELL: " + sellvalue.ToString();
            }
        }
        updateSoap = false;
    }

    //TODO remove
    public void Update()
    {
        string itemLabel = "Item_shop" + ItemID.ToString();
        gameObject.name = itemLabel;

       // //shorter
       // PriceTxt.text = "Buy: " + shopManagerScript.shopItems[2, ItemID].ToString();
       // QuantityTxt.text = shopManagerScript.shopItems[3, ItemID].ToString();
       // SellTxt.text = "Sell: " + shopManagerScript.shopItems[4, ItemID].ToString();

       ////first approach, check but I believe this is more "expensive"
       // PriceTxt.text = "Buy: " + ShopManager.GetComponent<ShopManagerScript>().shopItems[2, ItemID].ToString();
       // QuantityTxt.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[3, ItemID].ToString();
       // SellTxt.text = "Sell: " + ShopManager.GetComponent<ShopManagerScript>().shopItems[4, ItemID].ToString();

    }
}
