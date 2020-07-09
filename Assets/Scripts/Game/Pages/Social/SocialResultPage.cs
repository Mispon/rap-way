﻿using Core;
using Game.Analyzers;
using UnityEngine;
using UnityEngine.UI;
using Models.Info;

namespace Game.Pages.Social
{
    /// <summary>
    /// Страница результата социального действия
    /// </summary>
    public class SocialResultPage : Page
    {
        [Header("Страница настроек")]
        [SerializeField] protected SocialSettingsPage settingsPage;

        [Header("Анализатор")] 
        [SerializeField] protected SocialAnalyzer analyzer;
        
        protected SocialInfo _social;
        
        public void ShowPage(SocialInfo social)
        {
            _social = social;
            Open();
        }

        /// <summary>
        /// Заполняет данные страницы результата социального действия
        /// </summary>
        protected virtual void DisplayResult() {}
        
        /// <summary>
        /// Сохраняет результат социального действия
        /// </summary>
        private static void SaveResult(SocialInfo social)
        {
            PlayerManager.Instance.SpendMoney(social.CharityMoney);
            PlayerManager.Instance.AddHype(social.HypeIncome);
        }
        
        /// <summary>
        /// Установка параметров деактивации
        /// </summary>
        private void OnSocialCooldownStart(SocialInfo social)
        {
            TimeManager.Instance.onDayLeft += social.Activity.OnDayLeft;
            social.Activity.SetDisable(social.Data.Cooldown, ()=> { OnSocialCooldownEnd(social); });
        }

        /// <summary>
        /// Активация по окончанию кулдауна
        /// </summary>
        private void OnSocialCooldownEnd(SocialInfo social)
        {
            TimeManager.Instance.onDayLeft -= social.Activity.OnDayLeft;
            settingsPage.SetActiveAction(social.Data.Type, true);
        }

        #region PAGE CALLBACKS
        protected override void BeforePageOpen()
        {
            analyzer.Analyze(_social);
            SaveResult(_social);
            DisplayResult();
        }

        protected override void BeforePageClose()
        {
            OnSocialCooldownStart(_social);
        }

        protected override void AfterPageClose()
        {
            _social = null;
        }
        #endregion
    }
}
