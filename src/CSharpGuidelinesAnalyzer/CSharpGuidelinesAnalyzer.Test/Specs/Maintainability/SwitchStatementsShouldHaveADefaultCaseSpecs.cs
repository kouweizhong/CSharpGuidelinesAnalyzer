using CSharpGuidelinesAnalyzer.Rules.Maintainability;
using CSharpGuidelinesAnalyzer.Test.TestDataBuilders;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace CSharpGuidelinesAnalyzer.Test.Specs.Maintainability
{
    public sealed class SwitchStatementsShouldHaveADefaultCaseSpecs : CSharpGuidelinesAnalysisTestFixture
    {
        protected override string DiagnosticId => SwitchStatementsShouldHaveADefaultCaseAnalyzer.DiagnosticId;

        [Fact]
        internal void When_switch_statement_type_is_bool_and_contains_a_default_case_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool b)
                    {
                        switch (b)
                        {
                            default:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_bool_and_is_complete_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool b)
                    {
                        switch (b)
                        {
                            case true:
                                return;
                            case false:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_bool_and_is_incomplete_it_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool b)
                    {
                        [|switch (b)
                        {
                            case false:
                                throw new NotImplementedException();
                        }|]
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Incomplete switch statement without a default case clause.");
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_bool_and_contains_a_default_case_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool? b)
                    {
                        switch (b)
                        {
                            default:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_bool_and_is_complete_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool? b)
                    {
                        switch (b)
                        {
                            case true:
                            case null:
                                return;
                            case false:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_bool_and_is_incomplete_it_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool? b)
                    {
                        [|switch (b)
                        {
                            case true:
                            case false:
                                throw new NotImplementedException();
                        }|]
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Incomplete switch statement without a default case clause.");
        }

        [Fact]
        internal void When_switch_statement_type_is_flags_enum_and_is_incomplete_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    [Flags]
                    public enum Status { Pending, Active, Completed }

                    void M(Status s)
                    {
                        switch (s)
                        {
                            default:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_enum_and_contains_a_default_case_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public enum Status { Pending, Active, Completed }

                    void M(Status s)
                    {
                        switch (s)
                        {
                            default:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_enum_and_is_complete_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public enum Status { Pending, Active, Completed }

                    void M(Status s)
                    {
                        switch (s)
                        {
                            case Status.Pending:
                                return;
                            case Status.Active:
                            case Status.Completed:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_enum_and_is_incomplete_it_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public enum Status { Pending, Active, Completed }

                    void M(Status s)
                    {
                        [|switch (s)
                        {
                            case Status.Pending:
                            case Status.Active:
                                throw new NotImplementedException();
                        }|]
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Incomplete switch statement without a default case clause.");
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_enum_and_contains_a_default_case_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public enum Status { Pending, Active, Completed }

                    void M(Status? s)
                    {
                        switch (s)
                        {
                            default:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_enum_and_is_complete_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public enum Status { Pending, Active, Completed }

                    void M(Status? s)
                    {
                        switch (s)
                        {
                            case null:
                            case Status.Pending:
                                return;
                            case Status.Active:
                            case Status.Completed:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_enum_and_is_incomplete_it_must_be_reported()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    public enum Status { Pending, Active, Completed }

                    void M(Status? s)
                    {
                        [|switch (s)
                        {
                            case Status.Pending:
                            case Status.Active:
                            case Status.Completed:
                                throw new NotImplementedException();
                        }|]
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source,
                "Incomplete switch statement without a default case clause.");
        }

        [Fact]
        internal void When_switch_statement_type_is_nullable_byte_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(byte? b)
                    {
                        switch (b)
                        {
                            case 0xF0:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_char_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(char c)
                    {
                        switch (c)
                        {
                            case 'A':
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_int_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(int i)
                    {
                        switch (i)
                        {
                            case 5:
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_type_is_string_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(string s)
                    {
                        switch (s)
                        {
                            case ""X"":
                                throw new NotImplementedException();
                        }
                    }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_contains_a_nonconstant_case_expression_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .InDefaultClass(@"
                    void M(bool b)
                    {
                        switch (b)
                        {
                            case true && true:
                                throw new NotImplementedException();
                        }

                        switch (b)
                        {
                            case !true:
                                throw new NotImplementedException();
                        }
                   }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        [Fact]
        internal void When_switch_statement_is_invalid_it_must_be_skipped()
        {
            // Arrange
            ParsedSourceCode source = new MemberSourceCodeBuilder()
                .AllowingCompileErrors()
                .InDefaultClass(@"
                    void M(bool b)
                    {
                        switch (b)
                        {
                            case true:
                                throw new NotImplementedException();
                            case true:
                                throw new NotImplementedException();
                        }
                   }
                ")
                .Build();

            // Act and assert
            VerifyGuidelineDiagnostic(source);
        }

        protected override DiagnosticAnalyzer CreateAnalyzer()
        {
            return new SwitchStatementsShouldHaveADefaultCaseAnalyzer();
        }
    }
}
