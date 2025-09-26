using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject _navigationBar;
    
    private Stack<GameObject> _screensStack = new();

    public void OpenScreen(GameObject screen, bool disableActiveScreen = true, bool navBarActiveState = true)
    {
        if (disableActiveScreen && _screensStack.Count > 0)
        {
            _screensStack.Pop().SetActive(false);
        }
        screen.SetActive(true);
        _screensStack.Push(screen);
        _navigationBar.SetActive(navBarActiveState);
    }

    public void CloseScreen()
    {
        if (_screensStack.Count > 2)
        {
            _screensStack.Pop().SetActive(false);
        }
    }
}
