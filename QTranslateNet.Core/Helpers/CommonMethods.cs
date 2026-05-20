using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using QTranslateNet.Core.Infrastructure;

namespace QTranslateNet.Core.Helpers
{
    public static class CommonMethods
    {
        /// <summary>
        ///     Получить набор заголовков для HttpGet запросов
        /// </summary>
        /// <returns>
        ///     Коллекция заголовков требуемых, по умолчанию, во всех сервисах.
        /// </returns>
        public static Dictionary<string, string> HttpGetDefaultHeaders()
        {
            // TODO Options.LanguageCode найти как читается.
            // По умолчанию: en
            // Используем пока en, чтобы не вызывать отторжения у зарубежных сервисов. И для лучшей совместимости.
            string appLanguage = "en";

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Accept", "*/*" },
                { "Accept-Language", appLanguage + ";q=0.8,en-US;q=0.6,en;q=0.4" },
                { "Accept-Encoding", "gzip,deflate" },
                { "Accept-Charset", "utf-8" }
            };

            return headers;
        }

        /// <summary>
        ///     Получить набор заголовков для HttpPost запросов
        /// </summary>
        /// <param name="json">
        ///     Используется формат данных JSON.
        ///     Если нет, то, по умолчанию, form-urlencoded.
        /// </param>
        /// <returns>
        ///     Коллекция заголовков требуемых, по умолчанию, во всех сервисах.
        ///     По умолчанию, включает в себя заголовки для GET запросов.
        /// </returns>
        public static Dictionary<string, string> HttpPostDefaultHeaders(bool json = false)
        {
            Dictionary<string, string> headers = HttpGetDefaultHeaders();

            string type = json ? "application/json" : "application/x-www-form-urlencoded";
            headers.Add("Content-Type", type + ";charset=utf-8");

            return headers;
        }

        #region Вспомогательные функции

        /*
    Работа с языками
isLanguage(code)                    // true если код валиден
codeFromLanguage(langName)          // "Russian" → "ru"
languageFromCode(code)              // "ru" → "Russian"
getSourceLanguage(json)             // Извлечение языка из авто-определения

    Кодирование и парсинг
encodeGetParam(str)                 // Для query string: ?q=...
encodeUriParam(str)                 // Для пути URL: /translate/...
encodePostParam(str)                // Для тела POST-запроса
parseJSON(str)                      // Безопасный JSON.parse

    Утилиты
format(template, ...args)           // "#{0}/{1}" → "a/b"
limitSource(text, maxLen)           // Обрезка текста под лимиты API

         * */

        // Работа с языками

        /*

        function isLanguage(a)
        {
            return a > AUTO_DETECT_LANGUAGE && a < SupportedLanguages.length;
        }
        function codeFromLanguage(a)
        {
            return a === AUTO_DETECT_LANGUAGE || isLanguage(a)
              ? SupportedLanguages[a]
              : UNKNOWN_LANGUAGE_CODE;
        }
        function languageFromCode(a)
        {
            if (SupportedLanguages[ENGLISH_LANGUAGE] === a) return ENGLISH_LANGUAGE;
            for (var b = AUTO_DETECT_LANGUAGE; b < SupportedLanguages.length; b++)
                if (SupportedLanguages[b] === a) return b;
            return UNKNOWN_LANGUAGE;
        }
        */


        // Кодирование и парсинг

        /// <summary>
        ///     Закодировать строковое значение
        /// </summary>
        /// <param name="param">Значение</param>
        /// <returns>Закодированное значение</returns>
        public static String EncodeUriParam(String? param)
        {
            return param == null ? String.Empty : HttpUtility.UrlEncode(param);
        }

        /// <summary>
        ///     Закодировать строковое значение GET запроса
        /// </summary>
        /// <remarks>
        ///     При превышении ограничения длины, текст обрезается.
        /// </remarks>
        /// <param name="param">Значение</param>
        /// <returns>Закодированное значение</returns>
        public static String EncodeGetParam(String param)
        {
            String encoded = EncodeUriParam(param);

            encoded = LimitSource(encoded, MyConstants.MaxUriLen);

            return encoded;
        }

        /// <summary>
        ///     Закодировать строковое значение POST запроса
        /// </summary>
        /// <param name="param">Значение</param>
        /// <returns>Закодированное значение</returns>
        public static String EncodePostParam(String param)
        {
            return EncodeUriParam(param);
        }

        // Утилиты

        /// <summary>
        ///     Ограничить длину текста под лимиты API
        /// </summary>
        /// <param name="text">Отправляемый текст</param>
        /// <param name="limit">Запрашиваемый лимит</param>
        public static String LimitSource(String text, Int32 limit = MyConstants.MaxSourceLen)
        {
            return text.Length > limit ? text.Remove(startIndex: limit) : text;
        }

        /// <summary>
        ///     Подготовка текста
        /// </summary>
        /// <param name="text">Необработанный текст</param>
        /// <returns>Готовый текст</returns>
        public static String PrepareSource(String? text)
        {
            string result = text ?? String.Empty;

            return result
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");
        }

        #endregion

        #region Разное



        #endregion
    }
}