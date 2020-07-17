using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] private GameObserver observer;
    public void ReplayClicked() 
    {
        observer.OnReload?.Invoke();
    }
    public void QuitClicked()
    {
        Application.Quit();
    }
}