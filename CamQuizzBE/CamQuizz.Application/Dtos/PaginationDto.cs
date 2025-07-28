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

public class PagedResultMessgeDto<T> : PagedResultDto<T>
{
    public string? GroupName { get; set; }
    public Guid? GroupId  { get; set; }
}

public class PagedRequestKeysetDto
{
    public Guid? AfterId { get; set; }
    public DateTime? AfterCreatedAt { get; set; }
    public int Size { get; set; } = 10;
}

public class QuizzPagedRequestDto:PagedRequestBasicDto
{
    public string? Keyword { get; set; }
    public Guid? CategoryId { get; set; }
    public bool Popular { get; set; } = true;
    public bool Newest {get; set; } = true;
}

public class QuestionPagedRequestDto:PagedRequestBasicDto
{
    public string? Keyword { get; set; }
    public bool Newest {get; set; } = true;
    
}