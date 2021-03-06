﻿using System;
using Core;
using Data;
using Enums;
using Models.Info.Production;
using Models.Player;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Pages.Album
{
    /// <summary>
    /// Страница работы над альбомом
    /// </summary>
    public class AlbumWorkingPage : BaseWorkingPage
    {
        [Header("Идентификаторы прогресса работы")] 
        [SerializeField] private Text bitPoints;
        [SerializeField] private Text textPoints;

        [Header("Команда игрока")] 
        [SerializeField] private WorkPoints playerBitWorkPoints;
        [SerializeField] private WorkPoints playerTextWorkPoints;
        [SerializeField] private WorkPoints bitmakerWorkPoints;
        [SerializeField] private WorkPoints textwritterWorkPoints;
        [SerializeField] private Image bitmakerAvatar;
        [SerializeField] private Image textwritterAvatar;

        [Header("Данные")] 
        [SerializeField] private ImagesBank imagesBank;

        [Header("Страница результата")] 
        [SerializeField] private AlbumResultPage albumResult;

        private AlbumInfo _album;
        private bool _hasBitmaker;
        private bool _hasTextwritter;

        /// <summary>
        /// Начинает выполнение работы 
        /// </summary>
        public override void StartWork(params object[] args)
        {
            _album = (AlbumInfo) args[0];
            Open();
            RefreshWorkAnims();
        }

        /// <summary>
        /// Работа, выполняемая за один день
        /// </summary>
        protected override void DoDayWork()
        {
            GenerateWorkPoints();
            DisplayWorkPoints();
        }

        /// <summary>
        /// Обработчик перехода к странице результата
        /// </summary>
        private void ShowResultPage()
        {
            albumResult.Show(_album);
            Close();
        }

        /// <summary>
        /// Обработчик завершения работы
        /// </summary>
        protected override void FinishWork()
        {
            GameEventsManager.Instance.CallEvent(GameEventType.Album, ShowResultPage);
        }

        /// <summary>
        /// Возвращает длительность действия
        /// </summary>
        protected override int GetDuration()
        {
            return settings.AlbumWorkDuration;
        }

        /// <summary>
        /// Генерирует очки работы над альбомом
        /// </summary>
        private void GenerateWorkPoints()
        {
            SoundManager.Instance.PlayWorkPoint();

            var bitWorkPoints = CreateBitPoints(PlayerManager.Data);
            var textWorkPoints = CreateTextPoints(PlayerManager.Data);

            int addPoints = GoodsManager.Instance.GenerateAdditionalWorkPoints();
            int equipBonus = Convert.ToInt32(addPoints * 0.5f);

            _album.BitPoints += bitWorkPoints + equipBonus;
            _album.TextPoints += textWorkPoints + equipBonus;
        }

        /// <summary>
        /// Создает очки работы по биту 
        /// </summary>
        private int CreateBitPoints(PlayerData data)
        {
            var playersBitPoints = Random.Range(1, data.Stats.Bitmaking.Value + 1);
            playerBitWorkPoints.Show(playersBitPoints);

            var bitmakerPoints = 0;
            if (_hasBitmaker)
            {
                bitmakerPoints = Random.Range(1, data.Team.BitMaker.Skill.Value + 1);
                bitmakerWorkPoints.Show(bitmakerPoints);
            }

            return playersBitPoints + bitmakerPoints;
        }

        /// <summary>
        /// Создает очки работы по тексту 
        /// </summary>
        private int CreateTextPoints(PlayerData data)
        {
            var playersTextPoints = Random.Range(1, data.Stats.Vocobulary.Value + 1);
            playerTextWorkPoints.Show(playersTextPoints);

            var textwritterPoints = 0;
            if (_hasTextwritter)
            {
                textwritterPoints = Random.Range(1, data.Team.TextWriter.Skill.Value + 1);
                textwritterWorkPoints.Show(textwritterPoints);
            }

            return playersTextPoints + textwritterPoints;
        }

        /// <summary>
        /// Обновляет значение рабочих очков в интерфейсе
        /// </summary>
        private void DisplayWorkPoints()
        {
            bitPoints.text = _album.BitPoints.ToString();
            textPoints.text = _album.TextPoints.ToString();
        }

        protected override void BeforePageOpen()
        {
            _hasBitmaker = TeamManager.IsAvailable(TeammateType.BitMaker);
            _hasTextwritter = TeamManager.IsAvailable(TeammateType.TextWriter);

            bitmakerAvatar.sprite = _hasBitmaker ? imagesBank.BitmakerActive : imagesBank.BitmakerInactive;
            textwritterAvatar.sprite = _hasTextwritter ? imagesBank.TextwritterActive : imagesBank.TextwritterInactive;
        }

        protected override void BeforePageClose()
        {
            base.BeforePageClose();

            bitPoints.text = textPoints.text = "0";
            _album = null;
        }
    }
}