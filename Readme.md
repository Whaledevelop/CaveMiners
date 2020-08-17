# CaveMiners 0.1
2D игра по разработке пещер на Unity. Бесконечное прохождение с автоматическим усложнением уровней. Цель каждого уровня - докопаться до перехода на следующий уровень, который изначально не видно, путь надо разведать. В начале игры есть определенный запас денег. Он тратися на зарплату персонажам. Игра кончается, если кончаются деньги. Добываются деньги из месторождений ресурсов, которые также нужно находить по уровню. Для выполнения задачи у игрока есть три персонажа, которые он выбирает из доступных перед стартом. Персонажи развиваются по мере выполнения соответствующих действий.

## Версия Unity - 2020.1

## Механики
* A* алгоритм поиска пути, основанный на сравнении приоритета клеток (приоритет зависит от выполняемого действия). В нем есть много недочетов и лишних итераций, буду оптимизировать.
* Архитектура по Single Responsibilty Principle, реализованная на ScriptableObjects. Вдохновлено https://www.youtube.com/watch?v=raQ3iHhE_Kk&list=PLB8F3398G-ZsPa0piiMEglkbLSyRggTf8
* Использование tilemap, создание RuleTile из 2D extras, динамическое изменение
* Новая InputSystem, т.к. в планах несколько девайсов
* Базовая процедурная генерация уровня на основе правил. Пока есть только базовые, но создан гибкий интерфейс для создания новых
* 2D анимации с использование 2D rigging. Работа с skinning editor для своих персонажей, нарисованных в piskel (вдохновлены https://www.kenney.nl/assets)
* Базовые шейдеры для выделения персонажа
* Два режима камеры : свободное передвижение в пределах уровня, прикрепленная к персонажу
* Мультиуровневая система с автоматическим усложнением и увеличением размера уровня, вдохновлено Roguelike проектом от Unity
* "Туман войны", развеивающийся по мере продвижения персонажей (развеивание зависит от таланта персонажа)
* Система талантов персонажей, влиющих на эффективность персонажа при выполнении действий. Активные таланты : Раскопка, Добыча, Передвижение. Пассивные таланты : Разведка местности
* Автоматические улучшение некоторых талантов при выполнении соответствующих действий


### Что планируется в 0.2
* Поддержка для WebGL (сейчас веб-версия не выдерживает алгоритм поиска пути, выдает ошибку с переполнением памяти, нужно оптимизировать алгоритм)
* Поддержка для Andoid
* UI для описания механик игры внутри самой игры
* UI в процессе игры : данные персонажа, окно быстрого доступа, данные по финансам

У меня много планов по данной игре, в том числе добавление боевой системы (враги и боевой навык у персонажей) и доработка визуальной части (бОльшее разнообразие тайлов, анимированные тайлы, пещеры, эффекты для ресурсов, нормальная обводка персонажа) и т.д.