﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace YourEasyRent.Services
{
    public class TelegramMenu : ITelegramMenu
    {
        public TelegramMenu() { }

        public InlineKeyboardMarkup Mein =>
            new(new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Brand", callbackData: "Brand"),
            InlineKeyboardButton.WithCallbackData(text:"Product Category", callbackData: "Category") } });

        public InlineKeyboardMarkup Brand =>
            new(new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"TARTE",callbackData:"TARTE"),
            InlineKeyboardButton.WithCallbackData(text:"MAC",callbackData:"MAC") },
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Maybelline",callbackData:"Maybelline"),
            InlineKeyboardButton.WithCallbackData(text:"FENTY BEAUTY",callbackData:"FENTY_BEAUTY")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});

        public InlineKeyboardMarkup Category =>
            new(new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Foundation",callbackData:"Foundation"),
            InlineKeyboardButton.WithCallbackData(text:"Concealer",callbackData:"Concealer"),},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Blush",callbackData:"Blush"),
            InlineKeyboardButton.WithCallbackData(text:"Highlighter",callbackData:"Highlighter")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Mascara",callbackData:"Mascara"),
            InlineKeyboardButton.WithCallbackData(text:"Eyeshadow",callbackData:"Eyeshadow")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Brow pencils",callbackData:"Brow pencils"),
            InlineKeyboardButton.WithCallbackData(text:"Lipstick",callbackData:"Lipstick")},
            new[]
            {InlineKeyboardButton.WithCallbackData(text:"Back",callbackData: "Back")}});

        public InlineKeyboardMarkup ReturnToMeinMenu =>
            new(new[]
            {new[]
            {InlineKeyboardButton.WithCallbackData(text:"Return To Mein Menu",callbackData:"Return_To_MeinMenu")}});
    }
}
