﻿using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.Data.Entity;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using System.Data.Entity.Infrastructure;

namespace BeautyMeWEB.Controllers
{
    public class AppointmentController : ApiController
    {
        // GET: Appointment
        [HttpGet]
        [Route("api/Appointment/AllAppointment")]
        public HttpResponseMessage GetAllAppointment()
        {
            BeautyMeDBContext db = new BeautyMeDBContext();
            List<AppointmentDTO> AllAppointment = db.Appointment.Select(x => new AppointmentDTO
            {
                Number_appointment = x.Number_appointment,
                Date = x.Date,
                Start_time = x.Start_time,
                End_time = x.End_time,
                Is_client_house = x.Is_client_house,
                Business_Number = x.Business_Number,
            }).ToList();
            if (AllAppointment != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllAppointment);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }


        // Post: api/Post
        [HttpPost]
        [Route("api/Appointment/NewAppointment")]
        public HttpResponseMessage PostNewAppointment([FromBody] AppointmentDTO x)
        {
            try
            {
                BeautyMeDBContext db = new BeautyMeDBContext();
                Appointment newAppointment = new Appointment()
                {
                    //Number_appointment = x.Number_appointment,
                    Date = x.Date,
                    Start_time = x.Start_time,
                    End_time = x.End_time,
                    Is_client_house = x.Is_client_house,
                    Business_Number = x.Business_Number,
                };
                db.Appointment.Add(newAppointment);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "new Appointment added to the dataBase");
            }
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Appointment to the database: " + ex.InnerException.InnerException.Message);
            }
        }


        // Put: api/Put
        [HttpPut]
        [Route("api/Appointment/UpdateAppointment")]
        public HttpResponseMessage PutUpdateAppointment([FromBody] AppointmentDTO x)
        {
            BeautyMeDBContext db = new BeautyMeDBContext();
            Appointment AppointmentToUpdate = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
            if (AppointmentToUpdate == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Appointment with number {x.Number_appointment} not found.");
            }

            else
            {
                //AppointmentToUpdate.Number_appointment = x.Number_appointment;
                AppointmentToUpdate.Date = x.Date;
                AppointmentToUpdate.Start_time = x.Start_time;
                AppointmentToUpdate.End_time = x.End_time;
                AppointmentToUpdate.Is_client_house = x.Is_client_house;
                AppointmentToUpdate.Business_Number = x.Business_Number;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "The Appointment update in the dataBase");
            }
        }


        // Delete: api/Delete
        [HttpDelete]
        [Route("api/Appointment/CanceleAppointment")]
        public IHttpActionResult DeleteCanceleAppointment([FromBody] AppointmentDTO x)
        {
            BeautyMeDBContext db = new BeautyMeDBContext();
            {
                if (x == null)  // בדיקת תקינות ה-DTO שהתקבל
                {
                    return BadRequest("הפרטים שהתקבלו אינם תקינים.");
                }

                Appointment CanceleAppointment = db.Appointment.Find(x.Number_appointment);   // חיפוש הרשומה המתאימה לפי המזהה שלה
                if (CanceleAppointment == null)
                {
                    return NotFound();
                }

                db.Appointment.Remove(CanceleAppointment);   // מחיקת הרשומה מבסיס הנתונים

                db.SaveChanges();

                return Ok("הנתונים נמחקו בהצלחה.");  // החזרת תשובה מתאימה לפי המצב
            }
        }
    }
}



//// Put: api/Put
//[HttpPut]
//[Route("api/Appointment/UpdateAppointment")]
//public HttpResponseMessage PutUpdateAppointment([FromBody] AppointmentDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Appointment AppointmentToUpdate = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
//    if (AppointmentToUpdate == null)
//    {
//        return Request.CreateResponse(HttpStatusCode.NotFound, $"Appointment with number {x.Number_appointment} not found.");
//    }

//    else
//    {
//        //AppointmentToUpdate.Number_appointment = x.Number_appointment;
//        AppointmentToUpdate.Date = x.Date;
//        AppointmentToUpdate.Start_time = x.Start_time;
//        AppointmentToUpdate.End_time = x.End_time;
//        AppointmentToUpdate.Is_client_house = x.Is_client_house;
//        AppointmentToUpdate.Business_Number = x.Business_Number;
//        db.Appointment.Add(AppointmentToUpdate);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "The Appointment update in the dataBase");
//    }
//}