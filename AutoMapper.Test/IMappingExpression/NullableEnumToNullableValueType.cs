﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace AutoMapper.Test.IMappingExpression
{
    public class NullableEnumToNullableValueType
    {
        [TestFixture]
        public class CannotConvertEnumToNullableWhenPassedNull : AutoMapperSpecBase
        {
            public enum DummyTypes : int
            {
                Foo = 1,
                Bar = 2
            }

            public class DummySource
            {
                public DummyTypes? Dummy { get; set; }
            }

            public class DummyDestination
            {
                public int? Dummy { get; set; }
            }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DummySource, DummyDestination>();
            });

            [Test]
            public void Should_map_null_enum_to_nullable_base_type()
            {
                DummySource src = new DummySource() { Dummy = null };

                var destination = Mapper.Map<DummySource, DummyDestination>(src);

                destination.Dummy.ShouldBeNull();
            }
        }
    }
}