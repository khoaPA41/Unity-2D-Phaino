using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    private SerializedProperty _cardInformation;
    private SerializedProperty _cardNameId;
    private SerializedProperty _cardType;
    private SerializedProperty _cardElemental;

    private string _message;

    private void OnEnable()
    {
        _cardInformation = serializedObject.FindProperty("CardInformation");
        _cardNameId = serializedObject.FindProperty("CardNameId");
        _cardType = serializedObject.FindProperty("CardType");
        _cardElemental = serializedObject.FindProperty("CardElemental");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_cardInformation);

        bool sourceChanged = EditorGUI.EndChangeCheck();

        var information = _cardInformation.objectReferenceValue as CardInformation;

        if (sourceChanged) _message = null;

        if (information == null)
        {
            EditorGUILayout.HelpBox(
                      "Let's attach asset CardInformation.",
                MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
            return;
        }

        DrawIdentity(information, sourceChanged);


        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.PropertyField(_cardType);
            EditorGUILayout.PropertyField(_cardElemental);
        }


        DrawValidation(information);

        if (!string.IsNullOrEmpty(_message))
            EditorGUILayout.HelpBox(_message, MessageType.Error);

        EditorGUILayout.Space();

        DrawPropertiesExcluding(
            serializedObject,
            "m_Script",
            "CardNameId",
            "CardInformation",
            "CardNameId",
            "CardType",
            "CardElemental"
        );

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Change name by Card ID"))
        {
            CardNameId id = (CardNameId)_cardNameId.intValue;
            RenameCardAsset(id.ToString());
        }
    }

    private void DrawIdentity(CardInformation information, bool sourceChanged)
    {
        var currentId = (CardNameId)_cardNameId.intValue;

        EditorGUI.BeginChangeCheck();
        var selectedId = (CardNameId)EditorGUILayout.EnumPopup(
            "Card Name", currentId);
        bool idChanged = EditorGUI.EndChangeCheck();

        bool syncRequested = GUILayout.Button("Synchronize type and elemental");

        if (!idChanged && !sourceChanged && !syncRequested)
            return;

        if (!information.TryGetInformation(
            selectedId, out var type, out var element))
        {
            // Không ghi đè ID, loại hoặc hệ khi tra cứu thất bại.
            _message =
                $"Cannot get information {selectedId}: "
                + "ID has not been declared or is duplicated in the source table.";
            return;
        }

        _cardNameId.intValue = (int)selectedId;
        _cardType.intValue = (int)type;
        _cardElemental.intValue = (int)element;


        _message = null;
    }

    private void DrawValidation(CardInformation information)
    {
        var id = (CardNameId)_cardNameId.intValue;

        if (!information.TryGetInformation(
            id, out var expectedType, out var expectedElement))
        {
            EditorGUILayout.HelpBox(
                "The current ID doesn't have unique information in the original table. "
                + "The type and elemental being saved haven't been confirmed as valid yet.",
                MessageType.Error);
            return;
        }

        if (_cardType.intValue != (int)expectedType
            || _cardElemental.intValue != (int)expectedElement)
        {
            EditorGUILayout.HelpBox(
                "The type or elemental is different from the original table. "
                + "Click Synchronize type and elemental to update.",
                MessageType.Warning);
        }
    }

    private void RenameCardAsset(string newName)
    {
        Card card = (Card)target;
        var path = AssetDatabase.GetAssetPath(card);
        if (string.IsNullOrEmpty(path))
        {
            _message = "Card chưa được lưu thành asset.";
            return;
        }
        string error = AssetDatabase.RenameAsset(path, newName);
        _message = string.IsNullOrEmpty(error) ? null : error;

        card.name = newName;
        EditorUtility.SetDirty(card);
        AssetDatabase.SaveAssets();
        _message = null;
    }

}
