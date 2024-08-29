using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TextStorage.UnitTests
{
    public class BalancerTests
    {
        private readonly LoadBalancer _sut;

        public BalancerTests()
        {
            _sut = new LoadBalancer(["master1", "master2", "master3"]);
        }

        [Fact]
        public void Balancer()
        {
            _sut.TenantId.Should().Be(-1);
            _sut.MoveNext();
            _sut.TenantId.Should().Be(0);
            _sut.MoveNext();
            _sut.TenantId.Should().Be(1);
            _sut.MoveNext();
            _sut.TenantId.Should().Be(2);
            _sut.MoveNext();
            _sut.TenantId.Should().Be(0);
        }
    }
}