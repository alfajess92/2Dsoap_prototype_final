using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{

    public ShopManagerScript shop;

    //Added sprites
    public Image yellow, cyan, green;
    public GameObject buybutton, sellbutton, menubutton, trigger, chatbox, chatbutton, switchscenebutton, title;

    private Color color1, color2, color3, color4, color5, color6, color7;
    // Start is called before the first frame update
    void Start()
    {
        //Colors from the buttons
        color1 = buybutton.GetComponent<Image>().color;
        //Debug.Log("this is the color 5" + color5);
        color2 = sellbutton.GetComponent<Image>().color;
        color3 = menubutton.GetComponent<Image>().color; 
        color4=trigger.GetComponent<Image>().color;
        color5=chatbox.GetComponent<Text>().color;
        color6=chatbutton.GetComponent<Image>().color;
        color7=switchscenebutton.GetComponent<Image>().color;
    }

 

    public void ChangeColor()
    {
        //Colors from each object
        for (int i = 1; i < 19; i++)
        {
            shop.itemsInfoShop[i].SellTxt.color = Color.cyan;
            shop.itemsInfoShop[i].PriceTxt.color = Color.cyan;//yellow
            shop.itemsInfoShop[i].QuantityTxt.color = Color.white;
            shop.itemsInfoShop[i].name_item.color = Color.cyan;

        }

        //Money Text
        shop.CoinsTXT.color = Color.yellow;

        //Change the sprite to cyan-frame
        for (int i=1; i<19; i++)
        {
            var frame =shop.shopItemsList[i].GetComponent<Image>();
            frame.sprite = cyan.sprite;

        }

        //Color in buttons
        buybutton.GetComponent<Image>().color = Color.cyan;
        sellbutton.GetComponent<Image>().color = Color.cyan;
        menubutton.GetComponent<Image>().color = Color.cyan;
        trigger.GetComponent<Image>().color = Color.cyan;
        chatbox.GetComponent<Text>().color = Color.cyan;
        chatbutton.GetComponent<Image>().color = Color.cyan;
        switchscenebutton.GetComponent<Image>().color = Color.cyan;

        title.GetComponent<TextMeshProUGUI>().color = Color.cyan;

        //var colors = GetComponent<Button>().colors;
        //colors.normalColor = Color.black;
        //GetComponent<Button>().colors = colors;
        //Debug.Log("Color is changed");
        //var image = shop.shopItemsList[1].GetComponent<Image>();
        //image.sprite = yellow.sprite;

        //var image2 = shop.shopItemsList[2].GetComponent<Image>();
        //image2.sprite = cyan.sprite;


    }

    //Reset to original green
    public void ResetColor()
    {
        buybutton.GetComponent<Image>().color = color1;
        sellbutton.GetComponent<Image>().color = color2;
        menubutton.GetComponent<Image>().color = color3;
        trigger.GetComponent<Image>().color = color4;
        chatbox.GetComponent<Text>().color = color5;
        chatbutton.GetComponent<Image>().color = color6;
        switchscenebutton.GetComponent<Image>().color = color7;

        for (int i = 1; i < 19; i++)
        {
            var frame = shop.shopItemsList[i].GetComponent<Image>();
            frame.sprite = green.sprite;
        }

        for (int i = 1; i < 19; i++)
        {
            shop.itemsInfoShop[i].SellTxt.color = color1;
            shop.itemsInfoShop[i].PriceTxt.color = color5;
            shop.itemsInfoShop[i].QuantityTxt.color = color5;
            shop.itemsInfoShop[i].name_item.color = color5;

        }

        shop.CoinsTXT.color = color5;
        title.GetComponent<TextMeshProUGUI>().color = color5;
    }
}