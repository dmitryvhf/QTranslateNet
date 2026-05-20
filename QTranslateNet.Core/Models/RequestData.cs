using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;

using QTranslateNet.Core.Infrastructure;

namespace QTranslateNet.Core.Models
{
    /// <summary>
    ///     Модель запроса к сервису перевода
    /// </summary>
    public class RequestData
    {
        #region QTranslate base properties

        /// <summary>
        ///     Тип метода запроса к API
        /// </summary>
        public required RequestHttpMethodType Method { get; init; }

        /// <summary>
        ///     Адрес сервиса перевода
        /// </summary>
        /// <remarks>
        ///     Относительный путь от корневого адреса сервиса,
        ///     который определён в функции <see cref="ITranslateService.GetServiceHost"/>.
        /// </remarks>
        public required String RelativeUrl { get; init; }

        /// <summary>
        ///     Тело запроса
        /// </summary>
        /// <remarks>
        ///     Используется для POST запросов.
        ///     Тело в известном формате (json, form и т.п.)
        /// </remarks>
        public HttpContent? Body { get; init; }

        /// <summary>
        ///     Список заголовков для запроса
        /// </summary>
        /// <remarks>
        ///     Используйте пакет Microsoft.Net.Http.Headers, с константами имён заголовков (HeaderNames),
        ///     при добавлении свох значений в коллекцию.
        /// </remarks>
        public Dictionary<String, String> Headers { get; init; } = new Dictionary<String, String>();

        ///// <summary>
        /////     Тип ответа
        ///// </summary>
        //public required String? ResponseType { get; set; }

        ///// <summary>
        /////     Callback для JSONP
        ///// </summary>
        //public required String? Callback { get; init; }

        #endregion

        #region QTranslateNet properties

        /// <summary>
        ///     Определённый набор поддерживаемых безопасных протоколов соединения HttpClient
        /// </summary>
        public SslProtocols? SslProtocols { get; init; }

        #endregion
    }
}
