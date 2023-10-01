using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Vegetable : Entity
{
    [SerializeField] protected float m_waterRequire = 0.0f;
    [SerializeField] protected float m_waterDrain = 0.0f;
    [SerializeField] protected float m_energyRequire = 0.0f;
    [SerializeField] protected float m_energyProduction = 0.0f;

    [SerializeField] protected List<ExpandData> m_expandDatas;
    
    public override void Init(Tile _position)
    {
        base.Init(_position);
        foreach (ExpandData childs in m_expandDatas)
        {
            childs.Init(this);
        }
    }
    public override void UpdateTurn()
    {
        foreach (ExpandData childs in m_expandDatas)
        {
            childs.UpdateTurn();
        }
    }
    
    protected void DrainWaterAndEnergy(out float _waterDrained, out float _energyCollected)
    {
        _waterDrained = m_position.DrainWater(m_waterDrain);
        _energyCollected = m_energyProduction;
        foreach (var expandData in m_expandDatas)
        {
            foreach (var child in expandData.childs)
            {
                child.DrainWaterAndEnergy(out float waterDrainedByChild, out float energyCollectedByChild);
                _waterDrained += waterDrainedByChild;
                _energyCollected += energyCollectedByChild;
            }
        }
    }

    protected float ConsumeWater(float _remainingWater)
    {
        if (_remainingWater >= m_waterRequire)
        {
            float remainingWater = _remainingWater - m_waterRequire;
            m_position.AddWaterToCeil(m_waterRequire);
            
            foreach (var expandData in m_expandDatas)
            {
                foreach (var child in expandData.childs)
                {
                    remainingWater = child.ConsumeWater(remainingWater);
                }
            }
            return remainingWater;
        }
        
        m_position.AddWaterToCeil(_remainingWater);
        Die();
        return 0.0f;
    }
    protected float ConsumeEnergy(float _remainingEnergy)
    {
        if (_remainingEnergy >= m_energyRequire)
        {
            float remainingEnergy = _remainingEnergy - m_energyRequire;
            foreach (var expandData in m_expandDatas)
            {
                foreach (var child in expandData.childs)
                {
                    remainingEnergy = child.ConsumeEnergy(remainingEnergy);
                }
            }
            return remainingEnergy;
        }
        Die();
        return 0.0f;
    }
    
    public override void Die()
    {
        foreach (var expandData in m_expandDatas)
        {
            foreach (var child in expandData.childs)
            {
                child.Die();
            }
        }
        base.Die();
    }

    public override void RequestDestroy()
    {
        
        foreach (var expandData in m_expandDatas)
        {
            foreach (var child in expandData.childs)
            {
                child.RequestDestroy();
            }
        }
        base.RequestDestroy();
    }

    
}
