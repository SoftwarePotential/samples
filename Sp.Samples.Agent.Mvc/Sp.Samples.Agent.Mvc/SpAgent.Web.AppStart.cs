using Sp.Agent.Configuration.Web;

// Wire the configuration verification into Application Startup. 
// NB if you need to remove this attribute, you should ensure the verification continues to be guaranteed 
// (e.g. one could inline the code into your Global.asax.cs Application_Start method)
[assembly: WebActivatorEx.PreApplicationStartMethod( typeof( SpAgentStoresVerification ), "Start" )]

namespace Sp.Agent.Configuration.Web
{
	/// <summary>
	/// Verifies that all storage/configuration prerequisites of the Sp.Agent 
	/// libraries have been fulfilled.
	/// </summary>
	/// <remarks>
	/// NB Any problems will result in an Exception being bubbled up to the 
	/// ASP.NET Application Initialization error handling. This will normally 
	/// prevent the Application from starting.
	/// 
	/// Removing this check is NOT recommended as, without such a functioning 
	/// store, it will be impossible to:
	/// - Activate licenses
	/// - examine any License state 
	/// - Execute Licensed Code 
	/// 
	/// Note also that any problems flagged by the verification process 
	/// will almost certainly require developer/operator intervention, e.g.:
	/// - Identifying why App_Data\Licenses is not present and/or discrepancies in the file access permissions of the Applucation Pool on the host machine
	/// - Identifying issues with the Web Deploy (MSDeploy) package, i.e., inadvertent removal of the required base directory placeholder file
	/// </remarks>
	static class SpAgentStoresVerification
	{
		/// <summary>
		/// Ensures that the Sp.Agent License Store is initialized and accessible or fails the Application startup.
		/// </summary>
		static void Start()
		{
			SpAgent.Configuration.VerifyStoresInitialized();
		}
	}
}