namespace EzBooking.SwaggerExamples
{
    public class Examples
    {
        public const string NameExample = "Casa Azul";
        public const int DoorNumberExample = 123;
        public const int FloorNumberExample = 2;
        public const double PriceExample = 100.0;
        public const double PriceYearExample = 12000.0;
        public const int GuestsNumberExample = 5;
        public const string RoadExample = "Rua Principal";
        public const string PropertyAssessmentExample = "A4gffX";
        public const int CodDoorExample = 456;
        public const bool SharedRoomExample = false;

        //Exemplos para o User

        public const string UserNameExample = "Roberto Rodrigues";
        public const string UserEmailExample = "robertorodrigues@gmail.com";
        public const string UserPasswordExample = "robertorodrigues123";
        public const string UserPhoneExample = "939882761";

        //Exemplos para o feedback

        public const int ClassificationExample = 5;
        public const string CommentExample = "Adorei";
        public const int ReservationFeedbackExample = 1;

        public const int postalCode = 4750;
        public const string concelho = "Barcelos";
        public const string district = "Braga";

        //Exemplos para o pagamento

        public static DateTime CreationDateExample => DateTime.Now;
        public static DateTime PaymentDateExample => DateTime.Now;
        public const string PaymentMethodExample = "Paypal";
        public const int PaymentValueExample = 354;
        public const int PaymentStateExample = 1;

        //Exemplos para o login

        public const string EmailExample = "robertorodrigues@gmail.com";
        public const string PasswordExample = "robertorodrigues123";

        // Exemplos para as Reservas
        public static readonly DateTime InitDateExample = new DateTime(2023, 12, 20);
        public static readonly DateTime EndDateExample = new DateTime(2023, 12, 31);
        public const int GuestNumResExample = 3;

        //Exemplos para os UserTypes
        public const string TypeExample = "Utilizador";


        // Adicione exemplos para outras propriedades aqui...
    }
}
