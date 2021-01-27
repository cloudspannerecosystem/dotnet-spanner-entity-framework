﻿// Copyright 2021 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.ComponentModel.DataAnnotations;

namespace Google.Cloud.EntityFrameworkCore.Spanner.IntegrationTests
{
    [InterleaveInParent(typeof(Author), OnDelete.Cascade)]
    public class Article
    {
        public long AuthorId { get; set; }
        public long ArticleId { get; set; }
        public DateTime PublishDate { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public Author Author { get; set; }
    }
}
