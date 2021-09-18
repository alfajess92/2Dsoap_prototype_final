using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]//helps to follow in editor mode in console
[SelectionBase] //To make sure only the image is selected

//This script helps to snap tiles in certain ranges to avoid moving tiles in strange plces

public class EditorSnap : MonoBehaviour
{

    // Update is called once per frame
    //To constraint the position of the block

    [SerializeField] [Range (1f, 100f)] float gridSize = 10f;

    //TextMesh textMesh;

    void Update()
    {
        Vector2 snapPos;
        //RectTransform rectTransform;

        snapPos.x = Mathf.RoundToInt(transform.position.x / gridSize) * gridSize;
        //snapPos.x = Mathf.RoundToInt(transform.position.x / 10f) * 10f;
        snapPos.y = Mathf.RoundToInt(transform.position.y / gridSize) * gridSize;

        transform.position = new Vector2(snapPos.x, snapPos.y);

        //textMesh = GetComponentInChildren<TextMesh>();
        //string itemLabel = snapPos.x/gridSize + " , " + snapPos.y / gridSize;
        //textMesh.text = itemLabel;

        ////label in the inspector with the item
        //gameObject.name = itemLabel;

    }
}
