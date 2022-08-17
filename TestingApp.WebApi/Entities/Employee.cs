using System;

namespace TestingApp.WebApi.Entities
{
    public class Employee : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}