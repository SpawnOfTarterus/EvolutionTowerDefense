using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliderControl : MonoBehaviour
{
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    [SerializeField] float moveSpeed;

    [SerializeField] RectTransform[] menuesToForceClose;

    #region MenuControl
    public void ToggleMenu(RectTransform menuToControl)
    {
        if (menuToControl.anchoredPosition.y == minHeight) { StartCoroutine(OpenMenu(menuToControl)); }
        if (menuToControl.anchoredPosition.y == maxHeight) { StartCoroutine(CloseMenu(menuToControl)); }
    }

    public void ForceCloseMenus()
    {
        foreach(RectTransform menu in menuesToForceClose)
        {
            menu.anchoredPosition = new Vector2(menu.anchoredPosition.x, minHeight);
            menu.gameObject.SetActive(false);
        }
    }

    private IEnumerator OpenMenu(RectTransform menuToControl)
    {
        menuToControl.gameObject.SetActive(true);
        while (menuToControl.anchoredPosition.y < maxHeight)
        {
            menuToControl.anchoredPosition = new Vector2(
                menuToControl.anchoredPosition.x,
                Mathf.Min(menuToControl.anchoredPosition.y + (1 * moveSpeed), maxHeight));
            yield return null;
        }
        yield return null;
    }

    private IEnumerator CloseMenu(RectTransform menuToControl)
    {
        while (menuToControl.anchoredPosition.y > minHeight)
        {
            menuToControl.anchoredPosition = new Vector2(
                menuToControl.anchoredPosition.x,
                Mathf.Max(menuToControl.anchoredPosition.y - (1 * moveSpeed), minHeight));
            yield return null;
        }
        menuToControl.gameObject.SetActive(true);
        yield return null;
    }
    #endregion

}
