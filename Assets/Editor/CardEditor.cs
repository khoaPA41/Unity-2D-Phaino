using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    private const string ImagePathRoot = "Assets/Third-Party Assets/Card/ChatGpt/Card Image";
    private const string ImageElementPathRoot = "Assets/Third-Party Assets/Card/ChatGpt/Card Icon/Elemental";
    private const string ImageTypePathRoot = "Assets/Third-Party Assets/Card/ChatGpt/Card Icon/Type";

    private SerializedProperty _cardInformation;
    private SerializedProperty _cardNameId;
    private SerializedProperty _cardType;
    private SerializedProperty _cardElemental;
    private SerializedProperty _cardDetailImage;
    private SerializedProperty _cardElementalImage;
    private SerializedProperty _cardTypeImage;


    private string _message;
    private string _imageMessage;

    private void OnEnable()
    {
        _cardInformation = serializedObject.FindProperty("CardInformation");
        _cardNameId = serializedObject.FindProperty("CardNameId");
        _cardType = serializedObject.FindProperty("CardType");
        _cardElemental = serializedObject.FindProperty("CardElemental");
        _cardDetailImage = serializedObject.FindProperty("CardDetailImage");
        _cardElementalImage = serializedObject.FindProperty("CardElementalImage");
        _cardTypeImage = serializedObject.FindProperty("CardTypeImage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (serializedObject.isEditingMultipleObjects)
        {
            EditorGUILayout.LabelField($"Choosing {targets.Length} card assets");

            if (GUILayout.Button("Synchronize Image"))
                SyncSelectedArtwork();
            return;
        }

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
        // "CardDetailImage"
        );

        if (!string.IsNullOrEmpty(_imageMessage))
            EditorGUILayout.HelpBox(_imageMessage, MessageType.Error);

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

        ArtWork();
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

    private void ArtWork()
    {
        string folder;

        switch ((CardType)_cardType.intValue)
        {
            case CardType.Mighty:
                folder = "Mighty";
                break;
            case CardType.Rupture:
                folder = "Rupture";
                break;
            case CardType.Soul:
                folder = "Soul";
                break;
            default:
                _imageMessage = "Card type is invalid";
                return;
        }

        var id = (CardNameId)_cardNameId.intValue;
        var typeId = (CardType)_cardType.intValue;
        var elementId = (CardElemental)_cardElemental.intValue;

        string path = $"{ImagePathRoot}/{folder}/{id}.png";
        string typePath = $"{ImageTypePathRoot}/{typeId}.png";
        string elementPath = $"{ImageElementPathRoot}/{elementId}.png";

        Sprite result = null;
        Sprite type = null;
        Sprite element = null;

        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
        {
            if (!(asset is Sprite sprite)) continue;

            if (result != null)
            {
                _imageMessage = $"Image has many sprite, cannot choose: {path}.";
                return;
            }

            result = sprite;
        }

        if (result == null)
        {
            _imageMessage = $"Cannot find sprite {path}. Check file and import settings.";
        }
        //
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(typePath))
        {
            if (!(asset is Sprite sprite)) continue;

            if (type != null)
            {
                _imageMessage = $"Image has many sprite, cannot choose: {path}.";
                return;
            }

            type = sprite;
        }
        //
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(elementPath))
        {
            if (!(asset is Sprite sprite)) continue;

            if (element != null)
            {
                _imageMessage = $"Image has many sprite, cannot choose: {path}.";
                return;
            }

            element = sprite;
        }

        _cardDetailImage.objectReferenceValue = result;
        _cardElementalImage.objectReferenceValue = element;
        _cardTypeImage.objectReferenceValue = type;
        _imageMessage = null;
    }

    private void SyncSelectedArtwork()
    {
        int updated = 0;
        int failed = 0;

        foreach (var target in targets)
        {
            if (!(target is Card card)) continue;

            string folder;

            switch (card.CardType)
            {
                case CardType.Mighty:
                    folder = "Mighty";
                    break;
                case CardType.Rupture:
                    folder = "Rupture";
                    break;
                case CardType.Soul:
                    folder = "Soul";
                    break;
                default:
                    Debug.LogWarning(
                    $"{card.name}: loại bài không hợp lệ.", card);
                    failed++;
                    continue;
            }

            string path = $"{ImagePathRoot}/{folder}/{card.CardNameId}.png";
            string typePath = $"{ImageTypePathRoot}/{card.CardType}.png";
            string elementPath = $"{ImageElementPathRoot}/{card.CardElemental}.png";

            Sprite result = null;
            Sprite type = AssetDatabase.LoadAssetAtPath<Sprite>(typePath);
            Sprite element = AssetDatabase.LoadAssetAtPath<Sprite>(elementPath);

            bool multipleSprites = false;

            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (!(asset is Sprite sprite)) continue;

                if (result != null)
                {
                    multipleSprites = true;
                    break;
                }

                result = sprite;
            }

            if (result == null || multipleSprites)
            {
                Debug.LogWarning(
                $"{card.name}: Just one Sprite at {path}. "
                + "Keep older.",
                card);

                failed++;
                continue;
            }

            if (card.CardDetailImage == result && card.CardTypeImage == type && card.CardElementalImage == element) continue;

            Undo.RecordObject(card, "Sync Card Artwork");
            EditorUtility.SetDirty(card);
            card.CardDetailImage = result;
            card.CardTypeImage = type;
            card.CardElementalImage = element;
            updated++;
        }

        Debug.Log(
            $"Sync Image: updated {updated}, error {failed}. "
            + "Card has right image will keep.");

        serializedObject.Update();
        Repaint();
    }
}
