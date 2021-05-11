using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppGas.Models;
using AppGas.Extensions;
using System.Linq;

namespace AppGas.Data
{
    public class SQLiteDatabase
    {
        //lazy nos ayuda a que al crear-conectar a nuestra base de datos de SQLite no se bloquee la app
        static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() => {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Connection => LazyInitializer.Value;

        static bool IsInitialized = false;

        async Task InitializeAsync()
        {
            if (!IsInitialized)
            {
                if (!Connection.TableMappings.Any(m => m.MappedType.Name == typeof(GasStationModel).Name))
                {
                    await Connection.CreateTablesAsync(CreateFlags.None, typeof(GasStationModel)).ConfigureAwait(false);
                    IsInitialized = true;
                }
            }
        }

        public SQLiteDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        private void OnInitializeError(Exception exc)
        {
            throw new NotImplementedException();
        }

        public Task<List<GasStationModel>> GetAllGasStationsAsync()
        {
            return Connection.Table<GasStationModel>().ToListAsync();
        }

  

        public Task<GasStationModel> GetGasStationAsync(int id)
        {
            return Connection.Table<GasStationModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveGasStationAsync(GasStationModel item)
        {
            if (item.ID != 0)
            {
                return Connection.UpdateAsync(item);
            }
            else
            {
                return Connection.InsertAsync(item);
            }
        }

        public Task<int> DeleteGasStationAsync(GasStationModel item)
        {
            return Connection.DeleteAsync(item);
        }
    }
}
