﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;
#pragma warning disable CS0618 // Type or member is obsolete
public class RazorEngineBuilderExtensionsTest
{
    [Fact]
    public void AddDirective_ExistingFeature_UsesFeature()
    {
        // Arrange
        var expected = new DefaultRazorDirectiveFeature();
        var engine = RazorEngine.CreateEmpty(b =>
        {
            b.Features.Add(expected);

                // Act
                b.AddDirective(DirectiveDescriptor.CreateDirective("test", DirectiveKind.SingleLine));
        });

        // Assert
        var actual = Assert.Single(engine.Features.OfType<IRazorDirectiveFeature>());
        Assert.Same(expected, actual);

        var directive = Assert.Single(actual.Directives);
        Assert.Equal("test", directive.Directive);
    }

    [Fact]
    public void AddDirective_NoFeature_CreatesFeature()
    {
        // Arrange
        var engine = RazorEngine.CreateEmpty(b =>
        {
                // Act
                b.AddDirective(DirectiveDescriptor.CreateDirective("test", DirectiveKind.SingleLine));
        });

        // Assert
        var actual = Assert.Single(engine.Features.OfType<IRazorDirectiveFeature>());
        Assert.IsType<DefaultRazorDirectiveFeature>(actual);

        var directive = Assert.Single(actual.Directives);
        Assert.Equal("test", directive.Directive);
    }

    [Fact]
    public void AddTargetExtensions_ExistingFeature_UsesFeature()
    {
        // Arrange
        var extension = new MyTargetExtension();

        var expected = new DefaultRazorTargetExtensionFeature();
        var engine = RazorEngine.CreateEmpty(b =>
        {
            b.Features.Add(expected);

                // Act
                b.AddTargetExtension(extension);
        });

        // Assert
        var actual = Assert.Single(engine.Features.OfType<IRazorTargetExtensionFeature>());
        Assert.Same(expected, actual);

        Assert.Same(extension, Assert.Single(actual.TargetExtensions));
    }

    [Fact]
    public void AddTargetExtensions_NoFeature_CreatesFeature()
    {
        // Arrange
        var extension = new MyTargetExtension();

        var engine = RazorEngine.CreateEmpty(b =>
        {
                // Act
                b.AddTargetExtension(extension);
        });

        // Assert
        var actual = Assert.Single(engine.Features.OfType<IRazorTargetExtensionFeature>());
        Assert.IsType<DefaultRazorTargetExtensionFeature>(actual);

        Assert.Same(extension, Assert.Single(actual.TargetExtensions));
    }

    private class MyTargetExtension : ICodeTargetExtension
    {
    }
}
