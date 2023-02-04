using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static bool IsIntialised { get; private set; }
    public static GameObject mainMenu, settingsMenu;

   public static void Init()
   {
        GameObject canvas = GameObject.Find("Canvas");
        mainMenu = canvas.transform.Find("MenuContainer").gameObject;
        settingsMenu = canvas.transform.Find("SettingsContainer").gameObject;

        IsIntialised = true;
   }

   public static void OpenMenu(Menu menu, GameObject callingMenu)
   {
        if(!IsIntialised)
            Init();

        switch(menu)
        {
            case Menu.MAIN_MENU:
                mainMenu.SetActive(true);
                break;
            case Menu.SETTINGS:
                settingsMenu.SetActive(true);
                break;
            
        }

        callingMenu.SetActive(false);    
   }
}
