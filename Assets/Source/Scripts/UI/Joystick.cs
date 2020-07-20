using System;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler,IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private GameObserver gameObserver;
    [SerializeField] private Transform cashedCamera;
    [SerializeField] private PlayerUnit playerUnit;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image joystickImage;

    private bool isTouch = false;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector3 inputVector;
    private float rotation;

    private void Awake()
    {
        gameObserver.OnPlayerDied += OnGameEnded;
        gameObserver.OnPlayerWon += OnGameEnded;
        gameObserver.OnReload += Reloading;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = new Vector3(eventData.position.x, eventData.position.y, cashedCamera.position.z);
        bgImage.rectTransform.position = startPoint;
        bgImage.gameObject.SetActive(true);
        //OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isTouch = true;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out endPoint))
        {
            endPoint.x = (endPoint.x / bgImage.rectTransform.sizeDelta.x);
            endPoint.y = (endPoint.y / bgImage.rectTransform.sizeDelta.y);
        }
            inputVector = new Vector3(endPoint.x * 2, 0, endPoint.y * 2);
            inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;
            joystickImage.rectTransform.anchoredPosition = new Vector3(
                inputVector.x * (bgImage.rectTransform.sizeDelta.x / 3),
                inputVector.z * (bgImage.rectTransform.sizeDelta.y / 3));
        rotation = Mathf.Atan2(endPoint.y - 0, endPoint.x - 0) * 180 / Mathf.PI;
        //rotation = Mathf.Acos((startPoint.x * endPoint.normalized.x + startPoint.y * endPoint.normalized.y) / (startPoint.magnitude * endPoint.normalized.magnitude));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        EndDrag();
    }

    private void EndDrag() 
    {
        isTouch = false;
        bgImage.gameObject.SetActive(false);
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
        playerUnit.EndMove();
    }

    private void FixedUpdate()
    {
        if (isTouch)
        {
            playerUnit.Move(inputVector,rotation);
        }
    }

    private void OnGameEnded()
    {
        EndDrag();
        this.gameObject.SetActive(false);
    }
    private void Reloading() 
    {
        this.gameObject.SetActive(true);
    }
}