﻿/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Kafka.Client.Producers.Partitioning
{
    using Kafka.Client.Utils;
    using System;
    using System.Text;

    /// <summary>
    /// Default partitioner using hash code to calculate partition
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class DefaultPartitioner<TKey> : IPartitioner<TKey>
    {
        private static readonly Random Randomizer = new Random();

        private static int Abs(int n)
        {
            return n == int.MinValue ? 0 : Math.Abs(n);
        }

        /// <summary>
        /// Uses the key to calculate a partition bucket id for routing
        /// the data to the appropriate broker partition
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="numPartitions">The num partitions.</param>
        /// <returns>ID between 0 and numPartitions-1</returns>
        /// <remarks>
        /// Uses hash code to calculate partition
        /// </remarks>
        public int Partition(TKey key, int numPartitions)
        {
            Guard.Greater(numPartitions, 0, "numPartitions");
            if (key.GetType() == typeof(byte[]))
            {
                return key == null
                ? Randomizer.Next(numPartitions)
                : Abs(Encoding.UTF8.GetString((byte[])Convert.ChangeType(key, typeof(byte[]))).GetHashCode()) % numPartitions;
            }

            return key == null
                ? Randomizer.Next(numPartitions)
                : Abs(key.GetHashCode()) % numPartitions;
        }
    }
}
