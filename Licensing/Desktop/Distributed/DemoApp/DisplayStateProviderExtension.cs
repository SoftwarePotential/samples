using System;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using DemoApp.Common;

namespace DemoApp
{
	[MarkupExtensionReturnType( typeof( Func<IDisplayState> ) )]
	public class DisplayStateProviderExtension : MarkupExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var rootObjectProvider = (IRootObjectProvider)serviceProvider.GetService( typeof( IRootObjectProvider ) );
			var root = (DependencyObject)rootObjectProvider.RootObject;
			return new Func<IDisplayState>( () => (IDisplayState) Window.GetWindow( root ) );
		}
	}
}