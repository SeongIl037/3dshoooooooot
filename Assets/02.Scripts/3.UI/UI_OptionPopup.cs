using UnityEngine;

public class UI_OptionPopup : UI_Popup
{
    public UI_CreditPopup CreditPopup;

    public void OnClickContinueButton()
    {
        gameObject.SetActive(false);
        GameManager.instance.Continue();
    }

    public void OnClickRetryButton()
    {
        GameManager.instance.Restart();
    }

    public void OnClickButtonQuitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnClickCreditsButton()
    {
        PopupManager.instance.Open(EPopupType.UI_CreditPopup);
    }
}
