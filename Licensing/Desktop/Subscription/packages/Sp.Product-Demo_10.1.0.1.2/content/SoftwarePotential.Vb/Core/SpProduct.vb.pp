' NB This file is auto-generated via the Sp.Product-Demo_10 NuGet package.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Module SpProduct
    Public Const Vendor As String = "InishTech"
    Public Const Name As String = "Demo"
    Public Const Version As String = "1.0"
End Module
'''<summary>Defines features and properties associated with the Software Potential Product Definition for 'Demo' - '1.0'</summary>
Class [Demo_10]
    ''' <summary>Defines Attributes used to mark code for Protection by the Software Potential Code Protector, together with the (string) Names of the features for the purposes of generating custom licensing queries.</summary>
    Partial Class Features
#Region "Feature4"
            ''' <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature4' Feature to be present.</summary>
            <System.AttributeUsage(System.AttributeTargets.Constructor Or System.AttributeTargets.Method)>
            Public Class [Feature4]
                Inherits LicensedFeatureAttribute
                Public Const Name As String = "Feature4"
                Public Sub New()
                    MyBase.New(Name)
                End Sub
            End Class
#End Region
#Region "Feature2"
            ''' <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature2' Feature to be present.</summary>
            <System.AttributeUsage(System.AttributeTargets.Constructor Or System.AttributeTargets.Method)>
            Public Class [Feature2]
                Inherits LicensedFeatureAttribute
                Public Const Name As String = "Feature2"
                Public Sub New()
                    MyBase.New(Name)
                End Sub
            End Class
#End Region
#Region "Feature1"
            ''' <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature1' Feature to be present.</summary>
            <System.AttributeUsage(System.AttributeTargets.Constructor Or System.AttributeTargets.Method)>
            Public Class [Feature1]
                Inherits LicensedFeatureAttribute
                Public Const Name As String = "Feature1"
                Public Sub New()
                    MyBase.New(Name)
                End Sub
            End Class
#End Region
#Region "Feature3"
            ''' <summary>Require a License for 'Demo' - '1.0' with the 'Name: Feature3' Feature to be present.</summary>
            <System.AttributeUsage(System.AttributeTargets.Constructor Or System.AttributeTargets.Method)>
            Public Class [Feature3]
                Inherits LicensedFeatureAttribute
                Public Const Name As String = "Feature3"
                Public Sub New()
                    MyBase.New(Name)
                End Sub
            End Class
#End Region
#Region "LicensedFeatureAttribute"
        ''' <summary>
        ''' <para>Marks a method to be replaced with Protected code by the Software Potential Code Protector during the build process.</para>
        ''' <para>Typically one should use the specific Feature attributes in this <c>Features</c> class in preference to using this directly.</para>
        ''' <para>Updating the NuGet package associated with this file will pick up new Features.</para>
        ''' </summary>
        ''' <remarks>This class fulfills two key requirements in order to be identified by the Software Potential Code Protector versions >= 3.2.1942 as a Protection Attribute just as Slps.ProtectionAttributes.FeatureAttribute is:
        ''' - class name is LicensedFeatureAttribute (even after Obfuscation)
        ''' - bears a read/write property called FeatureName which has a value matching that in the Software Potential Product Definition (even after Obfuscation)
        ''' </remarks>
        <System.AttributeUsage(System.AttributeTargets.Constructor Or System.AttributeTargets.Method)>
        <System.Reflection.Obfuscation>
        Public MustInherit Class LicensedFeatureAttribute
            Inherits System.Attribute
            Protected Sub New(featureName As String)
                Me.FeatureName = featureName
            End Sub

            <System.Reflection.Obfuscation>
            Public Property FeatureName As String
        End Class
#End Region
    End Class
#Region "LicenseAttribute"
    ''' <summary>Require any License for 'Demo' - '1.0' to be present.</summary>
    <System.AttributeUsage(System.AttributeTargets.Constructor Or System.AttributeTargets.Method)>
    Public NotInheritable Class LicenseAttribute
        Inherits Features.LicensedFeatureAttribute
        Sub New()
            MyBase.New(String.Empty)
        End Sub
    End Class
#End Region
End Class