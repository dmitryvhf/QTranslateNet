using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleTranslateServiceLibrary
{
    /// <summary>
    ///     Модель ответа от Google Translate API
    /// </summary>
    internal class GoogleServiceResponse
    {
        // [[["дом","home",null,null,10]], null, "en", null, null, null, null,[]]
        // [[["дом","home",null,null,10]], null, "en", null, null, null, 1,   [], [["en"], null, [1], ["en"]]]

        // массив перевода
        // null
        // string: язык перевода
        // null
        // null
        // null
        // number: при автодетекте число. возможно количество предполагаемых языков перевода
        // array всегда пустой
        // массив
        // пара: каждое значение - массив, внутри 1 значение
        //  первая пара: выбранный язык сервисом; null
        //  дальнешие пары (не было примеров с большим набором, только 1 пара): значение шанса; код языка
    }
}
