using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HMSDataRepo.Controllers;
using HMSDataRepo.Model;
using Kemorave.Collection;
using Kemorave.Command;
using SSHAA.Services;

namespace SSHAA.VM
{
    public class AppData : Kemorave.ObservableObject
    {
        public static AppData Instance { get; private set; } = new AppData();

        private AppData()
        {
            _recordsController = new RecordsController(HMSDataRepo.WebApiConfig.Default);
            _peopleController = new PeopleController(HMSDataRepo.WebApiConfig.Default);
            _reservationsController = new ReservationsController(HMSDataRepo.WebApiConfig.Default);
            _roomsController = new RoomsController(HMSDataRepo.WebApiConfig.Default);
            Rooms = new ObservableList<Rooms>();
            Reservations = new ObservableList<Reservations>();
            AvailableReservations = new ObservableList<Reservations>();
            Records = new ObservableList<Records>();
            ShowReservationCommand = new RelayCommand<Reservations>(ShowReservation);
            RefreshReservationsCommand = new RelayCommand(async () => { await RefreshReservations(); });
            RefreshRoomsCommand = new RelayCommand(async () => { await RefreshRooms(); });
            RefreshRecordsCommand = new RelayCommand(async () => { await RefreshRecords(); });

        }



        private async Task RefreshRecords(bool silent = false)
        {
            if (IsLoadingRecords)
            {
                return;
            }
            IsLoadingRecords = silent ? false : true;
            await Task.Run(async () =>
            {
                try
                {
                    List<Records> records = (await _recordsController.GetAllEntries())?.Where(r => r.Person_Id == CurrentLoginInfo.ID).ToList();

                    if (records == null || records?.Count == 0)
                    {
                        if (!silent)
                        {
                            Toast = "No records available";
                        }
                        //  ErrorDialog("Please try later", "No room available");
                    }
                    else
                    {
                        Records.AddRange(records, true);
                    }
                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = "Bad network \n" + e.Message;
                    }
                }
                finally
                {
                    IsLoadingRecords = false;
                }
            });
        }

        internal static void RefreshInstanse()
        {
            Instance = new AppData();
        }

