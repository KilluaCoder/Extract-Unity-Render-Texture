using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ExtractRenderTexture))]
public class ExtractRenderTextureEditor : Editor {

    SerializedProperty[] properties;
    ExtractRenderTexture extractRenderTexture;
    float buttonWidth = 125f;
    private static GUILayoutOption midButtonWidth;// = GUILayout.Width(buttonWidth);
    private static GUILayoutOption midButtonHeigth = GUILayout.Height(25f);
    
    SerializedProperty
        renderTextureCam, textureFormat , format  ,quality ,path ,fileName ,appendIfExists;

    GUIContent capturePicture;
    void OnEnable()
    {
        extractRenderTexture = (ExtractRenderTexture)target;
        midButtonWidth = GUILayout.Width(buttonWidth);
        capturePicture = new GUIContent(" Capture ");

    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        SerializedProperty prop = serializedObject.GetIterator();

        if (prop.NextVisible(true))
        {
            do
            {
                // Draw default property field.
                EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);

            }

            while (prop.NextVisible(false));
        }
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space((EditorGUIUtility.currentViewWidth * 0.5f) -(buttonWidth * 0.5f));
        GUIStyle buttonStyle = EditorStyles.miniButton;
        
        buttonStyle.fontSize = 16;
            
        if (GUILayout.Button(capturePicture, buttonStyle, midButtonWidth, midButtonHeigth))
        {
            extractRenderTexture.ExtractTexture();

        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
