using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardInformation))]
public class CardInformationEditor : Editor
{
    private SerializedProperty _groups;
    private string _error;


    private void OnEnable()
    {
        _groups = serializedObject.FindProperty("Groups");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (_groups == null)
        {
            EditorGUILayout.HelpBox(
                "Not found Groups property",
                MessageType.Error
            );
            return;
        }

        _error = ValidateGroups();

        if (_error != null)
        {
            EditorGUILayout.HelpBox(_error, MessageType.Error);
            EditorGUILayout.PropertyField(_groups, true);
        }
        else
        {
            EnsureMissingGroups();

            foreach (CardElemental element in Enum.GetValues(typeof(CardElemental)))
            {
                DrawGroup(element);
            }
        }

        serializedObject.ApplyModifiedProperties();

    }

    private string ValidateGroups()
    {
        var seen = new System.Collections.Generic.HashSet<int>();
        for (int i = 0; i < _groups.arraySize; i++)
        {
            var group = _groups.GetArrayElementAtIndex(i);
            var element = group.FindPropertyRelative("CardElemental");
            int value = element.intValue;

            if (!Enum.IsDefined(typeof(CardElemental), value))
                return $"Group {i} contain elemental invalid.";

            if (!seen.Add(value))
                return $"There are many groups of the same {(CardElemental)value}. "
                                    + "Merge the data before continuing.";
        }
        return null;
    }

    private void EnsureMissingGroups()
    {
        foreach (CardElemental element
                        in Enum.GetValues(typeof(CardElemental)))
        {
            if (FindGroupIndex(element) >= 0)
                continue;

            int index = _groups.arraySize;
            _groups.InsertArrayElementAtIndex(index);

            var group = _groups.GetArrayElementAtIndex(index);

            group.FindPropertyRelative("CardElemental").intValue =
                (int)element;

            // Phần tử mới có thể sao chép dữ liệu phần tử trước.
            // Chỉ xóa danh sách của nhóm vừa thêm.
            group.FindPropertyRelative("CardInfomationEntries")
                 .ClearArray();
        }
    }

    private int FindGroupIndex(CardElemental element)
    {
        for (int i = 0; i < _groups.arraySize; i++)
        {
            var group = _groups.GetArrayElementAtIndex(i);
            var storedElement =
                group.FindPropertyRelative("CardElemental");

            if (storedElement.intValue == (int)element)
                return i;
        }

        return -1;
    }

    private void DrawGroup(CardElemental element)
    {
        int index = FindGroupIndex(element);
        var group = _groups.GetArrayElementAtIndex(index);
        var entries =
            group.FindPropertyRelative("CardInfomationEntries");

        group.isExpanded = EditorGUILayout.Foldout(
            group.isExpanded,
            element.ToString(),
            true);

        if (!group.isExpanded)
            return;

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(
            entries,
            new GUIContent("Cards"),
            true);

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
    }
}

