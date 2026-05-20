using System;

namespace QTranslateNet.Core.Models
{
    /// <summary>
    ///     Описание поддерживаемого языка
    /// </summary>
    public class SupportedLanguage
    {
        /// <summary>
        ///     Сокращённый код языка
        /// </summary>
        public required String Code { get; set; }

        /// <summary>
        ///     Полное название языка
        /// </summary>
        public required String Name { get; set; }
    }
}
