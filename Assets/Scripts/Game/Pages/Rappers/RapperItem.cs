using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Pages.Rappers
{
    /// <summary>
    /// Элемент списка реальных исполнителей
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class RapperItem : MonoBehaviour
    {
        [SerializeField] private Image avatar;
        [SerializeField] private Text nickname;
        [SerializeField] private Image country;
        [SerializeField] private Text fans;
        
        public RapperInfo Info { get; private set; }

        /// <summary>
        /// Обработчик клика по элементу
        /// </summary>
        public Action<RapperItem> onClick = item => {};

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => onClick.Invoke(this));
        }

        /// <summary>
        /// Инициализирует элемент списка 
        /// </summary>
        public void Setup(RapperInfo info)
        {
            Info = info;
            avatar.sprite = info.Avatar;
            nickname.text = info.Name;
            country.sprite = info.CountryImage;
            fans.text = info.Fans.ToString(); // TODO: convert to display string
        }
    }
}