// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Dapplo.CaliburnMicro.Behaviors
{
    /// <summary>
    ///     This handles the checking of values
    ///     This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
    /// </summary>
    /// <typeparam name="TValue">The type of the value to work with</typeparam>
    public class ValueChecker<TValue> where TValue : class
    {
        private readonly IList<string> _parsedTargetValues = new List<string>();

        /// <summary>
        ///     Returns if the value
        /// </summary>
        public bool IsMatch { get; private set; }

        /// <summary>
        ///     The value to check against (this can be a comma separated list which will be parsed)
        /// </summary>
        public string TargetValue { get; private set; }

        /// <summary>
        ///     The actual value of the property
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        ///     Parse the target value(s), this also depends on the type of the value so it should be called no matter which
        ///     changes
        /// </summary>
        private void ParseTargetValues()
        {
            _parsedTargetValues.Clear();

            if (string.IsNullOrEmpty(TargetValue))
            {
                return;
            }
            var parsedValues =
                from targetValue in TargetValue.Split(',')
                where !string.IsNullOrWhiteSpace(targetValue)
                select targetValue.Trim();

            foreach (var parsedValue in parsedValues)
            {
                _parsedTargetValues.Add(parsedValue);
            }
        }

        /// <summary>
        ///     Either the value or the targetValue has been changed, update this
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="targetValue">possible value(s)</param>
        public void Update(TValue value, string targetValue)
        {
            var stringValue = value.ToString();
            var valueChanged = stringValue != Value;
            var targetValueChanged = targetValue != TargetValue;

            if (!valueChanged && !targetValueChanged)
            {
                return;
            }
            if (valueChanged)
            {
                Value = stringValue;
            }

            if (targetValueChanged)
            {
                TargetValue = targetValue;
            }

            ParseTargetValues();

            // Update the IsMatch
            IsMatch = Value == null || Value.Equals("") ? string.IsNullOrEmpty(TargetValue) : _parsedTargetValues.Contains(stringValue);
        }
    }
}