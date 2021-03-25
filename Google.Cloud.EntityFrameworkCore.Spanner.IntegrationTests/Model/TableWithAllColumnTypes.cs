﻿using System;
using System.Collections.Generic;
using Google.Cloud.EntityFrameworkCore.Spanner.Storage;
using Google.Cloud.Spanner.V1;

namespace Google.Cloud.EntityFrameworkCore.Spanner.IntegrationTests.Model
{
    public partial class TableWithAllColumnTypes
    {
        public long ColInt64 { get; set; }
        public double? ColFloat64 { get; set; }
        public SpannerNumeric? ColNumeric { get; set; }
        public bool? ColBool { get; set; }
        public string ColString { get; set; }
        public string ColStringMax { get; set; }
        public byte[] ColBytes { get; set; }
        public byte[] ColBytesMax { get; set; }
        public SpannerDate? ColDate { get; set; }
        public DateTime? ColTimestamp { get; set; }
        public DateTime? ColCommitTs { get; set; }
        public List<Nullable<long>> ColInt64Array { get; set; }
        public List<Nullable<double>> ColFloat64Array { get; set; }
        public List<Nullable<SpannerNumeric>> ColNumericArray { get; set; }
        public List<Nullable<bool>> ColBoolArray { get; set; }
        public List<string> ColStringArray { get; set; }
        public List<string> ColStringMaxArray { get; set; }
        public List<byte[]> ColBytesArray { get; set; }
        public List<byte[]> ColBytesMaxArray { get; set; }
        public List<Nullable<SpannerDate>> ColDateArray { get; set; }
        public List<Nullable<DateTime>> ColTimestampArray { get; set; }
        public string ColComputed { get; set; }
        public string ASC { get; set; }
    }
}
