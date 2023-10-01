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
        if (GameManager.instance.isRunning)
        {
            m_hoverTileUI.gameObject.SetActive(false);
            return;
        }
        
        var mouseWorldPos = m_cameraRef.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Position mousePosition = GameManager.terrarium.WorldToTerrariumPosition(mouseWorldPos);
        if(GameManager.terrarium.TryFindTileAtPosition(m_currentMousePosition, out Tile tile))
        {
            m_hoverTileUI.position = GameManager.terrarium.TerrariumPositionToWorld(m_currentMousePosition);
            m_hoverTileUI.gameObject.SetActive(true);
            
            if(m_click && mousePosition != m_currentMousePosition && m_selectedBrush)
            {
                m_selectedBrush.TryApply(GameManager.terrarium, mousePosition);
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
        if (GameManager.instance.isRunning) return;
        m_click = true;
        if (m_selectedBrush)
        {
            m_selectedBrush.TryApply(GameManager.terrarium, m_currentMousePosition);
        }
    }
    private void OnRelease()
    {
        m_click = false;
    }
}
