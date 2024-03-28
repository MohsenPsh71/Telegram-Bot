using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Color = System.Drawing.Color;

namespace TelegramClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtConnect_Click(object sender, EventArgs e)
        {
            var bot = new Telegram.Bot.TelegramBotClient(txtToken.Text.Trim());

            lblStatus.Text = "Online";
            lblStatus.ForeColor = Color.Green;

            var receivingOptions = new ReceiverOptions()
            {
                AllowedUpdates = new Telegram.Bot.Types.Enums.UpdateType[]
                {
                    Telegram.Bot.Types.Enums.UpdateType.Message,
                    Telegram.Bot.Types.Enums.UpdateType.CallbackQuery
                }
            };

            bot.StartReceiving(updateHandler: updateHandler, errorHandler, receivingOptions);
        }

        private async Task updateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            var text = update.Message.Text.ToLower();
            var chatId = update.Message.Chat.Id;
            var sendDate = update.Message.Date;
            var firstname = update.Message.From.FirstName;

            if (text == "/start")
                await bot.SendTextMessageAsync(chatId, $"Hidodo {firstname}");
            else
                await bot.SendTextMessageAsync(chatId, $"Hi {firstname}\nYou Entered : {text}");
        }

        private async Task errorHandler(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}