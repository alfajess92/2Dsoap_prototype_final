using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerClickHandler
{
   
   //Modifies the transparency of the image from 50-255
    Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            image = GetComponent<Image>();
            Color c = image.color;
            
            c.a = 50f;
            image.color = c;
            print("transparency reduce");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            image = GetComponent<Image>();
            Color c = image.color;
            c.a = 255f;
            image.color = c;
            print("transparency increase");
        }
    }

    //public void ReduceTransparency()
    //{
    //    image = GetComponent<Image>();
    //    Color c = image.color;
    //    c.a = 50f;
    //    image.color = c;
    //    print("transparency reduce");
    //}

    //public void IncreaseTransparency()
    //{
    //    image = GetComponent<Image>();
    //    Color c = image.color;
    //    c.a = 255f;
    //    image.color = c;
    //    print("transparency increase");
    //}

    ////This code change the color of a button component
    //public Color newColor;
    //public Button button;

    //public void ChangeButtonColor()
    //{
    //    ColorBlock cb = button.colors;
    //    cb.normalColor = newColor;
    //    cb.pressedColor = newColor;

    //    cb.selectedColor = newColor;

    //    button.colors = cb;

    //}


    //// Similar Code to ProgressPanel in TESTING component
    //public void TurnOnEquipment()
    //{
    //    if (onOffButton != null)
    //    {
    //        bool isActive = onOffButton.activeSelf;
    //        onOffButton.SetActive(!isActive);
    //    }
    //}

    //public void TurnOffEquipment()
    //{
    //    if (onOffButton != null)
    //    {
    //        bool isActive = onOffButton.activeSelf;
    //        onOffButton.SetActive(isActive);
    //    }



}
