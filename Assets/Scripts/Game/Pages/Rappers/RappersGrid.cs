using System.Collections.Generic;
using Core;
using Data;
using UnityEngine;

namespace Game.Pages.Rappers
{
    /// <summary>
    /// Страница со списком всех существующих исполнителей
    /// </summary>
    public class RappersGrid : MonoBehaviour
    {
        [Header("Контролы и настройки")]
        [SerializeField] private RectTransform listContainer;
        [SerializeField] private RapperGridItem rapperItemTemplate;

        [Header("Персональная карточка")]
        [SerializeField] private RapperCard rapperCard;
        
        [Header("Данные об исполнителях")]
        [SerializeField] private RappersData data;

        private readonly List<RapperGridItem> _rappersList = new List<RapperGridItem>();

        /// <summary>
        /// Инициализирует грид при первом вызове
        /// </summary>
        public void Init()
        {
            if (_rappersList.Count > 0)
                return;

            foreach (var rapperInfo in data.Rappers)
            {
                CreateItem(rapperInfo);
            }
        }
        
        /// <summary>
        /// Создает элемент списка реперов
        /// </summary>
        private void CreateItem(RapperInfo info)
        {
            var rapperItem = Instantiate(rapperItemTemplate, listContainer);
                
            rapperItem.Setup(info);
            rapperItem.onClick += HandleItemClick;
            rapperItem.gameObject.SetActive(true);
                
            _rappersList.Add(rapperItem);
        }
        
        /// <summary>
        /// Обработчик нажатия на элемент списка
        /// </summary>
        private void HandleItemClick(RapperGridItem item)
        {
            SoundManager.Instance.PlayClick();
            rapperCard.Show(item.Info);
        }

        private void OnDestroy()
        {
            foreach (var rapperItem in _rappersList)
            {
                rapperItem.onClick -= HandleItemClick;
            }
            
            _rappersList.Clear();
        }
    }
}