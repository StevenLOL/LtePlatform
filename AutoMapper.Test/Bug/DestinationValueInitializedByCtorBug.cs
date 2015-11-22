using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AutoMapper.Test.Bug
{
    [TestFixture]
	public class DestinationValueInitializedByCtorBug : AutoMapperSpecBase
	{
        public class ItemToMapDto
        {
            public ItemToMapDto()
            {
                /* Remove the line below and the mapping works correctly*/
                this.Tag = new TagDto() { Name = Guid.NewGuid().ToString() };
            }
            public string Name { get; set; }
            public TagDto Tag { get; set; }
        }
        public class TagDto
        {
            public string Name { get; set; }
            public bool IsTrue { get; set; }
        }
        public class ItemToMap
        {
            public string Name { get; set; }
            public Tag Tag { get; set; }
        }
        public class Tag
        {
            public string Name { get; set; }
            public bool IsTrue { get; set; }
        }

	    protected override void Establish_context()
	    {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ItemToMap, ItemToMapDto>();
                cfg.CreateMap<Tag, TagDto>();
            });
        }

        [Test]
		public void Should_map_all_null_values_to_its_substitute()
		{
            var tag = new Tag();

            var entities = new List<ItemToMap>();

            for (var i = 0; i < 10; i++)
            {
                entities.Add(new ItemToMap()
                {
                    Name = Guid.NewGuid().ToString(),
                    Tag = tag,
                });
            }

		    Mapper.Map<List<ItemToMap>, List<ItemToMapDto>>(entities);
            typeof(AutoMapperMappingException).ShouldNotBeThrownBy(() => Mapper.Map<List<ItemToMap>, List<ItemToMapDto>>(entities));
		}
	}
}
