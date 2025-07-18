namespace CamQuizz.Application.Dtos;

public class PagedRequestDto:PagedRequestBasicDto
{
    public string? Keyword { get; set; }
}
public class PagedRequestBasicDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}
public class PagedResultDto<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int Total { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}