using System;
using UnityEngine;

namespace Impasta {
    internal class LightCaster: MonoBehaviour {
        private struct angledVerts { //used for updating the vertices and UVs of the light mesh. The angle variable is for properly sorting the ray hit points.
            public Vector3 vert;
            public float angle;
            public Vector2 uv;
        }

        #region Fields

        private Collider[] colliders; //Colliders that affect lighting
        private Mesh lightMesh;
        private GameObject lightMask;

        [SerializeField] private bool showRed; //Show -ve offset rays casted
        [SerializeField] private bool showGreen; //Show +ve offset rays casted
        [SerializeField] private float offset; //Offset of the 2 rays cast to left and right of each vertex of scene objs
        [SerializeField] private float radius;
        [SerializeField] private LayerMask objMask;
        [SerializeField] private LayerMask ignoreMe;

        #endregion

        #region Properties

        public GameObject LightMask {
            get {
                return lightMask;
            }
            set {
                lightMask = value;
                lightMesh = lightMask.GetComponent<MeshFilter>().mesh;
            }
        }

        #endregion

        #region Ctors and Dtor

        public LightCaster() {
            colliders = Array.Empty<Collider>();
            lightMesh = null;
            lightMask = null;

            showRed = false;
            showGreen = false;
            offset = 0.0f;
            radius = 0.0f;
            objMask = ~0; //Everything
            ignoreMe = ~0; //Everything
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            if(lightMask == null || lightMesh == null) {
                return;
            }

            GetColliders();

            lightMesh.Clear();

            Vector3[] vertices = colliders[0].GetComponent<MeshFilter>().mesh.vertices;
            for(int i = 1; i < colliders.Length; i++) {
                vertices = ConcatenateArrs(vertices, colliders[i].GetComponent<MeshFilter>().mesh.vertices);
            }

            angledVerts[] angleds = new angledVerts[(vertices.Length * 2)];
            Vector3[] verts = new Vector3[(vertices.Length * 2) + 1];
            Vector2[] uvs = new Vector2[(vertices.Length * 2) + 1];

            verts[0] = lightMask.transform.worldToLocalMatrix.MultiplyPoint3x4(this.transform.position);
            uvs[0] = new Vector2(lightMask.transform.worldToLocalMatrix.MultiplyPoint3x4(this.transform.position).x, lightMask.transform.worldToLocalMatrix.MultiplyPoint3x4(this.transform.position).y);

            int h = 0;

            for(int j = 0; j < colliders.Length; j++)
            {
                for(int i = 0; i < colliders[j].GetComponent<MeshFilter>().mesh.vertices.Length; i++)
                {
                    Vector3 me = this.transform.position;
                    Vector3 other = colliders[j].transform.localToWorldMatrix.MultiplyPoint3x4(vertices[h]);

                    float angle1 = Mathf.Atan2(((other.y - me.y) - offset), ((other.x - me.x) - offset));
                    float angle3 = Mathf.Atan2(((other.y - me.y) + offset), ((other.x - me.x) + offset));

                    RaycastHit hit;
                    Physics.Raycast(transform.position, new Vector2((other.x - me.x) - offset, (other.y - me.y) - offset), out hit, 100, ~ignoreMe); //~ignoreMe for raycast to pass through objs of ignoreMe LayerMask
                    RaycastHit hit2;
                    Physics.Raycast(transform.position, new Vector2((other.x - me.x) + offset, (other.y - me.y) + offset), out hit2, 100, ~ignoreMe); //~ignoreMe for raycast to pass through objs of ignoreMe LayerMask

                    angleds[(h * 2)].vert = lightMask.transform.worldToLocalMatrix.MultiplyPoint3x4(hit.point);
                    angleds[(h * 2)].angle = angle1;
                    angleds[(h * 2)].uv = new Vector2(angleds[(h * 2)].vert.x, angleds[(h * 2)].vert.y);

                    angleds[(h * 2) + 1].vert = lightMask.transform.worldToLocalMatrix.MultiplyPoint3x4(hit2.point);
                    angleds[(h * 2) + 1].angle = angle3;
                    angleds[(h * 2) + 1].uv = new Vector2(angleds[(h * 2) + 1].vert.x, angleds[(h * 2) + 1].vert.y);

                    h++;

                    if(showRed && hit.collider != null)
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                    }
                    if(showGreen) {
                        Debug.DrawLine(transform.position, hit2.point, Color.green);
                    }

                }
            }

            Array.Sort(angleds, delegate (angledVerts one, angledVerts two) {
                return one.angle.CompareTo(two.angle);
            });

            for(int i = 0; i < angleds.Length; i++)
            {
                verts[i + 1] = angleds[i].vert;
                uvs[i + 1] = angleds[i].uv;
            }

            lightMesh.vertices = verts;

            for(int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(uvs[i].x + .5f, uvs[i].y + .5f);
            }

            lightMesh.uv = uvs;

            int[] triangles = { 0, 1, verts.Length - 1 };

            for(int i = verts.Length - 1; i > 0; i--)
            {
                triangles = Add3IntsToArr(triangles, 0, i, i - 1);
            }

            lightMesh.triangles = triangles;
        }

        #endregion

        private void GetColliders() {
            colliders = Physics.OverlapSphere(transform.position, radius, objMask);
        }

        private static int[] Add3IntsToArr(int[] OG, int val0, int val1, int val2) {
            int[] finalArr = new int[OG.Length + 3];
            for(int i = 0; i < OG.Length; i++) {
                finalArr[i] = OG[i];
            }

            finalArr[OG.Length] = val0;
            finalArr[OG.Length + 1] = val1;
            finalArr[OG.Length + 2] = val2;
            return finalArr;
        }

        private static Vector3[] ConcatenateArrs(Vector3[] first, Vector3[] second) {
            Vector3[] concatted = new Vector3[first.Length + second.Length];

            Array.Copy(first, concatted, first.Length);
            Array.Copy(second, 0, concatted, first.Length, second.Length);

            return concatted;
        }
    }
}