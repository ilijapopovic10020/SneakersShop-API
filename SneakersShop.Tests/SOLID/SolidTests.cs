using System.Diagnostics;
using FluentAssertions;
using Moq;
using Xunit;

namespace SneakersShop.Tests.SOLID;

/*
        SOLID Principles

    S - Single responsibility Principle (Princip jedne vrednosti)
    o - Open / Closed Principle
    L - Liskov Substitution Principle
    I - Interface seggregation Principle
    D - Dependency Inversion Principle

*/
public class SolidTests
{
    [Fact]
    public void PaymentProccesorThrowsException_WhenPaymentFails()
    {
        var proccesor = new OrderProcessor(new TestPaymentMethod());
        var order = Order;

        Action a = () => proccesor.ProcessOrder(order);
        a.Should().ThrowExactly<Exception>().WithMessage("Placanje neuspesno");
        proccesor.emailSent.Should().BeFalse();
    }

    [Fact]
    public void EmailSent_WhenPaymentDoesntFail()
    {
        var mock = new Mock<IPaymentMethod>();

        mock.Setup(x => x.Pay(360)).Returns(true);

        var paymentMethod = mock.Object;
        var proccesor = new OrderProcessor(paymentMethod);

        var order = Order;

        Action a = () => proccesor.ProcessOrder(order);

        a.Should().NotThrow();
        proccesor.emailSent.Should().BeTrue();

        mock.Verify(x => x.Pay(360), Times.Once);
    }

    public class TestPaymentMethod : IPaymentMethod
    {
        public bool Pay(decimal amount)
        {
            return false;
        }
    }

    public Order Order => new Order
    {
        Lines = new List<OrderLine>
        {
            new OrderLine
            {
                Name = "OL 1",
                Price = 100,
                Quantity = 3
            },
            new OrderLine
            {
                Name = "OL 2",
                Price = 30,
                Quantity = 2
            },
        }
    };
}
