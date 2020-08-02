﻿using Core;
using Enums;
using Game.UI.GameScreen;
using Models.Info;
using Models.Info.Production;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.Pages.Track
{
    /// <summary>
    /// Страница настройки трека
    /// </summary>
    public class TrackSettingsPage : Page
    {
        [Header("Контроллы")]
        [SerializeField] private InputField trackNameInput;
        [SerializeField] private Switcher themeSwitcher;
        [SerializeField] private Switcher styleSwitcher;
        [SerializeField] private Button startButton;

        [Header("Страница разработки")]
        [SerializeField] private BaseWorkingPage workingPage;

        protected TrackInfo _track;

        private void Start()
        {
            trackNameInput.onValueChanged.AddListener(OnTrackNameInput);
            startButton.onClick.AddListener(CreateTrack);
        }
        
        /// <summary>
        /// Обработчик ввода названия трека 
        /// </summary>
        private void OnTrackNameInput(string value)
        {
            _track.Name = value;
        }

        /// <summary>
        /// Обработчик запуска работы над треком
        /// </summary>
        private void CreateTrack()
        {
            SoundManager.Instance.PlayClick();
            
            _track.Id = PlayerManager.GetNextProductionId<TrackInfo>();
            if (string.IsNullOrEmpty(_track.Name))
            {
                _track.Name = $"Track {_track.Id}";
            }

            _track.TrendInfo = new TrendInfo
            {
                Style = GetToneValue<Styles>(styleSwitcher),
                Theme = GetToneValue<Themes>(themeSwitcher)
            };
            
            workingPage.StartWork(_track);
            Close();
        }

        #region PAGE EVENTS

        protected override void BeforePageOpen()
        {
            _track = new TrackInfo();
            
            themeSwitcher.InstantiateElements(PlayerManager.GetPlayersThemes());
            styleSwitcher.InstantiateElements(PlayerManager.GetPlayersStyles());
            
            GameScreenController.Instance.HideProductionGroup();
        }

        protected override void AfterPageClose()
        {
            _track = null;
            
            trackNameInput.SetTextWithoutNotify(string.Empty);
            themeSwitcher.ResetActive();
            styleSwitcher.ResetActive();
        }

        #endregion
    }
}