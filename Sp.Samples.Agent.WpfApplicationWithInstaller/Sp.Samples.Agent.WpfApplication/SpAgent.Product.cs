namespace Sp.Samples.Agent.WpfApplication
{
	partial class SpAgent
	{
		// Used as a subdirectory prefix within:
		// 1) Environment.SpecialFolder.ProgramFiles (Shell: %ProgramFiles%)
		// 2) Environment.SpecialFolder.CommonApplicationData (Shell: %ProgramData% or %ALLUSERSPROFILE%)
		// See SpAgent.SharedDirectoryInitializedByInstaller
		public const string ProductDirectoryPrefix = "Software Potential";

		// NOTE: The values here need to match those in <ProjectName>.SLMCfg 
		// NOTE: These values need to match those of your Product definition in softwarepotential.com for the licenses generated/activated there to be valid
		public const string ProductName = "WPF Sample";
		public const string ProductVersion = "1.0";
	}
}