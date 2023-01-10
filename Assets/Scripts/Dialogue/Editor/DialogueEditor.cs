using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rpg.Dialogue.Editor
{
    
    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }
    }
}
