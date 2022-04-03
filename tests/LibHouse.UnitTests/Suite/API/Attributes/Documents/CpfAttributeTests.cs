using LibHouse.API.Attributes.Documents;
using Xunit;

namespace LibHouse.UnitTests.Suite.API.Attributes.Documents
{
    public class CpfAttributeTests
    {
        [Theory]
        [InlineData("22527856080")]
        [InlineData("44271974005")]
        [InlineData("10326783059")]
        [InlineData("19141144007")]
        [InlineData("37064090031")]
        public void IsValid_ValidCpf_ReturnsSuccess(string cpf)
        {
            CpfAttribute cpfAttribute = new();

            bool isValid = cpfAttribute.IsValid(cpf);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("06633959094")]
        [InlineData("45550841029")]
        [InlineData("0488990904")]
        public void IsValid_InvalidCpf_ReturnsFailure(string invalidCpf)
        {
            CpfAttribute cpfAttribute = new();

            bool isValid = cpfAttribute.IsValid(invalidCpf);

            Assert.False(isValid);
        }
    }
}