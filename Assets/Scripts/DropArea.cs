using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler

{
    public void OnPointerEnter(PointerEventData eventData)   // 포인트가 어디에 들어갔다
    {

        if (eventData.pointerDrag == null)
            return;
        
        draggable d = eventData.pointerDrag.GetComponent<draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
            Debug.Log("placeholderParent "+ d.placeholderParent);
        }
    }

    public void OnPointerExit(PointerEventData eventData) // 포인트가 어디에 들어갔던것이 나왔다
    {

        if (eventData.pointerDrag == null)
            return;
        
        draggable d = eventData.pointerDrag.GetComponent<draggable>();
        if (d != null && d.placeholderParent ==this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        
        draggable d = eventData.pointerDrag.GetComponent<draggable>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }

    }


}
