using UnityEngine;
using UnityEditor;

public class SetTextureImportSettings : AssetPostprocessor
{
    void OnPreprocessTexture() {
        if (!assetPath.Contains("/Editor/"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.filterMode = FilterMode.Point;
        }
    }
}
