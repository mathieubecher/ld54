using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Terrarium Brush/Entity", order = 1)]
public class EntityBrush :Brush
{
    public Entity m_entity;
    private List<Entity> m_entities;

    private void Awake()
    {
        m_entities = new List<Entity>();
    }

    private void OnDestroy()
    {
        foreach (Entity entity in m_entities)
        {
            entity.OnRemoveByTool -= OnRemoveEntityByTool;
        }

        m_entities = new List<Entity>();
    }

    public override BrushResult TryApply(Terrarium _terrarium, Position _position)
    {
        if (m_number <= 0) return BrushResult.FAIL;
        if (!_terrarium.TryFindTileAtPosition(_position, out _)) return BrushResult.OUTSIDE;
        if (_terrarium.CanSpawnEntityAtPosition(_position, m_entity, out Tile _tile))
        {
            Apply(_terrarium, _tile);
            return BrushResult.SUCCESS;
        }

        return BrushResult.FAIL;
    }

    protected override void Apply(Terrarium _terrarium, Tile _tile)
    {
        base.Apply(_terrarium, _tile);
        Entity entity = Entity.Spawn(_terrarium, _tile, m_entity);
        entity.OnRemoveByTool += OnRemoveEntityByTool;
        m_entities.Add(entity);
    }

    private void OnRemoveEntityByTool(Entity _entity)
    {
        _entity.OnRemoveByTool -= OnRemoveEntityByTool;
        m_entities.Remove(_entity);
        RemoveItemInTerrarium();
    }
}