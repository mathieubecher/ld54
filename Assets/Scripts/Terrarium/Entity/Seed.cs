using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Vegetable
{
    public override bool CanSpawnOnTile(Tile _tile)
    {
        bool typeCompatible = base.CanSpawnOnTile(_tile);
        return typeCompatible && (_tile.neighbours.up && _tile.neighbours.up.type == TileData.TileType.AIR) ;
    }

    private float m_remainingWater = 0.0f;
    
    public override void UpdateTurn()
    {
        base.UpdateTurn();
        DrainWaterAndEnergy(out float waterDrained, out float energyCollected);
        
        m_remainingWater = ConsumeWater(waterDrained + m_remainingWater);
        ConsumeEnergy(energyCollected);
        if(dead && m_remainingWater > 0.0f) m_position.AddWaterToCeil(m_remainingWater);
        else if(m_remainingWater > 1.0f)
        {
            m_position.AddWaterToCeil(m_remainingWater - 1.0f);
            m_remainingWater = 1.0f;
        }
        RequestDestroy();
    }
}
