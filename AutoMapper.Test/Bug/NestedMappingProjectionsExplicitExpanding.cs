﻿using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper.Should;
using NUnit.Framework;

namespace AutoMapper.Test.Bug
{
    [TestFixture]
    public class NestedMappingProjectionsExplicitExpanding : AutoMapperSpecBase
    {
        Fu _destination;
        int _propValue = 23;

        public class FuEntity
        {
            public ManEntity Man { get; set; }
        }

        public class ManEntity
        {
            public ChuEntity Chu { get; set; }
        }

        public class ChuEntity
        {
            public int Prop { get; set; }
        }

        public class Fu
        {
            public Man Man { get; set; }
        }

        public class Man
        {
            public Chu Chu { get; set; }
        }

        public class Chu
        {
            public int Prop { get; set; }
        }

        protected override void Establish_context()
        {
            Mapper.CreateMap<FuEntity, Fu>().ForMember(dest => dest.Man, opt => opt.ExplicitExpansion());
            Mapper.CreateMap<ManEntity, Man>().ForMember(dest => dest.Chu, opt => opt.ExplicitExpansion());
            Mapper.CreateMap<ChuEntity, Chu>();
        }

        protected override void Because_of()
        {
            var fuEntity = new FuEntity { Man = new ManEntity { Chu = new ChuEntity { Prop = _propValue } } };
            _destination = new[] { fuEntity }.AsQueryable().ProjectTo<Fu>(m=>m.Man, m=>m.Man.Chu).First();
        }

        [Test]
        public void Should_map_nested_classes()
        {
            _destination.Man.Chu.Prop.ShouldEqual(_propValue);
        }
    }
}