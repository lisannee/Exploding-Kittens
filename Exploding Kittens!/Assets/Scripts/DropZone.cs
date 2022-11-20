using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    private int ChildrenCounter;


    void Start()
    {

        //ChildrenCounter = this.transform.childCount;
        //Debug.Log($"{this.name} has {ChildrenCounter} children");


        //Debug.Log(this.name);

        //foreach (Transform child in this.transform)
        //{

        //    if (child.name == "GeneralCard (4)")
        //    {
        //        Debug.Log(child.name + "got destroyed");
        //        Destroy(child.gameObject);

        //    }
        //    else
        //    {
        //        Debug.Log(child.name);
        //    }


        //}
    }

    private void Update()
    {

    }


    void ExpandDropZone()
    {
        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"{gameObject.name} Entered");

        // prevents null reference exception
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeholderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"{gameObject.name} Exited");
        // prevents null reference exception
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{eventData.pointerDrag.name} was dropped on {gameObject.name}");

        // when dropped in this canvas the item will get assigned a new parent, namely (this dropzone??)
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }

        AdjustHandCompactness();
    }

    private void AdjustHandCompactness()
    {
        // count the children in the dropzone
        ChildrenCounter = this.transform.childCount;

        // get the spacing parametere of the horizontal layout group component of the dropzone
        float horizontalLayoutGroupSpacing = GetComponent<HorizontalLayoutGroup>().spacing;

        //  adjust the hand to make space for the other cards
        if (ChildrenCounter > 5 && horizontalLayoutGroupSpacing > 10)
        {
            // import detail, you cannot use 'horizontalLayoutGroupSpacing' because that would be local
            GetComponent<HorizontalLayoutGroup>().spacing -= 50;
        }
        // reset the state of the hand
        else if (ChildrenCounter <= 5 && horizontalLayoutGroupSpacing < 210)
        {
            GetComponent<HorizontalLayoutGroup>().spacing = 210;
        }
    }
}
