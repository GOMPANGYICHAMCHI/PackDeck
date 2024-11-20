using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTransform : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private UI_StageCardPannel stageCardPannel;

    public Card cardData;
    private Canvas canvas;
    private Image imageComponent;
    [SerializeField] private bool instantiateVisual = true;
    private Vector3 offset;

    // 선택 및 비선택 함수
    public Action SelectFunc;
    public Action UnSelectFunc;

    [Header("이동")]
    [SerializeField] private float moveSpeedLimit = 50;

    [Header("선택")]
    public bool selected;
    public float selectionOffset = 50;
    private float pointerDownTime;
    private float pointerUpTime;

    [Header("비주얼")]
    [SerializeField] private GameObject cardVisualPrefab;
    [HideInInspector] public CardVisual cardVisual;

    [Header("가능여부")]
    public bool isDraggable;
    public bool isSelectable;

    [Header("상태")]
    public bool isHovering;
    public bool isDragging;
    [HideInInspector] public bool wasDragged;

    [Header("이벤트")]
    [HideInInspector] public UnityEvent<CardTransform> PointerEnterEvent;
    [HideInInspector] public UnityEvent<CardTransform> PointerExitEvent;
    [HideInInspector] public UnityEvent<CardTransform, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<CardTransform> PointerDownEvent;
    [HideInInspector] public UnityEvent<CardTransform> BeginDragEvent;
    [HideInInspector] public UnityEvent<CardTransform> EndDragEvent;
    [HideInInspector] public UnityEvent<CardTransform, bool> SelectEvent;

    public void ShakeAnimation()
    {
        cardVisual.ShakeAnimation();
    }

    public void Initialize(Transform visualHolder, Action selectFunc, Action unSelectFunc, Card inputcard, Sprite pattern, Color32 color)
    {
        // 선택 및 비선택 이벤트 할당
        SelectFunc = selectFunc;
        UnSelectFunc = unSelectFunc;
        cardData = inputcard;

        canvas = GetComponentInParent<Canvas>();
        imageComponent = GetComponent<Image>();

        if (!instantiateVisual)
            return;

        // 카드 비주얼 생성
        cardVisual = Instantiate(cardVisualPrefab, visualHolder).GetComponent<CardVisual>();
        cardVisual.Initialize(this);
        // 카드 외형 설정
        cardVisual.SetCardAppear(pattern, color, inputcard.number, inputcard.Index);
    }

    public void SetStageCardPannel(UI_StageCardPannel input)
    {
        stageCardPannel = input;    
    }

    void Update()
    {
        //ClampPosition();

        // 드래그 중이라면
        if (isDragging)
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    void ClampPosition()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
    }

    // 드래그 시작시
    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDragEvent.Invoke(this);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - (Vector2)transform.position;
        isDragging = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        imageComponent.raycastTarget = false;

        wasDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    // 드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDragEvent.Invoke(this);
        isDragging = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        imageComponent.raycastTarget = true;

        StartCoroutine(FrameWait());

        IEnumerator FrameWait()
        {
            yield return new WaitForEndOfFrame();
            wasDragged = false;
        }
    }

    public void SetInteractable(bool isInteractable)
    {
        imageComponent.raycastTarget = isInteractable;
    }

    public void SetCardVisualNormal()
    {
        cardVisual.SetCardVisualNormal();
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
        isHovering = false;
    }

    public void SetSelect()
    {
        cardVisual.SetSelect();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left && !isSelectable)
            return;

        if(selected)
        {
            UnSelectFunc();
        }
        else
        {
            SelectFunc();
        }

        PointerDownEvent.Invoke(this);
        pointerDownTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        pointerUpTime = Time.time;

        PointerUpEvent.Invoke(this, pointerUpTime - pointerDownTime > .2f);

        if (pointerUpTime - pointerDownTime > .2f)
            return;

        if (wasDragged)
            return;

        //selected = !selected;
        SelectEvent.Invoke(this, selected);

        if (selected)
            transform.localPosition += (cardVisual.transform.up * selectionOffset);
        else
            transform.localPosition = Vector3.zero;
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            if (selected)
                transform.localPosition += (cardVisual.transform.up * 50);
            else
                transform.localPosition = Vector3.zero;
        }
    }


    public int SiblingAmount()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.parent.childCount - 1 : 0;
    }

    public int ParentIndex()
    {
        return transform.parent.CompareTag("Slot") ? transform.parent.GetSiblingIndex() : 0;
    }

    public float NormalizedPosition()
    {
        return transform.parent.CompareTag("Slot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
    }

    private void OnDestroy()
    {
        if (cardVisual != null)
            Destroy(cardVisual.gameObject);
    }
}
