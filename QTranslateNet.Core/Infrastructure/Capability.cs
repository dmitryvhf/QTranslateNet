using System;
using System.ComponentModel.DataAnnotations;

namespace QTranslateNet.Core.Infrastructure
{
    /// <summary>
    ///     Возможности приложения
    /// </summary>
    public enum Capability
    {
        /// <summary>
        ///     Перевод текста
        /// </summary>
        [Display(Name = "Перевод текста")]
        Translate = 1,

        /// <summary>
        ///     Автоопределение языка, переводимого текста
        /// </summary>
        [Display(Name = "Автоопределение языка")]
        DetectLanguage = 2,

        /// <summary>
        ///     Озвучка перевода
        /// </summary>
        [Display(Name = "Озвучка перевода")]
        Listen = 3,

        /// <summary>
        ///     Сервис словарей
        /// </summary>
        [Display(Name = "Сервис словарей")]
        Dictionary = 4
    }
}
