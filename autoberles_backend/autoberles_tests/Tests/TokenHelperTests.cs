using autoberles_backend.Classes;
using FluentAssertions;
using Xunit;

namespace autoberles_tests.Tests
{
    public class TokenHelperTests
    {
        [Fact(DisplayName = "[TokenHelper] Same input should always produce same hash")]
        public void HashToken_ShouldBeDeterministic()
        {
            var input = "123456";
            var hash1 = TokenHelper.HashToken(input);
            var hash2 = TokenHelper.HashToken(input);
            hash1.Should().Be(hash2, "because hashing the same input should always give the same result");
        }


        [Fact(DisplayName = "[TokenHelper] Different inputs should produce different hashes")]
        public void HashToken_ShouldProduceDifferentHashes_ForDifferentInputs()
        {
            var input1 = "123456";
            var input2 = "654321";
            var hash1 = TokenHelper.HashToken(input1);
            var hash2 = TokenHelper.HashToken(input2);
            hash1.Should().NotBe(hash2, "because different inputs should not result in the same hash");
        }
    }
}