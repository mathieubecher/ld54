using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private static MouseManager m_instance;
    public static MouseManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType<MouseManager>();
            }
            return m_instance;
        }
    }
    
    private Camera m_cameraRef;
    private bool m_click;
    
    [SerializeField] private Terrarium m_terrarium;
    [SerializeField] private Transform m_hoverTileUI;
    
    [SerializeField] private Position m_currentMousePosition;
    [SerializeField] private Brush m_selectedBrush;

    void Awake()
    {
        m_cameraRef = Camera.main;
    }

    private void OnEnable()
    {
        ControlsManager.OnClick += OnClick;
        ControlsManager.OnRelease += OnRelease;
    }

    private void OnDisable()
    {
        
        ControlsManager.OnClick -= OnClick;
        ControlsManager.OnRelease -= OnRelease;
    }

    void Update()
    {
        var mouseWorldPos = m_cameraRef.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Position mousePosition = m_terrarium.WorldToTerrariumPosition(mouseWorldPos);
        if(m_terrarium.TryFindTileAtPosition(m_currentMousePosition, out Tile tile))
        {
            m_hoverTileUI.position = m_terrarium.TerrariumPositionToWorld(m_currentMousePosition);
            m_hoverTileUI.gameObject.SetActive(true);
            
            if(m_click && mousePosition != m_currentMousePosition && m_selectedBrush)
            {
                m_selectedBrush.TryApply(m_terrarium, m_currentMousePosition);
            }
        }
        else
        {
            m_hoverTileUI.gameObject.SetActive(false);
        }

        m_currentMousePosition = mousePosition;
    }

    public void SetBrush(Brush _brush)
    {
        m_selectedBrush = _brush;
    }

    public void ResetBrush()
    {
        m_selectedBrush = null;
    }
    
    private void OnClick()
    {
        m_click = true;
        if (m_selectedBrush)
        {
            m_selectedBrush.TryApply(m_terrarium, m_currentMousePosition);
        }
    }
    private void OnRelease()
    {
        m_click = false;
    }
}
