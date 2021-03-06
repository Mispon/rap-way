using Enums;
using Models.Info;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Game.Pages.Social.Tabs
{
    /// <summary>
    /// Вкладка пожертвований
    /// </summary>
    public class CharityTab : BaseSocialsTab
    {
        [SerializeField] private InputField messageInput;
        [SerializeField] private InputField fondInput;
        [SerializeField] private Text balance;
        [Space]
        [SerializeField] private Slider amountSlider;
        [SerializeField] private Text amountLabel;

        private int _amount;

        protected override void TabStart()
        {
            amountSlider.onValueChanged.AddListener(SetAmount);
        }

        /// <summary>
        /// Обработчик изменения величины пожертвования 
        /// </summary>
        private void SetAmount(float value)
        {
            _amount = (int) value;
            amountLabel.text = _amount.GetMoney();
        }

        /// <summary>
        /// Возвращает информацию соц. действия 
        /// </summary>
        protected override SocialInfo GetInfo()
        {
            return new SocialInfo
            {
                Type = SocialType.Charity,
                MainText = messageInput.text,
                AdditionalText = fondInput.text,
                CharityAmount = _amount
            };
        }

        /// <summary>
        /// Вызывается при открытии вкладки
        /// </summary>
        protected override void OnOpen()
        {
            SetSliderBorders();

            SetAmount(amountSlider.minValue);
            balance.text = $"{PlayerManager.Data.Money.GetMoney()}";
            fondInput.text = string.Empty;
            messageInput.text = string.Empty;
            
            base.OnOpen();
        }

        /// <summary>
        /// Проверяет условия запуска соц. действия
        /// </summary>
        protected override bool CheckStartConditions()
        {
            bool noCooldown = base.CheckStartConditions();
            return noCooldown && PlayerManager.Data.Money >= GameManager.Instance.Settings.MinBalanceForCharity;
        }

        /// <summary>
        /// Устанавливает предельные границы пожертвования
        /// </summary>
        private void SetSliderBorders()
        {
            int money = PlayerManager.Data.Money;

            int min = 0;
            int max = 0;

            if (money >= GameManager.Instance.Settings.MinBalanceForCharity)
            {
                min = Mathf.Max(1, money / 100);
                max = min * 10;
            }

            amountSlider.minValue = min;
            amountSlider.maxValue = max;
        }
    }
}