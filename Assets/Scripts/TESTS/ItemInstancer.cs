using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//source: https://stackoverflow.com/questions/58207967/attaching-unity-scriptable-object-to-gameobject
public class ItemInstancer : MonoBehaviour
{
    public ItemUser itemPrefab;
    public Item item;

    public void Instantiate()
    {
        ItemUser thisItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        thisItem.ItemScriptableObject= item;
    }

}
