using UnityEngine;
using UnityEditor;

/// <summary>
/// По сути своей enum, но такой способ лучше подходит для глобальных енамов, число которых может сильно разрастись.
/// При работе с енамами возникают сложности, когда их очень много, можно сломать ссылки добавляя новые. С скриптаблей такого не будет.
/// Поэтому енамы использую только для небольших перечислений.
/// Вдохновлено https://www.youtube.com/watch?v=raQ3iHhE_Kk&list=PLB8F3398G-ZsPa0piiMEglkbLSyRggTf8
/// </summary>
[CreateAssetMenu(fileName = "TilemapCode", menuName = "ScriptableObjects/GenerativeTilemapCode")]
public class GenerativeTilemapCode : ScriptableObject { }