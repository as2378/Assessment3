//======================================================
//Website link with executable:
//http://www-users.york.ac.uk/~ch1575/documentation
//======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class arrowScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData) //when hovering over arrow button, tint it's colour
    {
        GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
    }
     
    public void OnPointerExit(PointerEventData eventData) //when no longer hovering over arrow button, remove the tint from it's colour
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }
}