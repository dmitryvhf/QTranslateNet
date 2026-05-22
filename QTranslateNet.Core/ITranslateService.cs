using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

using QTranslateNet.Core.Models;

namespace QTranslateNet.Core
{
    /// <summary>
    ///     Интерфейс сервиса перевода
    /// </summary>
    public interface ITranslateService
    {
        /// <summary>
        ///     Получить описание сервиса
        /// </summary>
        ServiceHeader GetServiceHeader();

        /// <summary>
        ///     Получить базовый URL API
        /// </summary>
        /// <remarks>
        ///     Параметры можно использовать для динамического выбора домена
        /// </remarks>
        /// <param name="from">Исходный язык текста</param>
        /// <param name="to">Язык перевода текста</param>
        /// <param name="text">Переводимый текст</param>
        /// <returns>Базовый адрес API сервиса</returns>
        // maybe return URI?
        String GetServiceHost(String langFrom, String langTo, String text);

        /// <summary>
        ///     Получить ссылку на веб-версию сервиса
        /// </summary>
        /// <remarks>
        ///     Используется для кнопки "Открыть в браузере"
        /// </remarks>
        /// <param name="from">Исходный язык текста</param>
        /// <param name="to">Язык перевода текста</param>
        /// <param name="text">Переводимый текст</param>
        /// <returns>Адрес веб-версии сервиса</returns>
        // maybe return URI?
        String GetServiceLink(String langFrom, String langTo, String text);

        #region Проверка сервиса перед переводом

        /// <summary>
        ///     Сформировать запрос для проверки сервиса перед переводом
        /// </summary>
        /// <remarks>
        ///     Для использования, сервис должен иметь флаг <see cref="Infrastructure.Сapability.Translate"/>
        /// </remarks>
        /// <param name="text">Переводимый текст</param>
        /// <param name="from">Исходный язык текста</param>
        /// <param name="to">Язык перевода текста</param>
        /// <param name="requestData">Модель данных ответа</param>
        /// <returns>
        ///     True - если требуется выполнить специальный запрос проверки сервиса.
        ///     Подготовлена модель данных для запроса в <paramref name="requestData"/>
        /// </returns>
        bool ServiceTranslateBootstrapRequest(
            String text,
            String langFrom,
            String langTo,
            [NotNullWhen(true)] out RequestData? requestData);

        /// <summary>
        ///     Разбор ответа при запросе проверке сервиса перед переводом
        ///     методом <see cref="ServiceTranslateBootstrapRequest"/>
        /// </summary>
        /// <param name="response">Данные ответа от сервиса</param>
        /// <param name="from">Исходный язык текста</param>
        /// <param name="to">Язык перевода текста</param>
        /// <returns>True - если данные обработаны успешно и можно продолжать запросы</returns>
        bool ServiceTranslateBootstrapResponse(
            HttpResponseMessage response,
            String langFrom,
            String langTo);

        #endregion

        #region Перевод текста

        /// <summary>
        ///     Сформировать запрос на перевод текста
        /// </summary>
        /// <remarks>
        ///     Для использования, сервис должен иметь флаг <see cref="Infrastructure.Сapability.Translate"/>
        /// </remarks>
        /// <param name="text">Переводимый текст</param>
        /// <param name="from">Исходный язык текста</param>
        /// <param name="to">Язык перевода текста</param>
        /// <returns>Модели данных для запроса</returns>
        RequestData[] ServiceTranslateRequest(String text, String langFrom, String langTo);

        /// <summary>
        ///     Разбор ответа при переводе
        ///     методом <see cref="ServiceTranslateRequest"/>
        /// </summary>
        /// <param name="responses">Данные ответа от сервиса</param>
        /// <param name="from">Исходный язык текста</param>
        /// <param name="to">Язык перевода текста</param>
        /// <returns>Модель данных ответа</returns>
        ResponseData ServiceTranslateResponse(HttpResponseMessage[] responses, String langFrom, String langTo);

        #endregion

        #region Автоопределение языка переводимого текста

        /// <summary>
        ///     Сформировать запрос на автоопределение языка текста
        /// </summary>
        /// <remarks>
        ///     Для использования, сервис должен иметь флаг <see cref="Infrastructure.Сapability.DetectLanguage"/>
        /// </remarks>
        /// <param name="text">Переводимый текст</param>
        /// <returns>Модель данных для запроса</returns>
        RequestData ServiceDetectLanguageRequest(String text);

        /// <summary>
        ///     Разбор ответа метода автоопределения языка
        ///     методом <see cref="ServiceDetectLanguageRequest"/>
        /// </summary>
        /// <param name="responseText">Данные ответа от сервиса</param>
        /// <returns>Модель данных ответа</returns>
        ResponseData ServiceDetectLanguageResponse(HttpResponseMessage responseText);

        #endregion

        #region Озвучка (Text-to-Speech)

        /// <summary>
        ///     Сформировать запрос на озвучку текста
        /// </summary>
        /// <remarks>
        ///     Для использования, сервис должен иметь флаг <see cref="Infrastructure.Сapability.Listen"/>
        /// </remarks>
        /// <param name="text">Переводимый текст</param>
        /// <param name="lang">Язык озвучки</param>
        /// <param name="slow">Скорость воспроизведения</param>
        /// <returns>Модель данных для запроса</returns>
        RequestData ServiceListenRequest(String text, String lang, bool slow);

        #endregion

        #region Разное

        /// <summary>
        ///     Запрос списка поддерживаемых языков
        /// </summary>
        /// <returns>Модель данных для запроса</returns>
        RequestData ServiceLanguagesRequest();

        /// <summary>
        ///     Разбор ответа метода автоопределения языка
        ///     методом <see cref="ServiceLanguagesRequest"/>
        /// </summary>
        /// <param name="responseText">Данные ответа от сервиса</param>
        /// <returns>Коллекция кодов, поддерживаемых языков перевода</returns>
        String[] ServiceLanguagesResponse(String responseText);

        #endregion
    }
}