using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageEditor.Core
{
	public class Features
	{
		public async Task<object> ConvertToGreyscale( dynamic input )
		{
			using ( var stream = new MemoryStream( ToBytes( (string)input.base64String ) ) )
			using ( Image<Rgba32> image = Image.Load( stream, new JpegDecoder() ) )
			{
				MutateGreyscale( image );
				return ToBase64String( image );
			}
		}

		public async Task<object> Rotate( dynamic input )
		{
			var rotateMode = (bool)input.isClockwise ? RotateMode.Rotate90 : RotateMode.Rotate270;

			using ( var stream = new MemoryStream( ToBytes( (string)input.base64String ) ) )
			using ( Image<Rgba32> image = Image.Load( stream, new JpegDecoder() ) )
			{
				MutateRotate( image, rotateMode );
				return ToBase64String( image );
			}
		}

		public async Task<object> Crop( dynamic input )
		{
			var rectangle = new Rectangle( (int)input.offsetX, (int)input.offsetY, (int)input.width, (int)input.height );

			using ( var stream = new MemoryStream( ToBytes( (string)input.base64String ) ) )
			using ( Image<Rgba32> image = Image.Load( stream, new JpegDecoder() ) )
			{
				MutateCrop( image, rectangle, (int)input.originalWidth, (int)input.originalHeight );
				return ToBase64String( image );
			}
		}

		[Demoapp_10.Features.Feature1]
		void MutateGreyscale( Image<Rgba32> image ) => image.Mutate( x => x.Grayscale() );

		[Demoapp_10.Features.Feature2]
		void MutateRotate( Image<Rgba32> image, RotateMode rotateMode ) => image.Mutate( x => x.Rotate( rotateMode ) );

		[Demoapp_10.Features.Feature3]
		void MutateCrop( Image<Rgba32> image, Rectangle rectangle, int w, int h ) => image.Mutate( x => x.Resize( w, h ).Crop( rectangle ) );

		byte[] ToBytes( string base64String ) => Convert.FromBase64String( base64String );

		string ToBase64String( Image<Rgba32> image )
		{
			using ( var outputStream = new MemoryStream() )
			{
				image.Save( outputStream, new JpegEncoder() );
				var bytes = outputStream.ToArray();
				return Convert.ToBase64String( bytes );
			}
		}
	}
}
