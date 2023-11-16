using BepInEx;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum SaveTextureFileFormat
{
  JPG, PNG
};

static public async Task SaveTexture2DToFileAsync(Texture2D tex, string filePath, SaveTextureFileFormat fileFormat, int jpgQuality) {
  await Task.Run(() => {
    switch (fileFormat) {
    case SaveTextureFileFormat.JPG:
      File.WriteAllBytes(filePath, tex.EncodeToJPG(jpgQuality));
      break;
    case SaveTextureFileFormat.PNG:
      File.WriteAllBytes(filePath, tex.EncodeToPNG());
      break;
    }
  });
}

static public void SaveMaterialTexturesToFileAsync(Material material, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG, int jpgQuality = 95) {
  string path = Path.Combine(Paths.PluginPath, "textures_dump", material.name);

  string[] names = material.GetTexturePropertyNames();
  foreach(string name in names) {
    string filePath = Path.Combine(path, $"{name}.png");
    if (File.Exists(filePath)) return;

    Texture currentTexture = material.GetTexture(name);

    Texture2D texture2D = new Texture2D(currentTexture.width, currentTexture.height, TextureFormat.RGBAFloat, false);

    RenderTexture currentRT = RenderTexture.active;

    RenderTexture renderTexture = new RenderTexture(currentTexture.width, currentTexture.height, 32);
    Graphics.Blit(currentTexture, renderTexture);

    RenderTexture.active = renderTexture;
    texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    texture2D.Apply();

    RenderTexture.active = currentRT;

    Task.Run(async () => await SaveTexture2DToFileAsync(texture2D, filePath, fileFormat, jpgQuality));
  }
}
