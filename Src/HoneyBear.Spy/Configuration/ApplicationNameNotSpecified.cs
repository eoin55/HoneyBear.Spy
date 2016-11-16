using System;

namespace HoneyBear.Spy.Configuration
{
    public sealed class ApplicationNameNotSpecified : Exception
    {
        private static string ExceptionMessage =>
            @"
Application Name cannot be resolved.  Please add an appsetting key like the following:

<add key=""HoneyBear.Spy.ApplicationName""
	 value=""HoneyBear.Spy.Sample.Host.Web""/>";

        public ApplicationNameNotSpecified()
            : base(ExceptionMessage)
        {
            
        }
    }
}