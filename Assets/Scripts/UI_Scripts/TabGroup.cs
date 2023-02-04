using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<SwitchButton> switchButtons;
    public SwitchButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(SwitchButton button)
    {
        if(switchButtons == null)
        {
            switchButtons = new List<SwitchButton>();
        }

        switchButtons.Add(button);
    }

    public void OnTabEnter(SwitchButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.background.faceColor = new Color32(255,255,255,255);
        }
    }

    public void OnTabExit(SwitchButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(SwitchButton button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        button.background.faceColor = new Color32(255,255,255,255);
        int index = button.transform.GetSiblingIndex();

        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (SwitchButton button in switchButtons)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.background.faceColor = new Color32(255,255,255,100);
        }
    }
}
