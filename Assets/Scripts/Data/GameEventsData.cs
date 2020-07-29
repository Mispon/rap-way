using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Data
{
    /// <summary>
    /// Данные о событиях при создании Production
    /// </summary>
    [CreateAssetMenu(fileName = "GameEventsData", menuName = "Data/Game Events")]
    public class GameEventsData: ScriptableObject
    {
        private readonly Dictionary<GameEventType, GameEventInfo[]> _gameEventInfosCollection = new Dictionary<GameEventType, GameEventInfo[]>();

        [SerializeField] private GameEventInfo[] trackGameEventInfos;
        [SerializeField] private GameEventInfo[] albumGameEventInfos;
        [SerializeField] private GameEventInfo[] clipGameEventInfos;
        [SerializeField] private GameEventInfo[] concertGameEventInfos;

        public void Initialize()
        {
            _gameEventInfosCollection.Add(GameEventType.Track, trackGameEventInfos);
            _gameEventInfosCollection.Add(GameEventType.Album, albumGameEventInfos);
            _gameEventInfosCollection.Add(GameEventType.Clip, clipGameEventInfos);
            _gameEventInfosCollection.Add(GameEventType.Concert, concertGameEventInfos);
        }

        /// <summary>
        /// Возвращает случайное игровое событие
        /// Возвращает null, если не найдено событий этого типа
        /// </summary>
        public GameEventInfo GetRandomInfo(GameEventType type)
        {
            if (!_gameEventInfosCollection.ContainsKey(type))
                return null;

            return _gameEventInfosCollection[type].GetRandom();
        }
    }

    /// <summary>
    /// Данные конкретного события: выводимая пользователю информация и набор данных решений
    /// </summary>
    [Serializable]
    public class GameEventInfo
    {
        [Tooltip("UI события")]
        public GameEventUi SituationUi;
        [ArrayElementTitle("DecisionType"), Tooltip("Набор данных, описывающих решение")]
        public GameEventDecision[] gameEventDecisions;

        /// <summary>
        /// Возвращает случайные данные решения по типу
        /// </summary>
        public GameEventDecision GetRandomDecision(GameEventDecisionType decisionType) 
            => gameEventDecisions.GetRandom(decisionType);
    }

    /// <summary>
    /// Данные решения: 
    /// </summary>
    [Serializable]
    public class GameEventDecision
    {
        [Tooltip("Тип решения")]
        public GameEventDecisionType DecisionType;
        [Tooltip("Изменение метрик игрока в связи с выбором этого решения")]
        public MetricsIncome MetricsIncome;
        [Tooltip("UI решения")]
        public GameEventUi DecisionUi;
    }
    
    /// <summary>
    /// Информация, выводимая на экран, для описания ситуации или решения
    /// </summary>
    [Serializable]
    public struct GameEventUi
    {
        public string Description;
        public Sprite Background;
    }

    /// <summary>
    /// Набор данных об изменнии метрик в связи с принятым решением
    /// </summary>
    [Serializable]
    public struct MetricsIncome
    {
        public int Money;
        public int Fans;
        public int Hype;
        public int Experience;
    }

    public static partial class Extensions
    {
        /// <summary>
        /// Возвращает случайное событие из набора. Если набор пусто, то возвращает null
        /// </summary>
        public static GameEventInfo GetRandom(this GameEventInfo[] array)
            => array.Length == 0 ? null : array[Random.Range(0, array.Length)];

        /// <summary>
        /// Возвращает случайное решение из типизированного набора решений, если таковые имеются.
        /// </summary>
        public static GameEventDecision GetRandom(this GameEventDecision[] gameEventDecisions, GameEventDecisionType decisionType)
        {
            var typedDecisions = gameEventDecisions.Where(el => el.DecisionType == decisionType).ToArray();
            if (typedDecisions.Length == 0)
                throw new RapWayException($"Нет ни одного решения типа \"{decisionType}\"");

            return typedDecisions[Random.Range(0, typedDecisions.Length)];
        }
    }
}