using System.Collections.Generic;
using System.IO;
using UnityEngine;

void ExportToOBJ(Mesh mesh, string filePath) {
  List < string > lines = new List < string > ();

  lines.Add("o " + gameObject.name);

  foreach(Vector3 v in mesh.vertices)
  lines.Add(string.Format("v {0} {1} {2}", v.x, v.y, v.z));

  foreach(Vector3 n in mesh.normals)
  lines.Add(string.Format("vn {0} {1} {2}", n.x, n.y, n.z));

  foreach(Vector2 uv in mesh.uv)
  lines.Add(string.Format("vt {0} {1}", uv.x, uv.y));

  for (int i = 0; i < mesh.subMeshCount; i++) {
    int[] tris = mesh.GetTriangles(i);
    for (int j = 0; j < tris.Length; j += 3) {
      int index0 = tris[j] + 1;
      int index1 = tris[j + 1] + 1;
      int index2 = tris[j + 2] + 1;

      lines.Add(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", index0, index1, index2));
    }
  }

  File.WriteAllLines(filePath, lines.ToArray());
}
