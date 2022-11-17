using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//source: https://www.youtube.com/watch?v=h-2HwlGHfig

public class DraggableComponent : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Events
    public event Action<PointerEventData> OnBeginDragHandler;
    public event Action<PointerEventData> OnDragHandler;
    public event Action<PointerEventData, bool> OnEndDragHandler;

    //Properties
    public bool FollowCursor { get; set; } = true;
    public Vector3 StartPosition;
    public bool CanDrag { get; set; } = true;


    //add canvasgroup to the draggable item in the inspector
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    ////To get the rect transform reference from the beginning
    private RectTransform rectTransform;
    private ButtonInfoLab buttonInfolab;
    private Image image;

    //TEST
    private Audiocontroller audiocontroller;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        buttonInfolab = GetComponentInParent<ButtonInfoLab>();
        image = GetComponent<Image>();
        audiocontroller = GetComponentInParent<Audiocontroller>();
    }

    //called in the start of a drag event
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            return;
        }
        OnBeginDragHandler?.Invoke(eventData);
        Debug.Log("OnBeginDrag");
        ReduceTransparency();

        
        //audiocontroller.PlayMoveItem();//TODO removed on 17/11/22 bug on sound in Android

    }

    //called every frame in which the object is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            return;
        }

        OnDragHandler?.Invoke(eventData);
        //implement following the cursor
        if (FollowCursor)
        {
            //To modify the rectTransform position of the image the distance it has moved(delta)
            //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;//only works on rendermode "ScreenSpace"
            rectTransform.anchoredPosition += eventData.delta;
            //Debug.Log("ondrag position"+ rectTransform.anchoredPosition);
            Debug.Log("OnDrag");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            return;
        }
        var results = new List<RaycastResult>();

        //to detect if any drop area is under drop position
        EventSystem.current.RaycastAll(eventData, results);
        DropArea dropArea = null;

        //iterate through the hit results to detect if any contains a droparea
        foreach (var result in results)
        {
            dropArea = result.gameObject.GetComponent<DropArea>();

            //check if the area accepts the draggable and if it does drop the draggable in that area, invoke onend drag handler event 
            if (dropArea != null)
            {
                break;
            }
        }

        if (dropArea != null)
        {
            if (dropArea.Accepts(this))
            {
                dropArea.Drop(this);
                
                OnEndDragHandler?.Invoke(eventData, true);//bool

                //Audio TEST, transfer to each equipment
                //audiocontroller.playDrop();
                Debug.Log("the item is dropped");

                //This will return the item in the same position
                //Debug.Log("This is position draggable object" + rectTransform.anchoredPosition);
                rectTransform.anchoredPosition = StartPosition;
                canvasGroup.alpha = 1f;//reset transparency when drag is finished
                canvasGroup.blocksRaycasts = true;
                return;
            }

        }

        //otherwise return the object to start position and invoke the event with the value false
        rectTransform.anchoredPosition = StartPosition;
        OnEndDragHandler?.Invoke(eventData, false);
        Debug.Log("OnEndDrag");
        IncreaseTransparency();

    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        StartPosition = rectTransform.anchoredPosition;
    }

    private void IncreaseTransparency()
    {
        canvasGroup.alpha = 1f;//reset transparency when drag is finished
        canvasGroup.blocksRaycasts = true;
    }

    private void ReduceTransparency()
    {
        canvasGroup.alpha = 0.6f;//make it a bit transparent while dragging
        //the raycast will go through this object and rends into the drop area
        canvasGroup.blocksRaycasts = false;
    }

}