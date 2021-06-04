using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int menuItemIndex = 0;

    TitleScreen titleScreen;

    private void Start()
    {
        titleScreen = GetComponentInParent<TitleScreen>();
    }

    public void MouseHover()
    {
        //Change to selected colour, and update the menu selection
        titleScreen.UpdateMenuSelection(menuItemIndex);
    }

    public void MouseClickDown()
    {
        //Change to a click down colour
    }

    public void MouseClickUp()
    {
        //select the menu item
        titleScreen.SelectItem(menuItemIndex);

        Debug.Log("Menu Item: " + this + " Clicked");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {

    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //select the menu item
        titleScreen.SelectItem(menuItemIndex);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Change to selected colour, and update the menu selection
        titleScreen.UpdateMenuSelection(menuItemIndex);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

    }
}
