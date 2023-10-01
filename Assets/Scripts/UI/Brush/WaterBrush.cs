using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Terrarium Brush/Water", order = 1)]
public class WaterBrush : Brush
{
    public float m_waterAdd = 0.1f;
        
    public override BrushResult TryApply(Terrarium _terrarium, Position _position)
    {
        if (m_total >= 0 && m_number <= 0) return BrushResult.FAIL;
        if (!_terrarium.TryFindTileAtPosition(_position, out Tile _tile)) return BrushResult.OUTSIDE;
        Apply(_terrarium, _tile);
        return BrushResult.SUCCESS;
    }

    protected override void Apply(Terrarium _terrarium, Tile _tile)
    {
        base.Apply(_terrarium, _tile);
        _tile.Humidify(m_waterAdd);
    }
}
