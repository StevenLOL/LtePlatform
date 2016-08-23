﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace AutoMapper.Test.Membel
{
    [TestFixture]
    public class SequenceContainsNoElementsTest : AutoMapperSpecBase
    {
        protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Person, PersonModel>();
        });

        [Test]
        public void should_not_throw_InvalidOperationException()
        {
            Person[] personArr = new Person[] { };
            People people = new People(personArr);
            var pmc = Mapper.Map<People, List<PersonModel>>(people);
            pmc.ShouldNotBeNull();
            pmc.Count.ShouldBe(0);
        }
    }

    public class People : IEnumerable
    {
        private readonly Person[] people;
        public People(Person[] people)
        {
            this.people = people;
        }
        public IEnumerator GetEnumerator() => people.GetEnumerator();
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public class PersonModel
    {
        public string Name { get; set; }
    }
}