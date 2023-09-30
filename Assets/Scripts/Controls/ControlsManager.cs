using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    private static ControlsManager m_instance;
    public static ControlsManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType<ControlsManager>();
            }
            return m_instance;
        }
    }
    
    public delegate void SimpleEvent();

    public static SimpleEvent OnClick;
    public static SimpleEvent OnRelease;
    public static SimpleEvent OnRightClick;
    public static SimpleEvent OnRightRelease;
    public static SimpleEvent OnEscape;
    
    public void ReadClickInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
            OnClick?.Invoke();
        else if (_context.canceled)
        {
            OnRelease?.Invoke();
        }
    }
    public void ReadRightClickInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnRightClick?.Invoke();
        }
        else if(_context.canceled)
            OnRightRelease?.Invoke();
    }

    public void ReadEscape(InputAction.CallbackContext _context)
    {
        if (_context.performed)
            OnEscape?.Invoke();
    }
}
