using System;

namespace QTranslateNet.Core.Infrastructure
{
    /// <summary>
    ///     Http методы запросов
    /// </summary>
    /// <remarks>
    ///     Аналог <see cref="System.Net.Http.HttpMethod"/>.
    ///     Отдельное перечисление чтобы не иметь связи с библиотекой и ограничивать количество.
    /// </remarks>
    public enum RequestHttpMethodType
    {
        HttpGet,
        HttpPost
    }
}
