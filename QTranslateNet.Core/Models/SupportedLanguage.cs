using System;

namespace QTranslateNet.Core.Models
{
    /// <summary>
    ///     Описание поддерживаемого языка
    /// </summary>
    public class SupportedLanguage
    {
        /// <summary>
        ///     Код языка
        /// </summary>
        public required String Code { get; init; }

        /// <summary>
        ///     Полное название языка
        /// </summary>
        public required String Name { get; init; }
    }
}
