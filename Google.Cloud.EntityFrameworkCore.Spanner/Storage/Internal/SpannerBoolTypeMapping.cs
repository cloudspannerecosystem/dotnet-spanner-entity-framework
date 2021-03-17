﻿// Copyright 2020, Google Inc. All rights reserved.
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

using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Google.Cloud.EntityFrameworkCore.Spanner.Storage.Internal
{
    //Note: This is required to customize the literals for 'true' and 'false' where
    // EFCore uses '1' and '0' by default.
    internal class SpannerBoolTypeMapping : BoolTypeMapping
    {
        public SpannerBoolTypeMapping(
            string storeType,
            DbType? dbType = null)
            : base(storeType, dbType)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        {
            return new SpannerBoolTypeMapping(parameters.StoreType, parameters.DbType);
        }

        protected override string GenerateNonNullSqlLiteral(object value)
            => (bool)value ? "true" : "false";
    }
}
