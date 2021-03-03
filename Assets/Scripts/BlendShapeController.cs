using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeController : MonoBehaviour
{

    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    public int blendShapeCount;
    [Range(0, 100f)] public float blendFeet;
    [Range(0, 100f)] public float blendLegs;
    [Range(0, 100f)] public float blendTorso;

    public bool blendOneFinished = false;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    void Start()
    {
        blendShapeCount = skinnedMesh.blendShapeCount;
        blendFeet = skinnedMeshRenderer.GetBlendShapeWeight(0);
        blendLegs = skinnedMeshRenderer.GetBlendShapeWeight(1);
        blendTorso = skinnedMeshRenderer.GetBlendShapeWeight(2);
    }

    void Update()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, blendFeet);
        skinnedMeshRenderer.SetBlendShapeWeight(1, blendLegs);
        skinnedMeshRenderer.SetBlendShapeWeight(2, blendTorso);
    }
}
