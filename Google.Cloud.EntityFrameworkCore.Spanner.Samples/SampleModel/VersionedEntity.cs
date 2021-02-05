﻿// Copyright 2021 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Google.Cloud.EntityFrameworkCore.Spanner.Samples.SampleModel
{
    /// <summary>
    /// Abstract base class for entities that uses a client side version number as concurrency token.
    /// </summary>
    public abstract class VersionedEntity
    {
        /// <summary>
        /// Version number that is automatically increased by the database context when the entity is updated.
        /// This property is used as a concurrency token to let Entity Framework detect when an entity has been
        /// updated after it was loaded into memory.
        /// </summary>
        public long Version { get; set; }
    }
}
