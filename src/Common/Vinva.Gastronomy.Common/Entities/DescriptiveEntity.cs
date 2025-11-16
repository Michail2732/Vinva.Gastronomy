using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Entities
{
    public abstract class DescriptiveEntity : Entity, IDescriptiveEntity
    {
        private string? _comment;
        private string _name;
        private string _description;


        public string Name
        {
            get => _name;
            set => SetName(value, ref _name);
        }

        public string Description
        {
            get => _description;
            set => SetDescription(value, ref _description);
        }

        public string? Comment
        {
            get => _comment;
            set => SetComment(value, ref _comment);
        }


#pragma warning disable CS8618
        protected DescriptiveEntity() { }
#pragma warning restore CS8618 

        protected DescriptiveEntity(string name, string description)
        {
            _name = "";
            _description = "";
            SetName(name, ref _name!);
            SetDescription(description, ref _description);            
        }

        public abstract override int GetHashCode();

        public override bool Equals(object? obj)
        {
            return Equals(obj as IEntity);
        }


        protected void SetName(string name, ref string field)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"\"{nameof(name)}\" не может быть неопределенным или пустым.", nameof(name));
            }
            if (name.Length > CommonConstants.MaxLengthName)
                throw new EntityDomainException(GetType(), CommonErrorMessages.NameOverflow(name));
            field = name;
        }

        protected void SetDescription(string description, ref string field)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"\"{nameof(description)}\" не может быть неопределенным или пустым.", nameof(description));
            }
            if (description.Length > CommonConstants.MaxLengthDescription)
                throw new EntityDomainException(GetType(), CommonErrorMessages.DescriptionOverflow(description));
            field = description;
        }

        protected void SetComment(string? comment, ref string? commentField)
        {
            if (comment?.Length > CommonConstants.MaxLengthComment)
                throw new EntityDomainException(GetType(), CommonErrorMessages.CommentOverflow(comment));
            commentField = comment;
        }        
    }
}
