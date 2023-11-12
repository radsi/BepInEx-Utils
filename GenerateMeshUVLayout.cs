using UnityEngine;

Texture2D CreateUVTexture(Mesh mesh, Texture2D texture) {
  Vector2[] uvLayout = new Vector2[mesh.vertices.Length];

  float minX = float.MaxValue;
  float minY = float.MaxValue;
  float maxX = float.MinValue;
  float maxY = float.MinValue;

  for (int i = 0; i < mesh.vertices.Length; i++) {
    uvLayout[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);

    minX = Mathf.Min(minX, uvLayout[i].x);
    minY = Mathf.Min(minY, uvLayout[i].y);
    maxX = Mathf.Max(maxX, uvLayout[i].x);
    maxY = Mathf.Max(maxY, uvLayout[i].y);
  }

  for (int i = 0; i < uvLayout.Length; i++) {
    uvLayout[i] -= new Vector2(minX, minY);
  }

  int width = Mathf.CeilToInt(maxX - minX) * texture.width;
  int height = Mathf.CeilToInt(maxY - minY) * texture.height;

  for (int i = 0; i < uvLayout.Length; i++) {
    uvLayout[i] /= new Vector2(maxX - minX, maxY - minY);
  }

  Texture2D uvTexture = new Texture2D(width, height);

  Color[] colors = new Color[width * height];

  for (int i = 0; i < uvLayout.Length; i++) {
    int x1 = Mathf.FloorToInt(uvLayout[i].x * width);
    int y1 = Mathf.FloorToInt(uvLayout[i].y * height);

    int nextIndex = (i + 1) % uvLayout.Length;
    int x2 = Mathf.FloorToInt(uvLayout[nextIndex].x * width);
    int y2 = Mathf.FloorToInt(uvLayout[nextIndex].y * height);

    DrawLine(colors, width, height, x1, y1, x2, y2, Color.green);
  }

  uvTexture.SetPixels(colors);
  uvTexture.Apply();

  return uvTexture;
}

void DrawLine(Color[] colors, int width, int height, int x1, int y1, int x2, int y2, Color color) {
  int dx = Mathf.Abs(x2 - x1);
  int dy = Mathf.Abs(y2 - y1);
  int sx = (x1 < x2) ? 1 : -1;
  int sy = (y1 < y2) ? 1 : -1;
  int err = dx - dy;

  while (true) {
    int index = y1 * width + x1;
    if (index >= 0 && index < colors.Length) {
      colors[index] = color;
    }

    if (x1 == x2 && y1 == y2) {
      break;
    }

    int e2 = 2 * err;
    if (e2 > -dy) {
      err -= dy;
      x1 += sx;
    }

    if (e2 < dx) {
      err += dx;
      y1 += sy;
    }
  }
}
