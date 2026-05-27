using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;

using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace QTranslateNet.Core
{
    /// <summary>
    ///     Базовый класс для всех сервисов
    /// </summary>
    public abstract class TranslateServiceBase : ITranslateService
    {
        private ServiceHeader? _serviceHeader;

        #region Required service properties

        /// <summary>
        ///     Базовый URL API
        /// </summary>
        protected abstract String BaseUrlApi { get; }

        /// <summary>
        ///     URL на веб-версию сервиса
        /// </summary>
        protected abstract String BaseUrlWeb { get; }

        /// <summary>
        ///     Название сервиса перевода
        /// </summary>
        protected abstract String Name { get; }

        /// <summary>
        ///     Уникальное имя сервиса используемое для специальных возможностей приложения
        /// </summary>
        /// <remarks>
        ///     Рекомендуется короткое имя без символов. Используется как уникальный ключ в коллекции сервисов.
        /// </remarks>
        protected abstract String AccessibleName { get; }

        /// <summary>
        ///     Информация о библиотеке
        /// </summary>
        /// <remarks>
        ///     Версия, дата обновления, автор...
        /// </remarks>
        protected abstract String Info { get; }

        /// <summary>
        ///     Картинка сервиса
        /// </summary>
        protected abstract byte[] ServiceIco { get; }

        /// <summary>
        ///     Список возможностей сервиса
        /// </summary>
        protected abstract Capability[] Capabilities { get; }

        /// <summary>
        ///     Список поддерживаемых языков
        /// </summary>
        /// <remarks>
        ///     Если пустой массив - используется полный глобальный список поддерживаемых языков.
        /// </remarks>
        protected virtual String[] SupportedLanguages { get; }
            = MyConstants.SupportedLanguage.Select(x => x.Code).ToArray();

        #endregion

        /// <inheritdoc />
        public virtual ServiceHeader GetServiceHeader()
        {
            return _serviceHeader ??=
                new ServiceHeader()
                {
                    AccessibleName = AccessibleName,
                    Info = Info,
                    Name = Name,
                    ServiceIco = ServiceIco,
                    SupportedLanguages = SupportedLanguages,
                    Capabilities = Capabilities
                };
        }

        /// <inheritdoc />
        public abstract String GetServiceHost(String langFrom, String langTo, String text);

        /// <inheritdoc />
        public abstract String GetServiceLink(String langFrom, String langTo, String text);

        #region Запросы для подготовки сервиса

        /// <inheritdoc />
        public virtual bool ServiceTranslateBootstrapRequest(
            String text,
            String langFrom,
            String langTo,
            [NotNullWhen(true)] out RequestData? requestData)
        {
            requestData = null;
            return false;
        }

        /// <inheritdoc />
        public virtual bool ServiceTranslateBootstrapResponse(
            HttpResponseMessage response,
            String langFrom,
            String langTo)
        {
            return false;
        }

        #endregion

        #region Перевод текста

        /// <inheritdoc />
        public abstract RequestData[] ServiceTranslateRequest(String text, String langFrom, String langTo);

        /// <inheritdoc />
        public abstract ResponseData ServiceTranslateResponse(HttpResponseMessage[] responses, String langFrom, String langTo);

        #endregion

        #region Автоопределение языка переводимого текста

        /// <inheritdoc />
        public virtual RequestData ServiceDetectLanguageRequest(String text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual ResponseData ServiceDetectLanguageResponse(HttpResponseMessage responseText)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Озвучка (Text-to-Speech)

        /// <inheritdoc />
        public virtual RequestData ServiceListenRequest(String text, String lang, bool slow)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Разное

        /// <inheritdoc />
        public virtual RequestData ServiceLanguagesRequest()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual string[] ServiceLanguagesResponse(String responseText)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}