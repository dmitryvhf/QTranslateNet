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
        public static readonly SupportedLanguage AutoDetectLanguage =
            new SupportedLanguage() { Code = "auto", Name = "Auto-Detect" };

        #endregion

        #region Other global constants

        /// <summary>
        ///     Коллекция известных поддерживаемых языков перевода
        /// </summary>
        /// <remarks>
        ///     Список: <see href="https://en.wikipedia.org/wiki/List_of_ISO_639_language_codes"/>
        /// </remarks>
        public static readonly SupportedLanguage[] SupportedLanguage = new SupportedLanguage[]
        {
            // new SupportedLanguage(){ Code = AutoDetectLanguageCode, Name = "Auto-Detect" },

            new SupportedLanguage(){ Code = "af", Name = "Afrikaans" },
            new SupportedLanguage(){ Code = "az", Name = "Albanian" },
            new SupportedLanguage(){ Code = "sq", Name = "Arabic" },
            new SupportedLanguage(){ Code = "ar", Name = "Armenian" },
            new SupportedLanguage(){ Code = "hy", Name = "Azerbaijani" },

            new SupportedLanguage(){ Code = "eu", Name = "Basque" },
            new SupportedLanguage(){ Code = "be", Name = "Belarusian" },
            new SupportedLanguage(){ Code = "bg", Name = "Bengali" },

            new SupportedLanguage(){ Code = "ca", Name = "Catalan" },
            new SupportedLanguage(){ Code = "zh-CN", Name = "Chinese (Simplified)" },
            new SupportedLanguage(){ Code = "zh-TW", Name = "Chinese (Traditional)" },
            new SupportedLanguage(){ Code = "hr", Name = "Croatian" },
            new SupportedLanguage(){ Code = "cs", Name = "Czech" },

            new SupportedLanguage(){ Code = "da", Name = "Danish" },
            new SupportedLanguage(){ Code = "nl", Name = "Dutch" },

            new SupportedLanguage(){ Code = "en", Name = "English" },
            new SupportedLanguage(){ Code = "eo", Name = "Esperanto" },
            new SupportedLanguage(){ Code = "et", Name = "Estonian" },

            new SupportedLanguage(){ Code = "tl", Name = "Filipino" },
            new SupportedLanguage(){ Code = "fi", Name = "Finnish"},
            new SupportedLanguage(){ Code = "fr", Name = "French" },

            new SupportedLanguage(){ Code = "gl", Name = "Galician" },
            new SupportedLanguage(){ Code = "ka", Name = "Georgian" },
            new SupportedLanguage(){ Code = "de", Name = "German" },
            new SupportedLanguage(){ Code = "el", Name = "Greek" },

            new SupportedLanguage(){ Code = "ht", Name = "Haitian Creole" },
            new SupportedLanguage(){ Code = "iw", Name = "Hebrew" },
            new SupportedLanguage(){ Code = "hi", Name = "Hindi" },
            new SupportedLanguage(){ Code = "hu", Name = "Hungarian" },

            new SupportedLanguage(){ Code = "is", Name = "Icelandic" },
            new SupportedLanguage(){ Code = "id", Name = "Indonesian" },
            new SupportedLanguage(){ Code = "ga", Name = "Irish" },
            new SupportedLanguage(){ Code = "it", Name = "Italian" },

            new SupportedLanguage(){ Code = "ja", Name = "Japanese" },

            new SupportedLanguage(){ Code = "kn", Name = "Kannada" },
            new SupportedLanguage(){ Code = "kk", Name = "Kazakh" },
            new SupportedLanguage(){ Code = "km", Name = "Khmer" },
            new SupportedLanguage(){ Code = "ko", Name = "Korean" },

            new SupportedLanguage(){ Code = "lo", Name = "Lao" },
            new SupportedLanguage(){ Code = "la", Name = "Latin" },
            new SupportedLanguage(){ Code = "lv", Name = "Latvian" },
            new SupportedLanguage(){ Code = "lt", Name = "Lithuanian" },

            new SupportedLanguage(){ Code = "mk", Name = "Macedonian" },
            new SupportedLanguage(){ Code = "ms", Name = "Malay" },
            new SupportedLanguage(){ Code = "mt", Name = "Maltese" },
            new SupportedLanguage(){ Code = "mr", Name = "Marathi" },
            new SupportedLanguage(){ Code = "mn", Name = "Mongolian" },

            new SupportedLanguage(){ Code = "no", Name = "Norwegian" },

            new SupportedLanguage(){ Code = "fa", Name = "Persian" },
            new SupportedLanguage(){ Code = "pl", Name = "Polish" },
            new SupportedLanguage(){ Code = "pt", Name = "Portuguese" },

            new SupportedLanguage(){ Code = "ro", Name = "Romanian" },
            new SupportedLanguage(){ Code = "ru", Name = "Russian" },

            new SupportedLanguage(){ Code = "sr", Name = "Serbian" },
            new SupportedLanguage(){ Code = "si", Name = "Sinhala" },
            new SupportedLanguage(){ Code = "sk", Name = "Slovak" },
            new SupportedLanguage(){ Code = "sl", Name = "Slovenian" },
            new SupportedLanguage(){ Code = "es", Name = "Spanish" },
            new SupportedLanguage(){ Code = "sw", Name = "Swahili" },
            new SupportedLanguage(){ Code = "sv", Name = "Swedish" },

            new SupportedLanguage(){ Code = "tg", Name = "Tajik" },
            new SupportedLanguage(){ Code = "ta", Name = "Tamil" },
            new SupportedLanguage(){ Code = "tt", Name = "Tatar" },
            new SupportedLanguage(){ Code = "te", Name = "Telugu" },
            new SupportedLanguage(){ Code = "th", Name = "Thai" },
            new SupportedLanguage(){ Code = "tr", Name = "Turkish" },

            new SupportedLanguage(){ Code = "uk", Name = "Ukrainian" },
            new SupportedLanguage(){ Code = "ur", Name = "Urdu" },
            new SupportedLanguage(){ Code = "uz", Name = "Uzbek" },

            new SupportedLanguage(){ Code = "vi", Name = "Vietnamese" },

            new SupportedLanguage(){ Code = "cy", Name = "Welsh" },

            new SupportedLanguage(){ Code = "yi", Name = "Yiddish" }
        };

        #endregion
    }
}