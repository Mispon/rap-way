﻿using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Pages
{
    /// <summary>
    /// Popup для отображения сгенерированных очков работы
    /// </summary>
    public class WorkPoints : MonoBehaviour
    {
        [SerializeField] private Text label;
        [SerializeField] private Animation clip;
        
        /// <summary>
        /// Показывает количество созданных очков работы 
        /// </summary>
        public void Show(int value)
        {
            SoundManager.Instance.PlayWorkPoint();
            label.text = $"+{value}";
            clip.Play();
        }
    }
}