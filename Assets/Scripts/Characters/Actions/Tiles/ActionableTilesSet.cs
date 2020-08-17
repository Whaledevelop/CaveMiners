using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Обработчик событий, связанных с взаимодействием с тайлами. По сути, должен быть RuntimeSet, но осложняет
/// связь с grid (может быть решено Request) и необходимость иметь рядом gameEventListener (можно разместить на пустом объекте)
/// Пока MonoBehaviour
/// </summary>
public class ActionableTilesSet : MonoBehaviour
{
    [Header("Доступные тайлы для взаимодействия")]
    [SerializeField] private List<ActionableTile> tilesActions;
    [SerializeField] private Grid grid;
    [SerializeField] private CharacterActionGameEvent tileWorkedOutEvent;

    // Тайлы, с которыми осуществляется взаимодействие
    private List<ActionableTile> activeTiles = new List<ActionableTile>();

    // Метод вызываемый из CharacterActionEventListener
    public void OnTileAction(CharacterAction actionData)
    {        
        // Получаем текущий активный тайл. Может быть один и тот же для нескольких персонажей
        ActionableTile activeTile = activeTiles.Find(tile =>
        {
            return tile.actionData.endPosition == actionData.endPosition;
        });

        // Если такового нет, значит действие новое - инициализируем тайл
        if (activeTile == null)
        {
            ActionableTile newActiveTile = tilesActions.Find(tile => tile.State == actionData.state);       
            if (newActiveTile != null)
            {
                ActionableTile instantiatedTile = Instantiate(newActiveTile);
                instantiatedTile.Init(actionData, grid.WorldToCell(Vector3Int.FloorToInt(actionData.endPosition)));
                instantiatedTile.OnWorkedOut += OnTileWorkedOut;
                activeTiles.Add(instantiatedTile);
            }
        }
        // Наносим урон тайлу
        if (activeTile != null)
        {
            activeTile.Damage(actionData.SkillValue);
        }
    }

    public void OnTileWorkedOut(ActionableTile ActionableTile)
    {
        activeTiles.Remove(ActionableTile);
        tileWorkedOutEvent.Raise(ActionableTile.actionData);

    }
}
