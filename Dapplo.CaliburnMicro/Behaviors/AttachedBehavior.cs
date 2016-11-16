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
using System.Diagnostics.Contracts;
using System.Windows;

#endregion

namespace Dapplo.CaliburnMicro.Behaviors
{
	/// <summary>
	/// This code comes from <a href="http://www.executableintent.com/attached-behaviors-part-2-framework/">here</a>
	/// </summary>
	public sealed class AttachedBehavior
	{
		private readonly Func<DependencyObject, IBehavior> _behaviorFactory;

		private readonly DependencyProperty _property;

		internal AttachedBehavior(DependencyProperty property, Func<DependencyObject, IBehavior> behaviorFactory)
		{
			_property = property;
			_behaviorFactory = behaviorFactory;
		}

		/// <summary>
		/// Register a behavior to a DependencyObject
		/// </summary>
		/// <param name="behaviorFactory">Func with DependencyObject and IBehavior</param>
		/// <returns>AttachedBehavior</returns>
		public static AttachedBehavior Register(Func<DependencyObject, IBehavior> behaviorFactory)
		{
			Contract.Requires(behaviorFactory != null);

			return new AttachedBehavior(RegisterNextProperty(), behaviorFactory);
		}

		private static DependencyProperty RegisterNextProperty()
		{
			return DependencyProperty.RegisterAttached(GetNextPropertyName(), typeof(IBehavior), typeof(AttachedBehavior));
		}

		private static string GetNextPropertyName()
		{
			return "_" + Guid.NewGuid().ToString("N");
		}

		/// <summary>
		/// Update (or create) the Behavior for the DependencyObject
		/// </summary>
		/// <param name="host"></param>
		public void Update(DependencyObject host)
		{
			Contract.Requires(host != null);

			var behavior = (IBehavior) host.GetValue(_property);

			if (behavior == null)
			{
				TryCreateBehavior(host);
			}
			else
			{
				UpdateBehavior(host, behavior);
			}
		}

		private void TryCreateBehavior(DependencyObject host)
		{
			var behavior = _behaviorFactory(host);

			if (behavior.IsApplicable())
			{
				behavior.Attach();

				host.SetValue(_property, behavior);

				behavior.Update();
			}
		}

		private void UpdateBehavior(DependencyObject host, IBehavior behavior)
		{
			if (behavior.IsApplicable())
			{
				behavior.Update();
			}
			else
			{
				host.ClearValue(_property);

				behavior.Detach();
			}
		}
	}
}