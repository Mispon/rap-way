using System;
using Game.Pages.Training.Tabs;
using UnityEngine;
using UnityEngine.UI;
using Utils.Carousel;

namespace Game.Pages.Training
{
    /// <summary>
    /// Главная страница тренировок персонажа
    /// </summary>
    public class TrainingMainPage : Page
    {
        [Header("Контролы")]
        [SerializeField] private Carousel tabsCarousel;
        [SerializeField] private TrainingTab[] tabs;
        [SerializeField] private Text expLabel;

        private int _tabIndex;

        private void Start()
        {
            tabsCarousel.onChange += OnTabChanged;

            foreach (var tab in tabs)
            {
                tab.Init();
                tab.onStartTraining += ApplyTraining;
            }
        }

        /// <summary>
        /// Открывает страницу и указанную вкладку
        /// </summary>
        public void OpenPage(int tabIndex)
        {
            _tabIndex = tabIndex;
            Open();
            OpenTab(tabIndex);
        }

        /// <summary>
        /// Обработчик изменения индекса вкладки 
        /// </summary>
        private void OnTabChanged(int index)
        {
            OpenTab(index);
        }

        /// <summary>
        /// Запускает тренировку 
        /// </summary>
        private void ApplyTraining(Func<int> training)
        {
            int cost = training.Invoke();
            PlayerManager.Data.Exp -= cost;
            
            DisplayExp();
            RefreshTab();
        }

        /// <summary>
        /// Открывает вкладку по индексу 
        /// </summary>
        private void OpenTab(int index)
        {
            _tabIndex = index;

            for (var i = 0; i < tabs.Length; i++)
            {
                var tab = tabs[i];
                tab.Toggle(i == index);
            }
        }

        /// <summary>
        /// Обновляет текущую вкладку методом её переоткрытия
        /// </summary>
        private void RefreshTab()
        {
            OpenTab(_tabIndex);
        }
        
        /// <summary>
        /// Отображает текущее количество очков опыта
        /// </summary>
        private void DisplayExp() => expLabel.text = GetLocale("exp_value", PlayerManager.Data.Exp);

        /// <summary>
        /// Вызывается перед открытием страницы
        /// </summary>
        protected override void BeforePageOpen()
        {
            DisplayExp();
        }

        protected override void AfterPageOpen()
        {
            tabsCarousel.SetIndex(_tabIndex);
        }

        protected override void AfterPageClose()
        {
            tabsCarousel.SetIndex(0);
        }

        private void OnDestroy()
        {
            tabsCarousel.onChange -= OnTabChanged;
            foreach (var tab in tabs)
            {
                tab.onStartTraining -= ApplyTraining;
            }
        }
    }
}