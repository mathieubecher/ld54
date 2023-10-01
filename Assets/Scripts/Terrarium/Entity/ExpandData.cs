using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ExpandData
    {
        [Serializable] struct PossibileExpandPos
        {
            public Position position;
            public int strength;

            public PossibileExpandPos(Position _position, int _strength)
            {
                position = _position;
                strength = _strength;
            }
        }
        [SerializeField] private Vegetable m_vegetableObject;
        [SerializeField] private int m_timeToExpand; 
        [SerializeField, TextArea] private string m_expandShapeText;
        [SerializeField] private bool m_isSeparated = false;

        private Vegetable m_parent;
        [SerializeField] private List<Vegetable> m_childs;
        public List<Vegetable> childs => m_childs;
        
        private List<PossibileExpandPos> m_expandShape;
        private Position m_expandPos;
        private int m_expandCoolDown;

        public void Init(Vegetable _parent)
        {
            m_parent = _parent;
            m_childs = new List<Vegetable>();
            m_expandCoolDown = m_timeToExpand;
            InitExpandShape(m_expandShapeText);
        }

        private void InitExpandShape(string _expandShape)
        {
            m_expandShape = new List<PossibileExpandPos>();
            int y = 0;
            foreach (string line in _expandShape.Split('\n'))
            {
                int x = 0;
                foreach (string box in line.Split(','))
                {
                    if(int.TryParse(box, out int result) && result > 0)
                    {
                        m_expandShape.Add(new PossibileExpandPos(new Position(x, y), result));
                    }
                    else
                    {
                        if (box == "x")
                        {
                            m_expandPos = new Position(x, y);
                        }
                    }
                    ++x;
                }
                ++y;
            }
            m_expandShape.Sort((a,b) => b.strength.CompareTo(a.strength));
        }

        public void UpdateTurn()
        {
            Expand();
            m_childs.RemoveAll(x => x == null);
            foreach (Vegetable child in m_childs)
            {
                child.UpdateTurn();
            }
        }

        private void Expand()
        {
            --m_expandCoolDown;
            if (m_expandCoolDown < 0)
            {
                Tile position = SelectTileToExpand();
                if (position)
                {
                    //Debug.Log("Try to spawn " + m_vegetableObject.m_type + " in " + position);
                    Entity entity = Entity.Spawn(GameManager.terrarium, position, m_vegetableObject, false);
                    if(entity)
                    {
                        if(!m_isSeparated) m_childs.Add(entity as Vegetable);
                        else GameManager.terrarium.AddEntity(entity);
                        
                        m_expandCoolDown = m_timeToExpand;
                    }
                }
            }
        }

        private Tile SelectTileToExpand()
        {
            if (!m_vegetableObject) return null;
            foreach (var position in m_expandShape)
            {
                if (GameManager.terrarium.CanSpawnEntityAtPosition(m_parent.position + position.position - m_expandPos, m_vegetableObject, out Tile _tile))
                {
                    return _tile;
                }
            }

            return null;

        }
    }
