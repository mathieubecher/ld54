using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    public static Terrarium terrarium => instance.m_terrarium;
    
    public delegate void NextTurnEvent(int _currentTurn);
    public static NextTurnEvent OnNextTurnBegin;

    private bool m_isRunning = false;
    private bool m_isPaused = false;
    
    public bool isRunning => m_isRunning;
    public bool isPaused => m_isPaused;
    
    [SerializeField] private Terrarium m_terrarium;
    [SerializeField] private TextMeshProUGUI m_waterUI;
    [SerializeField] private Animator m_animatorUI;
    [SerializeField] private Slider m_turnSlider;
    [SerializeField] private float m_turnDuration = 0.5f;

    private int m_currentTurn = 0;
    private float m_currentTurnTimer = 0.0f;
    private void Update()
    {
        if (isRunning && !isPaused)
        {
            m_currentTurnTimer -= Time.deltaTime;
            if (m_currentTurnTimer <= 0.0f)
            {
                m_terrarium.UpdateTurn();
                m_currentTurnTimer = m_turnDuration;
                ++m_currentTurn;
                m_turnSlider.value = m_currentTurn;
                OnNextTurnBegin?.Invoke(m_currentTurn);
            }
        }

        m_waterUI.text = $"Water : {(int)math.ceil(terrarium.GetWaterPercent() * 100.0f)}%";
    }


    public void OnStartButtonPressed()
    {
        m_isRunning = true;
        m_isPaused = false;
        m_animatorUI.SetTrigger("Start");
    }

    public void OnStopButtonPressed()
    {
        m_terrarium.ResetSimulation();
        m_isRunning = false;
        m_isPaused = false;
        m_animatorUI.SetTrigger("Stop");
    }
    
    public void OnPauseButtonPressed()
    {
        if (!isRunning) return;
        m_isPaused = true;
        m_animatorUI.SetBool("isPaused", true);
    }

    public void OnResumeButtonPressed()
    {
        if (!isRunning) return;
        m_isPaused = false;
        m_animatorUI.SetBool("isPaused", false);
    }
    
}
