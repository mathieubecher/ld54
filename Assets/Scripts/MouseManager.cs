using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private Camera m_cameraRef;
    [SerializeField] private Terrarium m_terrarium;
    [SerializeField] private Transform m_hoverTileUI;
    
    [SerializeField] private Tile m_hoverTile;
    [SerializeField] private TileData m_selectedTile;

    void Awake()
    {
        m_cameraRef = Camera.main;
    }

    private void OnEnable()
    {
        ControlsManager.OnClick += OnClick;
        ControlsManager.OnRightClick += OnRightClick;
    }

    private void OnDisable()
    {
        
        ControlsManager.OnClick -= OnClick;
        ControlsManager.OnRightClick -= OnRightClick;
    }

    void Update()
    {
        var mouseWorldPos = m_cameraRef.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        
        if(m_terrarium.TryFindTileAtPosition(m_terrarium.WorldToTerrariumPosition(mouseWorldPos), out Tile tile))
        {
            m_hoverTile = tile;
            m_hoverTileUI.position = m_hoverTile.transform.position;
            m_hoverTileUI.gameObject.SetActive(true);
        }
        else
        {
            m_hoverTile = null;
            m_hoverTileUI.gameObject.SetActive(false);
        }
    }
    

    private void OnClick()
    {
        if (m_hoverTile && m_selectedTile)
        {
            m_hoverTile.SetData(m_selectedTile);
        }
    }
    private void OnRightClick()
    {
        
    }
}
