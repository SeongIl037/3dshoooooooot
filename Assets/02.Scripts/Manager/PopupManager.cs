using System;
using System.Collections.Generic;
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
    private List<UI_Popup> _openedPopups = new List<UI_Popup>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                while (true)
                {
                    bool opened = _openedPopups[_openedPopups.Count - 1].isActiveAndEnabled;
                    _openedPopups[_openedPopups.Count - 1].Close();
                    _openedPopups.RemoveAt(_openedPopups.Count - 1);

                    if (opened || _openedPopups.Count == 0)
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
                _openedPopups.Add(popup);
                break;
            }
        }
    }
}
