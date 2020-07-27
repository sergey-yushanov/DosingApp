using DosingApp.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DosingApp.Services
{
    public class DBDataAccess<T> where T : class
    {
        private readonly AppDbContext _context;

        public DBDataAccess() => _context = App.GetContext();

        // Создание элемента БД
        public bool Create(T entity)
        {
            bool created;

            try
            {
                _context.Entry(entity).State = EntityState.Added;
                _context.SaveChanges();

                created = true;
            }
            catch (Exception)
            {
                throw;
            }

            return created;
        }

        // Получаем набор элементов из БД
        public IEnumerable<T> Get()
        {
            return _context.Set<T>().AsEnumerable();
        }

        // Получаем набор элементов из БД по условию
        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsEnumerable<T>();
        }

        // Получаем набор элементов из БД по условиям where, orderby
        public IEnumerable<T> Get(Expression<Func<T, bool>> whereCondition = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        // Обновляем запись в БД
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Attach(entity);
            _context.SaveChanges();
        }

        // Удаляем запись из БД
        public void Delete(T entity)
        {
            T existing = _context.Set<T>().Find(entity);

            if (existing != null)
            {
                _context.Set<T>().Remove(existing);
                _context.SaveChanges();
            }
        }

        // Сохраняем набор записей в БД
        public async void SaveList(List<T> list)
        {
            try
            {
                foreach (var record in list)
                {
                    _context.Add(record);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Удаляем набор записей из БД
        public async void DeleteList(List<T> list)
        {
            try
            {
                foreach (var record in list)
                {
                    _context.Remove(record);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
