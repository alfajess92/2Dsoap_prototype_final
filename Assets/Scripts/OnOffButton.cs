using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//source change text: https://www.youtube.com/watch?v=kdkrjCF0KCo
//other: https://www.youtube.com/watch?v=B0ZDMMpx8qA

public class OnOffButton : MonoBehaviour
{

    public bool isOnPressed;
    private Button button;
    private Text txt;
    //TEST
    //public Audiocontroller audiocontroller;

    public void Awake()
    {
        isOnPressed = false;
        button = transform.GetComponent<Button>();
        txt = transform.Find("Text").GetComponent<Text>();
        //TEST
        //audiocontroller = GetComponentInParent<Audiocontroller>();
    }

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TurnOnandOff);

    }

    //OnClick event
    private void TurnOnandOff()
    {
        //isOnPressed ^= true;//true or false toggle

        var answer = isOnPressed ^= true;//true or false toggle

        //TEST audio
        //audiocontroller.PlayClickSound();

        //if (answer == true)
        //{
        //    SetTextOFF();

        //}
        //else if (answer == false)
        //{
        //    SetTextON();

        //}
    }

    //change text of the button
    public void SetTextOFF()
    {
        txt.text = "OFF";
        //ChangeButtonColorOFF();
    }

    public void SetTextON()
    {
        txt.text = "ON";
        //ChangeButtonColorON();
    }

    //To change the color of the button
    public void ChangeButtonColorOFF()
    {
        ColorBlock cb = button.colors;
        //change color to red
        Color  newColor = Color.red;
        cb.normalColor = newColor;
        cb.pressedColor = newColor;
        cb.selectedColor = newColor;
        button.colors = cb;
    }

    public void ChangeButtonColorON()
    {
        ColorBlock cb = button.colors;
        //change color to green
        Color newColor = Color.green;
        cb.normalColor = newColor;
        cb.pressedColor = newColor;
        cb.selectedColor = newColor;
        button.colors = cb;
    }

}


