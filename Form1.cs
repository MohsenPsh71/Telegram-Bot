using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int yes = 0;
        int no = 0;
        int alaki = 0;

        private void txtConnect_Click(object sender, EventArgs e)
        {
            var bot = new Telegram.Bot.TelegramBotClient(txtToken.Text.Trim());

            lblStatus.Text = "Online";
            lblStatus.ForeColor = System.Drawing.Color.Green;

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

        private async Task<int> updateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if(update.CallbackQuery != null)
            {
                return (await ManageCallbackQueryAsync(update, bot, cancellationToken));
            }
            
            var text = update.Message.Text.ToLower();
            var chatId = update.Message.Chat.Id;
            var sendDate = update.Message.Date;
            var firstname = update.Message.From.FirstName;

            if (text == "/start")
                await bot.SendTextMessageAsync(chatId, $"Hi {firstname}", replyMarkup: GenerateMainKeyboard());
            else if(text == "button 1")
            {
                var rows = new List<KeyboardButton[]>();

                // Row 1
                rows.Add(new KeyboardButton[]
                {
                    new KeyboardButton(MyTexts.Button1)
                });

                // Row 2
                rows.Add(new KeyboardButton[]
                {
                    new KeyboardButton("Button 2"),
                    new KeyboardButton("Button 3")
                });

                // Row 3
                rows.Add(new KeyboardButton[]
                {
                    new KeyboardButton("Back")
                });

                var keyboard = new ReplyKeyboardMarkup(rows);

                await bot.SendTextMessageAsync(chatId, $"Hi", replyMarkup: keyboard);
            }

            else if(text == "button 2")
            {
                await bot.SendTextMessageAsync(chatId, $"آیا از این ربات راضی هستید؟", replyMarkup: GenerateInlineKeyboard());
            }

            else if(text == "back")
                await bot.SendTextMessageAsync(chatId, $"Hi {firstname}", replyMarkup: GenerateMainKeyboard());
            else
                await bot.SendTextMessageAsync(chatId, $"Hi {firstname}\nYou Entered : {text}");

            return 0;
        }

        private async Task<int> ManageCallbackQueryAsync(Update update, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            var text = update.CallbackQuery.Data.ToLower();
            var chatId = update.CallbackQuery.Message.Chat.Id;
            var messageId = update.CallbackQuery.Message.MessageId;

            if(text == "yes")
            {
                yes++;
                await bot.SendTextMessageAsync(chatId, $"مرسی که از ما راضی هستید");
            }
            else if(text == "no")
            {
                no++;
                await bot.SendTextMessageAsync(chatId, $"چرا اینجوری میگی داداش");
            }
            else
            {
                alaki++;
                await bot.SendTextMessageAsync(chatId, $"باشه");
            }

            //await bot.SendTextMessageAsync(chatId, $"آیا از این ربات راضی هستید؟", replyMarkup: GenerateInlineKeyboard());
            await bot.EditMessageTextAsync(chatId, messageId, $"آیا از این ربات راضی هستید؟", replyMarkup: GenerateInlineKeyboard());

            return 0;
        }

        private InlineKeyboardMarkup GenerateInlineKeyboard()
        {
            var rows = new List<InlineKeyboardButton[]>()
            {
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton($"بله {yes}") { CallbackData = "yes" },
                    new InlineKeyboardButton($"خیر {no}") { CallbackData = "no" }
                },
                new InlineKeyboardButton[]
                {
                    new InlineKeyboardButton($"نظری ندارم {alaki}") { CallbackData = "alaki" }
                }
            };

            var keyboard = new InlineKeyboardMarkup(rows);
            return keyboard;
        }

        private ReplyKeyboardMarkup GenerateMainKeyboard()
        {
            var rows = new List<KeyboardButton[]>();

            // Row 1
            rows.Add(new KeyboardButton[]
            {
                    new KeyboardButton("Button 1"),
                    new KeyboardButton("Button 2"),
                    new KeyboardButton("Button 3")
            });

            // Row 2
            rows.Add(new KeyboardButton[]
            {
                    new KeyboardButton("Button 4")
            });

            // Row 3
            rows.Add(new KeyboardButton[]
            {
                    new KeyboardButton("Button 5"),
                    new KeyboardButton("Button 6")
            });

            var keyboard = new ReplyKeyboardMarkup(rows);
            return keyboard;
        }

        private async Task errorHandler(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}