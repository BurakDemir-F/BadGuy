using UnityEditor;

namespace EditorSpecific.MultiObjectEditSaveEditor
{
    public class MultiObjectEditSaveEditor : EditorWindow
    {
        protected string _sourcePath;
        protected string _savePath;

        private void ShowPath(string path)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Foldout(false, path);
            EditorGUILayout.EndHorizontal();
        }
    }
}