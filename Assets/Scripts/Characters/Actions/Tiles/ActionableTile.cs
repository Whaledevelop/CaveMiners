using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Определенный тайл, с которым могут взаимодействовать персонажи (сразу несколько)
/// </summary>
[CreateAssetMenu(fileName = "ActionableTile", menuName = "ScriptableObjects/ActionableTile")]
public class ActionableTile : ScriptableObject
{
    [System.Serializable]
    public class TileDependOnHP
    {
        public float HP;
        public TileBase tile;
    }

    // Данные из эдитора - данные типа тайлов
    [Header("Состояние, работающее с тайлом")]
    [SerializeField] private CharacterActionState actionState;
    [Header("Список всех тайлмапов")]
    [SerializeField] private GenerativeTilemapsSet tilemapsSet;
    [Header("Код тайлмапа до действий с тайлом")]
    [SerializeField] private GenerativeTilemapCode initialTilemapCode;
    [Header("Код тайлмапа после выполнения действия с тайлом")]
    [SerializeField] private GenerativeTilemapCode destroyedTilemapCode;
    [Header("Тайлы при определенном HP")]
    [SerializeField] private List<TileDependOnHP> tilesSet;
    [Header("Начальное \"здоровье\"")]
    [SerializeField] private float initialHP;

    // Данные экземпляра - определенное действие с определенным тайлом
    [NonSerialized] public CharacterAction actionData; // Выполняемое действие
    public Action<ActionableTile> OnWorkedOut;         // Событие, вызываемое при выполнении действия
    private TileBase currentTile;                      // Ссылка на тайл
    private float HP;                                  // Текущее здоровье
    private Vector3Int PointPosition;                   // Позиция тайла
    private GenerativeTilemap initialTilemap;          // Тайлмап до действий с тайлом
    private GenerativeTilemap destroyedTilemap;        // Тайлмап после выполнения действия с тайлом"

    public CharacterActionState State => actionState;

    /// <summary>
    /// Инициализация тайла происходит при начала взаимодействия персонажа с клеткой
    /// </summary>
    /// <param name="actionData">Данные действия</param>
    /// <param name="PointPosition">Интовое значение, где выполняется действие</param>
    public void Init(CharacterAction actionData, Vector3Int PointPosition)
    {
        this.actionData = actionData;
        this.PointPosition = PointPosition;
        initialTilemap = tilemapsSet.FindByCode(initialTilemapCode);
        destroyedTilemap = tilemapsSet.FindByCode(destroyedTilemapCode);
        HP = initialHP;
    }

    /// <summary>
    /// Нанести урон тайлу
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        HP -= damage;
        // Меняем тайл в зависимости от HP
        TileDependOnHP neededTile = tilesSet.LastOrDefault(tileData => tileData.HP >= HP);
        if (neededTile != null && neededTile.tile != currentTile)
        {
            currentTile = neededTile.tile;
            initialTilemap.SetTile(PointPosition, currentTile);
        }

        // Если здоровье на нуле, то действие окончено - меняем тайлмап и вызываем событие
        if (HP <= 0)
        {
            initialTilemap.SetTile(PointPosition, null);
            destroyedTilemap.SetTile(PointPosition, currentTile);
            OnWorkedOut?.Invoke(this);
        }
    }

    public override string ToString()
    {
        return actionState + " - " + PointPosition + " - " + HP;
    }
}
