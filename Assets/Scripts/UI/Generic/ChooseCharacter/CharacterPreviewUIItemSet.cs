using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CharacterPreviewUIItemSet", menuName = "Sets/CharacterPreviewUIItemSet")]
public class CharacterPreviewUIItemSet : RuntimeSet<CharacterPreviewUIItem>
{
    [SerializeField] private CharacterPreviewUIItem characterPreviewPrefab;

    private GameObject previewsParent;

    public CharacterPreviewUIItem InstantiateItem(CharacterInitialData characterData)
    {
        if (previewsParent == null)
        {
            previewsParent = new GameObject("CharactersPreviewsParent");
            previewsParent.transform.position = new Vector3(0, 0, -10);
        }
        CharacterPreviewUIItem characterPreview = Instantiate(characterPreviewPrefab, previewsParent.transform);
        characterPreview.Init(characterData);

        Vector3 positionInGrid = characterPreview.transform.position;
        positionInGrid.x = characterPreview.SpriteSize.x * Items.Count;
        characterPreview.transform.position = positionInGrid;
        Add(characterPreview);
        return characterPreview;
    }
}
