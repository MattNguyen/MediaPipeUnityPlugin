﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ReadyPlayerMe
{
    public class AvatarLodExampleUI : MonoBehaviour
    {
        private Slider uiSlider;
        private Vector3 cameraStartPos;
        private Text lodInfoText;
        private Camera mainCamera;

        public LODGroup LodGroup { set; get; }

        public void Init()
        {
            lodInfoText = GetComponentInChildren<Text>();
            uiSlider = GetComponentInChildren<Slider>();
            mainCamera = Camera.main;
            cameraStartPos = (mainCamera) ? mainCamera.transform.position : Vector3.zero;

            uiSlider.onValueChanged.AddListener(UpdatePosition);
            uiSlider.onValueChanged.AddListener(UpdateCurrentLod);
        }

        public void Show()
        {
            GetComponent<Canvas>().enabled = true;
        }

        private void UpdatePosition(float value)
        {
            mainCamera.transform.position = new Vector3(cameraStartPos.x, cameraStartPos.y, cameraStartPos.z + value);
        }

        private void UpdateCurrentLod(float value)
        {
            if (LodGroup != null)
            {
                Transform lodTransform = LodGroup.transform;
                foreach (Transform child in lodTransform)
                {
                    var thisRenderer = child.GetComponent<SkinnedMeshRenderer>();
                    if (thisRenderer != null && thisRenderer.isVisible)
                    {
                        var currentLod = child.name.Split('_').Last();
                        var vertexCount = thisRenderer.sharedMesh.vertexCount;
                        var textureSize = thisRenderer.sharedMaterial.mainTexture.height;
                        var morphTargetCount = thisRenderer.sharedMesh.blendShapeCount;
                        UpdateLodInfoPanel(currentLod, vertexCount, textureSize, morphTargetCount);
                    }
                }
            }
        }

        private void UpdateLodInfoPanel(string currentLod, int vertexCount, int textureSize, int morphTargetCount)
        {
            lodInfoText.text = "Current LOD\n" + currentLod + "\n" +
                               "Vertex Count\n" + vertexCount + "\n" +
                               "Texture Size\n" + textureSize + "\n" +
                               "Blend Shapes\n" + morphTargetCount;
        }

        private void OnDisable()
        {
            uiSlider.onValueChanged.RemoveAllListeners();
        }
    }
}