        public async Task RefreshRooms(bool silent = false)
        {
            if (IsLoadingRooms)
            {
                return;
            }
            IsLoadingRooms = silent ? false : true; ;
            await Task.Run(async () =>
            {
                try
                {
                    List<Rooms> tempRo = (await _roomsController.GetAllEntries())?.ToList();
                    IEnumerable<Reservations> tempRes = (await _reservationsController.GetAllEntries());

                    IEnumerable<Rooms> tempAVRO = from Rooms room in tempRo
                                                  where tempRes.FirstOrDefault(re => (re.Room_Id == room.ID && re.IsAvailable)) == null
                                                  select room;

                    if (tempAVRO.Count() <= 0)
                    {
                        if (!silent)
                        {
                            ErrorDialog("Please try later", "No room available");
                        }
                    }
                    Rooms.AddRange(tempAVRO, true);

                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = $"Bad network {e.Message}";
                    }
                }
                finally
                {
                    IsLoadingRooms = false;
                }
            });
        }

        public async Task RefreshReservations(bool silent = false)
        {
            if (IsLoadingResrvations)
            {
                return;
            }
            IsLoadingResrvations = silent ? false : true; ;
            await Task.Run(async () =>
            {
                try
                {
                    List<Reservations> reservations = (await _reservationsController.GetAllEntries())?.Where(r => r.Person_Id == CurrentLoginInfo.ID)?.ToList();
                    if (reservations == null || reservations?.Count <= 0)
                    {
                        if (!silent)
                        {
                            Toast = "No reservations available";
                        }
                    }
                    else
                    {
                        Reservations.AddRange(reservations, true);
                        AvailableReservations.AddRange(reservations.Where(r => r.IsAvailable), true);
                    }
                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = "Bad network \n" + e.Message;
                    }
                }
                finally
                {
                    IsLoadingResrvations = false;
                }
            });
        }

        private async void Initialize(bool silent = false)
        {
            try
            {
                await RefreshRooms(silent);
                await RefreshRecords(silent);
                await RefreshReservations(silent);
                //IsBusy = false;
            }
            catch (Exception) { }
        }
        internal async Task<bool> AddReservation(Reservations reservation)
        {
            if (IsBusy)
            {
                return false;
            }
            IsBusy = true;

            return await Task.Run(async () =>
            {
                try
                {
                    await RefreshReservations(true);
                    if (Reservations.FirstOrDefault(r => r.Room_Id == reservation.Room_Id) is Reservations dub)
                    {
                        ErrorDialog("Unkown error please try again");
                        return false;
                    }
                    System.Net.HttpStatusCode res = await _reservationsController.AddEntry(reservation);
                    if (res == System.Net.HttpStatusCode.OK)
                    {
                        await RefreshReservations(true);
                        Toast = "Room Booked Succesfully";
                        RecordEvent("Reservation", "You have booked room " + reservation.Room_Id + " with total price of " + reservation.TotalPrice + "$");
                        return true;
                    }
                    else
                    {
                        ErrorDialog("Unkown error please try again");
                    }
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message);
                }
                finally { IsBusy = false; }
                return false;
            });
        }


        private void ShowReservation(Reservations obj)
        {
            throw new NotImplementedException();
        }

        private void RecordEvent(string type, string content)
        {
            try
            {
                Records _ = new HMSDataRepo.Model.Records() { Type = type, Content = content, Person_Id = CurrentLoginInfo.ID };
                Records.Add(_);
                _recordsController.AddEntry(_);
            }
            catch (Exception e)
            {
                Toast = "Records error " + e.Message;
            }
        }


        public async Task<bool> RegisterUser(People obj)
        {
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    if (_peopleController.GetVerfications(obj) is Verfication verfication)
                    {
                        ErrorDialog(verfication.VaildationMessage, "Vaildation faild");
                        return false;
                    }
                    System.Net.HttpStatusCode res = await _peopleController.AddEntry(obj);
                    if (res != System.Net.HttpStatusCode.OK)
                    {
                        Toast = "Bad network please try again";
                    }

                    return res == System.Net.HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Registration error");
                    return false;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
        public async Task<bool?> Login(People obj)
        {
            return await Login(obj.Email, obj.PasswordHash);
        }
        public async Task<bool> EditAccount(People obj)
        {
            if (IsBusy)
            {
                return false;
            }
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    if (_peopleController.GetVerfications(obj) is Verfication verfication)
                    {
                        ErrorDialog(verfication.VaildationMessage, "Invaild input");
                        return false;
                    }
                    System.Net.HttpStatusCode res = await _peopleController.SetByID(CurrentLoginInfo.ID, obj);
                    if (res == System.Net.HttpStatusCode.OK)
                    {
                        CurrentLoginInfo = obj;
                        Services.LocalData.SaveLoginInfo(obj);
                        Toast = "Successfully updated";
                        return true;
                    }
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Task faild");
                }
                finally
                {
                    IsBusy = false;
                }
                return true;
            });
        }
        public async Task<bool> DeleteAccount(string password)
        {
            if (IsBusy)
            {
                return false;
            }
            if (People.HashString(password) != CurrentLoginInfo.PasswordHash)
            {
                ErrorDialog("Wrong password", "Verification failed");
                return false;
            }
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    System.Net.HttpStatusCode res = await _peopleController.DeleteByID(CurrentLoginInfo.ID);
                    if (res == System.Net.HttpStatusCode.OK)
                    {
                        Toast = "Successfully deleted";
                        Records.Clear();
                        Reservations.Clear();
                        Rooms.Clear();
                        Services.LocalData.DeleteLoginInfo();
                        IsLoggedIn = false;
                        CurrentLoginInfo = null;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Task faild");
                }
                finally
                {
                    IsBusy = false;
                }
                return true;
            });
        }
        public async Task<bool> DeleteReservation(Reservations reserv)
        {
            if (IsBusy)
            {
                return false;
            }
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    System.Net.HttpStatusCode res = await _reservationsController.DeleteByID(reserv.ID);
                    if (res == System.Net.HttpStatusCode.OK)
                    {
                        Toast = "Successfully deleted";
                        Reservations.Remove(reserv);
                        AvailableReservations.Remove(reserv);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Task faild");
                }
                finally
                {
                    IsBusy = false;
                }
                return true;
            });
        }
        public async Task<bool?> Login(string email, string hash)
        {
            IsBusy = true;
            return await Task.Run(new Func<bool?>(() =>
           {
               try
               {
                   IEnumerable<People> list = _peopleController.GetAllEntries().Result;
                   if (list == null)
                   {
                       ErrorDialog("No user found please register", "login failed");
                       return null;
                   }
                   foreach (People item in list)
                   {
                       if (item.Email?.ToLower() == email?.ToLower())
                       {
                           if (item.PasswordHash != hash)
                           {
                               ErrorDialog("Wrong password", "login failed");
                               return false;
                           }
                           Services.LocalData.SaveLoginInfo(item);

                           IsBusy = false;
                           IsLoggedIn = true;
                           CurrentLoginInfo = item;
                           Reservations.Clear();
                           AvailableReservations.Clear();
                           //Initialize();
                           Toast = "Loading";
                           RecordEvent("Login", "Login detected on device " + ServicesInterfaces.GetDeviceServices()?.GetDeviceName());

                           return true;
                       }
                   }
                   ErrorDialog("No account found please register", "login faild");
                   return null;
               }
               catch (Exception e)
               {
                   ErrorDialog("Please check you network \n" + e.Message, "connection faild");

                   return false;
               }
               finally
               {
                   IsBusy = false;
               }
           }));
        }


        public void Logout()
        {
            Services.LocalData.DeleteLoginInfo();
            IsLoggedIn = false;
            CurrentLoginInfo = null;
        }
        public async Task<bool?> AutoLoginAsync()
        {

            People person = LocalData.GetLoginInfo();
            if (person == null)
            {
                return null;
            }
            else
            {
                return await Login(person);
            }
        }

        private void ErrorDialog(string value, string title = "Error")
        {
            ServicesInterfaces.GetDialogService()?.ShowDialog(title, value);
        }

        #region Prop


        private string Toast
        {
            set => ServicesInterfaces.GetToastService()?.LongAlert(value);
        }
        public string RecordsFilter { get => _RecordsFilter; set { _RecordsFilter = value; FilterRecordsAsync(value); } }
        private async void FilterRecordsAsync(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value) && value != "None")
                {
                    await RefreshRecords();
                    Records.AddRange(Records.Where(s => s.Type.Equals(value, StringComparison.OrdinalIgnoreCase)).ToList(), true);
                }
                else
                {
                    await RefreshRecords();
                }
            }
            catch (Exception e)
            {
                Toast = e.Message;
            }
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingRecords
        {
            get => isLoadingRecords;
            set
            {
                isLoadingRecords = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingResrvations
        {
            get => isLoadingResrvations;
            set
            {
                isLoadingResrvations = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingRooms
        {
            get => isLoadingRooms;
            set
            {
                isLoadingRooms = value; RaisePropertyChanged();
            }
        }

        public bool IsLoggedIn
        {
            get => isLoaggedIn;
            set { isLoaggedIn = value; RaisePropertyChanged(); if (value) { Initialize(true); } }
        }
        public ObservableList<Reservations> Reservations { get; }


        public ObservableList<Records> Records { get; }
        public ObservableList<Rooms> Rooms { get; }
        public People CurrentLoginInfo
        {
            get => currentLoginInfo;
            set { currentLoginInfo = value; RaisePropertyChanged(); }
        }

        public ObservableList<Reservations> AvailableReservations { get; }
        public RelayCommand<Reservations> ShowReservationCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand RefreshRoomsCommand { get; }
        public RelayCommand RefreshRecordsCommand { get; }
        public RelayCommand RefreshReservationsCommand { get; }

        #endregion
        #region Vars
        private People currentLoginInfo;
        private readonly RecordsController _recordsController;
        private readonly PeopleController _peopleController;
        private readonly ReservationsController _reservationsController;
        private readonly RoomsController _roomsController;

        private bool isBusy, isLoaggedIn;
        private string _RecordsFilter;
        private bool isLoadingRecords;
        private bool isLoadingResrvations;
        private bool isLoadingRooms;



        #endregion
    }
}