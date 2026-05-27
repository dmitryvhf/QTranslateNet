using System;

using QTranslateNet.Core.Infrastructure;

namespace QTranslateNet.Core.Models
{
    /// <summary>
    ///     Описание сервиса
    /// </summary>
    public class ServiceHeader
    {
        /// <summary>
        ///     Уникальное имя сервиса используемое для специальных возможностей приложения
        /// </summary>
        /// <remarks>
        ///     Рекомендуется короткое имя без символов.
        ///     Используется как уникальный ключ в коллекции сервисов.
        /// </remarks>
        public required String AccessibleName { get; init; }

        /// <summary>
        ///     Название сервиса перевода
        /// </summary>
        public required String Name { get; init; }

        /// <summary>
        ///     Информация о сервисе
        /// </summary>
        /// <remarks>
        ///     Описание сервиса и возможностей.
        ///     А так же могут быть версия, дата обновления, автор, ссылка на сайт.
        ///     Поддерживаются переносы строк
        /// </remarks>
        public required String Info { get; init; }

        /// <summary>
        ///     Название сервиса перевода
        /// </summary>
        public required Capability[] Capabilities { get; init; }

        /// <summary>
        ///     Поддерживаемые языки
        /// </summary>
        /// <remarks>
        ///     Пустая коллекция - все языки по умолчанию, известные приложению перевода.
        ///     Если коллекция не пустая, то языки перевода приложения заменяются на указанный список.
        /// </remarks>
        public required string[] SupportedLanguages { get; init; }

        /// <summary>
        ///     Картинка сервиса
        /// </summary>
        public required byte[] ServiceIco { get; init; }
    }
}