using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotionTransfer : MonoBehaviour
{
    // parent joints of all joints to be retargeted
    [SerializeField] private Transform srcLArm, srcRArm, srcHead;
    public Transform tgtLArm, tgtRArm, tgtHead;
    
    // blendshape meshes
    [SerializeField] private SkinnedMeshRenderer srcFace;
    public SkinnedMeshRenderer tgtFace, tgtTeeth;


    private bool inHub;
    [HideInInspector] public NetworkPlayer networkPlayer;

    void Start()
    {
        inHub = SceneManager.GetActiveScene().name == "Hub";
    }

    void Update()
    {
        // transfer face/teeth blendshapes
        for (int i = 0; i < srcFace?.sharedMesh?.blendShapeCount; i++)
        {
            float weight = srcFace.GetBlendShapeWeight(i);
            string name = srcFace.sharedMesh.GetBlendShapeName(i);

            int idx = tgtFace.sharedMesh.GetBlendShapeIndex(name);
            if (idx != -1) {
              tgtFace.SetBlendShapeWeight(idx, weight);
              return;
            }

            if (name == "jawOpen")
            {
                tgtTeeth.SetBlendShapeWeight(idx, weight);
            }
        }
    }

    void LateUpdate()
    {
        tgtHead.localRotation = srcHead.localRotation;

        if (inHub)
        {
            if (networkPlayer.idle)
            {
                mapJointRotation(srcLArm, tgtLArm);
                mapJointRotation(srcRArm, tgtRArm);
            }
        }
        else
        {
            mapJointRotation(srcLArm, tgtLArm);
            mapJointRotation(srcRArm, tgtRArm);
        }
    }

    private void mapJointRotation(Transform src, Transform tgt)
    {   
        tgt.localRotation = src.localRotation;

        foreach (Transform srcChild in src)
        {
            Transform tgtChild = tgt.Find(srcChild.name);
            mapJointRotation(srcChild, tgtChild);
        }
    }
}
