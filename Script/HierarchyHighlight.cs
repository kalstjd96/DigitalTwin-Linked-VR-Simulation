using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyHighlight : MonoBehaviour
{
    public static HierarchyHighlight instance { get; private set; }
    Material highlightMaterial;

    List<MeshRenderer[]> targets;
    Color[] defaultColors;
    List<Material[]> defaultMaterials;

    void Awake()
    {
        if (instance == null)
            instance = this;

        targets = new List<MeshRenderer[]>();
        defaultMaterials = new List<Material[]>();
    }

    void OnDestroy()
    {
        if (instance != null)
            instance = null;
    }

    public void On(GameObject[] models)
    {
        targets.Insert(0, new MeshRenderer[models.Length]);
        defaultMaterials.Insert(0, new Material[models.Length]);

        for (int i = 0; i < models.Length; i++)
        {
            targets[0][i] = models[i].GetComponent<MeshRenderer>();
            
            defaultMaterials[0][i] = targets[0][i].material;
        }
        
        for (int i = 0; i < targets[0].Length; i++)
            targets[0][i].material = highlightMaterial;
    }

    public void Off()
    {
        for (int i = 0; i < targets.Count; i++)
            for (int j = 0; j < targets[i].Length; j++)
                targets[i][j].material = defaultMaterials[i][j];

        targets.Clear();
        defaultMaterials.Clear();
    }

    public void SetMaterial(Material material)
    {
        highlightMaterial = material;
    }
}
