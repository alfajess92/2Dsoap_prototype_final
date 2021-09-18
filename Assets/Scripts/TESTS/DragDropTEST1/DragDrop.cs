using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Source: https://www.youtube.com/watch?v=BGr-7GZJNXg


public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    //,IDropHandler
{
    //Add the canvas to make sure the movement is according to the scale of the canvas
    [SerializeField] private Canvas canvas;
    //To get the rect transform reference from the beginning
    private RectTransform rectTransform;

    //add canvasgroup to the draggable item in the inspector
    private CanvasGroup canvasGroup;

    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        
        canvasGroup.alpha = 0.6f;//make it a bit transparent while dragging
        //the raycast will go through this object and rends into the itemslot
        canvasGroup.blocksRaycasts = false;
    }

    //Called on everyframe while the mouse is moving
    public void OnDrag(PointerEventData eventData)
    {

        Debug.Log("OnDrag");
        //To modify the rectTransform position of the image the distance it has moved(delta)
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

   
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;//reset transparency when drag is finished
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    ////Called when the droppable object is drag to the wanted position. It is implemented in the item that will receive the dragged item
    //public void OnDrop(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}
}
