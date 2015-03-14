﻿// This file is part of Hangfire.
// Copyright © 2015 Sergey Odinokov.
// 
// Hangfire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// Hangfire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with Hangfire. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using Hangfire.Annotations;
using Hangfire.Logging;

namespace Hangfire
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration<TStorage> UseStorage<TStorage>(
            [NotNull] this IGlobalConfiguration configuration,
            [NotNull] TStorage storage)
            where TStorage : JobStorage
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (storage == null) throw new ArgumentNullException("storage");

            JobStorage.Current = storage;

            return configuration.Use(storage);
        }

        public static IGlobalConfiguration<TActivator> UseActivator<TActivator>(
            [NotNull] this IGlobalConfiguration configuration,
            [NotNull] TActivator activator)
            where TActivator : JobActivator
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (activator == null) throw new ArgumentNullException("activator");

            JobActivator.Current = activator;

            return configuration.Use(activator);
        }

        public static IGlobalConfiguration<TLogProvider> UseLogProvider<TLogProvider>(
            [NotNull] this IGlobalConfiguration configuration,
            [NotNull] TLogProvider provider)
            where TLogProvider : ILogProvider
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (provider == null) throw new ArgumentNullException("provider");

            LogProvider.SetCurrentLogProvider(provider);

            return configuration.Use(provider);
        }

        public static IGlobalConfiguration<TFilter> UseFilter<TFilter>(
            [NotNull] this IGlobalConfiguration configuration, 
            [NotNull] TFilter filter)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (filter == null) throw new ArgumentNullException("filter");

            GlobalJobFilters.Filters.Add(filter);

            return configuration.Use(filter);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IGlobalConfiguration<T> Use<T>([NotNull] this IGlobalConfiguration configuration, T entry)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            return new ConfigurationEntry<T>(entry);
        }

        private class ConfigurationEntry<T> : IGlobalConfiguration<T>
        {
            private readonly T _entry;

            public ConfigurationEntry(T entry)
            {
                _entry = entry;
            }

            public T Entry
            {
                get { return _entry; }
            }
        }
    }
}