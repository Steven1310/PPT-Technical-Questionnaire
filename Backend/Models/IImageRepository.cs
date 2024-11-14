namespace Backend.Models;

public interface IImageRepository
{
    Task<IEnumerable<Images>> Get();
    Task<Images> GetImage(int Id);
}