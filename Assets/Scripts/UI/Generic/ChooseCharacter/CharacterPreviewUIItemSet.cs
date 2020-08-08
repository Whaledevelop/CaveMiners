using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CharacterPreviewUIItemSet", menuName = "Sets/CharacterPreviewUIItemSet")]
public class CharacterPreviewUIItemSet : RuntimeSet<CharacterPreview>
{
    [SerializeField] private CharacterPreview characterPreviewPrefab;

    private GameObject previewsParent;

    public CharacterPreview InstantiateItem(string spriteName)
    {
        CharacterPreview characterPreview = Items.Find(item => item.SpriteName == spriteName);
        if (characterPreview == null)
        {
            if (previewsParent == null)
            {
                previewsParent = new GameObject("CharactersPreviewsParent");
                previewsParent.transform.position = new Vector3(0, 0, -10);
            }
            characterPreview = Instantiate(characterPreviewPrefab, previewsParent.transform);
            characterPreview.Init(spriteName);

            Vector3 positionInGrid = characterPreview.transform.position;
            positionInGrid.x = characterPreview.SpriteSize.x * Items.Count;
            characterPreview.transform.position = positionInGrid;
            Add(characterPreview);
        }
        return characterPreview;
    }
}
