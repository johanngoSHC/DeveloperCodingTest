namespace DeveloperCodingTest.Tests.UnitTests.DeveloperCodingTests.Core.Extensions;

using Ardalis.GuardClauses;
using DeveloperCodingTest.Core.Exceptions;
using DeveloperCodingTest.Core.Extensions;
using FakeItEasy;

public class GuardExtensionsTests
{
    private readonly IGuardClause _fakeGuardClause;

    public GuardExtensionsTests()
    {
        this._fakeGuardClause = A.Fake<IGuardClause>();
    }

    [Theory]
    [InlineData(-44)]
    [InlineData(-1)]
    public void InvalidStoryId_ShouldThrowException_WhenIdIsZeroOrNegative(int invalidId)
    {
        // Act & Assert
        Assert.Throws<InvalidStoryException>(() => _fakeGuardClause.InvalidStoryId(invalidId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void InvalidStoryScore_ShouldThrowException_WhenScoreIsZeroOrNegative(int invalidScore)
    {
        // Act & Assert
        Assert.Throws<InvalidStoryException>(() => _fakeGuardClause.InvalidStoryScore(invalidScore));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void InvalidStoryUri_ShouldThrowException_WhenUriIsNullEmptyOrWhitespace(string? invalidUri)
    {
        // Act & Assert
        Assert.Throws<InvalidStoryException>(() => _fakeGuardClause.InvalidStoryUri(invalidUri));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("2023-10-10 10:10:10")]
    public void InvalidStoryDateTime_ShouldThrowException_WhenTimeIsInvalid(string? invalidTime)
    {
        // Act & Assert
        Assert.Throws<InvalidStoryException>(() => _fakeGuardClause.InvalidStoryDateTime(invalidTime));
    }
}