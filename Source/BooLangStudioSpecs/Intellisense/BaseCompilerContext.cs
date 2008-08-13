namespace Boo.BooLangStudioSpecs.Intellisense
{
    public abstract class BaseCompilerContext
    {
        private readonly FixtureCompiler fixtures = new FixtureCompiler();

        protected CompilationOutput CompiledFixtures
        {
            get { return fixtures.CompileForCurrentMethod(); }
        }
    }
}