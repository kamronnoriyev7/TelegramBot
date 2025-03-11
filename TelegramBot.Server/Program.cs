using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    private static readonly string Token = "7891981406:AAHbceeNo73Gb62LFnyd92FABAfTQ36T7Nc";
    private static TelegramBotClient botClient;

    // Viloyatlar ro'yxati
    private static readonly List<string> Regions = new()
    {
        "Toshkent shahri", "Toshkent viloyati", "Andijon", "Buxoro", "Farg'ona", "Jizzax",
        "Namangan", "Navoiy", "Qarshi", "Samarqand", "Surxondaryo", "Xorazm"
    };

    // Viloyatlar uchun ro'za vaqtlari
    private static readonly Dictionary<string, Dictionary<DateTime, (string Saharlik, string Iftorlik)>> RegionPrayerTimes = new()
    {
        {
            "Xorazm", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("06:00", "18:44") },
                { new DateTime(2025, 3, 2), ("05:58", "18:45") },
                { new DateTime(2025, 3, 3), ("05:56", "18:46") },
                { new DateTime(2025, 3, 4), ("05:54", "18:47") },
                { new DateTime(2025, 3, 5), ("05:52", "18:48") },
                { new DateTime(2025, 3, 6), ("05:50", "18:49") },
                { new DateTime(2025, 3, 7), ("05:48", "18:50") },
                { new DateTime(2025, 3, 8), ("05:46", "18:51") },
                { new DateTime(2025, 3, 9), ("05:44", "18:52") },
                { new DateTime(2025, 3, 10), ("05:42", "18:53") },
                { new DateTime(2025, 3, 11), ("05:40", "18:54") },
                { new DateTime(2025, 3, 12), ("05:38", "18:55") },
                { new DateTime(2025, 3, 13), ("05:36", "18:56") },
                { new DateTime(2025, 3, 14), ("05:34", "18:57") },
                { new DateTime(2025, 3, 15), ("05:32", "18:58") },
                { new DateTime(2025, 3, 16), ("05:30", "18:59") },
                { new DateTime(2025, 3, 17), ("05:28", "19:00") },
                { new DateTime(2025, 3, 18), ("05:26", "19:01") },
                { new DateTime(2025, 3, 19), ("05:24", "19:02") },
                { new DateTime(2025, 3, 20), ("05:22", "19:03") },
                { new DateTime(2025, 3, 21), ("05:20", "19:04") },
                { new DateTime(2025, 3, 22), ("05:18", "19:05") },
                { new DateTime(2025, 3, 23), ("05:16", "19:06") },
                { new DateTime(2025, 3, 24), ("05:14", "19:07") },
                { new DateTime(2025, 3, 25), ("05:12", "19:08") },
                { new DateTime(2025, 3, 26), ("05:10", "19:09") },
                { new DateTime(2025, 3, 27), ("05:08", "19:10") },
                { new DateTime(2025, 3, 28), ("05:06", "19:11") },
                { new DateTime(2025, 3, 29), ("05:04", "19:12") },
                { new DateTime(2025, 3, 30), ("05:02", "19:13") },
                { new DateTime(2025, 3, 31), ("05:00", "19:14") }
            }

        },
        {
            "Surxondaryo", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:33", "18:17") },
                { new DateTime(2025, 3, 2), ("05:31", "18:18") },
                { new DateTime(2025, 3, 3), ("05:29", "18:19") },
                { new DateTime(2025, 3, 4), ("05:27", "18:20") },
                { new DateTime(2025, 3, 5), ("05:25", "18:21") },
                { new DateTime(2025, 3, 6), ("05:23", "18:22") },
                { new DateTime(2025, 3, 7), ("05:21", "18:23") },
                { new DateTime(2025, 3, 8), ("05:19", "18:24") },
                { new DateTime(2025, 3, 9), ("05:17", "18:25") },
                { new DateTime(2025, 3, 10), ("05:15", "18:26") },
                { new DateTime(2025, 3, 11), ("05:13", "18:27") },
                { new DateTime(2025, 3, 12), ("05:11", "18:28") },
                { new DateTime(2025, 3, 13), ("05:09", "18:29") },
                { new DateTime(2025, 3, 14), ("05:07", "18:30") },
                { new DateTime(2025, 3, 15), ("05:05", "18:31") },
                { new DateTime(2025, 3, 16), ("05:03", "18:32") },
                { new DateTime(2025, 3, 17), ("05:01", "18:33") },
                { new DateTime(2025, 3, 18), ("04:59", "18:34") },
                { new DateTime(2025, 3, 19), ("04:57", "18:35") },
                { new DateTime(2025, 3, 20), ("04:55", "18:36") },
                { new DateTime(2025, 3, 21), ("04:53", "18:37") },
                { new DateTime(2025, 3, 22), ("04:51", "18:38") },
                { new DateTime(2025, 3, 23), ("04:49", "18:39") },
                { new DateTime(2025, 3, 24), ("04:47", "18:40") },
                { new DateTime(2025, 3, 25), ("04:45", "18:41") },
                { new DateTime(2025, 3, 26), ("04:43", "18:42") },
                { new DateTime(2025, 3, 27), ("04:41", "18:43") },
                { new DateTime(2025, 3, 28), ("04:39", "18:44") },
                { new DateTime(2025, 3, 29), ("04:37", "18:45") },
                { new DateTime(2025, 3, 30), ("04:35", "18:46") },
                { new DateTime(2025, 3, 31), ("04:33", "18:47") }
            }

        },
        {
            "Samarqand", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:42", "18:27") },
                { new DateTime(2025, 3, 2), ("05:40", "18:28") },
                { new DateTime(2025, 3, 3), ("05:38", "18:29") },
                { new DateTime(2025, 3, 4), ("05:36", "18:30") },
                { new DateTime(2025, 3, 5), ("05:34", "18:31") },
                { new DateTime(2025, 3, 6), ("05:32", "18:32") },
                { new DateTime(2025, 3, 7), ("05:30", "18:33") },
                { new DateTime(2025, 3, 8), ("05:28", "18:34") },
                { new DateTime(2025, 3, 9), ("05:26", "18:35") },
                { new DateTime(2025, 3, 10), ("05:24", "18:36") },
                { new DateTime(2025, 3, 11), ("05:22", "18:37") },
                { new DateTime(2025, 3, 12), ("05:20", "18:38") },
                { new DateTime(2025, 3, 13), ("05:18", "18:39") },
                { new DateTime(2025, 3, 14), ("05:16", "18:40") },
                { new DateTime(2025, 3, 15), ("05:14", "18:41") },
                { new DateTime(2025, 3, 16), ("05:12", "18:42") },
                { new DateTime(2025, 3, 17), ("05:10", "18:43") },
                { new DateTime(2025, 3, 18), ("05:08", "18:44") },
                { new DateTime(2025, 3, 19), ("05:06", "18:45") },
                { new DateTime(2025, 3, 20), ("05:04", "18:46") },
                { new DateTime(2025, 3, 21), ("05:02", "18:47") },
                { new DateTime(2025, 3, 22), ("05:00", "18:48") },
                { new DateTime(2025, 3, 23), ("04:58", "18:49") },
                { new DateTime(2025, 3, 24), ("04:56", "18:50") },
                { new DateTime(2025, 3, 25), ("04:54", "18:51") },
                { new DateTime(2025, 3, 26), ("04:52", "18:52") },
                { new DateTime(2025, 3, 27), ("04:50", "18:53") },
                { new DateTime(2025, 3, 28), ("04:48", "18:54") },
                { new DateTime(2025, 3, 29), ("04:46", "18:55") },
                { new DateTime(2025, 3, 30), ("04:44", "18:56") },
                { new DateTime(2025, 3, 31), ("04:42", "18:57") }
            }

        },
        {
            "Qarshi", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:43", "18:29") },
                { new DateTime(2025, 3, 2), ("05:41", "18:30") },
                { new DateTime(2025, 3, 3), ("05:39", "18:31") },
                { new DateTime(2025, 3, 4), ("05:37", "18:32") },
                { new DateTime(2025, 3, 5), ("05:35", "18:33") },
                { new DateTime(2025, 3, 6), ("05:33", "18:34") },
                { new DateTime(2025, 3, 7), ("05:31", "18:35") },
                { new DateTime(2025, 3, 8), ("05:29", "18:36") },
                { new DateTime(2025, 3, 9), ("05:27", "18:37") },
                { new DateTime(2025, 3, 10), ("05:25", "18:38") },
                { new DateTime(2025, 3, 11), ("05:23", "18:39") },
                { new DateTime(2025, 3, 12), ("05:21", "18:40") },
                { new DateTime(2025, 3, 13), ("05:19", "18:41") },
                { new DateTime(2025, 3, 14), ("05:17", "18:42") },
                { new DateTime(2025, 3, 15), ("05:15", "18:43") },
                { new DateTime(2025, 3, 16), ("05:13", "18:44") },
                { new DateTime(2025, 3, 17), ("05:11", "18:45") },
                { new DateTime(2025, 3, 18), ("05:09", "18:46") },
                { new DateTime(2025, 3, 19), ("05:07", "18:47") },
                { new DateTime(2025, 3, 20), ("05:05", "18:48") },
                { new DateTime(2025, 3, 21), ("05:03", "18:49") },
                { new DateTime(2025, 3, 22), ("05:01", "18:50") },
                { new DateTime(2025, 3, 23), ("04:59", "18:51") },
                { new DateTime(2025, 3, 24), ("04:57", "18:52") },
                { new DateTime(2025, 3, 25), ("04:55", "18:53") },
                { new DateTime(2025, 3, 26), ("04:53", "18:54") },
                { new DateTime(2025, 3, 27), ("04:51", "18:55") },
                { new DateTime(2025, 3, 28), ("04:49", "18:56") },
                { new DateTime(2025, 3, 29), ("04:47", "18:57") },
                { new DateTime(2025, 3, 30), ("04:45", "18:58") },
                { new DateTime(2025, 3, 31), ("04:43", "18:59") }
            }

        },
        {
            "Navoiy", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:46", "18:31") },
                { new DateTime(2025, 3, 2), ("05:44", "18:32") },
                { new DateTime(2025, 3, 3), ("05:42", "18:33") },
                { new DateTime(2025, 3, 4), ("05:40", "18:34") },
                { new DateTime(2025, 3, 5), ("05:38", "18:35") },
                { new DateTime(2025, 3, 6), ("05:36", "18:36") },
                { new DateTime(2025, 3, 7), ("05:34", "18:37") },
                { new DateTime(2025, 3, 8), ("05:32", "18:38") },
                { new DateTime(2025, 3, 9), ("05:30", "18:39") },
                { new DateTime(2025, 3, 10), ("05:28", "18:40") },
                { new DateTime(2025, 3, 11), ("05:26", "18:41") },
                { new DateTime(2025, 3, 12), ("05:24", "18:42") },
                { new DateTime(2025, 3, 13), ("05:22", "18:43") },
                { new DateTime(2025, 3, 14), ("05:20", "18:44") },
                { new DateTime(2025, 3, 15), ("05:18", "18:45") },
                { new DateTime(2025, 3, 16), ("05:16", "18:46") },
                { new DateTime(2025, 3, 17), ("05:14", "18:47") },
                { new DateTime(2025, 3, 18), ("05:12", "18:48") },
                { new DateTime(2025, 3, 19), ("05:10", "18:49") },
                { new DateTime(2025, 3, 20), ("05:08", "18:50") },
                { new DateTime(2025, 3, 21), ("05:06", "18:51") },
                { new DateTime(2025, 3, 22), ("05:04", "18:52") },
                { new DateTime(2025, 3, 23), ("05:02", "18:53") },
                { new DateTime(2025, 3, 24), ("05:00", "18:54") },
                { new DateTime(2025, 3, 25), ("04:58", "18:55") },
                { new DateTime(2025, 3, 26), ("04:56", "18:56") },
                { new DateTime(2025, 3, 27), ("04:54", "18:57") },
                { new DateTime(2025, 3, 28), ("04:52", "18:58") },
                { new DateTime(2025, 3, 29), ("04:50", "18:59") },
                { new DateTime(2025, 3, 30), ("04:48", "19:00") },
                { new DateTime(2025, 3, 31), ("04:46", "19:01") }
            }

        },
        {
           "Namangan", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:29", "18:07") },
                { new DateTime(2025, 3, 2), ("05:28", "18:08") },
                { new DateTime(2025, 3, 3), ("05:26", "18:09") },
                { new DateTime(2025, 3, 4), ("05:24", "18:10") },
                { new DateTime(2025, 3, 5), ("05:22", "18:11") },
                { new DateTime(2025, 3, 6), ("05:20", "18:12") },
                { new DateTime(2025, 3, 7), ("05:18", "18:13") },
                { new DateTime(2025, 3, 8), ("05:16", "18:14") },
                { new DateTime(2025, 3, 9), ("05:14", "18:15") },
                { new DateTime(2025, 3, 10), ("05:12", "18:16") },
                { new DateTime(2025, 3, 11), ("05:10", "18:17") },
                { new DateTime(2025, 3, 12), ("05:08", "18:18") },
                { new DateTime(2025, 3, 13), ("05:06", "18:19") },
                { new DateTime(2025, 3, 14), ("05:04", "18:20") },
                { new DateTime(2025, 3, 15), ("05:02", "18:21") },
                { new DateTime(2025, 3, 16), ("05:00", "18:22") },
                { new DateTime(2025, 3, 17), ("04:58", "18:23") },
                { new DateTime(2025, 3, 18), ("04:56", "18:24") },
                { new DateTime(2025, 3, 19), ("04:54", "18:25") },
                { new DateTime(2025, 3, 20), ("04:52", "18:26") },
                { new DateTime(2025, 3, 21), ("04:50", "18:27") },
                { new DateTime(2025, 3, 22), ("04:48", "18:28") },
                { new DateTime(2025, 3, 23), ("04:46", "18:29") },
                { new DateTime(2025, 3, 24), ("04:44", "18:30") },
                { new DateTime(2025, 3, 25), ("04:42", "18:31") },
                { new DateTime(2025, 3, 26), ("04:40", "18:32") },
                { new DateTime(2025, 3, 27), ("04:38", "18:33") },
                { new DateTime(2025, 3, 28), ("04:36", "18:34") },
                { new DateTime(2025, 3, 29), ("04:34", "18:35") },
                { new DateTime(2025, 3, 30), ("04:32", "18:36") },
                { new DateTime(2025, 3, 31), ("04:30", "18:37") }
            }

        },
        {
            "Jizzax", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:34", "18:12") },
                { new DateTime(2025, 3, 2), ("05:32", "18:13") },
                { new DateTime(2025, 3, 3), ("05:31", "18:14") },
                { new DateTime(2025, 3, 4), ("05:29", "18:15") },
                { new DateTime(2025, 3, 5), ("05:27", "18:16") },
                { new DateTime(2025, 3, 6), ("05:25", "18:17") },
                { new DateTime(2025, 3, 7), ("05:23", "18:18") },
                { new DateTime(2025, 3, 8), ("05:21", "18:19") },
                { new DateTime(2025, 3, 9), ("05:19", "18:20") },
                { new DateTime(2025, 3, 10), ("05:17", "18:21") },
                { new DateTime(2025, 3, 11), ("05:15", "18:22") },
                { new DateTime(2025, 3, 12), ("05:13", "18:23") },
                { new DateTime(2025, 3, 13), ("05:11", "18:24") },
                { new DateTime(2025, 3, 14), ("05:09", "18:25") },
                { new DateTime(2025, 3, 15), ("05:07", "18:26") },
                { new DateTime(2025, 3, 16), ("05:05", "18:27") },
                { new DateTime(2025, 3, 17), ("05:03", "18:28") },
                { new DateTime(2025, 3, 18), ("05:01", "18:29") },
                { new DateTime(2025, 3, 19), ("04:59", "18:30") },
                { new DateTime(2025, 3, 20), ("04:57", "18:31") },
                { new DateTime(2025, 3, 21), ("04:55", "18:32") },
                { new DateTime(2025, 3, 22), ("04:53", "18:33") },
                { new DateTime(2025, 3, 23), ("04:51", "18:34") },
                { new DateTime(2025, 3, 24), ("04:49", "18:35") },
                { new DateTime(2025, 3, 25), ("04:47", "18:36") },
                { new DateTime(2025, 3, 26), ("04:45", "18:37") },
                { new DateTime(2025, 3, 27), ("04:43", "18:38") },
                { new DateTime(2025, 3, 28), ("04:41", "18:39") },
                { new DateTime(2025, 3, 29), ("04:39", "18:40") },
                { new DateTime(2025, 3, 30), ("04:37", "18:41") },
                { new DateTime(2025, 3, 31), ("04:35", "18:42") }
            }

        },
         {
            "Farg‘ona", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:25", "18:02") },
                { new DateTime(2025, 3, 2), ("05:24", "18:03") },
                { new DateTime(2025, 3, 3), ("05:22", "18:04") },
                { new DateTime(2025, 3, 4), ("05:20", "18:05") },
                { new DateTime(2025, 3, 5), ("05:18", "18:06") },
                { new DateTime(2025, 3, 6), ("05:16", "18:07") },
                { new DateTime(2025, 3, 7), ("05:14", "18:08") },
                { new DateTime(2025, 3, 8), ("05:12", "18:09") },
                { new DateTime(2025, 3, 9), ("05:10", "18:10") },
                { new DateTime(2025, 3, 10), ("05:08", "18:11") },
                { new DateTime(2025, 3, 11), ("05:06", "18:12") },
                { new DateTime(2025, 3, 12), ("05:04", "18:13") },
                { new DateTime(2025, 3, 13), ("05:02", "18:14") },
                { new DateTime(2025, 3, 14), ("05:00", "18:15") },
                { new DateTime(2025, 3, 15), ("04:58", "18:16") },
                { new DateTime(2025, 3, 16), ("04:56", "18:17") },
                { new DateTime(2025, 3, 17), ("04:54", "18:18") },
                { new DateTime(2025, 3, 18), ("04:52", "18:19") },
                { new DateTime(2025, 3, 19), ("04:50", "18:20") },
                { new DateTime(2025, 3, 20), ("04:48", "18:21") },
                { new DateTime(2025, 3, 21), ("04:46", "18:22") },
                { new DateTime(2025, 3, 22), ("04:44", "18:23") },
                { new DateTime(2025, 3, 23), ("04:42", "18:24") },
                { new DateTime(2025, 3, 24), ("04:40", "18:25") },
                { new DateTime(2025, 3, 25), ("04:38", "18:26") },
                { new DateTime(2025, 3, 26), ("04:36", "18:27") },
                { new DateTime(2025, 3, 27), ("04:34", "18:28") },
                { new DateTime(2025, 3, 28), ("04:32", "18:29") },
                { new DateTime(2025, 3, 29), ("04:30", "18:30") },
                { new DateTime(2025, 3, 30), ("04:28", "18:31") },
                { new DateTime(2025, 3, 31), ("04:26", "18:32") }
            }

         },
        {
            "Toshkent viloyati", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:38", "18:16") },
                { new DateTime(2025, 3, 2), ("05:36", "18:17") },
                { new DateTime(2025, 3, 3), ("05:35", "18:18") },
                { new DateTime(2025, 3, 4), ("05:33", "18:19") },
                { new DateTime(2025, 3, 5), ("05:31", "18:20") },
                { new DateTime(2025, 3, 6), ("05:29", "18:21") },
                { new DateTime(2025, 3, 7), ("05:27", "18:22") },
                { new DateTime(2025, 3, 8), ("05:26", "18:23") },
                { new DateTime(2025, 3, 9), ("05:24", "18:24") },
                { new DateTime(2025, 3, 10), ("05:22", "18:25") },
                { new DateTime(2025, 3, 11), ("05:20", "18:26") },
                { new DateTime(2025, 3, 12), ("05:18", "18:27") },
                { new DateTime(2025, 3, 13), ("05:16", "18:28") },
                { new DateTime(2025, 3, 14), ("05:14", "18:29") },
                { new DateTime(2025, 3, 15), ("05:12", "18:30") },
                { new DateTime(2025, 3, 16), ("05:10", "18:31") },
                { new DateTime(2025, 3, 17), ("05:08", "18:32") },
                { new DateTime(2025, 3, 18), ("05:06", "18:33") },
                { new DateTime(2025, 3, 19), ("05:04", "18:34") },
                { new DateTime(2025, 3, 20), ("05:02", "18:35") },
                { new DateTime(2025, 3, 21), ("05:00", "18:36") },
                { new DateTime(2025, 3, 22), ("04:58", "18:37") },
                { new DateTime(2025, 3, 23), ("04:56", "18:38") },
                { new DateTime(2025, 3, 24), ("04:54", "18:39") },
                { new DateTime(2025, 3, 25), ("04:52", "18:40") },
                { new DateTime(2025, 3, 26), ("04:50", "18:41") },
                { new DateTime(2025, 3, 27), ("04:48", "18:42") },
                { new DateTime(2025, 3, 28), ("04:46", "18:43") },
                { new DateTime(2025, 3, 29), ("04:44", "18:44") },
                { new DateTime(2025, 3, 30), ("04:42", "18:45") },
                { new DateTime(2025, 3, 31), ("04:40", "18:46") }
            }

        },
        {
            "Toshkent shahri", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:39", "18:17") },
                { new DateTime(2025, 3, 2), ("05:38", "18:18") },
                { new DateTime(2025, 3, 3), ("05:36", "18:19") },
                { new DateTime(2025, 3, 4), ("05:35", "18:20") },
                { new DateTime(2025, 3, 5), ("05:33", "18:21") },
                { new DateTime(2025, 3, 6), ("05:32", "18:22") },
                { new DateTime(2025, 3, 7), ("05:30", "18:24") },
                { new DateTime(2025, 3, 8), ("05:28", "18:25") },
                { new DateTime(2025, 3, 9), ("05:27", "18:26") },
                { new DateTime(2025, 3, 10), ("05:25", "18:27") },
                { new DateTime(2025, 3, 11), ("05:23", "18:28") },
                { new DateTime(2025, 3, 12), ("05:22", "18:29") },
                { new DateTime(2025, 3, 13), ("05:20", "18:30") },
                { new DateTime(2025, 3, 14), ("05:18", "18:31") },
                { new DateTime(2025, 3, 15), ("05:17", "18:33") },
                { new DateTime(2025, 3, 16), ("05:15", "18:34") },
                { new DateTime(2025, 3, 17), ("05:13", "18:35") },
                { new DateTime(2025, 3, 18), ("05:11", "18:36") },
                { new DateTime(2025, 3, 19), ("05:10", "18:37") },
                { new DateTime(2025, 3, 20), ("05:08", "18:38") },
                { new DateTime(2025, 3, 21), ("05:06", "18:39") },
                { new DateTime(2025, 3, 22), ("05:04", "18:40") },
                { new DateTime(2025, 3, 23), ("05:02", "18:41") },
                { new DateTime(2025, 3, 24), ("05:01", "18:42") },
                { new DateTime(2025, 3, 25), ("04:59", "18:44") },
                { new DateTime(2025, 3, 26), ("04:57", "18:45") },
                { new DateTime(2025, 3, 27), ("04:55", "18:46") },
                { new DateTime(2025, 3, 28), ("04:53", "18:47") },
                { new DateTime(2025, 3, 29), ("04:51", "18:48") },
                { new DateTime(2025, 3, 30), ("04:50", "18:49") },
                { new DateTime(2025, 3, 31), ("04:48", "18:50") }
            }
        },
        {
            "Andijon", new Dictionary<DateTime, (string, string)>
            {
                 { new DateTime(2025, 3, 1), ("05:27", "18:05") },
                { new DateTime(2025, 3, 2), ("05:26", "18:06") },
                { new DateTime(2025, 3, 3), ("05:24", "18:07") },
                { new DateTime(2025, 3, 4), ("05:23", "18:08") },
                { new DateTime(2025, 3, 5), ("05:21", "18:09") },
                { new DateTime(2025, 3, 6), ("05:19", "18:10") },
                { new DateTime(2025, 3, 7), ("05:18", "18:12") },
                { new DateTime(2025, 3, 8), ("05:16", "18:13") },
                { new DateTime(2025, 3, 9), ("05:14", "18:14") },
                { new DateTime(2025, 3, 10), ("05:13", "18:15") },
                { new DateTime(2025, 3, 11), ("05:11", "18:16") },
                { new DateTime(2025, 3, 12), ("05:09", "18:17") },
                { new DateTime(2025, 3, 13), ("05:08", "18:18") },
                { new DateTime(2025, 3, 14), ("05:06", "18:19") },
                { new DateTime(2025, 3, 15), ("05:04", "18:21") },
                { new DateTime(2025, 3, 16), ("05:02", "18:22") },
                { new DateTime(2025, 3, 17), ("05:01", "18:23") },
                { new DateTime(2025, 3, 18), ("04:59", "18:24") },
                { new DateTime(2025, 3, 19), ("04:57", "18:25") },
                { new DateTime(2025, 3, 20), ("04:55", "18:26") },
                { new DateTime(2025, 3, 21), ("04:53", "18:27") },
                { new DateTime(2025, 3, 22), ("04:52", "18:28") },
                { new DateTime(2025, 3, 23), ("04:50", "18:29") },
                { new DateTime(2025, 3, 24), ("04:48", "18:30") },
                { new DateTime(2025, 3, 25), ("04:46", "18:32") },
                { new DateTime(2025, 3, 26), ("04:44", "18:33") },
                { new DateTime(2025, 3, 27), ("04:42", "18:34") },
                { new DateTime(2025, 3, 28), ("04:40", "18:35") },
                { new DateTime(2025, 3, 29), ("04:38", "18:36") },
                { new DateTime(2025, 3, 30), ("04:37", "18:37") },
                { new DateTime(2025, 3, 31), ("04:35", "18:38") }
            }
        },
        {
            "Buxoro", new Dictionary<DateTime, (string, string)>
            {
                { new DateTime(2025, 3, 1), ("05:49", "18:33") },
                { new DateTime(2025, 3, 2), ("05:47", "18:34") },
                { new DateTime(2025, 3, 3), ("05:46", "18:35") },
                { new DateTime(2025, 3, 4), ("05:44", "18:36") },
                { new DateTime(2025, 3, 5), ("05:42", "18:37") },
                { new DateTime(2025, 3, 6), ("05:40", "18:38") },
                { new DateTime(2025, 3, 7), ("05:39", "18:39") },
                { new DateTime(2025, 3, 8), ("05:37", "18:40") },
                { new DateTime(2025, 3, 9), ("05:35", "18:41") },
                { new DateTime(2025, 3, 10), ("05:33", "18:42") },
                { new DateTime(2025, 3, 11), ("05:31", "18:43") },
                { new DateTime(2025, 3, 12), ("05:30", "18:44") },
                { new DateTime(2025, 3, 13), ("05:28", "18:45") },
                { new DateTime(2025, 3, 14), ("05:26", "18:46") },
                { new DateTime(2025, 3, 15), ("05:24", "18:47") },
                { new DateTime(2025, 3, 16), ("05:22", "18:48") },
                { new DateTime(2025, 3, 17), ("05:20", "18:49") },
                { new DateTime(2025, 3, 18), ("05:18", "18:50") },
                { new DateTime(2025, 3, 19), ("05:16", "18:51") },
                { new DateTime(2025, 3, 20), ("05:14", "18:52") },
                { new DateTime(2025, 3, 21), ("05:12", "18:53") },
                { new DateTime(2025, 3, 22), ("05:10", "18:54") },
                { new DateTime(2025, 3, 23), ("05:08", "18:55") },
                { new DateTime(2025, 3, 24), ("05:06", "18:56") },
                { new DateTime(2025, 3, 25), ("05:04", "18:57") },
                { new DateTime(2025, 3, 26), ("05:02", "18:58") },
                { new DateTime(2025, 3, 27), ("05:00", "18:59") },
                { new DateTime(2025, 3, 28), ("04:58", "19:00") },
                { new DateTime(2025, 3, 29), ("04:56", "19:01") },
                { new DateTime(2025, 3, 30), ("04:54", "19:02") },
                { new DateTime(2025, 3, 31), ("04:52", "19:03") }
            }
        },
        // Boshqa viloyatlar uchun ham shunday qilib to'ldiring...
    };

    // Foydalanuvchining tanlagan viloyatini saqlash uchun lug'at
    private static readonly Dictionary<long, string> UserSelectedRegion = new();

    static async Task Main(string[] args)
    {
        botClient = new TelegramBotClient(Token);

        // Botni ishga tushirish
        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Bot ishlayapti: {me.Username}");

        // Xabarlarni qabul qilish
        botClient.StartReceiving(UpdateHandler, ErrorHandler);

        Console.WriteLine("Bot to'xtatish uchun Enter tugmasini bosing...");
        Console.ReadLine();
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            if (messageText == "/start")
            {
                // Viloyat tugmalarini yaratish
                var regionButtons = new List<KeyboardButton[]>();
                foreach (var region in Regions)
                {
                    regionButtons.Add(new[] { new KeyboardButton(region) });
                }

                // Tugmalarni yuborish
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Iltimos, viloyatingizni tanlang:",
                    replyMarkup: new ReplyKeyboardMarkup(regionButtons) { ResizeKeyboard = true }
                );
            }
            else if (Regions.Contains(messageText))
            {
                // Foydalanuvchi viloyatni tanladi, uni saqlash
                UserSelectedRegion[chatId] = messageText;

                // Kunlarni tanlash tugmalarini yuborish
                var startDate = new DateTime(2025, 3, 1); // 1-mart
                var endDate = new DateTime(2025, 3, 30); // 30-mart
                var dateButtons = new List<KeyboardButton[]>();

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    dateButtons.Add(new[] { new KeyboardButton(date.ToString("yyyy-MM-dd")) });
                }

                // Tugmalarni yuborish
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Iltimos, kunni tanlang:",
                    replyMarkup: new ReplyKeyboardMarkup(dateButtons) { ResizeKeyboard = true }
                );
            }
            else if (DateTime.TryParseExact(messageText, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var selectedDate))
            {
                // Foydalanuvchi tanlagan viloyatni olish
                if (UserSelectedRegion.TryGetValue(chatId, out var selectedRegion))
                {
                    // Faqat 1-martdan 30-martgacha bo'lgan sanalarni qabul qilish
                    var startDate = new DateTime(2025, 3, 1);
                    var endDate = new DateTime(2025, 3, 30);

                    if (selectedDate >= startDate && selectedDate <= endDate)
                    {
                        // Saharlik va iftorlik vaqtlari va duolarini olish
                        if (RegionPrayerTimes.TryGetValue(selectedRegion, out var times) && times.TryGetValue(selectedDate, out var prayerTimes))
                        {
                            var (saharlik, iftorlik) = prayerTimes;
                            var (saharlikDuosi, iftorlikDuosi) = GetPrayerDuo();

                            // Javobni yuborish
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: $"📅 Sana: {selectedDate:yyyy-MM-dd}\n" +
                                      $"🌅 Saharlik: {saharlik}\n" +
                                      $"📖 Saharlik duosi:\n{saharlikDuosi}\n\n" +
                                      $"🌇 Iftorlik: {iftorlik}\n" +
                                      $"📖 Iftorlik duosi:\n{iftorlikDuosi}"
                            );
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Bu sana uchun ma'lumot topilmadi."
                            );
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Noto'g'ri sana. Iltimos, 1-martdan 30-martgacha bo'lgan sanalardan tanlang."
                        );
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Iltimos, avval viloyatingizni tanlang."
                    );
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Noto'g'ri format. Iltimos, qaytadan urinib ko'ring."
                );
            }
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Xatolik yuz berdi: {exception.Message}");
        return Task.CompletedTask;
    }

    // Saharlik va iftorlik duolarini qaytarish (misol uchun)
    private static (string SaharlikDuosi, string IftorlikDuosi) GetPrayerDuo()
    {
        string saharlikDuosi =
            "Навайту ан асума совма шахри рамазона минал фажри илал маг‘риби, холисан лиллахи та'ала. Аллоҳу акбар" 
            ;

        string iftorlikDuosi =
            "Аллоҳумма лака сумту ва бика аманту ва а’лайка таваккалту ва а’ла ризқика афторту, фаг‘фирли ё ғоффару ма қоддамту ва ма аххорту. "
            ;

        return (saharlikDuosi, iftorlikDuosi);
    }
}