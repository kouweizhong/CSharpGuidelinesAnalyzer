using System.Diagnostics;
using CSharpGuidelinesAnalyzer.Rules.Documentation;
using CSharpGuidelinesAnalyzer.Test.TestDataBuilders;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace CSharpGuidelinesAnalyzer.Test.Specs.Documentation
{
    public sealed class AvoidInlineCommentsSpecs : CSharpGuidelinesAnalysisTestFixture
    {
        protected override string DiagnosticId => AvoidInlineCommentsAnalyzer.DiagnosticId;

        [Fact]
        internal void When_method_body_contains_single_line_comment_it_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
                        [|// Example|]
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Code blocks should not contain inline comments.");
        }

        [Fact]
        internal void When_method_body_contains_multi_line_comment_it_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
                        [|/* Example
                        block
                        of text*/|]
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Code blocks should not contain inline comments.");
        }

        [Fact]
        internal void When_method_contains_documentation_comment_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    /// <summary>...</summary>
                    void M()
                    {
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_body_contains_preprocessor_directive_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
#if DEBUG
                        Console.WriteLine(""Debug mode"");
#else
                        Console.WriteLine(""Release mode"");
#endif
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_body_contains_region_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
                        #region Example
                        int i = 4;
                        #endregion
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_body_contains_pragma_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
#pragma warning disable CS0219
                        int i = 4;
#pragma warning restore
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_property_getter_contains_multiple_comments_they_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public string P
                    {
                        get
                        {
                            [|// comment |]

                            return null;

                            [|/* unreachable */|]
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Code blocks should not contain inline comments.",
                "Code blocks should not contain inline comments.");
        }

        [Fact]
        internal void When_method_contains_leading_comment_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    // some
                    void M()
                    {
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_contains_trailing_comment_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
                    }
                    // some
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_body_contains_Resharper_inspections_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M()
                    {
                        string s = null;

                        // ReSharper disable PossibleNullReferenceException
                        if (s.Length > 1)
                        // ReSharper restore PossibleNullReferenceException
                        {
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_body_contains_Arrange_Act_Assert_pattern_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .Using(typeof(Debug).Namespace)
                .InDefaultClass(@"
                    void UnitTest()
                    {
                        // Arrange
                        int x = 10;

                        // Act
                        x -= 1;

                        // Assert
                        Debug.Assert(x == 9);

                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_method_body_contains_simplified_Arrange_Act_Assert_pattern_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .Using(typeof(Debug).Namespace)
                .InDefaultClass(@"
                    void UnitTest()
                    {
                        // Arrange
                        int x = 10;

                        // Act and assert
                        Debug.Assert(--x == 9);
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new AvoidInlineCommentsAnalyzer();
        }
    }
}
