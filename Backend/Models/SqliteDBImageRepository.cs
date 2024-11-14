using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class SqliteDBImageRepository:IImageRepository
{
    private readonly AppDbContext contextDB;

    public SqliteDBImageRepository(AppDbContext contextDB)
    {
        this.contextDB = contextDB;

    }

    public async Task<IEnumerable<Images>> Get()
    {
        return [.. contextDB.ImagesDBSet];
    }

    public async Task<Images> GetImage(int Id)
    {
        Images image = await contextDB.ImagesDBSet.FirstOrDefaultAsync(s => s.id == Id);
        return image;
    }
}