using Domain.Attributes;
using Domain.Revoke;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ApplicationUser : IdentityUser<Guid>, IRevoke
    {
        /// <summary>
        /// Уникальный Логин (запрещены все символы кроме латинских букв и цифр)
        /// </summary>
        [JsonPatchAllow]
        public string Login { get; set; }
        /// <summary>
        /// Имя (запрещены все символы кроме латинских и русских букв)
        /// </summary>
        [JsonPatchAllow]
        public string Name { get; set; }
        /// <summary>
        /// Пол 0 - женщина, 1 - мужчина, 2 - неизвестно
        /// </summary>
        [JsonPatchAllow]
        public int Gender { get; set; }
        /// <summary>
        /// поле даты рождения может быть Null
        /// </summary>
        [JsonPatchAllow]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// является ли пользователь админом
        /// </summary>
        [JsonPatchAllow]
        public bool IsAdmin { get; set; }
        /// <summary>
        /// Дата создания пользователя
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Логин Пользователя, от имени которого этот пользователь создан
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Дата изменения пользователя
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
        /// <summary>
        ///  Логин Пользователя, от имени которого этот пользователь изменён
        /// </summary>
        public string? ModifiedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        /// <summary>
        /// Логин Пользователя, от имени которого этот пользователь удалён
        /// </summary>
        public string? RevokedBy { get; set; }
        public bool IsRevoked { get; set; }
    }

    public enum Gender : int
    {
        Female = 0,
        Male = 1,
        Undefined = 2,
    }
}
