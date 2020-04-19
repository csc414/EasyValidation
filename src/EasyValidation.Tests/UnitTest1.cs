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
            Validator.Model<Student>(builder => {
                builder.Property(o => o.Name).DataAnnotation().DisplayName("名称").Length(3, 5);
                builder.Property(o => o.NickName).When(o => o.Age >= 18, o => o.NotEmpty());
                builder.Property(o => o.Age).DisplayName("年龄").Range(5,10);
            });
            
            var student = new Student { Name="he", Age = 18 };

            ValidationResult result = Validator.Validate(student, builder => {
                //builder.Include(o => o.Name);
            });
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
            [Required(ErrorMessage = "{0}不能为空")]
            public string Name { get; set; }

            public string NickName { get; set; }

            public int Age { get; set; }

            public IList<string> Names { get; set; } = new List<string>();
        }
    }
}
