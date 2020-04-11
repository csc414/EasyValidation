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
                builder.Property(o => o.Name).DisplayName("名称").Not(o=> o.Empty()).WithMessage("请输入正确的名称");
                builder.Property(o => o.NickName).Match(@"\d+");
                builder.Property(o => o.Age).DisplayName("年龄").Must(age =>  age >= 18, "{propertyName} 必须大于18岁");
            });

            var student = new Student { Age = 18 };
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

            public IList<string> Names { get; set; } = new List<string>();
        }
    }
}
