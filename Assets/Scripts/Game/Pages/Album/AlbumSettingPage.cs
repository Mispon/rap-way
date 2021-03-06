﻿using System;
using System.Linq;
using Core;
using Data;
using Enums;
using Game.UI.GameScreen;
using Localization;
using Models.Info;
using Models.Info.Production;
using Models.Player;
using UnityEngine;
using UnityEngine.UI;
using Utils.Carousel;
using Utils.Extensions;
using EventType = Core.EventType;

namespace Game.Pages.Album
{
    /// <summary>
    /// Страница настройки альбома
    /// </summary>
    public class AlbumSettingPage : Page
    {
        [Header("Контроллы")] 
        [SerializeField] private InputField albumNameInput;
        [SerializeField] private Carousel styleCarousel;
        [SerializeField] private Carousel themeCarousel;
        [SerializeField] private Button startButton;
        [Space] 
        [SerializeField] protected Text bitSkill;
        [SerializeField] protected Text textSkill;
        [SerializeField] private Image bitmakerAvatar;
        [SerializeField] private Image textwritterAvatar;
        
        [Header("Страница разработки")] 
        [SerializeField] private BaseWorkingPage workingPage;

        [Header("Данные")] 
        [SerializeField] private ImagesBank imagesBank;

        private AlbumInfo _album;

        private void Start()
        {
            albumNameInput.onValueChanged.AddListener(OnAlbumNameInput);
            startButton.onClick.AddListener(CreateAlbum);
        }

        /// <summary>
        /// Обработчик ввода названия альбома 
        /// </summary>
        private void OnAlbumNameInput(string value)
        {
            _album.Name = value;
        }

        /// <summary>
        /// Обработчик запуска работы над треком
        /// </summary>
        private void CreateAlbum()
        {
            SoundManager.Instance.PlayClick();

            _album.Id = PlayerManager.GetNextProductionId<AlbumInfo>();
            if (string.IsNullOrEmpty(_album.Name))
            {
                _album.Name = $"Album {_album.Id}";
            }

            _album.TrendInfo = new TrendInfo
            {
                Style = styleCarousel.GetValue<Styles>(),
                Theme = themeCarousel.GetValue<Themes>()
            };

            workingPage.StartWork(_album);
            Close();
        }

        /// <summary>
        /// Инициализирует карусели актуальными значениями 
        /// </summary>
        private void SetupCarousel(PlayerData data)
        {
            var styleProps = data.Styles.Select(ConvertToCarouselProps).ToArray();
            styleCarousel.Init(styleProps);
            var themeProps = data.Themes.Select(ConvertToCarouselProps).ToArray();
            themeCarousel.Init(themeProps);
        }

        /// <summary>
        /// Конвертирует элемент перечисление в свойство карусели 
        /// </summary>
        private CarouselProps ConvertToCarouselProps<T>(T value) where T : Enum
        {
            string text = LocalizationManager.Instance.Get(value.GetDescription());
            Sprite icon = value.GetType() == typeof(Themes)
                ? imagesBank.ThemesActive[Convert.ToInt32(value)]
                : imagesBank.StyleActive;

            return new CarouselProps {Text = text, Sprite = icon, Value = value};
        }

        /// <summary>
        /// Отображает состояние членов команды
        /// </summary>
        private void SetupTeam()
        {
            bitmakerAvatar.sprite = TeamManager.IsAvailable(TeammateType.BitMaker)
                ? imagesBank.BitmakerActive
                : imagesBank.BitmakerInactive;
            textwritterAvatar.sprite = TeamManager.IsAvailable(TeammateType.TextWriter)
                ? imagesBank.TextwritterActive
                : imagesBank.TextwritterInactive;
        }

        /// <summary>
        /// Показывает текущий суммарный скилл команды 
        /// </summary>
        private void DisplaySkills(PlayerData data)
        {
            int playerBitSkill = data.Stats.Bitmaking.Value;
            int bitmakerSkill = TeamManager.IsAvailable(TeammateType.BitMaker)
                ? data.Team.BitMaker.Skill.Value
                : 0;
            bitSkill.text = $"{playerBitSkill + bitmakerSkill}";

            int playerTextSkill = data.Stats.Vocobulary.Value;
            int textwritterSkill = TeamManager.IsAvailable(TeammateType.TextWriter)
                ? data.Team.TextWriter.Skill.Value
                : 0;
            textSkill.text = $"{playerTextSkill + textwritterSkill}";
        }

        /// <summary>
        /// Сбрасывает состояние членов команды и суммарный скилл команды
        /// </summary>
        private void DropTeam(object[] args)
        {
            bitmakerAvatar.sprite = imagesBank.BitmakerInactive;
            textwritterAvatar.sprite = imagesBank.TextwritterInactive;

            var playerStats = PlayerManager.Data.Stats;
            bitSkill.text = $"{playerStats.Bitmaking.Value}";
            textSkill.text = $"{playerStats.Vocobulary.Value}";
        }
        
        protected override void BeforePageOpen()
        {
            _album = new AlbumInfo();

            var data = PlayerManager.Data;
            SetupCarousel(data);
            SetupTeam();
            DisplaySkills(data);

            EventManager.AddHandler(EventType.UncleSamsParty, DropTeam);
            GameScreenController.Instance.HideProductionGroup();
        }

        protected override void AfterPageClose()
        {
            EventManager.RemoveHandler(EventType.UncleSamsParty, DropTeam);

            _album = null;
            albumNameInput.SetTextWithoutNotify(string.Empty);
        }
    }
}