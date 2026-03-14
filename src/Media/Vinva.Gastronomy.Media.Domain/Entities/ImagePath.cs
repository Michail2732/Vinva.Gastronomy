using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Validations;
using Vinva.Gastronomy.Media.Domain.Errors;
using Vinva.Gastronomy.Media.Domain.Exceptions;

namespace Vinva.Gastronomy.Media.Domain.Entities
{
    public record ImagePath
    {
        public Guid ImageId { get; }
        public string Format { get; }
        public int Width { get; }
        public int Height { get; }
        public int Quality { get; }

        public ImagePath(Guid imageId, string format, int width, int height, int quality)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(format);
            ImageId = imageId;
            Format = format;
            if (!DescriptiveEntityValidator.ValidateName(Format))
                throw new MediaDomainException(MediaErrorMessages.IncorrectImageFormat(Format));
            Width = width;
            Height = height;
            Quality = quality;
        }

        public string GetStringPath() => ToString();

        public static ImagePath Parce(string str)
        {                        
            try
            {
                var splitedResult = str.Split(str, StringSplitOptions.RemoveEmptyEntries);
                var id = Guid.Parse(splitedResult[0]);
                var format = splitedResult[1];
                var width = int.Parse(splitedResult[2]);
                var height = int.Parse(splitedResult[3]);
                var quality = int.Parse(splitedResult[4]);
                return new ImagePath(id, format, width, height, quality);                                
            }
            catch (Exception ex)
            {
                throw new MediaDomainException(MediaErrorMessages.IncorrectRawImagePath(str), ex);
            }
        }

        public static bool TryParce(string str, out ImagePath? path)
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
            return $"{ImageId}_{Format}_{Width}_{Height}_{Quality}";
        }        
    }
}
