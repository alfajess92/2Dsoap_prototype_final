using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;

public class ManagerDemo : MonoBehaviour
{
    EasyFileSave myFile;

    string userName;
    int userAge;
    int coins;
    public Text CoinsTXT;
    List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        myFile = new EasyFileSave();
        myFile.suppressWarning = false;

        // If this file already exists for some reason, I delete it.

        myFile.Delete();

        Debug.Log(">> HELLO! I'M READY!" + "\n");

    }

    // Update is called once per frame
    void Update()
    {
        CoinsTXT.text = "Money is " + coins;

        if (Input.GetKeyUp(KeyCode.S))
        {

            Debug.Log("Money start: " + coins);
            coins = 5700;
            userAge = 10;
            myFile.AddSerialized("coins", coins);
            myFile.AddSerialized("age", userAge);
            myFile.Save();
            Debug.Log("Money end: " + coins);
            CoinsTXT.text = "Money is " + coins;
  
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            // Load data from file.
            if (myFile.Load())
            {

                Debug.Log(">> I'M GOING TO USE LOADED DATA!" + "\n");

                // Simple data.

                coins = myFile.GetInt("coins");
                CoinsTXT.text = "Money is " + coins;

                // Class data (serialization).

                // myFile.Dispose();
                myFile.Dispose();

                Debug.Log(">> Data loaded from: " + myFile.GetFileName() + "\n");

            }
        }

    }

}

