using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushManager : MonoBehaviour
{
    [SerializeField] private List<Brush> m_brushes;
    [SerializeField] private BrushUI m_brushButtonPrefab;
    private List<BrushUI> m_bruhesButton;

    private void Start()
    {
        Init();
    }
    
    public void Init()
    {
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0));
        }
    
        foreach (Brush brush in m_brushes)
        {
            brush.Init();
            var brushUI = Instantiate(m_brushButtonPrefab, transform);
            brushUI.Init(brush);
        }
    }
}
