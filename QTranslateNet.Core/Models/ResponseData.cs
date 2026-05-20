using System;

namespace QTranslateNet.Core.Models
{
    /// <summary>
    ///     Модель ответа от сервиса перевода
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        ///     Переведённый текст
        /// </summary>
        public required String Text { get; init; }

        /// <summary>
        ///     Исходный язык
        /// </summary>
        public required String From { get; init; }

        /// <summary>
        ///     Целевой язык
        /// </summary>
        public required String To { get; init; }

        /// <summary>
        ///     Доп. инфо (транслит, словарь)
        /// </summary>
        public String? AdditionalInfo { get; init; }

        /// <summary>
        ///     Тип ответа
        /// </summary>
        /// <remarks>
        ///     Тип: "dictionaryRequest" для словарных запросов
        /// </remarks>
        public String? ResponseType { get; init; }
    }
}