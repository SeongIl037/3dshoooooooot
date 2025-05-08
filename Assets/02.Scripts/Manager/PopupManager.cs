using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum EPopupType
{
    UI_OptionPopup,
    UI_CreditPopup
}
public class PopupManager : Singletone<PopupManager>
{
    [Header("팝업 UI 참조")] 
    public List<UI_Popup> Popups;
    private Stack<UI_Popup> _openedPopups = new Stack<UI_Popup>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                while (true)
                {
                    UI_Popup popup = _openedPopups.Pop();
                    bool opened = popup.isActiveAndEnabled;
                    popup.Close();
                    
                    if (opened || _openedPopups.Peek() == null)
                    {
                        break;
                    }

                }
            }
            else
            {
                GameManager.instance.Pause();
            }
        }
    }
    public void Open(EPopupType type , Action closeCallback = null)
    {
        PopOpen(type.ToString(), closeCallback);
    }
    public void PopOpen(string popupName, Action closeCallback)
    {
        foreach (UI_Popup popup in Popups)
        {
            if (popup.name == popupName)
            {
                popup.Open(closeCallback);
                _openedPopups.Push(popup);
                break;
            }
        }
    }
}
