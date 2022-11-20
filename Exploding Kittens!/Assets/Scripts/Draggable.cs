using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;

    GameObject CardAsPlaceholder = null;



    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        //Configure the invisible CardAsPlaceHolder
        SetUpCardAsPlaceholder();

        // remember your initial parent (the hand)
        parentToReturnTo = this.transform.parent;

        //
        placeholderParent = parentToReturnTo;

        // parent is set to the canvas
        this.transform.SetParent(this.transform.parent.parent);

        /* we have to temporarily disable raycasting for the cards
            because otherwhise the drophandler can not see the mouse
            as it is covererd by the card underneath it */
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");

        // changes the position of the object to the position of the mouse
        this.transform.position = eventData.position;

        // performs the logic of comparing the card location between cards in each space
        CardLocationComparer();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);

        // sets the index card to the new position of the CardAsPlaceHolder
        this.transform.SetSiblingIndex(CardAsPlaceholder.transform.GetSiblingIndex());

        // after we stop dragging the raycasting has to be turned on
        // again otherwhise we can not start dragging the cards in the first place
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // alternatively
        // looks at where are standing on and asks, "what is underneath me?"
        // gets list of what is underneath it (could be useful later)
        // EventSystem.current.RaycastAll(eventData);

        // The CardAsPlaceholder has to be removed after someone is done dragging a card.
        Destroy(CardAsPlaceholder);
    }

    void SetUpCardAsPlaceholder()
    {
        // create invisible card as a place holder
        CardAsPlaceholder = new GameObject();
        CardAsPlaceholder.transform.SetParent(this.transform.parent);

        // we have added the component 'LayoutElement'to the invisible card and
        // we have set these settings to match the settings of this card
        LayoutElement layoutElementCard = CardAsPlaceholder.AddComponent<LayoutElement>();
        layoutElementCard.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        layoutElementCard.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        layoutElementCard.flexibleWidth = 0;
        layoutElementCard.flexibleHeight = 0;

        // sets the index of the placeholder to the index of the card which is currently being dragged
        CardAsPlaceholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
    }

    void CardLocationComparer()
    {

        if (CardAsPlaceholder.transform.parent != placeholderParent)
            CardAsPlaceholder.transform.SetParent(placeholderParent);

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {

                newSiblingIndex = i;

                if (CardAsPlaceholder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }

        CardAsPlaceholder.transform.SetSiblingIndex(newSiblingIndex);
    }

}
