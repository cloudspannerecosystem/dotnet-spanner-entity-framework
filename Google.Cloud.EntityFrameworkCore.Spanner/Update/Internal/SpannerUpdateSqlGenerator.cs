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

using Google.Cloud.EntityFrameworkCore.Spanner.Storage.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Google.Cloud.EntityFrameworkCore.Spanner.Update.Internal
{
    public class SpannerUpdateSqlGenerator : UpdateSqlGenerator, ISpannerUpdateSqlGenerator
    {
        private readonly ISqlGenerationHelper _sqlGenerationHelper;

        public SpannerUpdateSqlGenerator(UpdateSqlGeneratorDependencies dependencies)
            : base(dependencies)
        {
            if (dependencies.SqlGenerationHelper is SpannerSqlGenerationHelper spannerSqlGenerationHelper)
            {
                _sqlGenerationHelper = new SpannerSqlGenerationHelper(spannerSqlGenerationHelper.Dependencies, ";");
            }
            else
            {
                _sqlGenerationHelper = dependencies.SqlGenerationHelper;
            }
        }

        protected override ISqlGenerationHelper SqlGenerationHelper { get => _sqlGenerationHelper; }

        protected override void AppendIdentityWhereCondition([NotNull] StringBuilder commandStringBuilder, ColumnModification columnModification)
        {
            commandStringBuilder.Append(" TRUE ");
        }

        protected override void AppendRowsAffectedWhereCondition([NotNull] StringBuilder commandStringBuilder, int expectedRowsAffected)
        {
            commandStringBuilder.Append(" TRUE ");
        }

        public virtual ResultSetMapping AppendBulkInsertOperation(
            StringBuilder commandStringBuilder,
            IReadOnlyList<ModificationCommand> modificationCommands,
            int commandPosition)
        {
            if (modificationCommands.Count == 1
                && modificationCommands[0].ColumnModifications.All(
                    o =>
                        !o.IsKey
                        || !o.IsRead))
            {
                return AppendInsertOperation(commandStringBuilder, modificationCommands[0], commandPosition);
            }

            var writeOperations = modificationCommands[0].ColumnModifications.ToList();
            return AppendBulkInsertWithoutServerValues(commandStringBuilder, modificationCommands, writeOperations);

        }

        private ResultSetMapping AppendBulkInsertWithoutServerValues(
            StringBuilder commandStringBuilder,
            IReadOnlyList<ModificationCommand> modificationCommands,
            List<ColumnModification> writeOperations)
        {
            Debug.Assert(writeOperations.Count > 0);

            var name = modificationCommands[0].TableName;
            var schema = modificationCommands[0].Schema;

            AppendInsertCommandHeader(commandStringBuilder, name, schema, writeOperations);
            AppendValuesHeader(commandStringBuilder, writeOperations);
            AppendValues(commandStringBuilder, writeOperations);
            for (var i = 1; i < modificationCommands.Count; i++)
            {
                commandStringBuilder.AppendLine(",");
                AppendValues(commandStringBuilder, modificationCommands[i].ColumnModifications.Where(o => o.IsWrite).ToList());
            }

            commandStringBuilder.AppendLine(SqlGenerationHelper.StatementTerminator);

            return ResultSetMapping.NoResultSet;
        }
    }
}
