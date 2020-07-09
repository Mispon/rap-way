using Localization;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Game.Pages.Social.ResultPages
{
    public class PhotoResultPage: SocialResultPage
    {
        [Header("Контролы")] 
        [SerializeField] private Text headerText;
        [SerializeField] private Text nicknameText;
        [SerializeField] private Text commentText;

        [Header("Контролы анализатора")] 
        [SerializeField] private Text marksText;
        [SerializeField] private Text hypeIncomeText;

        protected override void DisplayResult ()
        {
            var typeLocalization = LocalizationManager.Instance.Get(Social.Data.Type.GetDescription());
            headerText.text = $"Новый {typeLocalization}";
            nicknameText.text = $"{PlayerManager.Data.Info.NickName}:";
            commentText.text = Social.ExternalText;
            
            //marksText
            hypeIncomeText.text = $"Хайп: +{Social.HypeIncome}";
        }
        
        protected override void AfterPageClose()
        {
            base.AfterPageClose();

            headerText.text = "";
            nicknameText.text = "";
            commentText.text = "";
            //marksText.text = "";
            hypeIncomeText.text = "";
        }
    }
}