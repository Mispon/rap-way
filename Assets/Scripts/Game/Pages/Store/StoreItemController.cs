using System;
using Data;
using Enums;
using Game.UI;
using Localization;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Game.Pages.Store
{
    /// <summary>
    /// Класс управkения UI-элементами конкретного товара
    /// </summary>
    public class StoreItemController : MonoBehaviour
    {
        [Header("Настройки слота иконки")]
        [SerializeField] private RectTransform itemIconBackgroundRect;
        [SerializeField] private RectTransform itemIconRect;
        [SerializeField] private float marginPadding = 20f;

        [Header("Информация")]
        [SerializeField] private Button itemBtn;
        [SerializeField] private Image iconImg;
        [SerializeField] private Text typeTxt;
        [SerializeField] private Price price;
        [SerializeField] private Button buyBtn;
        
        public GoodsType Type { get; private set; }
        public short Level { get; private set; }

        private float _itemIconMaxSize;
        
        /// <summary>
        /// Инициализация UI-элементов
        /// </summary>
        public void Initialize(
            GoodsType type, GoodUI uiData,
            Action<GoodsType, short, int, int, Price> onBuyAction,
            Action<GoodsType, short, int> onClickAction
        )
        {
            if (_itemIconMaxSize == 0)
            {
                _itemIconMaxSize = itemIconBackgroundRect.rect.width - marginPadding;
            }
            
            Type = type;
            Level = uiData.Level;
            
            iconImg.sprite = uiData.Image;
            
            if (uiData.Image != null)
            {
                var texture = uiData.Image.texture;
                var ratio = (float) texture.width / texture.height;
                var scale = ratio >= 1f
                    ? _itemIconMaxSize / texture.width
                    : _itemIconMaxSize / texture.height;
                
                itemIconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, texture.width * scale); 
                itemIconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, texture.height * scale); 
            }
            
            typeTxt.text = LocalizationManager.Instance.Get(type.GetDescription()).ToUpper();
            price.SetValue(uiData.Price.GetMoney());
            
            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() => onBuyAction(Type, Level, uiData.Price, uiData.Hype, price));

            itemBtn.onClick.RemoveAllListeners();
            itemBtn.onClick.AddListener(() => onClickAction(Type, Level, uiData.Price));
        }
    }
}