// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using Dapplo.Config.Language;

namespace Dapplo.CaliburnMicro.Translations
{
    /// <summary>
    ///     Supply an extension to simplify the usage of ILanguage.LanguageChanged
    /// </summary>
    public static class LanguageChangedExtensions
    {
        /// <summary>
        ///     Automatically call the update action when the LanguageChanged fires
        ///     If the is called on a DI object, make sure it's available.
        /// </summary>
        /// <param name="language">ILanguage</param>
        /// <param name="updateAction">Action to call on active and update, the argument is the property name</param>
        /// <param name="run">default the action is run when defining, specify false if this is not wanted</param>
        /// <returns>an IDisposable, calling Dispose on this will stop everything</returns>
        public static IDisposable OnLanguageChanged(this ILanguage language, Action<ILanguage> updateAction, bool run = true)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
            if (updateAction == null)
            {
                throw new ArgumentNullException(nameof(updateAction));
            }
            if (run)
            {
                updateAction(language);
            }
            var observable = Observable.FromEventPattern<EventHandler<EventArgs>, EventArgs>(
                h => language.LanguageChanged += h,
                h => language.LanguageChanged -= h);

            return observable.Subscribe(pce => updateAction(pce.Sender as ILanguage));
        }
    }
}