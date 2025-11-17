using Income.Common;
using Income.Database.Models.Common;
using Income.Database.Models.HIS_2026;
using Income.Database.Models.SCH0_0;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Income.Database
{
    public class Database
    {
        protected SQLiteAsyncConnection _database;
        public CommonFunction CommonFunction = new CommonFunction();
        public const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
        private const string dbKey = "YourStrongProductionKey!"; // Use a strong key!

        public Database()
        {
            try
            {
                Batteries_V2.Init(); // SQLCipher initialization
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, CommonConstants.DatabaseFilename);
                _database = new SQLiteAsyncConnection(databasePath, Flags, true);

                // Encryption and PRAGMA
                //_database.ExecuteAsync($"PRAGMA key = '{dbKey}';").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA cipher_compatibility = 4;").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA journal_mode = WAL;").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA foreign_keys = ON;").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA synchronous = NORMAL;").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA cache_size = 10000;").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA temp_store = MEMORY;").ConfigureAwait(false).GetAwaiter().GetResult();
                //_database.ExecuteAsync("PRAGMA mmap_size = 30000000000;").ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating DB: {ex.Message}");
            }
        }

        public async Task InitializeAsync()
        {
            // Create tables
            try
            {
                await _database.CreateTableAsync<Tbl_User_Details>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Fsu_List>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_0_1>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_FieldOperation>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_3>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_2_1>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_4>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_7>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Visited_Blocks>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_2_2>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Sch_0_0_Block_5>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Block_1>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Block_3>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Block_4>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Block_4_Q5>().ConfigureAwait(false);
                await _database.CreateTableAsync<Tbl_Warning>().ConfigureAwait(false);
                // Run indexes
                await CreateIndexesAsync();

                // Optimize
                await _database.ExecuteAsync("PRAGMA optimize;").ConfigureAwait(false);
                await _database.ExecuteAsync("PRAGMA page_size = 16384;").ConfigureAwait(false);
                await _database.ExecuteAsync("VACUUM;").ConfigureAwait(false);
                await _database.ExecuteAsync("ANALYZE;").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private async Task CreateIndexesAsync()
        {
            try
            {
                var indexCommands = new[]
                    {
                "CREATE INDEX IF NOT EXISTS idx_UserID ON Tbl_User_Details(id);",
                "CREATE INDEX IF NOT EXISTS idx_FsuId ON Tbl_Fsu_List(fsu_id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_1_id ON Tbl_Sch_0_0_Block_0_1(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_2_id ON Tbl_Sch_0_0_FieldOperation(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_3_id ON Tbl_Sch_0_0_Block_3(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_4_1_id ON Tbl_Sch_0_0_Block_2_1(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_4_2_id ON Tbl_Sch_0_0_Block_2_2(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_4_3_id ON Tbl_Sch_0_0_Block_4(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_5_id ON Tbl_Sch_0_0_Block_5(id);",
                "CREATE INDEX IF NOT EXISTS idx_sch_0_block_5_id ON Tbl_Sch_0_0_Block_7(id);",
            };

                foreach (var cmd in indexCommands)
                    await _database.ExecuteAsync(cmd).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
