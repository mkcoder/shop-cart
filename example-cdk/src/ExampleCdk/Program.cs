using Amazon.CDK;

namespace ExampleCdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new ExampleCdkStack(app, "ExampleCdkStack");

            app.Synth();
        }
    }
}
