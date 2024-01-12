using System;
using System.Collections.Generic;
using EzBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace EzBooking.Data
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {

            // Verifique se os dados já foram adicionados.
            if (dataContext.PostalCodes.Any() || dataContext.StatusHouses.Any() || dataContext.Houses.Any())
            {
                return; // O banco de dados já está populado.
            }

            // Adicione dados de exemplo aqui
            var postalCode1 = new PostalCode
            {
                postalCode = 4750,
                concelho = "Barcelos",
                district = "Braga"
            };

            var postalCode2 = new PostalCode
            {
                postalCode = 4500,
                concelho = "Matosinhos",
                district = "Porto"
            };

            var statusHouse1 = new StatusHouse
            {
                name = "Suspensa"
            };

            var statusHouse2 = new StatusHouse
            {
                name = "Disponivel"
            };

            var statusHouse3 = new StatusHouse
            {
                name = "Apagada"
            };

            var house1 = new House
            {
                name = "Example House 1",
                doorNumber = 123,
                floorNumber = 2,
                price = 100.0,
                rooms = 4,
                guestsNumber = 4,
                road = "Example Road 1",
                propertyAssessment="dffddsf2",
                sharedRoom = false,
                PostalCode = postalCode1,
                StatusHouse = statusHouse1
            };

            var house2 = new House
            {
                name = "Example House 2",
                doorNumber = 456,
                floorNumber = 3,
                price = 150.0,
                rooms = 4,
                guestsNumber = 6,
                road = "Example Road 2",
                propertyAssessment = "dffddsf3",
                sharedRoom = false,
                PostalCode = postalCode2,
                StatusHouse = statusHouse2
            };

            var image1 = new Images
            {
                image = "1_0_277b4fc1-a3ff-4d70-b9bb-c06f5363be07.webp",
                House = house1,
            };
            var image2 = new Images
            {
                image = "1_1_6c3e50db-017a-436b-83e7-158151bfe3e9.webp",
                House = house1,
            };
            var image3 = new Images
            {
                image = "2_0_6c3e50db-017a-436b-83e7-158151bfe3e9.webp",
                House = house2,
            };

            var userType1 = new UserTypes
            {
                type = "Utilizador"
            };

            var userType2 = new UserTypes
            {
                type = "Anunciante"
            };

            var userType3 = new UserTypes
            {
                type = "Admin"
            };

            var password1 = "password";
            var password2 = "password2";
            var password3 = "password3";

            string hashedPassword1 = BCrypt.Net.BCrypt.HashPassword(password1);
            string hashedPassword2 = BCrypt.Net.BCrypt.HashPassword(password2);
            string hashedPassword3 = BCrypt.Net.BCrypt.HashPassword(password3);

            var user1 = new User
            {
                name = "Pedro",
                email = "pedro@alunos.ipca.pt",
                password = hashedPassword1,
                phone = 123456789,
                token = null,
                status = true,
                image = null,
                userType = userType1
            };

            var user2 = new User
            {
                name = "Luis",
                email = "luis@alunos.ipca.pt",
                password = hashedPassword2,
                phone = 987654321,
                token = null,
                status = true,
                image = null,
                userType = userType2
            };

            var user3 = new User
            {
                name = "Diogo",
                email = "diogo@alunos.ipca.pt",
                password = hashedPassword3,
                phone = 12345432,
                token = null,
                status = true,
                image = null,
                userType = userType3
            };

            var statusReservation1 = new ReservationStates
            {
                state = "Pendente"
            };
            var statusReservation2 = new ReservationStates
            {
                state = "Aprovada (USO)"
            };
            var statusReservation3 = new ReservationStates
            {
                state = "Completa"
            };
            var statusReservation4 = new ReservationStates
            {
                state = "Avaliada"
            };
            var statusReservation5 = new ReservationStates
            {
                state = "Cancelada"
            };


            var reservation1 = new Reservation
            {
                init_date = new DateTime(2023, 11, 18),
                end_date = new DateTime(2023, 11, 20),
                guestsNumber = 1,
                User = user1,
                House = house1,
                ReservationStates = statusReservation1
            };

            var reservation2 = new Reservation
            {
                init_date = new DateTime(2023, 11, 18),
                end_date = new DateTime(2023, 11, 22),
                guestsNumber = 2,
                User = user2,
                House = house2,
                ReservationStates = statusReservation2
            };

            var reservation3 = new Reservation
            {
                init_date = new DateTime(2023, 11, 18),
                end_date = new DateTime(2023, 11, 23),
                guestsNumber = 3,
                User = user3,
                House = house2, 
                ReservationStates = statusReservation1
            };


            var feedback1 = new Feedback
            {
                classification = 5,
                comment = "Muito bom!",
                Reservation = reservation1,
            };

            var feedback2 = new Feedback
            {
                classification = 3,
                comment = "Razoável!",
                Reservation = reservation2
            };

            var feedback3 = new Feedback
            {
                classification = 1,
                comment = "Terrível!",
                Reservation = reservation3
            };

            var paymentState1 = new PaymentStates
            {
                state = "Pendente"
            };

            var paymentState2 = new PaymentStates
            {
                state = "Concluido"
            };

            var paymentState3 = new PaymentStates
            {
                state = "Cancelado"
            };

            var payment1 = new Payment
            {
                creationDate = DateTime.Now,
                paymentDate = DateTime.Now,
                paymentMethod = "MbWay",
                paymentValue = 341,
                state = paymentState1,
                Reservation = reservation1
            };

            var payment2 = new Payment
            {
                creationDate = DateTime.Now,
                paymentDate = DateTime.Now,
                paymentMethod = "Paypal",
                paymentValue = 187,
                state = paymentState2,
                Reservation = reservation2
            };

            var payment3 = new Payment
            {
                creationDate = DateTime.Now,
                paymentDate = DateTime.Now,
                paymentMethod = "Multibanco",
                paymentValue = 900,
                state = paymentState3,
                Reservation = reservation3
            };

            dataContext.PostalCodes.AddRange(postalCode1, postalCode2);
            dataContext.StatusHouses.AddRange(statusHouse1, statusHouse2, statusHouse3);
            dataContext.Houses.AddRange(house1, house2);
            dataContext.UserTypes.AddRange(userType1, userType2, userType3);
            dataContext.Users.AddRange(user1, user2, user3);
            dataContext.Feedbacks.AddRange(feedback1, feedback2, feedback3);
            dataContext.PaymentStates.AddRange(paymentState1, paymentState2, paymentState3);
            dataContext.Payments.AddRange(payment1, payment2, payment3);
            dataContext.Images.AddRange(image1,image2,image3);

            dataContext.ReservationStates.AddRange(statusReservation1, statusReservation2, statusReservation3, statusReservation4, statusReservation5);
            dataContext.Reservations.AddRange(reservation1, reservation2, reservation3);

            dataContext.SaveChanges();
        }
    }
}
