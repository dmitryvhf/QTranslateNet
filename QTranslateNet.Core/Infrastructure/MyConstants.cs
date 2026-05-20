using System;

using QTranslateNet.Core.Models;

namespace QTranslateNet.Core.Infrastructure
{
    /// <summary>
    ///     Глобальные значения
    /// </summary>
    public class MyConstants
    {
        #region Http constants

        /// <summary>
        ///     Время ожидания от сервера
        /// </summary>
        public const int TimeoutSeconds = 10;

        /// <summary>
        ///     Максимальная длина строки запроса
        /// </summary>
        public const Int32 MaxUriLen = 1800;

        /// <summary>
        ///     Максимальная длина переводимых данных
        /// </summary>
        public const Int32 MaxSourceLen = 5000;

        #endregion

        #region Translate constants

        /// <summary>
        ///     Перевод строки
        /// </summary>
        public const String NLine = "\r\n";

        /// <summary>
        ///     Двойной перевод строки
        /// </summary>
        public const String NLine2 = "\r\n";

        /// <summary>
        ///     Тип выбранного языка: неизвестный или неподдерживаемый
        /// </summary>
        public const String UnknownLanguageCode = "-1";

        /// <summary>
        ///     Тип выбранного языка: автоперевод
        /// </summary>
        public const String AutoDetectLanguageCode = "auto";

        #endregion

        #region Other global constants

        /// <summary>
        ///     Коллекция известных поддерживаемых языков перевода
        /// </summary>
        public static readonly SupportedLanguage[] SupportedLanguage = new SupportedLanguage[]
        {
            new SupportedLanguage(){ Code = AutoDetectLanguageCode, Name = "Auto-Detect" },
            new SupportedLanguage(){ Code = "en", Name = "English" },
            new SupportedLanguage(){ Code = "ru", Name = "Russian" }
        };

        #endregion
    }
}