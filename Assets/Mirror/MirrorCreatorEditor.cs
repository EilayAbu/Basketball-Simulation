using UnityEngine;
using UnityEditor;

public class MirrorCreatorEditor : MonoBehaviour
{
    [MenuItem("Tools/Create Simple Mirror")]
    public static void CreateMirror()
    {
        // === 1. Create Render Texture ===
        var rt = new RenderTexture(1024, 1024, 16);
        rt.name = "MirrorRT";

        // Save render texture as asset so it survives project reload
        AssetDatabase.CreateAsset(rt, "Assets/MirrorRT.renderTexture");

        // === 2. Create Camera ===
        GameObject camObj = new GameObject("MirrorCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.targetTexture = rt;
        cam.clearFlags = CameraClearFlags.Skybox;
        cam.enabled = true;

        // Position camera behind mirror default
        camObj.transform.position = new Vector3(0, 1, -2);
        camObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        // === 3. Create Material ===
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = rt;
        AssetDatabase.CreateAsset(mat, "Assets/MirrorMaterial.mat");

        // === 4. Create Quad ===
        GameObject mirror = GameObject.CreatePrimitive(PrimitiveType.Quad);
        mirror.name = "MirrorSurface";
        mirror.transform.position = new Vector3(0, 1, 0);
        mirror.transform.localScale = new Vector3(2, 2, 1);

        mirror.GetComponent<MeshRenderer>().sharedMaterial = mat;

        // === 5. Put mirror on its own layer to avoid recursion ===
        int mirrorLayer = 30;
        if (mirrorLayer < 32)
        {
            // Set layer name if empty
            SerializedObject tagManager = new SerializedObject(
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
            );
            SerializedProperty layers = tagManager.FindProperty("layers");
            if (layers.GetArrayElementAtIndex(mirrorLayer).stringValue == "")
            {
                layers.GetArrayElementAtIndex(mirrorLayer).stringValue = "Mirror";
                tagManager.ApplyModifiedProperties();
            }
        }

        mirror.layer = mirrorLayer;
        cam.cullingMask &= ~(1 << mirrorLayer);

        // Select result in editor
        Selection.activeGameObject = mirror;

        Debug.Log("Simple Mirror Created Successfully!");
    }
}
