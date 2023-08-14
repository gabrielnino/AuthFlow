using AuthFlow.Application.DTOs;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Test
{
    public static class UtilTest<T>
    {
        public static void Assert(Task<OperationResult_REVIEWED<T>> result)
        {
            result.Should().NotBeNull();
            result.Id.Should().NotBe(0);
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            result.Exception.Should().BeNull();
            result.AsyncState.Should().BeNull();
            result.Result.Should().NotBeNull();
        }
    }
}
