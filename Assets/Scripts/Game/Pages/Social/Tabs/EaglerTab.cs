using Enums;
using Models.Info;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Pages.Social.Tabs
{
    /// <summary>
    /// Вкладка псевдо-твиттера
    /// </summary>
    public class EaglerTab : BaseSocialsTab
    {
        [SerializeField] private InputField input;

        /// <summary>
        /// Возвращает информацию соц. действия 
        /// </summary>
        protected override SocialInfo GetInfo()
        {
            return new SocialInfo
            {
                Type = SocialType.Eagler,
                MainText = input.text
            };
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            input.text = string.Empty;
        }
    }
}