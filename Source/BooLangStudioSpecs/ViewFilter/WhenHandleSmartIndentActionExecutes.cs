using Boo.BooLangService;
using BooLangService;
using Microsoft.VisualStudio.TextManager.Interop;
using Rhino.Mocks;
using Xunit;

namespace BooLangStudioSpecs.ViewFilter
{
    public class WhenHandleSmartIndentActionExecutes
    {
        private const int OK = 1;
        private const int LineNumber = 2;
        private const int LastCharColumn = 0;

        [Fact]
        public void PositionOfCaretIsRetrievedFromTheView()
        {
            var view = MockRepository.GenerateMock<IVsTextView>();
            var indenter = MockRepository.GenerateStub<ILineIndenter>();

            int line, col;
            view.Expect(x => x.GetCaretPos(out line, out col))
                .IgnoreArguments()
                .Return(OK);

            new HandleSmartIndentAction(view, indenter)
                .Execute();

            view.VerifyAllExpectations();
        }

        [Fact]
        public void LineFromViewIsPassedToIndenter()
        {
            var view = MockRepository.GenerateStub<IVsTextView>();
            var indenter = MockRepository.GenerateMock<ILineIndenter>();

            int line, col;
            view.Stub(x => x.GetCaretPos(out line, out col))
                .OutRef(10, 0)
                .Return(OK);

            indenter.Expect(x => x.SetIndentationForNextLine(10));

            new HandleSmartIndentAction(view, indenter)
                .Execute();

            indenter.VerifyAllExpectations();
        }

        [Fact]
        public void MethodWithZeroIndentHasNextLineIndentedByOne()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("def Method():");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, "\t"));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void MethodWithOneIndentHasNextLineIndentedByTwo()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("\tdef Method():");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, "\t\t"));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void PassWithNoIndentHasNextLineWithNoIndent()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("pass");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, ""));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void PassIndentedByOneHasNextLineWithNoIndent()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("\tpass");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, ""));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void PassIndentedByTwoHasNextLineWithOneIndent()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("\t\tpass");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, "\t"));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void ReturnWithNoIndentHasNextLineWithNoIndent()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("return");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, ""));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void ReturnIndentedByOneHasNextLineWithNoIndent()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("\treturn");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, ""));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }

        [Fact]
        public void ReturnIndentedByTwoHasNextLineWithOneIndent()
        {
            var source = MockRepository.GenerateMock<ISource>();
            var view = MockRepository.GenerateStub<IVsTextView>();

            source.Stub(x => x.GetLine(LineNumber - 1)).Return("\t\treturn");
            source.Stub(x => x.ScanToNonWhitespaceChar(LineNumber)).Return(LastCharColumn);
            source.Stub(x => x.UseTabs).Return(true);

            // expect call to SetText with one tab indent
            source.Expect(x => x.SetText(LineNumber, LastCharColumn, "\t"));

            new LineIndenter(source, view)
                .SetIndentationForNextLine(LineNumber);

            source.VerifyAllExpectations();
        }
    }
}