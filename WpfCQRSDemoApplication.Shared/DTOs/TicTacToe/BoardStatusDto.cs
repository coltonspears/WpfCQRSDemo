namespace WpfCQRSDemoApplication.Shared.DTOs.TicTacToe
{
    /// <summary>
    /// Matches OpenAPI status schema: winner and 3x3 board.
    /// Winner: "." = no winner yet, "X" or "O" = winner.
    /// </summary>
    public class BoardStatusDto
    {
        public string Winner { get; set; }
        public string[][] Board { get; set; }
    }
}
