using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ChatRooms.Model;
using Newtonsoft.Json;

namespace ChatRooms.Utils
{
    public static class DownloadService
    {
        #region Login function

        public static async Task<string> LoginServer(string uri)
        {
            try
            {
                var client = new HttpClient();
                HttpResponseMessage message =
                    await client.GetAsync(uri);

                var result = await message.Content.ReadAsStringAsync();

                Debug.WriteLine(result);

                return result;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return string.Empty;
            }
        }

        #endregion

        #region Send Msg

        public static async Task PostAsync(string uri, string data)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("mms", data)
                });

                var myHttpClient = new HttpClient();
                var response = await myHttpClient.PostAsync(uri, formContent);

                var stringContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(stringContent);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static async Task LogoutAsync(string uri, string data)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("base64_encode()", data)
                });

                var myHttpClient = new HttpClient();
                var response = await myHttpClient.PostAsync(uri, formContent);

                var stringContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(stringContent);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        public static async Task SendPm(string uri, string data)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("pm", data)
                });

                var myHttpClient = new HttpClient();
                var response = await myHttpClient.PostAsync(uri, formContent);

                var stringContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(stringContent);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        #region Get Sticker

        public async static Task<ObservableCollection<GroupSticker>> ParseGroup(string parseUrl)
        {
            try
            {
                var client = new HttpClient();
                HttpResponseMessage message = await client.GetAsync(parseUrl);

                var result = await message.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    ObservableCollection<GroupSticker> parseItem;

                    parseItem = JsonConvert.DeserializeObject<ObservableCollection<GroupSticker>>(result);

                    return parseItem;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return null;
        }

        public async static Task<ObservableCollection<StickerItem>> ParseSticker(string parseUrl)
        {
            try
            {
                var client = new HttpClient();
                HttpResponseMessage message =
                    await client.GetAsync(parseUrl);

                var result = await message.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    ObservableCollection<StickerItem> parseItem;

                    parseItem = JsonConvert.DeserializeObject<ObservableCollection<StickerItem>>(result);

                    return parseItem;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return null;
        }

        #endregion
    }


}
