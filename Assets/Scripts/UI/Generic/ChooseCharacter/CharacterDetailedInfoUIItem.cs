using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailedInfoUIItem : CharacterUIItem
{
    [SerializeField] private Text characterHistory;

    public override void Init(CharacterInitialData setupData)
    {
        base.Init(setupData);
        characterHistory.text = setupData.description;
    }
}
