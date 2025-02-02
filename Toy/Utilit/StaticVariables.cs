namespace Toy.Utilit
{
    public static class StaticVariables
    {
        public static readonly string defaultPicture = "sign/Image-not-found.png";
        public static readonly string regEmail = "^[\\w\\.\\-]+@[\\w\\-]+\\.[\\w\\-]{2,4}$";
        public static readonly string regPhone = "^\\+?(\\(\\d{1,4}\\))?[\\d\\-]{1,13}$";
        public static readonly string contactInfo = "Введіть пошту або номер телефону";
        public static readonly string passwordInfo = "Пароль має бути від 4 до 8 символів";
        public static readonly string regUserName = "^[a-zA-ZА-Яа-яіїІЇ]{2,20}$";
        public static readonly string regUserSurname = "^[a-zA-ZА-Яа-яіїІЇ]{3,30}$";
        public static readonly int minPasswordLength = 4;
        public static readonly int maxPasswordLength = 8;
        public static readonly int minPhoneEmainLength = 5;
        public static readonly string nameInfo = "Ім'я має бути від 2 літер, без зайвих символів";
        public static readonly string surnameInfo = "Прізвище має бути від 3 літер, без зайвих символів";
        public delegate void RemoveCookie();
        public static RemoveCookie _del;
    }
}
