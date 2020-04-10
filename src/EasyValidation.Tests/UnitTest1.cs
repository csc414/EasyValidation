using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Xunit;

namespace EasyValidation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test2()
        {
            ValidationObjects<Student>.Property(o => o.Name).Equal(o=> o.NickName).DisplayName("Ãû³Æ");
            //ValidationObjects<Student>.Property(o => o.Names).NotEmpty();
            var student = new Student { Name = "ABC", NickName = "ABC1", Age = 17, MyAge = 18 };
            ValidationResult result = Validator.Validate(student);
            if (result.IsValid)
            {
                
            } 
            else
            {
                var error = result.ToString(",");
            }
        }

        class Student
        {
            public string Name { get; set; }

            public string NickName { get; set; }

            public int Age { get; set; }

            public int MyAge { get; set; }

            public IList<string> Names { get; set; } = new List<string>();
        }
    }
}
