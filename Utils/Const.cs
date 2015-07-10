using System;

namespace ChatRooms.Utils
{
    public static class Const
    {
        //public static string AllPhotoPath = "http://m.photo.tamtay.vn/category/ajax/all?sort=c2&page={0}";
        //public static string PhotoPath = "http://m.photo.tamtay.vn/category/ajax/{0}?sort=c1&page={1}";

        public static string LoginPath = "http://farjichat.com/wpnativesecure/in.php?nick={0}&g={1}&a={2}&c={3}&device={4}&url={5}";
        public static string OnlineUserPath = "http://farjichat.com/wpnativesecure/online.php?nick={0}&s={1}&text={2}";
        public static string ChatRoomsPath = "http://farjichat.com/wpnativesecure/chatajax.php?nick={0}&s={1}&text={2}&room={3}";
        public static string StickerPath = "http://farjichat.com/wpnativesecure/avatar.php";
        public static string HomePath = "http://farjichat.com/wpnativesecure/room.php?nick={0}&s={1}&text={2}";
        public static string SendMsgPath = "http://farjichat.com/wpnativesecure/chatiapp.php?nick={0}&room={1}";
        public static string UserInRoomPath = "http://farjichat.com/wpnativesecure/inside.php?nick={0}&room={1}";
        public static string PrivateRoomsPath = "http://farjichat.com/wpnativesecure/chatajax.php?nick={0}&room={1}";
        public static string LogoutPath = "http://farjichat.com/wpnativesecure/logout.php?nick={0}";
        public static string AboutPath = "http://farjichat.com/wpnativesecure/about.php";
        public static string BlockedPath = "http://farjichat.com/wpnativesecure/blocked.php?nick={0}";
        public static string BlockPath = "http://farjichat.com/wpnativesecure/block.php?block={0}&by={1}";
        public static string UnblockPath = "http://farjichat.com/wpnativesecure/block.php?unblock={0}&by={1}";
        public static string ChatPmPath = "http://farjichat.com/wpnativesecure/chatpm.php?nick={0}&to={1}&room={2}";
        public static string FlagPath = "http://farjichat.com/iapp/flag.php";
        public static string AvatarPath = "http://farjichat.com/iapp/avatar.php";
        public static string MemePath = "http://farjichat.com/memewp/avatar.php";

        public static string GroupStickerPath = "http://farjichat.com/wpnativesecure/avatarjson.php";
        public static string StickerItemPath = "http://farjichat.com/wpnativesecure/avatarjson.php?groups={0}";
    }
}
