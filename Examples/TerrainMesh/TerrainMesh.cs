﻿using UnityEngine;

namespace ProceduralToolkit.Examples
{
    [RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
    public class TerrainMesh : MonoBehaviour
    {
        public float xSize = 10;
        public float zSize = 10;
        public int xSegments = 100;
        public int zSegments = 100;
        public float scale = 100;

        private MeshDraft draft;
        private Gradient gradient;

        private void Start()
        {
            draft = MeshE.PlaneDraft(xSize, zSize, xSegments, zSegments);
            Generate();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Generate();
            }
        }

        private void Generate()
        {
            gradient = RandomE.gradientHSV;

            var offset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
            draft.colors.Clear();
            for (int i = 0; i < draft.vertices.Count; i++)
            {
                var vertex = draft.vertices[i];
                var x = scale*vertex.x/xSegments + offset.x;
                var y = scale*vertex.z/zSegments + offset.y;
                var noise = Mathf.PerlinNoise(x, y);
                draft.vertices[i] = new Vector3(vertex.x, noise, vertex.z);
                draft.colors.Add(gradient.Evaluate(noise));
            }

            GetComponent<MeshFilter>().mesh = draft.ToMesh();
        }

        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(20, 20, Screen.width, Screen.height), "Click to generate new terrain");
        }
    }
}