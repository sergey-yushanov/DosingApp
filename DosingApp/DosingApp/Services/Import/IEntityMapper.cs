using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DosingApp.Services.Import
{
    public interface IEntityMapper<TDto, TEntity, TKey>
        where TEntity : class
    {
        /// Ключ из DTO
        TKey GetKeyFromDto(TDto dto);

        /// Выражение для EF: e => e.Key  (для запроса существующих)
        Expression<Func<TEntity, TKey>> KeySelector { get; }

        /// Создать новую сущность из DTO
        TEntity CreateEntity(TDto dto);

        /// Обновить существующую сущность из DTO (ключ не трогать)
        void UpdateEntity(TEntity entity, TDto dto);

        /// Валидна ли DTO (например, ключ не пустой)
        bool IsValid(TDto dto);
    }
}
