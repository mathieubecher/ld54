using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Terrarium Brush/Tile", order = 1)]
public class TileBrush : Brush
{
    public TileData m_data;
    
    public override BrushResult TryApply(Terrarium _terrarium, Position _position)
    {
        if (m_total >= 0 && m_number <= 0) return BrushResult.FAIL;
        if (!_terrarium.TryFindTileAtPosition(_position, out _)) return BrushResult.OUTSIDE;
        if (_terrarium.CanSpawnTileAtPosition(_position, m_data, out Tile _tile))
        {
            Apply(_terrarium, _tile);
            return BrushResult.SUCCESS;
        }
        return BrushResult.FAIL;
    }

    protected override void Apply(Terrarium _terrarium, Tile _tile)
    {
        base.Apply(_terrarium, _tile);
        _tile.SetData(m_data, this);
    }
}
