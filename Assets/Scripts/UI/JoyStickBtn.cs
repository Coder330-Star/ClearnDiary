using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickBtn : ScrollRect
{    
    private float radious;//拖拽半径
    private RectTransform imgArrowTrans;//箭头

    protected override void Start()
    {
        base.Start();
        radious = content.sizeDelta.x * 0.5f;
        imgArrowTrans = transform.Find("img_Arrow").GetComponent<RectTransform>();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        Vector2 touchPos = content.anchoredPosition;
        if (touchPos.magnitude > radious)
        {
            touchPos = touchPos.normalized * radious;
            SetContentAnchoredPosition(touchPos);
        }

        Vector2 contentPos = content.anchoredPosition.normalized;
        GameManager.Instance.inputValue = contentPos;
        float angle = -Mathf.Atan2(contentPos.x, contentPos.y) * Mathf.Rad2Deg;
        GameManager.Instance.inputAngle = angle;
        imgArrowTrans.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        SetContentAnchoredPosition(Vector2.zero);
        GameManager.Instance.inputValue = Vector2.zero;
        imgArrowTrans.localRotation = Quaternion.identity;
    }
}
