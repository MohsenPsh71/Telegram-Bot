using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Color = System.Drawing.Color;

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
            lblStatus.ForeColor = Color.Green;

            var receivingOptions = new ReceiverOptions()
            {
                AllowedUpdates = new Telegram.Bot.Types.Enums.UpdateType[]
                {
                    Telegram.Bot.Types.Enums.UpdateType.Message,
                    Telegram.Bot.Types.Enums.UpdateType.CallbackQuery,

                    Telegram.Bot.Types.Enums.UpdateType.Poll,
                    Telegram.Bot.Types.Enums.UpdateType.PollAnswer
                }
            };

            bot.StartReceiving(updateHandler: updateHandler, errorHandler, receivingOptions);
        }

        private async Task<int> updateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.CallbackQuery != null)
            {
                return (await ManageCallbackQueryAsync(update, bot, cancellationToken));
            }

            if (update.Poll != null)
            {
                return 0;
            }

            if (update.PollAnswer != null)
            {
                string x = update.PollAnswer.OptionIds[0] == 1 ? "درست" : "نادرست";
                await bot.SendTextMessageAsync(update.PollAnswer.User.Id, $"کاربر گرامی, {update.PollAnswer.User.FirstName} شما گزینه {x} را انتخاب کرده اید");
                return 0;
            }

            var text = update.Message.Text.ToLower();
            var chatId = update.Message.Chat.Id;
            var sendDate = update.Message.Date;
            var firstname = update.Message.From.FirstName;

            if (text == "/start")
                await bot.SendTextMessageAsync(chatId, $"Hi {firstname}", replyMarkup: GenerateMainKeyboard());
            else if (text == "button 1")
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

            else if (text == "button 2")
            {
                await bot.SendTextMessageAsync(chatId, $"آیا از این ربات راضی هستید؟", replyMarkup: GenerateInlineKeyboard());
            }

            else if (text == "button 3")
            {
                //var message = await bot.SendPollAsync(chatId: chatId
                //    , question: "آیا از این ربات راضی هستید؟"
                //    , options: new string[] { "بله", "خیر", "دیدن نتایج" }
                //    , type: Telegram.Bot.Types.Enums.PollType.Regular
                //    , cancellationToken: cancellationToken);

                var message = await bot.SendPollAsync(chatId: chatId
                    , isAnonymous: false
                    , question: "امسال چه سالی است؟"
                    , options: new string[] { "1401", "1402", "1403" }
                    , correctOptionId: 1
                    , type: Telegram.Bot.Types.Enums.PollType.Quiz
                    , closeDate: DateTime.Now.AddMinutes(1)
                    , cancellationToken: cancellationToken);

            }

            else if (text == "button 4")
            {
                //var imageFile1 = new InputOnlineFile("https://picsum.photos/200/300.jpg");
                //var message = await bot.SendPhotoAsync(chatId, imageFile1, caption: "this file sent by url");

                //// Recommended
                //var imageFile2 = new InputOnlineFile(message.Photo[0].FileId);
                //await bot.SendPhotoAsync(chatId, imageFile2, caption: "this file sent by file_id");

                //var stream = new StreamReader("1.jpg").BaseStream;
                //var imageFile3 = new InputOnlineFile(stream);
                //await bot.SendPhotoAsync(chatId, imageFile3, caption: "this file sent by stream");

                //var stream = new StreamReader("1.mp3").BaseStream;
                //await bot.SendVoiceAsync(chatId, new InputOnlineFile(stream), caption: "voice");
                //await bot.SendAudioAsync(chatId, new InputOnlineFile("https://www.learningcontainer.com/wp-content/uploads/2020/02/Kalimba.mp3"), caption: "Audio");

                //var stream = new StreamReader("1.mp4").BaseStream;
                //await bot.SendVideoAsync(chatId, new InputOnlineFile(stream));

                await bot.SendContactAsync(chatId, "+989215488280", "فرزام",null, "یمینی");

                await bot.SendVenueAsync(chatId, 50.345678, 18.987654, "Title is IRAN", "Address is Tehran");
            }

            else if (text == "back")
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

            if (text == "yes")
            {
                yes++;
                await bot.SendTextMessageAsync(chatId, $"مرسی که از ما راضی هستید");
            }
            else if (text == "no")
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