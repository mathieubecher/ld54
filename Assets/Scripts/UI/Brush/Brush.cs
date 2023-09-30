using System;
using Unity.Mathematics;
using UnityEngine;

public enum BrushResult
{
    SUCCESS,
    FAIL,
    OUTSIDE,
}

[CreateAssetMenu(fileName = "Data", menuName = "Terrarium Brush/Default", order = 1)]
public class Brush :ScriptableObject
{
    public delegate void SimpleEvent();
    public SimpleEvent OnApply;
    public SimpleEvent OnUnapply;
    
    public Sprite m_cursorSprite;
    public Sprite m_buttonSprite;
    [SerializeField] protected int m_total = -1;
    protected int m_number;
    public int number => m_number; 
    public int total => m_total;

    public virtual BrushResult TryApply(Terrarium _terrarium, Position _position)
    {
        return BrushResult.FAIL;
    }

    protected virtual void Apply(Terrarium _terrarium, Tile _tile)
    {
        if (m_total > 0) --m_number;
        OnApply?.Invoke();
    }

    public void Init()
    {
        m_number = m_total;
    }

    public void RemoveItemInTerrarium()
    {
        m_number = math.min(m_number + 1, m_total);
        OnUnapply?.Invoke();
    }
}
