using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Определенная промежуточная точка в пути
/// </summary>
public class PathPoint
{
    #region Определяются при инициализации
    // Состояние, которое должно быть реализовано на точке
    public CharacterActionState state; 
    // Позиция точки, из которой была найдена текущая. Не подходит "предыдущая" или "следующая", т.к. поиск
    // может производиться от цели к персонажу, а дальнейшие действия - от персонажа к цели. Т.е. предыдущая точка может стать следующей
    public Vector2 parentPosition;      
    // Нормализованный вектор - направление из точки, от которой была найдена текущая
    public Vector2 axisFromParent;
    public Vector2 PointPosition => parentPosition + axisFromParent;
    public string Name => state.stateName;
    public int Priority => state.priority;

    #endregion

    #region Определяются при поиске пути    

    // Позиция в пути, где индекс - это уровень вложенности, а значение - это индекс среди точек данного уровня
    public List<int> positionInPath = new List<int>();

    // Точка, из которой была найдена текущая точка
    private PathPoint parentPoint;

    // Точки, найденные из текущей. Опять же, это не "следующие" и не "предыдущие"
    private List<PathPoint> nestedPoints = new List<PathPoint>();

    // Уровень вложенности - это, по сути, сколько точек от целевой до текущей, т.к. один уровень вложенности - это 
    // все клетки одинаковой удаленности от целевой точки
    public int NestLevel => positionInPath.Count;

    #endregion

    #region Определяются при нахождении пути

    // Позиция точки, которая ближе к персонажу. Она может быть как родительской (из неё найдена текущая), т.к. и унаследованной (найдена из текущей)
    // в зависимости от направления поиска
    public Vector2 closerToCharacterPointPosition;

    #endregion

    public PathPoint(CharacterActionState state, Vector2 parentPosition, Vector2 axisFromParent)
    {
        this.state = state;
        this.parentPosition = parentPosition;
        this.axisFromParent = axisFromParent;
    }

    /// <summary> Получить цепочку от целевой точки до текущей </summary>
    public void GetPointWithAllParents(ref List<PathPoint> parentPoints)
    {
        parentPoints.Add(this);
        if (parentPoint != null)
            parentPoint.GetPointWithAllParents(ref parentPoints);
    }

    /// <summary>
    /// Добавление точек в другую точку, находящуюся в определенной позиции в пути
    /// </summary>
    public void AddPathsToPositionInPath(List<int> positionInPath, List<PathPoint> paths)
    {
        // Если это нужная точка (точка-родитель), то добавляем в неё нужные точки, при этом помечая их позицию в пути
        if (this.positionInPath == positionInPath) 
        {
            for (int i = 0; i < paths.Count; i++)
            {
                paths[i].positionInPath = new List<int>(this.positionInPath) { i };
                paths[i].parentPoint = this;
            }
            nestedPoints.AddRange(paths);
        }
        // Если уровень вложенности меньше, то проверяем, является ли данная точка предком нужной
        else if (this.positionInPath.Count < positionInPath.Count)
        {
            for (int i = 0; i < this.positionInPath.Count; i++)
            {
                // Отсекаем другие ветки поиска пути
                if (this.positionInPath[i] != positionInPath[i])
                    return;
            }
            // Рекурсивно ищем по всему древу пути
            foreach (PathPoint availablePath in nestedPoints)
            {
                availablePath.AddPathsToPositionInPath(positionInPath, paths);
            }
        }
    }

    /// <summary>
    /// Производим поиск по уровню вложенности. Это нужно для того, чтобы производить поиск не ветками (т.е. от одной точки во все направления),
    /// а слоями. Сначала все точки удаленностью в 1 клетку, дальше 2 и т.д.
    /// </summary>
    public List<PathPoint> GetPointsWithNestLevel(int NestLevel)
    {
        List<PathPoint> points = new List<PathPoint>();
        if (this.NestLevel == NestLevel)
        {
            points.Add(this);
        }
        else if (this.NestLevel < NestLevel)
        {
            for (int i = 0; i < nestedPoints.Count; i++)
            {
                points.AddRange(nestedPoints[i].GetPointsWithNestLevel(NestLevel));
            }
        }
        return points;
    }

    public override string ToString()
    {
        return Name + " - " + PointPosition;
    }
}