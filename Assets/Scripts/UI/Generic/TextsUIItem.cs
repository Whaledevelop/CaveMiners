using UnityEngine;
using UnityEngine.UI;

public class TextsUIItem : UIItem<string[]>
{
    [SerializeField] private Text[] texts;

    public override void Init(string[] setupData)
    {
        for (int i = 0; i < setupData.Length; i++)
        {
            texts[i].text = setupData[i];
        }
    }
}
