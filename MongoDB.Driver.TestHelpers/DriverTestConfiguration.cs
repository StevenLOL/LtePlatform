/* Copyright 2010-2015 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;

namespace MongoDB.Driver.Tests
{
    /// <summary>
    /// A static class to handle online test configuration.
    /// </summary>
    public static class DriverTestConfiguration
    {
        // private static fields
        private static readonly Lazy<MongoClient> __client;

        // static constructor
        static DriverTestConfiguration()
        {
            var connectionString = CoreTestConfiguration.ConnectionString.ToString();
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            var serverSelectionTimeoutString = Environment.GetEnvironmentVariable("MONGO_SERVER_SELECTION_TIMEOUT_MS");
            if (serverSelectionTimeoutString == null)
            {
                serverSelectionTimeoutString = "30000";
            }
            clientSettings.ServerSelectionTimeout = TimeSpan.FromMilliseconds(int.Parse(serverSelectionTimeoutString));
            clientSettings.ClusterConfigurator = cb => CoreTestConfiguration.ConfigureLogging(cb);

            __client = new Lazy<MongoClient>(() => new MongoClient(clientSettings), true);
            DatabaseNamespace = CoreTestConfiguration.DatabaseNamespace;
            CollectionNamespace = new CollectionNamespace(DatabaseNamespace, "testcollection");
        }

        // public static properties
        /// <summary>
        /// Gets the test client.
        /// </summary>
        public static MongoClient Client => __client.Value;

        /// <summary>
        /// Gets the collection namespace.
        /// </summary>
        /// <value>
        /// The collection namespace.
        /// </value>
        public static CollectionNamespace CollectionNamespace { get; }

        /// <summary>
        /// Gets the database namespace.
        /// </summary>
        /// <value>
        /// The database namespace.
        /// </value>
        public static DatabaseNamespace DatabaseNamespace { get; }
    }
}
