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
            set => _name = string.IsNullOrEmpty(value) ? value : throw new ArgumentNullException();
        }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrEmpty(value) ? value : throw new ArgumentNullException();
        }

        public string? Comment
        {
            get => _comment;
            set => _comment = value;
        }


#pragma warning disable CS8618
        protected DescriptiveEntity() { }
#pragma warning restore CS8618 

        protected DescriptiveEntity(string name, string description)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(description);
            _name = name;
            _description = description;
        }

        public abstract override int GetHashCode();

        public override bool Equals(object? obj)
        {
            return Equals(obj as IEntity);
        }       
    }
}
