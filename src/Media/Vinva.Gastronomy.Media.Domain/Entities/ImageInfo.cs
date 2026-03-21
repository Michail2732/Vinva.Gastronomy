using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Media.Domain.Errors;
using Vinva.Gastronomy.Media.Domain.Exceptions;

namespace Vinva.Gastronomy.Media.Domain.Entities
{
    public record ImageInfo
    {        
        public string Format { get; }
        public int Width { get; }
        public int Height { get; }
        public int Quality { get; }

        public ImageInfo(string format, int width, int height, int quality)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(format);            
            Format = format;
            if (!ValidationService.ValidateName(Format))
                throw new MediaDomainException(MediaErrorMessages.IncorrectImageFormat(Format));
            Width = width;
            Height = height;
            Quality = quality;
        }

        public string GetStringPath() => ToString();

        public static ImageInfo Parce(string str)
        {                        
            try
            {
                var splitedResult = str.Split(str, StringSplitOptions.RemoveEmptyEntries);                
                var format = splitedResult[0];
                var width = int.Parse(splitedResult[1]);
                var height = int.Parse(splitedResult[2]);
                var quality = int.Parse(splitedResult[3]);
                return new ImageInfo(format, width, height, quality);                                
            }
            catch (Exception ex)
            {
                throw new MediaDomainException(MediaErrorMessages.IncorrectRawImagePath(str), ex);
            }
        }

        public static bool TryParce(string str, out ImageInfo? path)
        {
            path = null;
            try
            {
                path = Parce(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Format}_{Width}_{Height}_{Quality}";
        }        
    }
}
