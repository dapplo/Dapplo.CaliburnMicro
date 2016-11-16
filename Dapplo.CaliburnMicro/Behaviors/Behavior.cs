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
	/// <typeparam name="THost">DependencyObject</typeparam>
	public abstract class Behavior<THost> : IBehavior where THost : DependencyObject
	{
		private readonly WeakReference _hostReference;

		protected Behavior(DependencyObject host)
		{
			Contract.Requires(host is THost, "Host is not the expected type");

			_hostReference = new WeakReference(host);
		}

		private THost GetHost()
		{
			return (THost) _hostReference.Target;
		}

		protected virtual bool IsApplicable(THost host)
		{
			return true;
		}

		protected virtual void Attach(THost host)
		{
		}

		protected virtual void Detach(THost host)
		{
		}

		protected abstract void Update(THost host);

		protected void TryUpdate(Action<THost> update)
		{
			Contract.Requires(update != null);

			var host = GetHost();

			if (host != null)
			{
				update(host);
			}
		}

		#region IBehavior

		public bool IsApplicable()
		{
			var host = GetHost();

			return (host != null) && IsApplicable(host);
		}

		public void Attach()
		{
			var host = GetHost();

			if (host != null)
			{
				Attach(host);
			}
		}

		public void Detach()
		{
			var host = GetHost();

			if (host != null)
			{
				Detach(host);
			}
		}

		public void Update()
		{
			var host = GetHost();

			if (host != null)
			{
				Update(host);
			}
		}

		#endregion
	}
}