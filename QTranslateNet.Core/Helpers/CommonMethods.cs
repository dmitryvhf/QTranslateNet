using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core.Infrastructure;

namespace QTranslateNet.Core.Helpers
{
    public static class CommonMethods
    {
        /// <summary>
        ///     Получить набор заголовков для HttpGet запросов
        /// </summary>
        /// <remarks>
        ///     Используйте пакет Microsoft.Net.Http.Headers, с константами имён заголовков (HeaderNames),
        ///     при добавлении свох значений в коллекцию.
        /// </remarks>
        /// <returns>
        ///     Коллекция заголовков требуемых, по умолчанию, во всех сервисах.
        /// </returns>
        public static Dictionary<string, string> HttpDefaultHeaders()
        {
            // TODO Options.LanguageCode найти как читается.
            // По умолчанию: en
            // Используем пока en, чтобы не вызывать отторжения у зарубежных сервисов. И для лучшей совместимости.
            string appLanguage = "en";

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { HeaderNames.Accept, "*/*" },
                { HeaderNames.AcceptLanguage, appLanguage + ";q=0.8,en-US;q=0.6,en;q=0.4" },
                { HeaderNames.AcceptEncoding, "gzip,deflate" },
                { HeaderNames.AcceptCharset, "utf-8" }
            };

            return headers;
        }

        /// <summary>
        ///     Получить набор заголовков для HttpPost запросов.
        /// </summary>
        /// <remarks>
        ///     Добавляет заголовок "Content-Type" в зависимости от типа тела запроса.
        ///     <pre>
        ///         Используйте пакет Microsoft.Net.Http.Headers, с константами имён заголовков (HeaderNames),
        ///         при добавлении свох значений в коллекцию.
        ///     </pre>
        /// </remarks>
        /// <param name="json">
        ///     Используется формат данных JSON.
        ///     Если нет, то, по умолчанию, form-urlencoded.
        /// </param>
        /// <returns>
        ///     Коллекция заголовков требуемых, по умолчанию, во всех сервисах.
        ///     По умолчанию, включает в себя заголовки для GET запросов.
        /// </returns>
        [Obsolete("Используйте конкретный тип HttpContent для формирования нужных заголовоков тела запроса.")]
        public static Dictionary<string, string> HttpPostDefaultHeaders(bool json = false)
        {
            Dictionary<string, string> headers = HttpDefaultHeaders();

            string type = json ? "application/json" : "application/x-www-form-urlencoded";
            headers.Add(HeaderNames.ContentType, type + ";charset=utf-8");

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

         */

        /// <summary>
        ///     Возвращает полное название языка
        /// </summary>
        /// <param name="langCode">Короткий код языка</param>
        /// <returns>Полное название языка</returns>
        public static String? LanguageFromCode(String langCode)
        {
            return MyConstants.SupportedLanguage
                .FirstOrDefault(x => x.Code == langCode)?.Name ?? MyConstants.UnknownLanguageCode;
        }

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

        /// <summary>
        ///     Разбить текст на блоки с указанной длиной
        /// </summary>
        /// <param name="text">Оригинальный текст</param>
        /// <param name="maxLength">Ограничение длины текста каждого блока</param>
        /// <returns>Коллекция текста, разбитого на блоки</returns>
        public static IEnumerable<string> ChunkByWordLimit(String text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                yield break;
            }

            string[] words = text.Split(' ');
            var currentChunk = new StringBuilder();

            foreach (string word in words)
            {
                // If the word itself exceeds the max length, yield it as is 
                // or handle it depending on your specific fallback requirement.
                if (word.Length > maxLength)
                {
                    if (currentChunk.Length > 0)
                    {
                        yield return currentChunk.ToString().Trim();

                        _ = currentChunk.Clear();
                    }
                    yield return word;
                    continue;
                }

                // Calculate hypothetical length with the new word and a space
                int potentialLength = currentChunk.Length + word.Length + 1;

                if (potentialLength > maxLength)
                {
                    yield return currentChunk.ToString().Trim();

                    _ = currentChunk.Clear();
                }

                _ = currentChunk.Append(word).Append(' ');
            }

            if (currentChunk.Length > 0)
            {
                yield return currentChunk.ToString().Trim();
            }
        }

        #endregion
    }
}