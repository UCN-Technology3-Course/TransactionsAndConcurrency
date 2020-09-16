using System;

namespace ZooModel
{
    public partial class Animal : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastUpdate { get; set; }
        public Species Species { get; set; }

        public object Clone()
        {
            return new Animal
            {
                Id = Id,
                Name = Name,
                LastUpdate = LastUpdate,
                Species = Species
            };
        }

        public override string ToString()
        {
            return $"{Name} {Species?.Name}";
        }

        
    }
}
