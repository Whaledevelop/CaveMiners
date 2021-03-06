﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Действие персонажа - по сути реализация определенного состояния (CharacterActionState) на определенной клетке (endPosition)
/// </summary>
[System.Serializable]
public class CharacterAction
{
    #region Ссылки для выполнения действия

    public CharacterTaskManager taskManager;
    public CharacterSkillsManager skillsManager;

    #endregion

    public CharacterActionState state; // Реализуемое состояние

    public Vector2 startPosition;          // Позиция, от которой выполняется действия
    public Vector2 endPosition;            // Позиция, к которой выполняется действие
    public Vector2 actionDirection;        // Направленность действия

    public Vector3Int endCellPosition;     // Интовое значение окончательной позиция, переведенная из World в Cell

    private CharacterSkill actionSkill;    // Умение персонажа в реализации данного действия

    public CharacterSkill ActionSkill
    { 
        get
        {
            if (actionSkill == null)
                actionSkill = skillsManager.GetSkill(state.skillCode);

            return actionSkill;
        }

    }
    public int SkillValue => ActionSkill.Value;

    public CharacterAction(CharacterTaskManager taskManager, CharacterSkillsManager skillsManager, CharacterActionState state, Vector2 startPosition, Vector2 endPosition, Vector2 actionDirection)
    {
        this.taskManager = taskManager;
        this.skillsManager = skillsManager;
        this.state = state;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.actionDirection = actionDirection;
    }

    /// <summary>
    /// Обновление скилла при выполнении действия
    /// </summary>
    public void LearnSkill()
    {
        ActionSkill.LearnSkill();
    }
}
