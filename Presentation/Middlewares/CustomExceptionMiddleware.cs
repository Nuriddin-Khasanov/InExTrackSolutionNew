using Application.Exceptions;

namespace Presentation.Middlewares
{
    public class CustomExceptionMiddleware 
    {
        private readonly RequestDelegate _next;  // Ссылка на следующий middleware
        private readonly ILogger<CustomExceptionMiddleware> _logger;  // Логгер

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)  // В конструкторе принимаем ссылку на следующий middleware и логгер
        {
            _next = next;  // Сохраняем ссылку на следующий middleware
            _logger = logger; // Сохраняем логгер
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Передача запроса дальше
            }
            catch (Exception ex)  // Обработка исключений
            {
                if (ex.GetType() == typeof(NotFoundException)) { 
                    context.Response.StatusCode = StatusCodes.Status404NotFound;  // Устанавливаем статус код 404
                    await context.Response.WriteAsync(ex.Message);  // Отправляем ответ с сообщением об ошибке
                    return;
                }
                _logger.LogError(ex, "An error occurred");  // Логируем ошибку
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;  // Устанавливаем статус код 500
                await context.Response.WriteAsync("Ошибка сервера.");  // Отправляем ответ
            }
        }
    }
}
