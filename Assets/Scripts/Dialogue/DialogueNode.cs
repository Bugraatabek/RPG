using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    { 
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(10,10,200,100);

        public string GetText()
        {
            return text;
        }

        public void SetText(string newText)
        {
            Undo.RecordObject(this, "Update Dialogue Text");
            text = newText;
        }

        public IEnumerable<string> GetChildren()
        {
            foreach (string child in children)
            {
                yield return child;
            }
        }

        public void AddChild(string name)
        {
            Undo.RecordObject(this, "Added Dialogue Child");
            children.Add(name);
        }

        public void RemoveChild(string name)
        {
            Undo.RecordObject(this, "Removed Dialogue Child");
            children.Remove(name);
        }

        public bool ContainsChild(string name)
        {
            if(children.Contains(name)) return true;
            else return false;
        }

        public Rect GetRect()
        {
            return rect;
        }

        public void SetRectPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Update Dialogue Rect");
            rect.position = newPosition;
        }
    }
}
