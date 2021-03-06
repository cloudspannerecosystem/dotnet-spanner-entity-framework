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

using Google.Cloud.EntityFrameworkCore.Spanner.IntegrationTests.ShadowPropertiesModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Google.Cloud.EntityFrameworkCore.Spanner.IntegrationTests.ShadowPropertiesTests
{
    public class ShadowPropertiesTests : IClassFixture<ShadowPropertiesTestFixture>
    {
        private readonly ShadowPropertiesTestFixture _fixture;

        public ShadowPropertiesTests(ShadowPropertiesTestFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task CanUseRelationshipsUsingShadowProperty()
        {
            var singerId = _fixture.RandomLong();
            var albumId = _fixture.RandomLong();
            using (var db = new TestSpannerShadowPropertiesDbContext(_fixture.DatabaseName))
            {
                var singer = new Singer
                {
                    SingerId = singerId,
                    Name = "Joe Elliot",
                };
                db.AddRange(singer, new Album
                {
                    AlbumId = albumId,
                    Title = "Some title",
                    Singer = singer,
                });
                await db.SaveChangesAsync();
            }

            using (var db = new TestSpannerShadowPropertiesDbContext(_fixture.DatabaseName))
            {
                var singer = await db.Singers.FindAsync(singerId);
                Assert.Collection(singer.Albums, album => Assert.Equal("Some title", album.Title));
                Assert.NotNull(db.Entry(singer).Property("LastModified").CurrentValue);

                var album = await db.Albums.FindAsync(albumId);
                Assert.Equal("Joe Elliot", album.Singer.Name);
                Assert.NotNull(db.Entry(album).Property("LastModified").CurrentValue);
            }
        }

        [Fact]
        public async Task CanQueryUsingShadowProperty()
        {
            var singerId = _fixture.RandomLong();
            var albumId = _fixture.RandomLong();
            using var db = new TestSpannerShadowPropertiesDbContext(_fixture.DatabaseName);
            var singer = new Singer
            {
                SingerId = singerId,
                Name = "Zeke Pieterson",
            };
            var album = new Album
            {
                AlbumId = albumId,
                Title = "My title",
                Singer = singer,
            };
            db.AddRange(singer, album);
            await db.SaveChangesAsync();

            var albums = await db.Albums.Where(album => album.Singer.SingerId == singerId).ToListAsync();
            Assert.Collection(albums, album => Assert.Equal("My title", album.Title));

            var singerId2 = _fixture.RandomLong();
            db.Singers.Add(new Singer
            {
                SingerId = singerId2,
                Name = "Alice Wendelson",
            });
            await db.SaveChangesAsync();

            var singers = await db.Singers
                .Where(s => new[] { singerId, singerId2 }.Contains(s.SingerId))
                .OrderByDescending(singer => EF.Property<DateTime>(singer, "LastModified")).ToListAsync();
            Assert.Collection(singers,
                singer => Assert.Equal("Alice Wendelson", singer.Name),
                singer => Assert.Equal("Zeke Pieterson", singer.Name)
            );
        }
    }
}
