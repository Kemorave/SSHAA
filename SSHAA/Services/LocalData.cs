using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SSHAA.Services
{
    public class LocalData
    {
        public const string LoginInfoFile = "SSHLogin.txt";
        public const string ReservationsFile = "absfort";
        public static HMSDataRepo.Model.People GetLoginInfo()
        {
            string json = Xamarin.Forms.DependencyService.Get<ISaveAndLoadTextFile>()?.LoadText(LoginInfoFile);
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<HMSDataRepo.Model.People>(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to get login info " + e.Message);
                return null;
            }
        }
        
        public static bool HasLoginData()
        {
            var data = GetLoginInfo();
            if (data==null)
            {
                return false;
            }
            if (data.Email!=null&&data.PersonName!=null&&data.PasswordHash!=null)
            {
                return true;
            }
            return false;
        }
        public static void SaveLoginInfo(HMSDataRepo.Model.People person)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            try
            {
                Xamarin.Forms.DependencyService.Get<ISaveAndLoadTextFile>()?.SaveText(LoginInfoFile, json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to save login info " + e.Message);
            }
        }
        public static void DeleteLoginInfo()
        {
            try
            {
                Xamarin.Forms.DependencyService.Get<ISaveAndLoadTextFile>()?.DeleteText(LoginInfoFile);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to save login info " + e.Message);
            }
        }
        public static List<HMSDataRepo.Model.Reservations> GetLastSavedReservations()
        {
            string json = Xamarin.Forms.DependencyService.Get<ISaveAndLoadTextFile>()?.LoadText(ReservationsFile);
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<HMSDataRepo.Model.Reservations>>(json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to get Reservations " + e.Message);
                return null;
            }
        }
        public static void SaveReservations(List<HMSDataRepo.Model.Reservations> person)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
            try
            {
                Xamarin.Forms.DependencyService.Get<ISaveAndLoadTextFile>()?.SaveText(ReservationsFile, json);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Faild to save Reservations " + e.Message);
            }
        }
    }
}