using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrushUI : MonoBehaviour
{
    [SerializeField] private Image m_buttonSprite;
    [SerializeField] private TextMeshProUGUI m_number;
    [SerializeField] private Brush m_brush;

    private void OnDestroy()
    {
        m_brush.OnApply -= RefreshNumber;
        m_brush.OnUnapply -= RefreshNumber;
    }

    public void Init(Brush _brush)
    {
        m_brush = _brush;
        m_buttonSprite.sprite = m_brush.m_buttonSprite;
        m_brush.OnApply += RefreshNumber;
        m_brush.OnUnapply += RefreshNumber;
        RefreshNumber();
    }

    private void RefreshNumber()
    {
        if (m_brush.total < 0)
        {
            m_number.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            m_number.transform.parent.gameObject.SetActive(true);
            m_number.text = m_brush.number.ToString();
        }
    }

    public void OnClick()
    {
        if (m_brush.total < 0 || m_brush.number > 0)
        {
            MouseManager.instance.SetBrush(m_brush);
        }
    }
    
}
