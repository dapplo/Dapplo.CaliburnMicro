#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.CaliburnMicro
// 
// Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors
{
	/// <summary>
	/// This handles the 
	/// This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
	/// </summary>
	public class EnumCheck
	{
		private readonly IList<object> _parsedTargetValues = new List<object>();

		public object Value { get; private set; }

		public string TargetValue { get; private set; }

		public bool IsMatch { get; private set; }

		public void Update(object value, string targetValue)
		{
			var valueChanged = value != Value;
			var targetValueChanged = targetValue != TargetValue;

			if (valueChanged || targetValueChanged)
			{
				if (valueChanged)
				{
					Value = value;
				}

				if (targetValueChanged)
				{
					TargetValue = targetValue;
				}

				ParseTargetValues();

				MatchValueAndTargetValue();
			}
		}

		private void ParseTargetValues()
		{
			_parsedTargetValues.Clear();

			if (!(Value is Enum) || string.IsNullOrEmpty(TargetValue))
			{
				return;
			}
			var parsedValues =
				from targetValue in TargetValue.Split(',')
				let trimmedTargetValue = targetValue.Trim()
				where trimmedTargetValue.Length > 0
				select Enum.Parse(Value.GetType(), trimmedTargetValue, false);

			foreach (var parsedValue in parsedValues)
			{
				_parsedTargetValues.Add(parsedValue);
			}
		}

		private void MatchValueAndTargetValue()
		{
			IsMatch = (Value == null) || Value.Equals("")
				? string.IsNullOrEmpty(TargetValue)
				: _parsedTargetValues.Contains(Value);
		}
	}
}