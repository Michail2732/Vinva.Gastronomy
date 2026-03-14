using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Constants
{
    public class CommonErrorMessages
    {
        public static string DescriptionOverflow(string description) => $"Описание не должно быть длинее {CommonConstants.MaxLengthDescription}:\n'{description}'";
        public static string CommentOverflow(string comment) => $"Описание не должно быть длинее {CommonConstants.MaxLengthComment}:\n'{comment}'";
        public static string NameOverflow(string name) => $"Имя не должно быть длинее {CommonConstants.MaxLengthComment}:\n'{name}'";
        public const string IncorrectName = $"Некорректное значение наименования.";
        public const string IncorrectDescription = $"Некорректное значение описания.";
        public const string IncorrectComment = $"Некорректное значение комментария.";
    }
}
