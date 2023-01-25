using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RPG.Quests;
using System.Linq;
using System;
using static RPG.Utils.Condition;
using RPG.Inventories;

namespace RPG.Utils.Editor
{
    [CustomPropertyDrawer(typeof(Condition.Predicate))]
    public class PredicatePropertyDrawer : PropertyDrawer
    {
        Dictionary<string, Quest> quests;
        Dictionary<string, InventoryItem> items;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {   
            SerializedProperty predicate = property.FindPropertyRelative("predicate");
            SerializedProperty parameters = property.FindPropertyRelative("parameters");
            SerializedProperty negate = property.FindPropertyRelative("negate");

            
            float propHeight = EditorGUI.GetPropertyHeight(predicate);
            position.height = propHeight;
            EditorGUI.PropertyField(position, predicate);
            

            EPredicate selectedPredicate = (EPredicate)predicate.enumValueIndex;
            
            if (selectedPredicate == EPredicate.Select) return; //Stop drawing if there's no predicate
            
            while(parameters.arraySize < 2)
            {
                parameters.InsertArrayElementAtIndex(0);
            }

            SerializedProperty parameterZero = parameters.GetArrayElementAtIndex(0);
            SerializedProperty parameterOne = parameters.GetArrayElementAtIndex(1);

            if (selectedPredicate == EPredicate.HasQuest || selectedPredicate == EPredicate.CompletedQuest || selectedPredicate == EPredicate.CompletedObjective)
            {
                Rect questPosition = position;
                questPosition.y += propHeight;
                DrawQuest(questPosition, parameterZero);
                Rect negatePosition = position;
                negatePosition.y += propHeight * 2;
                EditorGUI.PropertyField(negatePosition, negate);
            }

            if(selectedPredicate == EPredicate.HasItem || selectedPredicate == EPredicate.HasItemEquipped)
            {
                Rect firstItemPosition = position;
                firstItemPosition.y += propHeight;
                DrawItem(firstItemPosition, parameterZero);
                Rect negatePosition = position;
                negatePosition.y += propHeight * 2;
                EditorGUI.PropertyField(negatePosition, negate);
            }
            if(selectedPredicate == EPredicate.HasItems)
            {
                Rect firstItemPosition = position;
                firstItemPosition.y += propHeight;
                Rect secondItemPosition = position;
                secondItemPosition.y += propHeight * 2;
                Rect negatePosition = position;
                negatePosition.y += propHeight * 3;
                DrawItem(firstItemPosition, parameterZero);
                DrawItem(secondItemPosition, parameterZero);
                EditorGUI.PropertyField(negatePosition, negate);
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty predicate = property.FindPropertyRelative("predicate");
            float propHeight = EditorGUI.GetPropertyHeight(predicate);
            EPredicate selectedPredicate = (EPredicate)predicate.enumValueIndex;
            switch (selectedPredicate)
            {
                case EPredicate.Select: //No parameters, we only want the bare enum. 
                    return propHeight; 
                case EPredicate.HasLevel:       //All of these take 1 parameter
                case EPredicate.CompletedQuest:
                case EPredicate.HasQuest:
                case EPredicate.HasItem:
                case EPredicate.HasItemEquipped:
                    return propHeight * 3.0f; //Predicate + one parameter + negate
                case EPredicate.CompletedObjective: //All of these take 2 parameters
                case EPredicate.HasItems:
                case EPredicate.MinimumTrait:
                    return propHeight * 4.0f; //Predicate + 2 parameters + negate;
            }
            return propHeight * 2.0f;
        }

        private void DrawQuest(Rect position, SerializedProperty element)
        {
            BuildQuestList();
            var names = quests.Keys.ToList();
            int index = names.IndexOf(element.stringValue);
            
            EditorGUI.BeginProperty(position, new GUIContent("Quest:"), element);
            int newIndex = EditorGUI.Popup(position,"Quest:", index, names.ToArray());
            if (newIndex != index)
            {
                element.stringValue = names[newIndex];
            }

            EditorGUI.EndProperty();
        }

        void BuildQuestList()
        {
            if (quests != null) return;
            quests = new Dictionary<string, Quest>();
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                quests[quest.name] = quest;
            }
        }
        
        private void DrawItem(Rect position, SerializedProperty element)
        {
            BuildItemList();
            var names = items.Keys.ToList();
            int index = names.IndexOf(element.stringValue);
            
            EditorGUI.BeginProperty(position, new GUIContent("Item:"), element);

            int newIndex = EditorGUI.Popup(position,"Item:", index, names.ToArray());
            if (newIndex != index)
            {
                element.stringValue = names[newIndex];
            }

            EditorGUI.EndProperty();
        }

        void BuildItemList()
        {
            if (items != null) return;
            items = new Dictionary<string,InventoryItem>();
            foreach (InventoryItem item in Resources.LoadAll<InventoryItem>(""))
            {
                items[item.name] = item;
            }
        }
    }
}
