using Snacks_App.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snacks_App.Services;



public class FavoriteService
{
    private readonly SQLiteAsyncConnection _database;

    public FavoriteService()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favoritos.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<FavoriteProduct>().Wait();
    }

    public async Task<FavoriteProduct> ReadAsync(int id)
    {
        try
        {
            return await _database.Table<FavoriteProduct>().Where(p => p.ProductId == id).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<FavoriteProduct>> ReadAllAsync()
    {
        try
        {
            return await _database.Table<FavoriteProduct>().ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CreateAsync(FavoriteProduct produtoFavorito)
    {
        try
        {
            await _database.InsertAsync(produtoFavorito);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(FavoriteProduct produtoFavorito)
    {
        try
        {
            await _database.DeleteAsync(produtoFavorito);
        }
        catch (Exception)
        {
            throw;
        }
    }

}
