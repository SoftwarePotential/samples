' NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
'
' CONSIDER RENAMING OR MOVING THIS FILE SO A PACKAGE UPDATE CANNOT UNDO ANY CHANGES YOU MAKE
Imports Sp.Agent
Imports Sp.Agent.Configuration.Product.Activation
Imports System.Diagnostics

Partial Class SpAgent
    ''' <summary>
    ''' 
    ''' <para>TODO: YOUR CUSTOMIZATIONS OR DELETE THIS METHOD</para>
    ''' 
    ''' <para>Should be edited as appropriate if you wish to customize any aspects of how your Licensing policies should affect your application</para>
    ''' </summary>
    ''' <remarks>
    ''' NB the name and namespace of the class needs to remain as-is in order for partial method to slot into the code in SpAgent.cs correctly
    ''' </remarks>
    Private Shared Sub ConfigureProduct(productContext As IProductContext)
        '=====================================================================================
        ' TODO: Tweak any settings as desired 
        ' (OR YOU CAN DELETE THIS METHOD TO HAVE THE DEFAULT CONFIGURATION BE APPLIED INSTEAD)
        '=====================================================================================
        productContext.Configure(Function(configure) configure _
            .Activation.Customize(Function(activation) activation _
                .WithTagging(AddressOf AddActivationTags) _
                .WithTransmission(Function(activationTransmission) activationTransmission _
                    .WithRetryPolicyDefault() _
                    .WithEndpointSelectionPolicyDefault() _
                    .BeforeEachAttempt(AddressOf WhenActivating) _
                        .CompleteWithDefaults()) _
                    .CompleteWithDefaults()) _
            .CompleteWithDefaults())
    End Sub
    Shared Sub AddActivationTags(context As IActivationTaggingContext)
        Debug.WriteLine("State passed to Activate() method: " & CStr(context.State))
        ' e.g. context.AddTag("MYKEY"."MYVALUE");
    End Sub
    Shared Sub WhenActivating(context As IActivationAttemptContext)
        Debug.WriteLine("Activation attempt #" & CStr(context.PreviousAttempts + 1))
    End Sub
End Class