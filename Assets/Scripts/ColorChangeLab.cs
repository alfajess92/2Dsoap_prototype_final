using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeLab : MonoBehaviour
{

    //public ShopManagerScript shop;
    public LabManagerScript lab;

    //Added sprites
    public Image yellow, cyan, green;

    //public GameObject buybutton, sellbutton, menubutton, trigger, chatbox, chatbutton, switchscenebutton, title;
    public GameObject trigger, chatbox, chatbutton, switchscenebutton, title;

 
    //TODO maybe do not need to save all colors
    private Color color1, color2, color3, color4, color5, color6, color7;

    void Start()
    {
        //Colors from the buttons
        //color1 = buybutton.GetComponent<Image>().color;
        //Debug.Log("this is the color 5" + color5);
        //color2 = sellbutton.GetComponent<Image>().color;
        //color3 = menubutton.GetComponent<Image>().color;
        color4 = trigger.GetComponent<Image>().color;
        color5 = chatbox.GetComponent<Text>().color;
        color6 = chatbutton.GetComponent<Image>().color;
        color7 = switchscenebutton.GetComponent<Image>().color;
    }



    public void ChangeColor()
    {
        //Money Text
        lab.CoinTxtlab.color = Color.yellow;
        //Change title
        title.GetComponent<TextMeshProUGUI>().color = Color.cyan;
        //Color in buttons
        trigger.GetComponent<Image>().color = Color.cyan;
        chatbox.GetComponent<Text>().color = Color.cyan;
        chatbutton.GetComponent<Image>().color = Color.cyan;
        switchscenebutton.GetComponent<Image>().color = Color.cyan;

        //Colors from each object
        for (int i = 1; i < 19; i++)
        {
            lab.itemsInfoLab[i].name_lab_item.color = Color.cyan;
            if (lab.itemsInfoLab[i].item.type != ItemType.Equipment)
            {
                lab.itemsInfoLab[i].GetComponent<ButtonInfoLabItem>().quantity_lab.color = Color.white;
                lab.itemsInfoLab[i].GetComponent<ButtonInfoLabItem>().quantity_unit.color = Color.white;
            }

        }

        //Change the sprite to cyan-frame
        for (int i = 1; i < 19; i++)
        {
            var frame = lab.labItemsList[i].GetComponent<Image>();
            frame.sprite = cyan.sprite;
        }

    }

    //Reset to original green
    public void ResetColor()
    {
        trigger.GetComponent<Image>().color = color4;
        chatbox.GetComponent<Text>().color = color5;
        chatbutton.GetComponent<Image>().color = color6;
        switchscenebutton.GetComponent<Image>().color = color7;

        title.GetComponent<TextMeshProUGUI>().color = color5;
        lab.CoinTxtlab.color = color5;

        for (int i = 1; i < 19; i++)
        {
            var frame = lab.labItemsList[i].GetComponent<Image>();
            frame.sprite = green.sprite;
        }

        for (int i = 1; i < 19; i++)
        {

            lab.itemsInfoLab[i].name_lab_item.color = color5;
            if (lab.itemsInfoLab[i].item.type !=ItemType.Equipment )
            {
                lab.itemsInfoLab[i].GetComponent<ButtonInfoLabItem>().quantity_lab.color = color5;
                lab.itemsInfoLab[i].GetComponent<ButtonInfoLabItem>().quantity_unit.color = color5;
            }
        }

    }
}
