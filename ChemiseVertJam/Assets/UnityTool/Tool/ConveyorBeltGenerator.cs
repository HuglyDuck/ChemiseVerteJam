#if(UNITY_EDITOR)
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace VekemanSacha
{
    [ExecuteInEditMode]
    public class ConveyorGenerator : MonoBehaviour
    {
        [Header("Prefabs Settings")]
        [Tooltip("Prefab used for the foot of the conveyor")]
        public GameObject footPrefab;
        [Tooltip("Prefab used for the belt of the conveyor")]
        public GameObject beltPrefab;

        [Header("Conveyor Settings")]
        [Tooltip("Space between each control points")]
        public float pointSpacing = 0.5f;

        [Header("Belt Settings")]
        public float beltWidth = 1f;
        public float beltHeight = 1f;
        public float beltLength = 1f;

        [Header("Foot Settings")]
        public float footWidth = 0.5f;
        public float footLength = 0.5f;
        public int footPlacementFactor = 1;

        [Header("Junction settings")]
        [Tooltip("Y offset of the junction")]
        public float junctionHeight = 0.5f;
        public float junctionYOffset = 0f;
        [Tooltip("Material used for the junction")]
        public Material conveyorMaterial;

        [Header("Control points list")]
        public List<GameObject> list = new List<GameObject>();

        private List<Vector3> lastBeltEnds = new List<Vector3>();
        private List<Vector3> nextBeltStarts = new List<Vector3>();
        private Vector3[] lastControlPointsPositions;

        private GameObject NewObject;
        public int NumberControlPoint = 0;

        private float lastPointSpacing;
        private float lastBeltWidth;
        private float lastBeltHeight;
        private float lastBeltLength;
        private float lastFootWidth;
        private float lastFootLength;
        private float lastJunctionHeight;
        private float lastJunctionYOffset;
        private Material lastConveyorMaterial;
        private int lastFootPlacementFactor;

        public void AddToList(GameObject obj)
        {
            if (list.Count == 0)
            {
                NumberControlPoint = 0;
            }
            list.Add(obj);
            NumberControlPoint++;
        }

        public void RemoveFromList(GameObject obj)
        {
            if (list.Contains(obj))
            {
                list.Remove(obj);
                DestroyImmediate(obj);
                NumberControlPoint--;
            }
            else
            {
                print("List does not contain the specified Control Point.");
            }
        }

        public void AddToList()
        {
            if (list.Count == 0)
            {
                NumberControlPoint = 0;
            }
            NewObject = new GameObject("Control Point " + NumberControlPoint);
            NewObject.AddComponent<ControlPoint>().generator = this;
            list.Add(NewObject);
            NumberControlPoint++;
        }

        public void RemoveFromList()
        {
            if (list.Count > 0)
            {
                GameObject obj = list[list.Count - 1];
                list.Remove(obj);
                DestroyImmediate(obj);
                NumberControlPoint--;
            }
            else
            {
                print("List is empty. No Control Point to remove.");
            }
        }

        private void OnDrawGizmos()
        {
            if (list == null || list.Count < 2)
                return;

            // Gizmos pour courbe de Bézier
            Gizmos.color = Color.yellow;

            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector3 p0 = list[i].transform.position;
                Vector3 p1 = list[i + 1].transform.position;

                Handles.DrawBezier(p0, p1, p0, p1, Color.yellow, null, 2f);
            }

            Gizmos.color = Color.blue;
            for (int i = 0; i < lastBeltEnds.Count - 1; i++)
            {
                Vector3 controlPoint = list[i + 1].transform.position;
                Handles.DrawBezier(lastBeltEnds[i], nextBeltStarts[i + 1], lastBeltEnds[i], controlPoint, Color.blue, null, 2f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(lastBeltEnds[i], controlPoint);
            }
        }

        private void Update()
        {
            if (list == null || list.Count < 2)
                return;

            // Check if any control points have moved
            if (lastControlPointsPositions == null || lastControlPointsPositions.Length != list.Count)
            {
                lastControlPointsPositions = new Vector3[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    lastControlPointsPositions[i] = list[i].transform.position;
                }
            }

            bool hasChanged = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].transform.position != lastControlPointsPositions[i])
                {
                    hasChanged = true;
                    lastControlPointsPositions[i] = list[i].transform.position;
                }
            }

            // Check if any public parameters have changed
            if (pointSpacing != lastPointSpacing ||
                beltWidth != lastBeltWidth ||
                beltHeight != lastBeltHeight ||
                beltLength != lastBeltLength ||
                footWidth != lastFootWidth ||
                footLength != lastFootLength ||
                junctionHeight != lastJunctionHeight ||
                junctionYOffset != lastJunctionYOffset ||
                conveyorMaterial != lastConveyorMaterial ||
                footPlacementFactor != lastFootPlacementFactor)
            {
                hasChanged = true;
                lastPointSpacing = pointSpacing;
                lastBeltWidth = beltWidth;
                lastBeltHeight = beltHeight;
                lastBeltLength = beltLength;
                lastFootWidth = footWidth;
                lastFootLength = footLength;
                lastJunctionHeight = junctionHeight;
                lastJunctionYOffset = junctionYOffset;
                lastConveyorMaterial = conveyorMaterial;
                lastFootPlacementFactor = footPlacementFactor;
            }

            if (hasChanged)
            {
                GenerateConveyor();
            }
        }

        public void GenerateConveyor()
        {
            ClearConveyor();

            if (list == null || list.Count < 2)
                return;

            lastBeltEnds.Clear();
            nextBeltStarts.Clear();

            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector3 direction = (list[i + 1].transform.position - list[i].transform.position).normalized;
                Vector3 start = list[i].transform.position;
                Vector3 end = list[i + 1].transform.position;

                if (i != 0) start += direction * pointSpacing;
                if (i != list.Count - 2) end -= direction * pointSpacing;

                float distance = Vector3.Distance(start, end);

                GameObject tempBelt = Instantiate(beltPrefab, Vector3.zero, Quaternion.identity);
                Collider beltCollider = tempBelt.GetComponent<Collider>() ?? tempBelt.AddComponent<BoxCollider>();
                float beltSegmentLength = beltCollider.bounds.size.z;
                DestroyImmediate(tempBelt);

                int segments = Mathf.CeilToInt(distance / beltSegmentLength);
                float totalLength = segments * beltSegmentLength;
                float scaleFactor = Mathf.Sqrt(distance * distance / (totalLength * totalLength));

                Vector3 previousPosition = start;
                Quaternion rotation = Quaternion.LookRotation(direction);

                for (int j = 0; j < segments; j++)
                {
                    float t = j / (float)segments;
                    Vector3 position = Vector3.Lerp(start, end, t) + rotation * new Vector3(0, 0, beltSegmentLength * scaleFactor * 0.5f);

                    GameObject beltSegment = Instantiate(beltPrefab, position, rotation, transform);
                    beltSegment.transform.localScale = new Vector3(beltWidth, beltHeight, beltLength * scaleFactor);

                    if (beltSegment.GetComponent<Collider>() == null)
                    {
                        beltSegment.AddComponent<BoxCollider>();
                    }

                    if (j == segments - 1)
                    {
                        lastBeltEnds.Add(position + rotation * new Vector3(0, 0, beltSegmentLength * scaleFactor * 0.5f));
                    }

                    if (j == 0)
                    {
                        nextBeltStarts.Add(position - rotation * new Vector3(0, 0, beltSegmentLength * scaleFactor * 0.5f));
                    }

                    if (j % footPlacementFactor == 0)
                    {
                        PlaceFeetAgainstCollider(position, beltCollider, rotation);
                    }

                    previousPosition = position + rotation * new Vector3(0, 0, beltSegmentLength * scaleFactor * 0.5f);
                }
            }

            GenerateBezierMesh();
        }

        private void PlaceFeetAgainstCollider(Vector3 startPosition, Collider beltCollider, Quaternion rotation)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPosition, Vector3.down, out hit))
            {
                GameObject tempFoot = Instantiate(footPrefab, Vector3.zero, Quaternion.identity);
                Collider footCollider = tempFoot.GetComponent<Collider>();
                if (footCollider == null)
                {
                    MeshCollider meshCollider = tempFoot.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    footCollider = meshCollider;
                }
                float distanceToCollider = Vector3.Distance(startPosition, hit.point);

                float tempFootHeight = footCollider.bounds.size.y;
                DestroyImmediate(tempFoot);

                int footCount = Mathf.CeilToInt(distanceToCollider / tempFootHeight);
                float totalFootHeight = footCount * tempFootHeight;
                float scaleFactor = distanceToCollider / totalFootHeight;

                Vector3 footPosition = startPosition;

                for (int i = 0; i < footCount; i++)
                {
                    if (beltCollider != null && footPosition.y - tempFootHeight * scaleFactor < beltCollider.bounds.max.y)
                    {
                        footPosition.y = beltCollider.bounds.max.y + tempFootHeight * scaleFactor / 2;
                    }
                    else
                    {
                        footPosition.y -= tempFootHeight * scaleFactor / 2;
                    }

                    if (Physics.Raycast(footPosition, Vector3.down, out hit, tempFootHeight * scaleFactor))
                    {
                        footPosition.y = hit.point.y + tempFootHeight * scaleFactor / 2;
                    }

                    GameObject foot = Instantiate(footPrefab, footPosition, Quaternion.Euler(0, rotation.eulerAngles.y, 0), transform);
                    foot.transform.localScale = new Vector3(footWidth, tempFootHeight * scaleFactor, footLength);

                    if (foot.GetComponent<Collider>() == null)
                    {
                        MeshCollider meshCollider = foot.AddComponent<MeshCollider>();
                        meshCollider.convex = true;
                    }

                    footPosition.y -= tempFootHeight * scaleFactor / 2;
                }
            }
        }

        public void ClearConveyor()
        {
            // Clear existing conveyor segments and feet
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            lastBeltEnds.Clear();
            nextBeltStarts.Clear();
        }

        // Create Bezier Mesh
        private void GenerateBezierMesh()
        {
            // Ensure we have at least two belt sections
            if (lastBeltEnds.Count < 2 || nextBeltStarts.Count < 2) return;

            for (int i = 0; i < lastBeltEnds.Count - 1; i++)
            {
                // Use the actual end of the previous belt section and start of the next section
                Vector3 p0 = lastBeltEnds[i];
                Vector3 p1 = list[i + 1].transform.position; // Control point, can be adjusted
                Vector3 p2 = nextBeltStarts[i + 1];

                int curveResolution = 20; // Increase for a smoother curve
                Vector3[] bezierPoints = new Vector3[curveResolution + 1];

                for (int j = 0; j <= curveResolution; j++)
                {
                    float t = j / (float)curveResolution;
                    bezierPoints[j] = CalculateBezierPoint(t, p0, p1, p2);
                }

                CreateMesh(bezierPoints);
            }
        }

        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0; // (1-t)^2 * p0
            p += 2 * u * t * p1; // 2 * (1-t) * t * p1
            p += tt * p2;        // t^2 * p2

            return p;
        }

        private void CreateMesh(Vector3[] bezierPoints)
        {
            Mesh mesh = new Mesh();
            int numVertices = bezierPoints.Length * 8;
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];
            int[] triangles = new int[(bezierPoints.Length - 1) * 36];

            float width = beltWidth; // Utiliser beltWidth pour la largeur du maillage

            for (int i = 0; i < bezierPoints.Length; i++)
            {
                Vector3 forward = Vector3.forward;
                if (i < bezierPoints.Length - 1)
                {
                    forward = bezierPoints[i + 1] - bezierPoints[i];
                }
                else
                {
                    forward = bezierPoints[i] - bezierPoints[i - 1];
                }

                Vector3 right = Vector3.Cross(Vector3.up, forward).normalized * width;
                Vector3 up = Vector3.up * junctionHeight;

                // Base vertices
                vertices[i * 8] = bezierPoints[i] - right * 0.5f + Vector3.up * junctionYOffset;
                vertices[i * 8 + 1] = bezierPoints[i] + right * 0.5f + Vector3.up * junctionYOffset;

                // Vertices for side walls
                vertices[i * 8 + 2] = vertices[i * 8] + up;
                vertices[i * 8 + 3] = vertices[i * 8 + 1] + up;

                if (i < bezierPoints.Length - 1)
                {
                    vertices[i * 8 + 4] = bezierPoints[i + 1] - right * 0.5f + Vector3.up * junctionYOffset;
                    vertices[i * 8 + 5] = bezierPoints[i + 1] + right * 0.5f + Vector3.up * junctionYOffset;
                    vertices[i * 8 + 6] = bezierPoints[i + 1] - right * 0.5f + up + Vector3.up * junctionYOffset;
                    vertices[i * 8 + 7] = bezierPoints[i + 1] + right * 0.5f + up + Vector3.up * junctionYOffset;
                }

                uv[i * 8] = new Vector2(1, 1 - (float)i / (bezierPoints.Length - 1));
                uv[i * 8 + 1] = new Vector2(0, 1 - (float)i / (bezierPoints.Length - 1));
                uv[i * 8 + 2] = new Vector2(1, 1 - (float)i / (bezierPoints.Length - 1));
                uv[i * 8 + 3] = new Vector2(0, 1 - (float)i / (bezierPoints.Length - 1));
                uv[i * 8 + 4] = new Vector2(1, 1 - (float)(i + 1) / (bezierPoints.Length - 1));
                uv[i * 8 + 5] = new Vector2(0, 1 - (float)(i + 1) / (bezierPoints.Length - 1));
                uv[i * 8 + 6] = new Vector2(1, 1 - (float)(i + 1) / (bezierPoints.Length - 1));
                uv[i * 8 + 7] = new Vector2(0, 1 - (float)(i + 1) / (bezierPoints.Length - 1));
            }

            for (int i = 0; i < bezierPoints.Length - 1; i++)
            {
                int vertIndex = i * 8;
                int triIndex = i * 36;

                // Base triangles
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 1] = vertIndex + 8;
                triangles[triIndex + 2] = vertIndex + 1;

                triangles[triIndex + 3] = vertIndex + 1;
                triangles[triIndex + 4] = vertIndex + 8;
                triangles[triIndex + 5] = vertIndex + 9;

                // Top wall triangles
                triangles[triIndex + 6] = vertIndex + 2;
                triangles[triIndex + 7] = vertIndex + 10;
                triangles[triIndex + 8] = vertIndex + 3;

                triangles[triIndex + 9] = vertIndex + 3;
                triangles[triIndex + 10] = vertIndex + 10;
                triangles[triIndex + 11] = vertIndex + 11;

                // Bottom wall triangles
                triangles[triIndex + 12] = vertIndex + 0;
                triangles[triIndex + 13] = vertIndex + 8;
                triangles[triIndex + 14] = vertIndex + 1;

                triangles[triIndex + 15] = vertIndex + 1;
                triangles[triIndex + 16] = vertIndex + 8;
                triangles[triIndex + 17] = vertIndex + 9;

                // Right wall triangles
                triangles[triIndex + 18] = vertIndex + 1;
                triangles[triIndex + 19] = vertIndex + 3;
                triangles[triIndex + 20] = vertIndex + 9;

                triangles[triIndex + 21] = vertIndex + 3;
                triangles[triIndex + 22] = vertIndex + 11;
                triangles[triIndex + 23] = vertIndex + 9;

                // Left wall triangles
                triangles[triIndex + 24] = vertIndex + 0;
                triangles[triIndex + 25] = vertIndex + 2;
                triangles[triIndex + 26] = vertIndex + 8;

                triangles[triIndex + 27] = vertIndex + 2;
                triangles[triIndex + 28] = vertIndex + 10;
                triangles[triIndex + 29] = vertIndex + 8;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            GameObject meshObject = new GameObject("BezierMesh");
            meshObject.transform.SetParent(transform);
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter.mesh = mesh;
            if (conveyorMaterial != null)
            {
                meshRenderer.material = conveyorMaterial;
            }
            else
            {
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }
        }

        public void FreezeConveyor()
        {
            GameObject frozenConveyor = new GameObject("FrozenConveyor");
            frozenConveyor.transform.position = transform.position;
            frozenConveyor.transform.rotation = transform.rotation;

            foreach (Transform child in transform)
            {
                GameObject copiedChild = Instantiate(child.gameObject, frozenConveyor.transform);
                RemoveScripts(copiedChild);
            }

            gameObject.SetActive(false);
        }

        private void RemoveScripts(GameObject obj)
        {
            MonoBehaviour[] scripts = obj.GetComponentsInChildren<MonoBehaviour>();
            foreach (var script in scripts)
            {
                DestroyImmediate(script);
            }
        }
    }
}

#endif