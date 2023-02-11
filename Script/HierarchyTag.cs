using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyTag : MonoBehaviour
{
    public static HierarchyTag instance { get; private set; }

    [SerializeField] Transform rightTagPrefab;
    [SerializeField] Transform leftTagPrefab;

    List<Transform> rightTagList;
    List<Transform> leftTagList;
    Dictionary<Transform, string> useTagDictionary;
    Transform originalModel;
    Vector3 center;

    void Awake()
    {
        if (instance == null)
            instance = this;

        rightTagList = new List<Transform>();
        leftTagList = new List<Transform>();
        useTagDictionary = new Dictionary<Transform, string>();
    }

    void OnDestroy()
    {
        if (instance != null)
            instance = null;
    }

    void Start()
    {
        originalModel = SequenceManager.instance.currentEquip.transform;

        MeshRenderer[] meshRenderers = originalModel.GetComponentsInChildren<MeshRenderer>();
        Bounds bounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
            bounds.Encapsulate(meshRenderers[i].bounds);

        center = bounds.center;
    }

    public void On(GameObject models)
    {
        MeshRenderer[] meshRenderers = models.GetComponentsInChildren<MeshRenderer>();
        Bounds bounds = meshRenderers[0].bounds;
        for (int i = 1; i < meshRenderers.Length; i++)
            bounds.Encapsulate(meshRenderers[i].bounds);

        Transform currentTag = null;
        float targetX = originalModel.InverseTransformPoint(bounds.center).x;
        float centerX = originalModel.InverseTransformPoint(center).x;

        if (targetX > centerX)
        {
            if (rightTagList.Count > 0)
            {
                currentTag = rightTagList[0];
                useTagDictionary.Add(currentTag, "Right");
                rightTagList.RemoveAt(0);
            }
            else
            {
                currentTag = Instantiate(rightTagPrefab);
                useTagDictionary.Add(currentTag, "Right");
            }
        }
        else
        {
            if (leftTagList.Count > 0)
            {
                currentTag = leftTagList[0];
                useTagDictionary.Add(currentTag, "Left");
                leftTagList.RemoveAt(0);
            }
            else
            {
                currentTag = Instantiate(leftTagPrefab);
                useTagDictionary.Add(currentTag, "Left");
            }
        }

        currentTag.gameObject.SetActive(true);
        currentTag.position = bounds.center;
        currentTag.GetComponentInChildren<Text>().text = models.name;
    }

    public void Off()
    {
        foreach (var key in useTagDictionary.Keys)
        {
            if (useTagDictionary[key].Equals("Right"))
                rightTagList.Add(key);
            else leftTagList.Add(key);
            
            key.gameObject.SetActive(false);
        }

        useTagDictionary.Clear();
    }
}
