using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class SwitchButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler //Take Mouse Input
{
    [SerializeField] private TabGroup tabGroup;

    public TMP_Text background; // To change the opacity of the text

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);   
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);    
    }
    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<TMP_Text>();
        tabGroup.Subscribe(this);
    }

    public void Select()
    {
        if(onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if(onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }
}
