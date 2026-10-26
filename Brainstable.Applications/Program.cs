namespace Brainstable.Applications
{
    /// <summary>
    /// Точка входа приложения: инициализирует WinForms и открывает главную форму.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Запускает приложение в STA-потоке, как требуется для WinForms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Настройка приложения по умолчанию: масштабирование DPI, шрифт и стили WinForms.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
