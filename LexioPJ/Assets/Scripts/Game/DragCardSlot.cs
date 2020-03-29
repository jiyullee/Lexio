using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragCardSlot : MonoBehaviourPun
{

    public const string DRAGGABLE_TAG = "Slot";
    private bool dragging = false;
    private Vector2 originalPos;
    private Transform objectToDrag;
    private Image objectToDragImage;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !TurnManager.Instance.CanRegister)
        {
            objectToDrag = GetDraggableTransformUnderMouse();

            if (objectToDrag != null)
            {
                dragging = true;

                originalPos = objectToDrag.position;
                objectToDragImage = objectToDrag.GetComponent<Image>();
                objectToDragImage.raycastTarget = false;
            }
        }
        if (dragging)
        {
            objectToDrag.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (objectToDrag != null)
            {                
                Transform objectToReplace = GetDraggableTransformUnderMouse();

                if (objectToReplace != null)
                {
                    int index = objectToDrag.GetSiblingIndex();
                    objectToDrag.SetSiblingIndex(objectToReplace.GetSiblingIndex());
                    objectToReplace.SetSiblingIndex(index);
                    objectToDrag.position = objectToReplace.position;
                    objectToReplace.position = originalPos;

                   
                }
                else
                {
                    objectToDrag.position = originalPos;
                }
                objectToDragImage.raycastTarget = true;
                objectToDrag = null;

            }
            dragging = false;
        }
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        return hitObjects[0].gameObject;
    }

    private Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();
        if (clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        {
            return clickedObject.transform;
        }
        return null;
    }
}
