using Income.Common;
using Income.Database.Models.Common;
using Income.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database.Queries
{
    public class Repository 
    {
        private readonly Database _database;
        private readonly ILoggingService _logService;
        public Repository(Database _database, ILoggingService _logService) 
        {
            this._database = _database;   
            this._logService = _logService;

        }
        /// <summary>
        /// that every entity saved to the database is valid, consistent, and complete — in ONE place.......
        /// Avoid duplicate code, keep data consistent, and guarantee valid entities before saving.
        /// dont need to wite this many times.
        /// </summary>
        /// <param name="entity"></param>
        private void ApplyDefaults(Tbl_Base entity)
        {
            if (entity.id == Guid.Empty)
                entity.id = Guid.NewGuid();

            entity.survey_timestamp ??= DateTime.Now;
            entity.survey_coordinates ??= SessionStorage.location;
            entity.user_id ??= SessionStorage.__user_id;
            entity.tenant_id ??= SessionStorage.tenant_id;
        }

        public async Task<int> InsertAsync<T>(T entity)
            where T : Tbl_Base, new()
        {
            try
            {
                if (entity == null)
                    return 0;

                ApplyDefaults(entity);
                return await _database.Connection.InsertAsync(entity);
            }
            catch (Exception ex)
            {
               
                await _logService.LogError($"Insert failed for {typeof(T).Name}", ex);
                return 0;
            }
        }//insert single

        public async Task<int> SaveAsync<T>(T entity)
            where T : Tbl_Base, new()
        {
            try
            {
                if (entity == null)
                    return 0;

                ApplyDefaults(entity);

                return await _database.Connection.InsertOrReplaceAsync(entity);// UPSERT  handles insert/update
            }
            catch (Exception ex)
            {
                await _logService.LogError($"Save failed for {typeof(T).Name}",ex);
                return 0;
            }
        }
        public async Task<int> SaveManyAsync<T>(IEnumerable<T> entities) where T : Tbl_Base, new()
        {
            try
            {
                if (entities == null)
                    return 0;

                int count = 0;

                foreach (var entity in entities)
                {
                    await SaveAsync(entity);
                    count++;
                }

                return count;
            }
            catch (Exception ex)
            {
                await _logService.LogError($"SaveMany failed for {typeof(T).Name}",ex );
                return 0;
            }
        }//bulk(List)  data save
        public async Task<int> InsertManyAsync<T>(IEnumerable<T> entities)where T : Tbl_Base, new()
        {
            try
            {
                if (entities == null)
                    return 0;

                return await _database.Connection.InsertAllAsync(entities);
            }
            catch (Exception ex)
            {
                await _logService.LogError($"InsertMany failed for {typeof(T).Name}",ex);
                return 0;
            }

        }//Insert  all new rows .bulk insert.does not update only insert...

        public async Task DeleteEntryAsync<T>(Guid id)
            where T : Tbl_Base, new()
        {
            try
            {
                var entry = await _database.Connection.Table<T>()
                    .FirstOrDefaultAsync(x => x.id == id);

                if (entry == null)
                    return;

                if (SessionStorage.FSU_Submitted)
                {
                    entry.is_deleted = true;
                    await _database.Connection.UpdateAsync(entry);
                }// If FSU is submitted → soft delete
                else
                {
                    await _database.Connection.DeleteAsync(entry);
                } // Hard delete
            }
            catch (Exception ex)
            {
               await  _logService.LogError($"DeleteEntry failed for {typeof(T).Name}, ID: {id}",ex);
            }
        }

        public async Task HardDeleteEntryAsync<T>(Guid id)
            where T : Tbl_Base, new()
        {
            try
            {
                var entry = await _database.Connection.Table<T>()
                    .FirstOrDefaultAsync(x => x.id == id);

                if (entry != null)
                    await _database.Connection.DeleteAsync(entry);
            }
            catch (Exception ex)
            {
                await _logService.LogError($"HardDeleteEntry failed for {typeof(T).Name}, ID: {id}",ex);
            }
        }
    }

}
