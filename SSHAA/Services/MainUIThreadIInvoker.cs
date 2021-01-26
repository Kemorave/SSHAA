using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace  SSHAA.Services
{
 public class MainUIThreadIInvoker : Kemorave.IMainUIThreadIInvoker
 {
  public void Invoke(Action action)
  {
   Device.BeginInvokeOnMainThread(action);
  }



  
 }
}
